using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SmartSchoolsV2.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;
using Rg.Plugins.Popup.Services;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassPage2 : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged1;

        public ObservableCollection<SchoolClassStudent> _listStudent;
        public ObservableCollection<SchoolClassStudent> listStudent
        {
            get
            {
                return _listStudent;
            }
            set
            {
                _listStudent = value;
                PropertyChanged1?.Invoke(this, new PropertyChangedEventArgs("listStudent"));
            }
        }

        public ObservableCollection<SchoolPost> _listPost;
        public ObservableCollection<SchoolPost> listPost
        {
            get
            {
                return _listPost;
            }
            set
            {
                _listPost = value;
                PropertyChanged1?.Invoke(this, new PropertyChangedEventArgs("listPost"));
            }
        }

        public ObservableCollection<ClassDailyAttendance> _listAttendance;
        public ObservableCollection<ClassDailyAttendance> listAttendance
        {
            get
            {
                return _listAttendance;
            }
            set
            {
                _listAttendance = value;
                PropertyChanged1?.Invoke(this, new PropertyChangedEventArgs("listAttendance"));
            }
        }

        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _relationship_id;
        public int _school_id;
        public string _school_name;
        public int _class_id;
        public int _merchant_id;
        public string _class_name;
        public string _session_code;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public static Command LoadSchoolInfo { get; set; }
        public static string _back;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }
        public void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
                _merchant_id = Settings.selectMerchantId;
                PopupNavigation.Instance.PushAsync(new OrderDownloadPopupPage(_merchant_id, _school_id, _class_id, _class_name));
            }
        }
        public ClassPage2(int relationship_id, int school_id, string school_name, int class_id, string class_name, string session_code)
        {
            InitializeComponent();
            BindingContext = this;

            var tapGestureRecognizer0 = new TapGestureRecognizer();
            tapGestureRecognizer0.Tapped += (s, e) => {
                carouselView.Position = 0;
            };
            imgClass.GestureRecognizers.Add(tapGestureRecognizer0);

            var tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += (s, e) => {
                carouselView.Position = 1;
            };
            imgStudent.GestureRecognizers.Add(tapGestureRecognizer1);

            var tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += (s, e) => {
                carouselView.Position = 2;
            };
            imgCalendar.GestureRecognizers.Add(tapGestureRecognizer2);

            _relationship_id = relationship_id;
            _school_id = school_id;
            _school_name = school_name;
            _class_id = class_id;
            _class_name = class_name;
            _session_code = session_code;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblSchoolName.Text = _school_name;
            lblClassName.Text = _class_name;

            if (carouselView.Position == 0)
            {
                ClassPostView.LoadSchoolClassPost.Execute(null);
            }
            else if (carouselView.Position == 1)
            {
                ClassStudentView.LoadSchoolClassStudent.Execute(null);
            }
            else if (carouselView.Position == 2)
            {
                ClassAttendanceView.LoadClassDailyAttendancePercentage.Execute(null);
            }
        }


        void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            int currentItemPosition = e.CurrentPosition;

            if (currentItemPosition == 0)
            {
                ClassPostView.LoadSchoolClassPost.Execute(null);

                imgClass.Source = "ic_classroom.png";
                imgStudent.Source = "ic_student_grey.png";
                imgCalendar.Source = "ic_schedule_grey.png";
            }
            else if (currentItemPosition == 1)
            {
                ClassStudentView.LoadSchoolClassStudent.Execute(null);

                imgClass.Source = "ic_classroom_grey.png";
                imgStudent.Source = "ic_student.png";
                imgCalendar.Source = "ic_schedule_grey.png";
            }
            else if (currentItemPosition == 2)
            {
                ClassAttendanceView.LoadClassDailyAttendancePercentage.Execute(null);

                imgClass.Source = "ic_classroom_grey.png";
                imgStudent.Source = "ic_student_grey.png";
                imgCalendar.Source = "ic_schedule.png";
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        async void OnLeaveClassClicked(object sender, EventArgs args)
        {
            //var page = new LeaveClassPopupPage(_class_id, _class_name, _session_code);
            //page.DetailSet += this.OnDetailSet;
            //await PopupNavigation.Instance.PushAsync(page);
            bool result = await DisplayAlert(AppResources.LeaveClassMenutext, AppResources.DoYouReallyWantToLeaveText + _class_name + "?", AppResources.YesText, AppResources.CancelText);
            
            if (result == true) 
            {
                await StaffLeaveClass();
            }
        }

        async void OnDownloadOrderClicked(object sender, EventArgs args)
        {
            var page = new SearchListPage2(AppResources.SelectMerchantTypeText, "class-order", _school_id);
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        public async Task StaffLeaveClass() 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffLeaveClass(_relationship_id, _class_id, Settings.staffId, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await Navigation.PopAsync();
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