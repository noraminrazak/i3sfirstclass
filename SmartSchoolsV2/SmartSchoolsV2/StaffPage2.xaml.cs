using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using Plugin.XamarinAppRating;
using SmartSchoolsV2.Resources;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Extensions;

namespace SmartSchoolsV2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffPage2 : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public ObservableCollection<UserNotify> listNotify { get; set; }
        public int _notifyCount = 0;
        public int _account_status_id;
        public string _account_status;
        public int _kyc_status_id;
        public string _kyc_status;
        public string _mpay_uid;
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
        public StaffPage2()
        {
            InitializeComponent();
            BindingContext = this;

            var tapGestureRecognizerCart = new TapGestureRecognizer();
            tapGestureRecognizerCart.Tapped += (s, e) =>
            {
                GetShoppingCartPage();
            };
            imgCart.GestureRecognizers.Add(tapGestureRecognizerCart);

            //carouselView.PositionChanged += OnPositionChanged;

            var tapGestureRecognizerNotify = new TapGestureRecognizer();
            tapGestureRecognizerNotify.Tapped += (s, e) =>
            {
                GetNotificationPage();
            };
            imgNotification.GestureRecognizers.Add(tapGestureRecognizerNotify);

            var tapGestureRecognizerTopup = new TapGestureRecognizer();
            tapGestureRecognizerTopup.Tapped += (s, e) =>
            {
                //DisplayAlert(AppResources.SorryText, AppResources.EWalletFunctionText, "OK");
                //GetTopupPage();
                AccountStatus();
            };
            imgTopup.GestureRecognizers.Add(tapGestureRecognizerTopup);

            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) =>
            {
                GetTxnHistoryPage();
            };
            lblAccountBalance.GestureRecognizers.Add(tapGestureRecognizerTxn);

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
            imgClass.GestureRecognizers.Add(tapGestureRecognizer2);

            var tapGestureRecognizer3 = new TapGestureRecognizer();
            tapGestureRecognizer3.Tapped += (s, e) => {
                carouselView.Position = 3;
            };
            imgTrophy.GestureRecognizers.Add(tapGestureRecognizer3);

            var tapGestureRecognizer4 = new TapGestureRecognizer();
            tapGestureRecognizer4.Tapped += (s, e) => {
                carouselView.Position = 4;
            };
            imgCalendar.GestureRecognizers.Add(tapGestureRecognizer4);

            var tapGestureRecognizer5 = new TapGestureRecognizer();
            tapGestureRecognizer5.Tapped += (s, e) => {
                carouselView.Position = 5;
            };
            imgOuting.GestureRecognizers.Add(tapGestureRecognizer5);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            StaffProfile();

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
                ClassView1.LoadSchoolInfo.Execute(null);
            }
            else if (carouselView.Position == 3)
            {
                ClubView.LoadClubRelationship.Execute(null);
            }
            else if (carouselView.Position == 4)
            {
                StaffAttendanceView.LoadStaffAttendanceMonthly.Execute(null);
            }
            else if (carouselView.Position == 5)
            {
                StaffOutingRequestView.LoadStaffOutingRequest.Execute(null);
            }
        }

        private async void GetTopupPage()
        {
            await Navigation.PushAsync(new TopupPage());
        }

        public async void AccountStatus()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostAccountStatus(Settings.profileId, Settings.walletNumber);
                    string jsonStr = await t;
                    AccStatusProperty response = JsonConvert.DeserializeObject<AccStatusProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        foreach (AccStatus r in response.Data)
                        {
                            _mpay_uid = r.mpay_uid;
                            _account_status_id = Convert.ToInt32(r.account_status_id);
                            _account_status = r.account_status;
                            _kyc_status_id = Convert.ToInt32(r.kyc_status_id);
                            _kyc_status = r.kyc_status;
                        }

                        if (_mpay_uid != "0")
                        {
                            if (_kyc_status_id != 11)
                            {
                                await DisplayAlert(AppResources.SorryText, AppResources.CantTopupKYCStatusText, "OK");
                            }
                            else
                            {
                                if (_account_status_id != 1)
                                {
                                    await DisplayAlert(AppResources.SorryText, AppResources.CantTopupAccStatusText, "OK");
                                }
                                else
                                {
                                    GetTopupPage();
                                }
                            }
                        }
                        else
                        {
                            await DisplayAlert("", AppResources.PleaseCompleteKYCText, "OK");

                            await Navigation.PushAsync(new AccountStatusPage());
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

        public async void StaffProfile()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    //ShowLoadingPopup();
                    var t = srvc.PostStaffProfile(Settings.profileId);
                    string jsonStr = await t;
                    StaffProfileProperty response = JsonConvert.DeserializeObject<StaffProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        string photo_url = string.Empty;
                        foreach (StaffProfile r in response.Data)
                        {
                            //Settings.staffId = r.staff_id;
                            Settings.walletId = r.wallet_id;
                            Settings.walletNumber = r.wallet_number;
                            lblAccountBalance.Text = "RM " + r.account_balance.ToString("F");
                            lblFullName.Text = r.full_name;
                            photo_url = r.photo_url;
                        }

                        int staff_type_id = Settings.staffTypeId;
                        if (staff_type_id == 1)
                        {
                            lblStaffType.Text = AppResources.HeadmasterText;
                        }
                        else if (staff_type_id == 2)
                        {
                            lblStaffType.Text = AppResources.AssistantHeadText;
                        }
                        else if (staff_type_id == 3)
                        {
                            lblStaffType.Text = AppResources.TeacherText;
                        }
                        else if (staff_type_id == 4)
                        {
                            lblStaffType.Text = AppResources.StaffText;
                        }
                        else if (staff_type_id == 5)
                        {
                            lblStaffType.Text = AppResources.TeacherWardenText;
                        }
                        else if (staff_type_id == 6)
                        {
                            lblStaffType.Text = AppResources.SchoolAdminText;
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

                        await CartCount();
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
                //HideLoadingPopup();
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }

        }
        public async Task CartCount()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostPurchaseCartCount(Settings.profileId, Settings.walletId);
                    string jsonStr = await t;
                    CartProperty response = JsonConvert.DeserializeObject<CartProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        if (response.Total > 0)
                        {
                            badgeCart.IsVisible = true;
                            badgeCart.Text = response.Total.ToString();
                        }
                        else
                        {
                            badgeCart.IsVisible = false;
                            badgeCart.Text = string.Empty;
                        }

                        await NotifyCount();
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
        private async void GetShoppingCartPage()
        {
            Settings.cartMode = "all";
            await Navigation.PushAsync(new ShoppingCartPage());
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
            await Navigation.PushAsync(new EditProfilePage("staff"));
        }

        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage());
        }
        async void OnDailyLimitClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new StaffCardDailyLimitPage());
        }
        //async void OnCardLostLimitClicked(object sender, EventArgs args)
        //{
        //    await Navigation.PushAsync(new StaffCardLostReportPage());
        //}
        //async void OnCardReplacementClicked(object sender, EventArgs args)
        //{
        //    await Navigation.PushAsync(new StaffCardReplacementPage());
        //}

        //private async void OnFAQClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new FAQPage());
        //}

        //private async void OnFeedbackClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new FeedbackPage());
        //}

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
            int currentItemPosition = e.CurrentPosition;

            if (currentItemPosition == 0)
            {
                SchoolView.LoadSchoolPost.Execute(null);

                imgSchool.Source = "ic_school.png";
                imgMessage.Source = "ic_chat_grey.png";
                imgClass.Source = "ic_classroom_grey.png";
                imgTrophy.Source = "ic_trophy_grey.png";
                imgCalendar.Source = "ic_schedule_grey.png";
                imgOuting.Source = "ic_wayfinding_grey.png";
            }
            else if (currentItemPosition == 1)
            {
                MessageView.LoadChatChannel.Execute(null);

                imgSchool.Source = "ic_school_grey.png";
                imgMessage.Source = "ic_chat.png";
                imgClass.Source = "ic_classroom_grey.png";
                imgTrophy.Source = "ic_trophy_grey.png";
                imgCalendar.Source = "ic_schedule_grey.png";
                imgOuting.Source = "ic_wayfinding_grey.png";
            }
            else if (currentItemPosition == 2)
            {
                ClassView1.LoadSchoolInfo.Execute(null);

                imgSchool.Source = "ic_school_grey.png";
                imgMessage.Source = "ic_chat_grey.png";
                imgClass.Source = "ic_classroom.png";
                imgTrophy.Source = "ic_trophy_grey.png";
                imgCalendar.Source = "ic_schedule_grey.png";
                imgOuting.Source = "ic_wayfinding_grey.png";
            }
            else if (currentItemPosition == 3)
            {
                ClubView.LoadClubRelationship.Execute(null);

                imgSchool.Source = "ic_school_grey.png";
                imgMessage.Source = "ic_chat_grey.png";
                imgClass.Source = "ic_classroom_grey.png";
                imgTrophy.Source = "ic_trophy.png";
                imgCalendar.Source = "ic_schedule_grey.png";
                imgOuting.Source = "ic_wayfinding_grey.png";
            }
            else if (currentItemPosition == 4)
            {
                StaffAttendanceView.LoadStaffAttendanceMonthly.Execute(null);

                imgSchool.Source = "ic_school_grey.png";
                imgMessage.Source = "ic_chat_grey.png";
                imgClass.Source = "ic_classroom_grey.png";
                imgTrophy.Source = "ic_trophy_grey.png";
                imgCalendar.Source = "ic_schedule.png";
                imgOuting.Source = "ic_wayfinding_grey.png";
            }
            else if (currentItemPosition == 5)
            {
                StaffOutingRequestView.LoadStaffOutingRequest.Execute(null);

                imgSchool.Source = "ic_school_grey.png";
                imgMessage.Source = "ic_chat_grey.png";
                imgClass.Source = "ic_classroom_grey.png";
                imgTrophy.Source = "ic_trophy_grey.png";
                imgCalendar.Source = "ic_schedule_grey.png";
                imgOuting.Source = "ic_wayfinding.png";
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
    }
}