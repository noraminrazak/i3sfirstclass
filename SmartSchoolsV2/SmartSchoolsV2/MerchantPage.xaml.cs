using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Views;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.XamarinAppRating;
using SmartSchoolsV2.Resources;
using System.Collections.ObjectModel;

namespace SmartSchoolsV2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MerchantPage : ContentPage, INotifyPropertyChanged
	{

        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public ObservableCollection<UserNotify> listNotify { get; set; }
        public int _notifyCount = 0;

        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
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
                GetLoginPage();
            }
        }

        public MerchantPage ()
		{
            InitializeComponent();

            var smallSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            var microSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label));

            var tapGestureRecognizerNotify = new TapGestureRecognizer();
            tapGestureRecognizerNotify.Tapped += (s, e) =>
            {
                GetNotificationPage();
            };
            imgNotification.GestureRecognizers.Add(tapGestureRecognizerNotify);

            var tapGestureRecognizer0 = new TapGestureRecognizer();
            tapGestureRecognizer0.Tapped += (s, e) => {
                carouselView.Position = 0;
            };
            imgSchool.GestureRecognizers.Add(tapGestureRecognizer0);

            var tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += (s, e) => {
                carouselView.Position = 1;
            };
            imgMessage.GestureRecognizers.Add(tapGestureRecognizer1);

            var tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += (s, e) => {
                carouselView.Position = 2;
            };
            imgTrophy.GestureRecognizers.Add(tapGestureRecognizer2);

            var tapGestureRecognizer3 = new TapGestureRecognizer();
            tapGestureRecognizer3.Tapped += (s, e) => {
                carouselView.Position = 3;
            };
            imgStore.GestureRecognizers.Add(tapGestureRecognizer3);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            MerchantProfile();

            await GetLastLogin();

            if (carouselView.Position == 0)
            {
                SchoolView.LoadSchoolPost.Execute(null);
            }
            else if (carouselView.Position == 1)
            {
                MessageView.LoadChatChannel.Execute(null);
            }
            else if (carouselView.Position == 2)
            {
                ClubView.LoadClubRelationship.Execute(null);
            }
            else if (carouselView.Position == 3)
            {
                MerchantStoreView.LoadMerchantSchoolRelationship.Execute(null);
            }
        }

        public async void MerchantProfile()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostMerchantProfile(Settings.profileId);
                    string jsonStr = await t;
                    MerchantProfileProperty response = JsonConvert.DeserializeObject<MerchantProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        string photo_url = string.Empty;
                        foreach (MerchantProfile r in response.Data)
                        {
                            Settings.merchantId = r.merchant_id;
                            lblFullName.Text = r.full_name;
                            lblCompanyName.Text = r.company_name + " (" + r.registration_number + ")";
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

                        await NotifyCount();
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

        public async Task NotifyCount()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostUserNotify(Settings.profileId);
                    string jsonStr = await t;
                    UserNotifyProperty response = JsonConvert.DeserializeObject<UserNotifyProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listNotify = new ObservableCollection<UserNotify>();
                        if (response.Data.Count > 0)
                        {
                            _notifyCount = 0;
                            foreach (UserNotify sl in response.Data)
                            {
                                UserNotify post = new UserNotify();
                                if (sl.read_flag == "N")
                                {
                                    _notifyCount++;
                                }
                            }

                            if (_notifyCount > 0)
                            {
                                badgeNotification.IsVisible = true;
                                badgeNotification.Text = _notifyCount.ToString();
                            }
                            else
                            {
                                badgeNotification.IsVisible = false;
                                badgeNotification.Text = string.Empty;
                            }
                        }
                        else
                        {
                            badgeNotification.IsVisible = false;
                            badgeNotification.Text = string.Empty;
                        }
                    }
                    else
                    {
                        //SnackB.Message = response.Message;
                        //SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {

                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        private async void GetNotificationPage()
        {
            await Navigation.PushAsync(new NotificationPage());
        }

        private async void GetTxnHistoryPage()
        {
            await Navigation.PushAsync(new TransactionHistoryPage());
        }

        private void OnSwitchRoleClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new SelectRolePage(Settings.userName));
        }

        private async void OnEditProfileClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditProfilePage("merchant"));
        }

        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage());
        }

        private async void OnHelpClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HelpCenterPage1());
        }
        private async void OnTnCClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TermsConditionsPage());
        }

        private Task RateApplicationOnStore()
        {
            if (CrossAppRating.IsSupported)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // This method use Facebook™'s store apps as example.
                    await CrossAppRating.Current.PerformRatingOnStoreAsync(packageName: Settings.androidPackageName, applicationId: Settings.iOSApplicationId, productId: Settings.uwpProductId);
                });

                //Preferences.Set("application_rated", true);
            }

            return Task.CompletedTask;
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var page = new LogoutPopupPage(Settings.userRoleId);
            page.DetailSet += this.OnDetailSet;
            await PopupNavigation.Instance.PushAsync(page);
        }
        public static Page GetLoginPage()
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage1())
            {
                BarBackgroundColor = Color.FromHex("#5E625A"),
                BarTextColor = Color.FromHex("#FFD612")
            };
            return Application.Current.MainPage;
        }

        void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            int previousItemPosition = e.PreviousPosition;
            int currentItemPosition = e.CurrentPosition;

            if (currentItemPosition == 0)
            {
                SchoolView.LoadSchoolPost.Execute(null);

                //lblSchool.TextColor = Color.Orange;
                //lblMessage.TextColor = Color.Gainsboro;
                //lblClub.TextColor = Color.Gainsboro;
                //lblStore.TextColor = Color.Gainsboro;
                //imgSchool.WidthRequest = 28;
                //imgSchool.HeightRequest = 28;
                //imgMessage.WidthRequest = 24;
                //imgMessage.HeightRequest = 24;
                //imgTrophy.WidthRequest = 24;
                //imgTrophy.HeightRequest = 24;
                //imgStore.WidthRequest = 24;
                //imgStore.HeightRequest = 24;
                imgSchool.Source = "ic_school.png";
                imgMessage.Source = "ic_chat_grey.png";
                imgTrophy.Source = "ic_trophy_grey.png";
                imgStore.Source = "ic_shop_grey.png";
            }
            else if (currentItemPosition == 1)
            {
                MessageView.LoadChatChannel.Execute(null);
                //lblSchool.TextColor = Color.Gainsboro;
                //lblMessage.TextColor = Color.Orange;
                //lblClub.TextColor = Color.Gainsboro;
                //lblStore.TextColor = Color.Gainsboro;
                //imgSchool.WidthRequest = 24;
                //imgSchool.HeightRequest = 24;
                //imgMessage.WidthRequest = 28;
                //imgMessage.HeightRequest = 28;
                //imgTrophy.WidthRequest = 24;
                //imgTrophy.HeightRequest = 24;
                //imgStore.WidthRequest = 24;
                //imgStore.HeightRequest = 24;
                imgSchool.Source = "ic_school_grey.png";
                imgMessage.Source = "ic_chat.png";
                imgTrophy.Source = "ic_trophy_grey.png";
                imgStore.Source = "ic_shop_grey.png";
            }
            else if (currentItemPosition == 2)
            {

                ClubView.LoadClubRelationship.Execute(null);

                //lblSchool.TextColor = Color.Gainsboro;
                //lblMessage.TextColor = Color.Gainsboro;
                //lblClub.TextColor = Color.Orange;
                //lblStore.TextColor = Color.Gainsboro;
                //imgSchool.WidthRequest = 24;
                //imgSchool.HeightRequest = 24;
                //imgMessage.WidthRequest = 24;
                //imgMessage.HeightRequest = 24;
                //imgTrophy.WidthRequest = 28;
                //imgTrophy.HeightRequest = 28;
                //imgStore.WidthRequest = 24;
                //imgStore.HeightRequest = 24;
                imgSchool.Source = "ic_school_grey.png";
                imgMessage.Source = "ic_chat_grey.png";
                imgTrophy.Source = "ic_trophy.png";
                imgStore.Source = "ic_shop_grey.png";
            }
            else if (currentItemPosition == 3)
            {

                MerchantStoreView.LoadMerchantSchoolRelationship.Execute(null);

                //lblSchool.TextColor = Color.Gainsboro;
                //lblMessage.TextColor = Color.Gainsboro;
                //lblClub.TextColor = Color.Gainsboro;
                //lblStore.TextColor = Color.Orange;
                //imgSchool.WidthRequest = 24;
                //imgSchool.HeightRequest = 24;
                //imgMessage.WidthRequest = 24;
                //imgMessage.HeightRequest = 24;
                //imgTrophy.WidthRequest = 24;
                //imgTrophy.HeightRequest = 24;
                //imgStore.WidthRequest = 28;
                //imgStore.HeightRequest = 28;
                imgSchool.Source = "ic_school_grey.png";
                imgMessage.Source = "ic_chat_grey.png";
                imgTrophy.Source = "ic_trophy_grey.png";
                imgStore.Source = "ic_shop.png";
            }
        }

        public async Task GetLastLogin()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostUserLastLogin(Settings.userName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        if (!string.IsNullOrEmpty(Settings.lastLogin) && !string.IsNullOrEmpty(response.Message))
                        {
                            if (Settings.lastLogin != response.Message)
                            {
                                Settings.isLogin = false;
                                Settings.profileId = 0;
                                Settings.fullName = string.Empty;
                                Settings.userName = string.Empty;
                                Settings.userRoleId = 0;
                                Settings.parentId = 0;
                                Settings.staffId = 0;
                                Settings.merchantId = 0;
                                Settings.schoolId = 0;
                                Settings.classId = 0;
                                Settings.className = string.Empty;
                                Settings.sessionCode = string.Empty;
                                Settings.lastLogin = string.Empty;
                                Application.Current.MainPage = new NavigationPage(new LoginPage1())
                                {
                                    BarBackgroundColor = Color.FromHex("#5F625B"),
                                    BarTextColor = Color.FromHex("#FFD612")
                                };

                                await DisplayAlert(AppResources.SorryText, AppResources.YouHaveBeenLogoutText, "OK");
                            }
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
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}