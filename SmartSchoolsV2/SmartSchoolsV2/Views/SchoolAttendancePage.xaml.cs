using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchoolAttendancePage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
        public int RowCount { get; set; }
        public static Label l = new Label();
        public DateTime _selectedDate;
        public int _school_id;
        public int _report_id;
        public int _staff_id;
        public int _attendance_id;
        public int _reason_id;
        public string _school_name;
        public string _class_name;
        public int _shift_id;
        public ObservableCollection<SchoolDailyAttendance> listAttendance { get; set; }
        public static string _back;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }
        public async void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
                _reason_id = Settings.reasonId;
                if (_reason_id > 0)
                {
                    await UpdateStaffAtteandance();
                }
            }
        }
        public SchoolAttendancePage(int school_id, string school_name, string city_name, string school_code, DateTime selected_date, int shift_id)
        {
            InitializeComponent();
            BindingContext = this;

            _school_id = school_id;
            lblSchoolName.Text = school_name;
            lblSchoolCode.Text = school_code;
            _selectedDate = selected_date;
            _shift_id = shift_id;

            lblTitle.Text = _selectedDate.ToString("dddd, dd MMM yyyy", ci);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            SchoolDailyAttendanceSummary();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        public async void SchoolDailyAttendanceSummary()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostSchoolStaffDailyAttendanceSummary(_school_id, _shift_id, _selectedDate.ToString("dd-MM-yyyy"));
                    string jsonStr = await t;
                    StaffDailyAttendanceSummaryProperty response = JsonConvert.DeserializeObject<StaffDailyAttendanceSummaryProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<StaffDailyAttendanceSummary> list = new List<StaffDailyAttendanceSummary>();
                        foreach (StaffDailyAttendanceSummary sl in response.Data)
                        {
                            lblPresent.Text = sl.total_present.ToString();
                            lblLateIn.Text = sl.total_late_in.ToString();
                            lblHalfDay.Text = sl.total_half_day.ToString();
                            lblAbsent.Text = sl.total_absent.ToString();
                            lblTotal.Text = sl.total_attendance.ToString() + "/" + sl.total_staff.ToString();
                        }

                        await StaffDailyAttendance();
                    }
                    else
                    {

                    }
                }
                catch (Exception)
                {

                }
                finally
                {

                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }
        public async Task StaffDailyAttendance()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvAttendance.ItemsSource = null;
                    var t = srvc.PostSchoolStaffDailyAttendance(_school_id, _shift_id, _selectedDate.ToString("dd-MM-yyyy"));
                    string jsonStr = await t;
                    SchoolDailyAttendanceProperty response = JsonConvert.DeserializeObject<SchoolDailyAttendanceProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listAttendance = new ObservableCollection<SchoolDailyAttendance>();
                        foreach (SchoolDailyAttendance sl in response.Data)
                        {
                            SchoolDailyAttendance post = new SchoolDailyAttendance();

                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url = requestUrl + sl.photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.photo_url = "";
                                post.initial_visible = true;
                                post.image_visible = false;
                            }

                            if (sl.reason_id > 0)
                            {
                                post.dot_visible = true;
                            }
                            else
                            {
                                post.dot_visible = false;
                            }

                            post.report_id = sl.report_id;
                            post.staff_id = sl.staff_id;
                            post.full_name = sl.full_name;

                            post.attendance_id = sl.attendance_id;
                            if (Settings.cultureInfo == "ms-MY")
                            {
                                post.attendance = sl.attendance_bm;
                            }
                            else if (Settings.cultureInfo == "en-US")
                            {
                                post.attendance = sl.attendance;
                            }
                            post.reason_id = sl.reason_id;
                            if (Settings.cultureInfo == "ms-MY")
                            {
                                post.reason_for_absent = sl.reason_for_absent_bm;
                            }
                            else if (Settings.cultureInfo == "en-US")
                            {
                                post.reason_for_absent = sl.reason_for_absent;
                            }
                            listAttendance.Add(post);
                        }
                        RowCount = listAttendance.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = "No records for selected date";
                        }
                        lvAttendance.Footer = l;
                        lvAttendance.ItemsSource = listAttendance;
                    }
                    else
                    {
                        listAttendance = new ObservableCollection<SchoolDailyAttendance>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = "No records for selected date";
                        lvAttendance.Footer = l;
                        lvAttendance.ItemsSource = listAttendance;
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {

            }
        }
        ViewCell lastCell;
        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.LightGoldenrodYellow;
                lastCell = viewCell;
            }
        }

        public SchoolDailyAttendance attendance;
        async void OnAttendanceSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as SchoolDailyAttendance;
            if (data == null) return;
            attendance = data;

            if (attendance.report_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                _report_id = attendance.report_id;
                _staff_id = attendance.staff_id;
                var action = await DisplayActionSheet(attendance.full_name, AppResources.CancelText, null, AppResources.PresentText, AppResources.LateInText, AppResources.HalfdayText, AppResources.AbsentText);
                switch (action)
                {
                    case "Present":
                        _attendance_id = 2;
                        break;
                    case "Late In":
                        _attendance_id = 3;
                        break;
                    case "Half Day":
                        _attendance_id = 4;
                        break;
                    case "Absent":
                        _attendance_id = 1;
                        var page = new SearchListPage1(AppResources.SelectReasonForAbsentText, "reason-for-absent", "school-attendance");
                        page.DetailSet += this.OnDetailSet;
                        await Navigation.PushAsync(page);
                        break;
                    case "Hadir":
                        _attendance_id = 2;
                        break;
                    case "Lewat":
                        _attendance_id = 3;
                        break;
                    case "Separuh Hari":
                        _attendance_id = 4;
                        break;
                    case "Tidak Hadir":
                        _attendance_id = 1;
                        var page2 = new SearchListPage1(AppResources.SelectReasonForAbsentText, "reason-for-absent", "school-attendance");
                        page2.DetailSet += this.OnDetailSet;
                        await Navigation.PushAsync(page2);
                        break;
                    default:
                        break;
                }

                if (_attendance_id != 1 && _attendance_id > 0)
                {
                    _reason_id = 0;
                    await UpdateStaffAtteandance();
                }
                ((ListView)sender).SelectedItem = null;
            }
        }

        public async Task UpdateStaffAtteandance()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffUpdateStaffAttendance(_report_id, _school_id, _staff_id, _attendance_id, _reason_id, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        SchoolDailyAttendanceSummary();
                    }
                    else
                    {
                        await DisplayAlert(AppResources.SorryText, response.Message, "OK");
                    }
                }
                catch (Exception)
                {
                    SnackB.Message = AppResources.SomethingWrongText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    HideLoadingPopup();
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        LoadingPopupPage loadingPage = new LoadingPopupPage();

        async void ShowLoadingPopup()
        {
            await Navigation.PushPopupAsync(loadingPage);
        }

        async void HideLoadingPopup()
        {
            await Task.Delay(500);
            await Navigation.RemovePopupPageAsync(loadingPage);
        }
    }
}