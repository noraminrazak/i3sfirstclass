using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamForms.Controls;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubAttendanceView : ContentView
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public DateTime _selectedDate = DateTime.Now;
        public DateTime _monthYear = DateTime.Now;
        CalendarViewModel _vm;
        public static Command LoadClubDailyAttendancePercentage { get; set; }
        public int RowCount { get; set; }
        public static Label l = new Label();

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
        public ClubAttendanceView()
        {
            InitializeComponent();
            this.BindingContext = this;

            LoadClubDailyAttendancePercentage = new Command(async () => await ClubDailyAttendancePercentage());

            calendar.SelectedDate = DateTime.Now;
            //var tapGestureRecognizerTxn = new TapGestureRecognizer();
            //tapGestureRecognizerTxn.Tapped += (s, e) => {
            //    PopupNavigation.Instance.PushAsync(new AttendanceDownloadPopupPage(Settings.selectedSchoolId, Settings.selectedSchoolName, Settings.selectedClassId, Settings.selectedClassName, _monthYear));
            //};
            //btnDownload.GestureRecognizers.Add(tapGestureRecognizerTxn);
        }

        async void OnLeftArrowClicked(object s, DateTimeEventArgs e)
        {
            _monthYear = _monthYear.AddMonths(-1);

            await ClubDailyAttendancePercentage();
        }
        async void OnRightArrowClicked(object s, DateTimeEventArgs e)
        {
            _monthYear = _monthYear.AddMonths(1);

            await ClubDailyAttendancePercentage();
        }
        async void OnDateClicked(object s, DateTimeEventArgs e)
        {
            _selectedDate = e.DateTime;
            ClubAttendancePage page = new ClubAttendancePage(Settings.selectedSchoolId, Settings.selectedSchoolName, Settings.selectedClubId, Settings.selectedClubName, _selectedDate);
            await Navigation.PushAsync(page);
        }


        public async Task ClubDailyAttendancePercentage()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostSchoolClubDailyAttendancePercentage(Settings.selectedSchoolId, Settings.selectedClubId, _monthYear.ToString("MM-yyyy"));
                    string jsonStr = await t;
                    ClubDailyAttendancePercentageProperty response = JsonConvert.DeserializeObject<ClubDailyAttendancePercentageProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        var special = new List<SpecialDate>();

                        List<ClubDailyAttendancePercentage> list = new List<ClubDailyAttendancePercentage>();
                        foreach (ClubDailyAttendancePercentage sl in response.Data)
                        {
                            string _dot = "";

                            foreach (ClubDailyAttendancePercentage sp in response.Data)
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