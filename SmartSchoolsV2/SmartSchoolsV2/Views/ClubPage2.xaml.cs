using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubPage2 : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged1;

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

        public ObservableCollection<ClubMember> _listMember;
        public ObservableCollection<ClubMember> listMember
        {
            get
            {
                return _listMember;
            }
            set
            {
                _listMember = value;
                PropertyChanged1?.Invoke(this, new PropertyChangedEventArgs("listMember"));
            }
        }
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _user_role_id;
        public int _relationship_id;
        public int _create_by_staff_id;
        public int _staff_id;
        public int _profile_id;
        public int _club_id;
        public int _school_id;
        public string _club_name;
        public string _school_name;
        public string _full_name;
        public string _photo_url;
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
                await GeneralLeaveClub();
            }
        }
        public ClubPage2(int relationship_id, int profile_id, int club_id, string club_name, int school_id,
            string school_name, string full_name, string photo_url, int create_by_staff_id)
        {
            InitializeComponent();
            BindingContext = this;

            _user_role_id = Settings.userRoleId;
            if (_user_role_id == 8)
            {
                _staff_id = Settings.staffId;
            }
            _relationship_id = relationship_id;
            _profile_id = profile_id;
            _club_id = club_id;
            _club_name = club_name;
            _school_id = school_id;
            _school_name = school_name;
            _full_name = full_name;
            _photo_url = photo_url;
            _create_by_staff_id = create_by_staff_id;

            var tapGestureRecognizer0 = new TapGestureRecognizer();
            tapGestureRecognizer0.Tapped += (s, e) => {
                carouselView.Position = 0;
            };
            imgPost.GestureRecognizers.Add(tapGestureRecognizer0);

            //var tapGestureRecognizer1 = new TapGestureRecognizer();
            //tapGestureRecognizer1.Tapped += (s, e) => {
            //    carouselView.Position = 1;
            //};
            //imgMember.GestureRecognizers.Add(tapGestureRecognizer1);

            var tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += (s, e) => {
                carouselView.Position = 1;
            };
            imgCalendar.GestureRecognizers.Add(tapGestureRecognizer2);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblFullName.Text = _full_name;
            lblSchoolName.Text = _school_name;
            lblClubName.Text = _club_name;

            if (!string.IsNullOrEmpty(_photo_url))
            {
                userInitial.IsVisible = false;
                userImg.IsVisible = true;
                userImg.Source = requestUrl + _photo_url;
            }
            else
            {
                userInitial.IsVisible = true;
            }


            if (carouselView.Position == 0)
            {
                ClubPostView.LoadSchoolClubPost.Execute(null);
            }
            else if (carouselView.Position == 1)
            {
                ClubMemberView.LoadClubMember.Execute(null);
            }
            else if (carouselView.Position == 2)
            {

            }
        }
        void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            int currentItemPosition = e.CurrentPosition;

            if (currentItemPosition == 0)
            {
                ClubPostView.LoadSchoolClubPost.Execute(null);

                imgPost.Source = "ic_trophy.png";
                //imgMember.Source = "ic_group_grey.png";
                imgCalendar.Source = "ic_schedule_grey.png";
            }
            //else if (currentItemPosition == 1)
            //{
            //    ClubMemberView.LoadClubMember.Execute(null);

            //    imgPost.Source = "ic_trophy_grey.png";
            //    imgMember.Source = "ic_group.png";
            //    imgCalendar.Source = "ic_schedule_grey.png";
            //}
            else if (currentItemPosition == 1)
            {
                ClubMemberAttendanceView.LoadClubMemberAttendanceMonthly.Execute(null);
                imgPost.Source = "ic_trophy_grey.png";
                //imgMember.Source = "ic_group_grey.png";
                imgCalendar.Source = "ic_schedule.png";
            }
        }
        async void OnLeaveClubClicked(object sender, EventArgs args)
        {
            if (_user_role_id == 8)
            {
                if (_staff_id == _create_by_staff_id)
                {
                    await Navigation.PushAsync(new StaffLeaveClubPage(_school_id, _club_id, _club_name));
                }
                else
                {
                    //var page = new LeaveClubPopupPage(_club_id, _club_name, _school_name);
                    //page.DetailSet += this.OnDetailSet;
                    //await PopupNavigation.Instance.PushAsync(page);
                    bool result = await DisplayAlert(AppResources.LeaveClubMenuText, AppResources.DoYouReallyWantToLeaveText + _club_name + "?", AppResources.YesText, AppResources.CancelText);
                    if (result == true)
                    {
                        await GeneralLeaveClub();
                    }
                }
            }
            else
            {
                //var page = new LeaveClubPopupPage(_club_id, _club_name, _school_name);
                //page.DetailSet += this.OnDetailSet;
                //await PopupNavigation.Instance.PushAsync(page);
                bool isStudent = Settings.studentClub;
                bool result = false;
                if (isStudent == true)
                {
                    string student_name = Settings.studentFullName;
                    result = await DisplayAlert(AppResources.LeaveClubMenuText, AppResources.DoYouReallyWantStudentText + student_name + AppResources.ToLeaveText + _club_name + "?", AppResources.YesText, AppResources.CancelText);
                }
                else
                {
                    result = await DisplayAlert(AppResources.LeaveClubMenuText, AppResources.DoYouReallyWantToLeaveText + _club_name + "?", AppResources.YesText, AppResources.CancelText);
                }

                if (result == true)
                {
                    await GeneralLeaveClub();
                }
            }
        }

        public async Task GeneralLeaveClub()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostGeneralLeaveClub(_relationship_id, _profile_id, _club_id, Settings.fullName);
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
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