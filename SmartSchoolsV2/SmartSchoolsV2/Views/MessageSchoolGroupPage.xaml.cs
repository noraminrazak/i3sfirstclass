using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageSchoolGroupPage : ContentPage
    {
        IFirebaseSubscribe subscribe = DependencyService.Get<IFirebaseSubscribe>();
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _user_role_id;
        public int _school_id;
        public int _staff_id;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public MessageSchoolGroupPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                SchoolInfo();
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex.Message);
            }
        }

        public async Task SchoolInfo()
        {
            int school_id = Settings.schoolId;
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvClass1.ItemsSource = null;
                    var t = srvc.PostSchoolInfo(school_id);
                    string jsonStr = await t;
                    SchoolInfoProperty response = JsonConvert.DeserializeObject<SchoolInfoProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<SchoolInfo> list = new List<SchoolInfo>();
                        foreach (SchoolInfo sl in response.Data)
                        {
                            SchoolInfo post = new SchoolInfo();
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            post.school_code = sl.school_code;
                            post.school_type_id = sl.school_type_id;
                            post.school_type = sl.school_type;
                            post.state_name = sl.state_name;
                            post.city = sl.city;
                            post.total_staff = sl.total_staff + AppResources.SpaceStaffText;
                            list.Add(post);

                            //subscribe.SubscribeToTopic("schools" + sl.school_id);
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
                        lvClass1.Footer = l;
                        lvClass1.ItemsSource = list;
                    }
                    else
                    {
                        List<SchoolInfo> list = new List<SchoolInfo>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvClass1.Footer = l;
                        lvClass1.ItemsSource = list;
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
        public SchoolInfo school;
        async void OnSchoolSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as SchoolInfo;
            if (data == null) return;
            school = data;

            if (school.school_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...

                bool answer = await DisplayAlert(AppResources.ChannelText + data.school_name, AppResources.DoYouReallyWantToJoinChannelText, AppResources.YesText, AppResources.CancelText);
                if (answer == true)
                {

                    await UserJoinChannel(data);
                }
                //string channelId = "sid" + data.school_id;
                ((ListView)sender).SelectedItem = null;
            }
        }

        async Task UserJoinChannel(SchoolInfo value) 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    string channelId = "sid" + value.school_id;

                    var t = srvc.PostChatJoinChannel(channelId, Settings.profileId, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await Navigation.PushAsync(new MessagePage(2, Settings.profileId, value.school_id, value.school_name + "_" + value.city, string.Empty, channelId));
                    }
                    else
                    {
                        bool answer = await DisplayAlert("", response.Message + AppResources.LoginToChannelNowText, AppResources.YesText, AppResources.CancelText);
                        if (answer == true)
                        {
                            await Navigation.PushAsync(new MessagePage(2, Settings.profileId, value.school_id, value.school_name + "_" + value.city, string.Empty, channelId));
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
    }
}