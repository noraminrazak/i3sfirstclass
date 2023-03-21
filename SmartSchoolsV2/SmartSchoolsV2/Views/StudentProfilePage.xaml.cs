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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentProfilePage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<StudentParentRelationship> listParent { get; set; }

        public string _photo_url;
        public string _full_name;
        public int _profile_id;
        public int _student_id;
        public int _school_id;
        public int _class_id;
        public int _new_class_id;
        public string _class_name;
        public static string _back;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }
        public StudentProfilePage(int profile_id, int student_id, string photo_url, string full_name, int school_id, int class_id, string class_name)
        {
            InitializeComponent();
            BindingContext = this;

            _profile_id = profile_id;
            _student_id = student_id;
            _photo_url = photo_url;
            _full_name = full_name;
            _school_id = school_id;
            _class_id = class_id;
            _class_name = class_name;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            txtFullName.Text = _full_name;
            txtClass.Text = _class_name;

            if (!string.IsNullOrEmpty(_photo_url))
            {
                userInitial.IsVisible = false;
                imagePhoto.IsVisible = true;
                imagePhoto.Source = ImageSource.FromUri(new Uri(requestUrl + _photo_url));
            }
            else
            {
                userInitial.IsVisible = true;
            }

            UserProfile();
            StudentParentRelationship();
        }

        public async void UserProfile()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostUserProfile(_profile_id);
                    string jsonStr = await t;
                    UserProfileProperty response = JsonConvert.DeserializeObject<UserProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        foreach (UserProfile r in response.Data)
                        {
                            txtAddress.Text = r.address;
                            txtCity.Text = r.city;
                            txtPostcode.Text = r.postcode;
                            txtState.Text = r.state_name;
                            txtCountry.Text = r.country_name;
                        }
                    }
                    else
                    {
                        SnackB.Message = response.Message;
                        SnackB.IsOpen = !SnackB.IsOpen;
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

                    var t = srvc.PostStaffRemoveStudentClass(_school_id, _student_id, _class_id, Settings.fullName);
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

        async void StartCall(object sender, EventArgs args)
        {
            var page = new SearchListPage2(AppResources.SelectClassNameText, "student", _school_id);
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        public void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
                txtClass.Text = Settings.className;
                _new_class_id = Settings.classId;
            }
        }

        public async void StudentParentRelationship()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvParent.ItemsSource = null;
                    var t = srvc.PostStudentParentRelationship(_student_id);
                    string jsonStr = await t;
                    StudentParentRelationshipProperty response = JsonConvert.DeserializeObject<StudentParentRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listParent = new ObservableCollection<StudentParentRelationship>();
                        foreach (StudentParentRelationship sl in response.Data)
                        {
                            StudentParentRelationship post = new StudentParentRelationship();
                            post.parent_id = sl.parent_id;
                            post.profile_id = sl.profile_id;
                            post.full_name = sl.full_name;
                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url = sl.photo_url;
                                post.photo_url = requestUrl + sl.photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.image_visible = false;
                                post.initial_visible = true;
                            }

                            post.mobile_number = sl.mobile_number;
                            listParent.Add(post);
                        }
                        RowCount = listParent.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            lvParent.HeightRequest = (30 + (75 * RowCount));
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            lvParent.HeightRequest = 30;
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvParent.Footer = l;
                        lvParent.ItemsSource = listParent;
                    }
                    else
                    {
                        lvParent.HeightRequest = 30;
                        listParent = new ObservableCollection<StudentParentRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvParent.Footer = l;
                        lvParent.ItemsSource = listParent;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void Call_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            StudentParentRelationship parent = item.BindingContext as StudentParentRelationship;

            await Call(parent.mobile_number);
        }

        public async Task Call(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (FeatureNotSupportedException ex)
            {
                SnackB.Message = AppResources.PhoneDialerNotSupportText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
            catch (Exception ex)
            {
                // Other error has occurred.  
            }
        }

        async void Message_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            StudentParentRelationship parent = item.BindingContext as StudentParentRelationship;
            int myPid = Settings.profileId;
            int receiverId = parent.profile_id;
            string channelId = string.Empty;
            if (myPid > receiverId)
            {
                channelId = "pid" + receiverId + "_pid" + myPid;
            }
            else
            {
                channelId = "pid" + myPid + "_pid" + receiverId;
            }
            await Navigation.PushAsync(new MessagePage(1, myPid, receiverId, parent.full_name,parent.photo_url, channelId));
        }

        async void OnUpdateClicked(object sender, EventArgs args)
        {
            if (txtClass.Text != _class_name)
            {
                if (conn.IsConnected() == true)
                {
                    try
                    {
                        ShowLoadingPopup();

                        var t = srvc.PostStaffEnrollStudentClass(_school_id, _student_id, _new_class_id, Settings.fullName);
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
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
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
            else {
                SnackB.Message = AppResources.PleaseSelectDifferClassText;
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
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}