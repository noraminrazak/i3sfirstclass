using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using SmartSchoolsV2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentPage1 : ContentPage, INotifyPropertyChanged
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

        public ObservableCollection<ParentStudentRelationship> _listClub;
        public ObservableCollection<ParentStudentRelationship> listClub
        {
            get
            {
                return _listClub;
            }
            set
            {
                _listClub = value;
                PropertyChanged1?.Invoke(this, new PropertyChangedEventArgs("listClub"));
            }
        }

        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;

        public int _student_id;
        public int _profile_id;
        public int _school_id;
        public int _school_type_id;
        public int _class_id;
        public string _full_name;
        public string _photo_url;
        public string _school_name;
        public string _class_name;
        public StudentPage1(int student_id, int profile_id, string full_name,string photo_url, int school_id, string school_name, int school_type_id,
            int class_id, string class_name)
        {
            InitializeComponent();
            BindingContext = this;

            _student_id = student_id;
            _profile_id = profile_id;
            _full_name = full_name;
            _school_id = school_id;
            _class_id = class_id;
            _school_type_id = school_type_id;
            //_photo_url = photo_url;
            _school_name = school_name;
            _class_name = class_name;

            var tapGestureRecognizer0 = new TapGestureRecognizer();
            tapGestureRecognizer0.Tapped += (s, e) => {
                carouselView.Position = 0;
            };
            imgClassroom.GestureRecognizers.Add(tapGestureRecognizer0);

            var tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += (s, e) => {
                carouselView.Position = 1;
            };
            imgClub.GestureRecognizers.Add(tapGestureRecognizer1);

            var tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += (s, e) => {
                carouselView.Position = 2;
            };
            imgAttendance.GestureRecognizers.Add(tapGestureRecognizer2);

            var tapGestureRecognizer3 = new TapGestureRecognizer();
            tapGestureRecognizer3.Tapped += (s, e) => {
                carouselView.Position = 3;
            };
            imgResult.GestureRecognizers.Add(tapGestureRecognizer3);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblFullName.Text = _full_name;
            lblSchoolName.Text = _school_name;

            StudentProfile();

            if (!string.IsNullOrEmpty(_class_name))
            {
                dotImage.IsVisible = true;
                lblClassName.Text = _class_name;
            }
            else
            {
                dotImage.IsVisible = false;
            }

            if (carouselView.Position == 0)
            {
                ClassPostView.LoadSchoolClassPost.Execute(null);
            }
            else if (carouselView.Position == 1)
            {
                StudentClubView.LoadClubRelationship.Execute(null);
            }
            else if (carouselView.Position == 2)
            {
                StudentAttendanceView.LoadStudentAttendanceMonthly.Execute(null);
            }
            else if (carouselView.Position == 3)
            {
                StudentResultView.LoadSchoolInfo.Execute(null);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            int previousItemPosition = e.PreviousPosition;
            int currentItemPosition = e.CurrentPosition;
            var smallSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            var microSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
            if (currentItemPosition == 0)
            {
                ClassPostView.LoadSchoolClassPost.Execute(null);

                imgClassroom.Source = "ic_classroom.png";
                imgClub.Source = "ic_trophy_grey";
                imgAttendance.Source = "ic_schedule_grey.png";
                imgResult.Source = "ic_score_grey.png";
            }
            else if (currentItemPosition == 1)
            {
                StudentClubView.LoadClubRelationship.Execute(null);

                imgClassroom.Source = "ic_classroom_grey.png";
                imgClub.Source = "ic_trophy";
                imgAttendance.Source = "ic_schedule_grey.png";
                imgResult.Source = "ic_score_grey.png";
            }
            else if (currentItemPosition == 2)
            {
                StudentAttendanceView.LoadStudentAttendanceMonthly.Execute(null);

                imgClassroom.Source = "ic_classroom_grey.png";
                imgClub.Source = "ic_trophy_grey";
                imgAttendance.Source = "ic_schedule.png";
                imgResult.Source = "ic_score_grey.png";
            }
            else if (currentItemPosition == 3)
            {
                StudentResultView.LoadSchoolInfo.Execute(null);

                imgClassroom.Source = "ic_classroom_grey.png";
                imgClub.Source = "ic_trophy_grey";
                imgAttendance.Source = "ic_schedule_grey.png";
                imgResult.Source = "ic_score.png";
            }
        }

        public async void StudentProfile()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostStudentProfile(_profile_id);
                    string jsonStr = await t;
                    ParentProfileProperty response = JsonConvert.DeserializeObject<ParentProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        string photo_url = string.Empty;
                        foreach (ParentProfile r in response.Data)
                        {
                            photo_url = r.photo_url;
                        }

                        if (!string.IsNullOrEmpty(photo_url))
                        {
                            userInitial.IsVisible = false;
                            userImg.IsVisible = true;
                            userImg.Source = requestUrl + photo_url;
                        }
                        else
                        {
                            userInitial.IsVisible = true;
                        }
                        //ParentSchoolRelationship();
                    }
                    else
                    {
                        SnackB.Message = response.Message;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                    //IsBusy = false;
                }
                catch (Exception)
                {
                    SnackB.Message = AppResources.SomethingWrongText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy = false;
                }
                //HideLoadingPopup();
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }

        }

        private async void OnRemoveStudentClicked(object sender, EventArgs e)
        {
            bool action = await DisplayAlert(AppResources.RemoveStudentMenuText, AppResources.DoYouReallyWantToRemoveText + _full_name + "?", AppResources.YesText, AppResources.CancelText);
            if (action)
            {
                await RemoveStudent();
            }
        }

        public async Task RemoveStudent()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostParentRemoveStudentRelationship(Settings.parentId, _student_id, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await Navigation.PopToRootAsync();
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
        async void OnEditProfileClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new EditProfilePage("student"));
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
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}