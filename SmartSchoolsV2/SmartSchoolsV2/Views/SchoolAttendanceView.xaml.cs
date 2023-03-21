using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.ViewModels;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamForms.Controls;
using SmartSchoolsV2.Resources;
using Rg.Plugins.Popup.Services;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SchoolAttendanceView : ContentView
	{
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _shift_id;
        public DateTime _selectedDate = DateTime.Now;
        public DateTime _monthYear = DateTime.Now;
        CalendarViewModel _vm;
        public static Command LoadSchoolDailyAttendancePercentage { get; set; }
        public int RowCount { get; set; }
        public static Label l = new Label();
        public static string _back;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }
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

        public async void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
                _shift_id = Settings.shiftId;
                txtShift.Text = Settings.shiftCode;

                await SchoolDailyAttendancePercentage();

                calendar.SelectedDate = DateTime.Now;
            }
        }
        public SchoolAttendanceView()
        {
            InitializeComponent();
            this.BindingContext = this;

            calendar.SelectedDate = DateTime.Now;

            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) => {
                if (!string.IsNullOrEmpty(txtShift.Text))
                {
                    PopupNavigation.Instance.PushAsync(new AttendanceDownloadPopupPage(Settings.schoolId, Settings.schoolName, 0, string.Empty, _monthYear, _shift_id, txtShift.Text));
                }
                else {
                    SnackB.Message = AppResources.WorkingShiftRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }

            };
            btnDownload.GestureRecognizers.Add(tapGestureRecognizerTxn);

            //LoadSchoolDailyAttendancePercentage = new Command(async () => await SchoolDailyAttendancePercentage());
        }

        async void OnLeftArrowClicked(object s, DateTimeEventArgs e)
        {
            if (_shift_id > 0)
            {
                _monthYear = _monthYear.AddMonths(-1);

                await SchoolDailyAttendancePercentage();
            }
            else {
                SnackB.Message = AppResources.WorkingShiftRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }

        }
        async void OnRightArrowClicked(object s, DateTimeEventArgs e)
        {
            if (_shift_id > 0)
            {
                _monthYear = _monthYear.AddMonths(1);

                await SchoolDailyAttendancePercentage();
            }
            else {
                SnackB.Message = AppResources.WorkingShiftRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }

        }
        async void OnDateClicked(object s, DateTimeEventArgs e)
        {
            if (_shift_id > 0)
            {
                _selectedDate = e.DateTime;
                SchoolAttendancePage page = new SchoolAttendancePage(Settings.schoolId, Settings.schoolName, Settings.cityName, Settings.schoolCode, _selectedDate, _shift_id);
                await Navigation.PushAsync(page);
            }
            else {
                SnackB.Message = AppResources.WorkingShiftRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void StartCall(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectWorkingShiftText, "shift", "attendance");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        public async Task SchoolDailyAttendancePercentage()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostSchoolStaffDailyAttendancePercentage(Settings.schoolId, _shift_id, _monthYear.ToString("MM-yyyy"));
                    string jsonStr = await t;
                    ClassDailyAttendancePercentageProperty response = JsonConvert.DeserializeObject<ClassDailyAttendancePercentageProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        var special = new List<SpecialDate>();

                        List<ClassDailyAttendancePercentage> list = new List<ClassDailyAttendancePercentage>();
                        foreach (ClassDailyAttendancePercentage sl in response.Data)
                        {
                            string _dot = "";

                            foreach (ClassDailyAttendancePercentage sp in response.Data)
                            {
                                if (sp.total_percentage >= 90)
                                {
                                    _dot = "dot_green.jpg";
                                }
                                else if (sp.total_percentage >= 60 && sp.total_attendance < 90)
                                {
                                    _dot = "dot_blue.jpg";
                                }
                                else if (sp.total_percentage >= 20 && sp.total_percentage < 60)
                                {
                                    _dot = "dot_yellow.jpg";
                                }
                                else if (sp.total_percentage < 20)
                                {
                                    _dot = "dot_red.jpg";
                                }

                                var specialDate = new SpecialDate(sp.entry_date)
                                {
                                    BackgroundImage = FileImageSource.FromFile(_dot) as FileImageSource,
                                    Selectable = true,
                                    TextColor = Color.Black
                                };
                                special.Add(specialDate);
                            }
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