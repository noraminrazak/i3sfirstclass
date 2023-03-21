using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;
using System.Globalization;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassAttendancePage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
        public int RowCount { get; set; }
        public static Label l = new Label();
        public DateTime _selectedDate;
        public int _school_id;
        public int _class_id;
        public int _report_id;
        public int _student_id;
        public int _attendance_id;
        public int _reason_id;
        public string _school_name;
        public string _class_name;
        public ObservableCollection<ClassDailyAttendance> listAttendance { get; set; }
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
                if (_reason_id > 0) {
                    await UpdateStudentAttendance();
                }
            }
        }
        public ClassAttendancePage(int school_id, string school_name,int class_id,string class_name, DateTime selected_date)
        {
            InitializeComponent();
            BindingContext = this;

            _school_id = school_id;
            lblSchoolName.Text = school_name;
            lblClassName.Text = class_name;
            _class_id = class_id;
            _selectedDate = selected_date;

            lblTitle.Text = _selectedDate.ToString("dddd, dd MMM yyyy", ci);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ClassDailyAttendanceSummary();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        public async void ClassDailyAttendanceSummary()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostSchoolClassDailyAttendanceSummary(_school_id, _class_id, _selectedDate.ToString("dd-MM-yyyy"));
                    string jsonStr = await t;
                    ClassDailyAttendanceSummaryProperty response = JsonConvert.DeserializeObject<ClassDailyAttendanceSummaryProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<ClassDailyAttendanceSummary> list = new List<ClassDailyAttendanceSummary>();
                        foreach (ClassDailyAttendanceSummary sl in response.Data)
                        {
                            lblPresent.Text = sl.total_present.ToString();
                            lblLateIn.Text = sl.total_late_in.ToString();
                            lblHalfDay.Text = sl.total_half_day.ToString();
                            lblAbsent.Text = sl.total_absent.ToString();
                            lblTotal.Text = sl.total_attendance.ToString() + "/" + sl.total_student.ToString();
                        }

                        await ClassDailyAttendance();
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
        public async Task ClassDailyAttendance()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvAttendance.ItemsSource = null;
                    var t = srvc.PostSchoolClassDailyAttendance(_school_id, _class_id, _selectedDate.ToString("dd-MM-yyyy"));
                    string jsonStr = await t;
                    ClassDailyAttendanceProperty response = JsonConvert.DeserializeObject<ClassDailyAttendanceProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listAttendance = new ObservableCollection<ClassDailyAttendance>();
                        foreach (ClassDailyAttendance sl in response.Data)
                        {
                            ClassDailyAttendance post = new ClassDailyAttendance();

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
                            post.student_id = sl.student_id;
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
                            l.Text = AppResources.NoRecordForSelectedDateText;
                        }
                        lvAttendance.Footer = l;
                        lvAttendance.ItemsSource = listAttendance;
                    }
                    else
                    {
                        listAttendance = new ObservableCollection<ClassDailyAttendance>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordForSelectedDateText;
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

        public ClassDailyAttendance attendance;
        async void OnAttendanceSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as ClassDailyAttendance;
            if (data == null) return;
            attendance = data;

            if (attendance.report_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                _report_id = attendance.report_id;
                _student_id = attendance.student_id;
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
                        var page = new SearchListPage1(AppResources.SelectReasonForAbsentText, "reason-for-absent", "class-attendance");
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
                        var page2 = new SearchListPage1(AppResources.SelectReasonForAbsentText, "reason-for-absent", "class-attendance");
                        page2.DetailSet += this.OnDetailSet;
                        await Navigation.PushAsync(page2);
                        break;
                    default:
                        break;
                }

                if (_attendance_id != 1 && _attendance_id > 0)
                { 
                    _reason_id = 0;
                    await UpdateStudentAttendance();
                }
                ((ListView)sender).SelectedItem = null;
            }
        }

        public async Task UpdateStudentAttendance() 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffUpdateStudentAttendance(_report_id,_school_id,_class_id,_student_id,_attendance_id,_reason_id,Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        ClassDailyAttendanceSummary();
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