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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageClassGroupPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _user_role_id;
        public int _school_id;
        public int _staff_id;
        public int RowCount { get; set; }
        public static Label l = new Label();
        IFirebaseSubscribe subscribe = DependencyService.Get<IFirebaseSubscribe>();
        public ObservableCollection<ParentStudentRelationship> listStudent { get; set; }
        public MessageClassGroupPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                GetUserRole();
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex.Message);
            }
        }
        public async Task GetUserRole()
        {
            _user_role_id = Settings.userRoleId;

            if (_user_role_id == 9)
            {
                await ContactStudentClass();
            }
            else if (_user_role_id == 8)
            {
                await StaffClassRelationship();
            }
        }
        public async Task ContactStudentClass()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvClass.ItemsSource = null;
                    var t = srvc.PostContactStudentClass(Settings.parentId);
                    string jsonStr = await t;
                    ParentStudentRelationshipProperty response = JsonConvert.DeserializeObject<ParentStudentRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listStudent = new ObservableCollection<ParentStudentRelationship>();
                        foreach (ParentStudentRelationship sl in response.Data)
                        {
                            ParentStudentRelationship post = new ParentStudentRelationship();
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            post.image_visible = false;
                            post.initial_visible = true;
                            if (sl.class_id > 0)
                            {
                                post.class_id = sl.class_id;
                                post.class_name = sl.class_name;
                                post.dot_visible = true;
                            }
                            else
                            {
                                post.dot_visible = false;
                            }
                            listStudent.Add(post);

                            //subscribe.SubscribeToTopic("schools" + sl.school_id + "class" + sl.class_id);
                        }
                        RowCount = listStudent.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvClass.Footer = l;
                        lvClass.ItemsSource = listStudent;
                    }
                    else
                    {
                        listStudent = new ObservableCollection<ParentStudentRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvClass.Footer = l;
                        lvClass.ItemsSource = listStudent;
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

        public async Task StaffClassRelationship()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostStaffClassRelationship(Settings.schoolId, Settings.staffId);
                    string jsonStr = await t;
                    StaffClassRelationshipProperty response = JsonConvert.DeserializeObject<StaffClassRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<StaffClassRelationship> list = new List<StaffClassRelationship>();
                        foreach (StaffClassRelationship sl in response.Data)
                        {
                            StaffClassRelationship post = new StaffClassRelationship();
                            post.relationship_id = sl.relationship_id;
                            post.class_id = sl.class_id;
                            post.class_name = sl.class_name;
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            post.school_type = sl.school_type;
                            post.session_code = sl.session_code;
                            post.total_student = sl.total_student + AppResources.SpaceStudentText;
                            list.Add(post);
                        }
                        RowCount = list.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvClass.Footer = l;
                        lvClass.ItemsSource = list;
                    }
                    else
                    {
                        List<StaffClassRelationship> list = new List<StaffClassRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvClass.Footer = l;
                        lvClass.ItemsSource = list;
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

        public StaffClassRelationship classroom;

        public ParentStudentRelationship student;
        async void OnClassSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (_user_role_id == 8)
            {
                var data = e.SelectedItem as StaffClassRelationship;
                if (data == null) return;
                classroom = data;

                if (classroom.class_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    bool answer = await DisplayAlert(AppResources.ChannelText + data.class_name, AppResources.DoYouReallyWantToJoinChannelText, AppResources.YesText, AppResources.CancelText);
                    if (answer == true)
                    {
                        await StaffJoinChannel(data);
                    }

                    ((ListView)sender).SelectedItem = null;
                }
            }
            else if (_user_role_id == 9)
            {
                var data = e.SelectedItem as ParentStudentRelationship;
                if (data == null) return;
                student = data;

                if (student.class_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    bool answer = await DisplayAlert(AppResources.ChannelText + data.class_name, AppResources.DoYouReallyWantToJoinChannelText, AppResources.YesText, AppResources.CancelText);
                    if (answer == true)
                    {
                        await ParentJoinChannel(data);
                    }

                    ((ListView)sender).SelectedItem = null;
                }
            }
        }

        async Task StaffJoinChannel(StaffClassRelationship value)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    string channelId = "sid" + value.school_id + "_csid" + value.class_id;

                    var t = srvc.PostChatJoinChannel(channelId, Settings.profileId, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await Navigation.PushAsync(new MessagePage(3, Settings.profileId, value.class_id, value.class_name + "_" + value.school_name, string.Empty, channelId));
                    }
                    else
                    {
                        bool answer = await DisplayAlert("", response.Message + AppResources.LoginToChannelNowText, AppResources.YesText, AppResources.CancelText);
                        if (answer == true)
                        {
                            await Navigation.PushAsync(new MessagePage(3, Settings.profileId, value.class_id, value.class_name + "_" + value.school_name, string.Empty, channelId));
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    HideLoadingPopup();
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }

        }

        async Task ParentJoinChannel(ParentStudentRelationship value)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    string channelId = "sid" + value.school_id + "_csid" + value.class_id;

                    var t = srvc.PostChatJoinChannel(channelId, Settings.profileId, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await Navigation.PushAsync(new MessagePage(3, Settings.profileId, value.class_id, value.class_name + "_" + value.school_name, string.Empty, channelId));
                    }
                    else
                    {
                        bool answer = await DisplayAlert("", response.Message + AppResources.LoginToChannelNowText, AppResources.YesText, AppResources.CancelText);
                        if (answer == true)
                        {
                            await Navigation.PushAsync(new MessagePage(3, Settings.profileId, value.class_id, value.class_name + "_" + value.school_name, string.Empty, channelId));
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    HideLoadingPopup();
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
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
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
    }
}