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
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageClubGroupPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        IFirebaseSubscribe subscribe = DependencyService.Get<IFirebaseSubscribe>();
        public ObservableCollection<ClubRelationship> listClub { get; set; }
        public static Command LoadClubRelationship { get; set; }
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _school_id;
        public int _staff_id;
        public int _parent_id;
        public int _merchant_id;
        public int _user_role_id;
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

        public MessageClubGroupPage()
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
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        public async Task GetUserRole()
        {
            _user_role_id = Settings.userRoleId;

            if (_user_role_id == 9)
            {
                await ParentClubRelationship();
            }
            else if (_user_role_id == 8)
            {
                await StaffClubRelationship();
            }
            else if (_user_role_id == 7)
            {
                await MerchantClubRelationship();
            }
        }

        public async Task StaffClubRelationship()
        {
            _school_id = Settings.schoolId;
            _staff_id = Settings.staffId;
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvClub.ItemsSource = null;
                    var t = srvc.PostStaffClubRelationship(_school_id, _staff_id);
                    string jsonStr = await t;
                    ClubRelationshipProperty response = JsonConvert.DeserializeObject<ClubRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listClub = new ObservableCollection<ClubRelationship>();
                        foreach (ClubRelationship sl in response.Data)
                        {
                            ClubRelationship post = new ClubRelationship();
                            post.relationship_id = sl.relationship_id;
                            post.club_id = sl.club_id;
                            post.club_name = sl.club_name;
                            post.school_id = sl.school_id;
                            post.create_by_staff_id = sl.create_by_staff_id;
                            post.school_name = sl.school_name;
                            post.school_type = sl.school_type;
                            post.total_member = sl.total_member + AppResources.MemberText;
                            listClub.Add(post);

                            //subscribe.SubscribeToTopic("schools" + sl.school_id + "clubs" + sl.club_id);
                        }
                        RowCount = listClub.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvClub.Footer = l;
                        lvClub.ItemsSource = listClub;
                    }
                    else
                    {
                        listClub = new ObservableCollection<ClubRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvClub.Footer = l;
                        lvClub.ItemsSource = listClub;
                    }
                    //IsBusy = false;
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
                //HideLoadingPopup();
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task MerchantClubRelationship()
        {
            _merchant_id = Settings.merchantId;

            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostMerchantClubRelationship(_merchant_id);
                    string jsonStr = await t;
                    ClubRelationshipProperty response = JsonConvert.DeserializeObject<ClubRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listClub = new ObservableCollection<ClubRelationship>();
                        foreach (ClubRelationship sl in response.Data)
                        {
                            ClubRelationship post = new ClubRelationship();
                            post.relationship_id = sl.relationship_id;
                            post.club_id = sl.club_id;
                            post.club_name = sl.club_name;
                            post.school_id = sl.school_id;
                            post.create_by_staff_id = sl.create_by_staff_id;
                            post.school_name = sl.school_name;
                            post.school_type = sl.school_type;
                            post.total_member = sl.total_member + AppResources.MemberText;
                            listClub.Add(post);
                        }
                        RowCount = listClub.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvClub.Footer = l;
                        lvClub.ItemsSource = listClub;
                    }
                    else
                    {
                        listClub = new ObservableCollection<ClubRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvClub.Footer = l;
                        lvClub.ItemsSource = listClub;
                    }
                    //IsBusy = false;
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
                //HideLoadingPopup();
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task ParentClubRelationship()
        {
            _parent_id = Settings.parentId;

            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostParentClubRelationship(_parent_id);
                    string jsonStr = await t;
                    ClubRelationshipProperty response = JsonConvert.DeserializeObject<ClubRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listClub = new ObservableCollection<ClubRelationship>();
                        foreach (ClubRelationship sl in response.Data)
                        {
                            ClubRelationship post = new ClubRelationship();
                            post.relationship_id = sl.relationship_id;
                            post.club_id = sl.club_id;
                            post.club_name = sl.club_name;
                            post.school_id = sl.school_id;
                            post.create_by_staff_id = sl.create_by_staff_id;
                            post.school_name = sl.school_name;
                            post.school_type = sl.school_type;
                            post.total_member = sl.total_member + AppResources.MemberText;
                            listClub.Add(post);
                        }
                        RowCount = listClub.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvClub.Footer = l;
                        lvClub.ItemsSource = listClub;
                    }
                    else
                    {
                        listClub = new ObservableCollection<ClubRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvClub.Footer = l;
                        lvClub.ItemsSource = listClub;
                    }
                    //IsBusy = false;
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
                //HideLoadingPopup();
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public ClubRelationship value;
        async void OnClubSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as ClubRelationship;
            if (data == null) return;
            value = data;

            if (value.relationship_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                ////Do stuff here with the SelectedItem ...
                bool answer = await DisplayAlert(AppResources.ChannelText + data.club_name, AppResources.DoYouReallyWantToJoinChannelText, AppResources.YesText, AppResources.CancelText);
                if (answer == true)
                {
                    await MemberJoinChannel(data);
                }

                ((ListView)sender).SelectedItem = null;
            }
        }

        async Task MemberJoinChannel(ClubRelationship value)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    string channelId = "sid" + value.school_id + "_cbid" + value.club_id;

                    var t = srvc.PostChatJoinChannel(channelId, Settings.profileId, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await Navigation.PushAsync(new MessagePage(4, Settings.profileId, value.club_id, value.club_name + "_" + value.school_name, string.Empty, channelId));
                    }
                    else
                    {
                        bool answer = await DisplayAlert("", response.Message + AppResources.LoginToChannelNowText, AppResources.YesText, AppResources.CancelText);
                        if (answer == true)
                        {
                            await Navigation.PushAsync(new MessagePage(4, Settings.profileId, value.club_id, value.club_name + "_" + value.school_name, string.Empty, channelId));
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
    }
}