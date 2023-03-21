using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SmartSchoolsV2.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamForms.Controls;
using System.Globalization;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StudentAttendanceView : ContentView
    {
		Connection conn = new Connection();
		ServiceWrapper srvc = new ServiceWrapper();
		public string requestUrl = Settings.requestUrl;
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
        public static Command LoadStudentAttendanceMonthly { get; set; }
        public int _m;
        public int _y;
        public string _month;
        public DateTime _monthYear = DateTime.Now;
        CalendarViewModel _vm;
        bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public StudentAttendanceView ()
		{
			InitializeComponent ();
            this.BindingContext = this;

            LoadStudentAttendanceMonthly = new Command(async () => await StudentMonthlyAttendance());

            calendar.SelectedDate = DateTime.Now;
        }

        private DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set 
            { 
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
        async void OnLeftArrowClicked(object s, DateTimeEventArgs e)
        {
            _monthYear = _monthYear.AddMonths(-1);

            await StudentMonthlyAttendance();
        }
        async void OnRightArrowClicked(object s, DateTimeEventArgs e)
        {
            _monthYear = _monthYear.AddMonths(1);

            await StudentMonthlyAttendance();
        }
        async void OnDateClicked(object s, DateTimeEventArgs e)
        {
            _selectedDate = e.DateTime;

            await StudentDailyAttendance();
        }
        public async Task StudentMonthlyAttendance()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostStudentMonthlyAttendance(Settings.studentSchoolId, Settings.studentClassId, Settings.studentId, _monthYear.ToString("MM-yyyy"));
                    string jsonStr = await t;
                    StudentMonthlyAttendanceProperty response = JsonConvert.DeserializeObject<StudentMonthlyAttendanceProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        string _dot = "";
                        var special = new List<SpecialDate>();
                        foreach (StudentMonthlyAttendance sl in response.Data)
                        {
                            if (sl.attendance_id == 1)
                            {
                                _dot = "dot_red.jpg";
                            }
                            else if (sl.attendance_id == 2)
                            {
                                _dot = "dot_green.jpg";
                            }
                            else if (sl.attendance_id == 3)
                            {
                                _dot = "dot_orange.jpg";
                            }
                            else if (sl.attendance_id == 4)
                            {
                                _dot = "dot_blue.jpg";
                            }

                            var specialDate = new SpecialDate(sl.entry_date)
                            {
                                BackgroundImage = FileImageSource.FromFile(_dot) as FileImageSource,
                                Selectable = true,
                                TextColor = Color.Black
                            };
                            special.Add(specialDate);
                        }
                        _vm = new CalendarViewModel();
                        _vm.Attendances = new ObservableCollection<SpecialDate>(special);
                        await Task.Delay(1000);
                        calendar.SpecialDates = _vm.Attendances;
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                }
                finally 
                {
                    HideLoadingPopup();
                }
            }
            else
            {

            }
        }

        public async Task StudentDailyAttendance()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    //ShowLoadingPopup();
                    var t = srvc.PostStudentDailyAttendance(Settings.studentSchoolId, Settings.studentClassId, Settings.studentId, _selectedDate.ToString("dd-MM-yyyy"));
                    string jsonStr = await t;
                    StudentMonthlyAttendanceProperty response = JsonConvert.DeserializeObject<StudentMonthlyAttendanceProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        if (response.Data.Count > 0)
                        {
                            foreach (StudentMonthlyAttendance sl in response.Data)
                            {
                                lblDate.Text = _selectedDate.ToString("dddd, dd MMMM yyyy", ci);
                                if (Settings.cultureInfo == "ms-MY")
                                {
                                    lblAttendance.Text = sl.attendance_bm;
                                }
                                else if (Settings.cultureInfo == "en-US")
                                {
                                    lblAttendance.Text = sl.attendance;
                                }
                                lblAttendance.HorizontalTextAlignment = TextAlignment.Start;
                                if (sl.attendance_id != 2 && sl.reason_id > 0)
                                {
                                    lblReason.IsVisible = true;
                                    if (Settings.cultureInfo == "ms-MY")
                                    {
                                        lblReason.Text = sl.reason_for_absent_bm;
                                    }
                                    else if (Settings.cultureInfo == "en-US")
                                    {
                                        lblReason.Text = sl.reason_for_absent;
                                    }
                                }
                                else
                                {
                                    lblReason.IsVisible = false;
                                }

                            }
                        }
                        else 
                        {
                            lblDate.Text = _selectedDate.ToString("dddd, dd MMMM yyyy", ci);
                            lblAttendance.Text = AppResources.NoRecordForSelectedDateText;
                            lblAttendance.HorizontalTextAlignment = TextAlignment.Center;
                            lblReason.IsVisible = false;
                        }
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                }
                finally
                {
                    //HideLoadingPopup();
                }
            }
            else
            {

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