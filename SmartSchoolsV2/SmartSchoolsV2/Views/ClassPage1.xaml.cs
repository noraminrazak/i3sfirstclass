using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassPage1 : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged1;

        public ObservableCollection<SchoolStaff> _listStaff;
        public ObservableCollection<SchoolStaff> listStaff
        {
            get
            {
                return _listStaff;
            }
            set
            {
                _listStaff = value;
                PropertyChanged1?.Invoke(this, new PropertyChangedEventArgs("listStaff"));
            }
        }
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _school_id;
        public string _school_name;
        public string _state_name;
        public string _city_name;
        public string _school_code;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public static Command LoadSchoolInfo { get; set; }
        //bool isBusy;
        //public bool IsBusy
        //{
        //    get => isBusy;
        //    set
        //    {
        //        isBusy = value;
        //        OnPropertyChanged();
        //    }
        //}
        public ClassPage1(int school_id, string school_name, string state_name, string city_name, string school_code)
        {
            InitializeComponent();
            BindingContext = this;

            var tapGestureRecognizerNotify = new TapGestureRecognizer();
            tapGestureRecognizerNotify.Tapped += (s, e) => {
                carouselView.Position = 0;
            };
            imgGroup.GestureRecognizers.Add(tapGestureRecognizerNotify);

            var tapGestureRecognizerTopup = new TapGestureRecognizer();
            tapGestureRecognizerTopup.Tapped += (s, e) => {
                carouselView.Position = 1;
            };
            imgCalendar.GestureRecognizers.Add(tapGestureRecognizerTopup);

            _school_id = school_id;
            _school_name = school_name;
            _state_name = state_name;
            _city_name = city_name;
            _school_code = school_code;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblSchoolName.Text = _school_name;
            lblStateName.Text = _city_name;

            if (carouselView.Position == 0)
            {
                StaffView.LoadSchoolStaff.Execute(null);
            }
            else if (carouselView.Position == 1)
            {
                //SchoolAttendanceView.LoadSchoolDailyAttendancePercentage.Execute(null);
            }
        }
        void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            int currentItemPosition = e.CurrentPosition;

            if (currentItemPosition == 0)
            {
                SchoolView.LoadSchoolPost.Execute(null);

                imgGroup.Source = "ic_group.png";
                imgCalendar.Source = "ic_schedule_grey.png";
            }
            else if (currentItemPosition == 1)
            {
                //SchoolAttendanceView.LoadSchoolDailyAttendancePercentage.Execute(null);

                imgGroup.Source = "ic_group_grey.png";
                imgCalendar.Source = "ic_schedule.png";
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

    }
}