using Newtonsoft.Json;
using Plugin.XamarinAppRating;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartSchoolsV2
{
    public partial class MainPage : ContentPage
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
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            var tapGestureRecognizerCart = new TapGestureRecognizer();
            tapGestureRecognizerCart.Tapped += (s, e) => 
            {
                    GetShoppingCartPage();
            };
            imgCart.GestureRecognizers.Add(tapGestureRecognizerCart);

            var tapGestureRecognizerNotify = new TapGestureRecognizer();
            tapGestureRecognizerNotify.Tapped += (s, e) =>
            {
                GetNotificationPage();
            };
            imgNotification.GestureRecognizers.Add(tapGestureRecognizerNotify);

            var tapGestureRecognizerTopup = new TapGestureRecognizer();
            tapGestureRecognizerTopup.Tapped += (s, e) =>
            {
                //DisplayAlert(AppResources.SorryText,AppResources.EWalletFunctionText,"OK");
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
            imgStudent.GestureRecognizers.Add(tapGestureRecognizer2);

            var tapGestureRecognizer3 = new TapGestureRecognizer();
            tapGestureRecognizer3.Tapped += (s, e) => {
                carouselView.Position = 3;
            };
            imgWallet.GestureRecognizers.Add(tapGestureRecognizer3);

            var tapGestureRecognizer4 = new TapGestureRecognizer();
            tapGestureRecognizer4.Tapped += (s, e) => {
                carouselView.Position = 4;
            };
            imgTrophy.GestureRecognizers.Add(tapGestureRecognizer4);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
 
            ParentProfile();

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
                StudentView.LoadParentStudentRelationship.Execute(null);
            }
            else if (carouselView.Position == 3)
            {
                WalletView.LoadParentStudentRelationship.Execute(null);
            }
            else if (carouselView.Position == 4)
            {
                ClubView.LoadClubRelationship.Execute(null);
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
                            await DisplayAlert("",AppResources.PleaseCompleteKYCText, "OK");

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

        public async void ParentProfile()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostParentProfile(Settings.profileId);
                    string jsonStr = await t;
                    ParentProfileProperty response = JsonConvert.DeserializeObject<ParentProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        string photo_url = string.Empty;
                        foreach (ParentProfile r in response.Data)
                        {
                            //Settings.parentId = r.parent_id;
                            Settings.walletId = r.wallet_id;
                            Settings.walletNumber = r.wallet_number;
                            lblAccountBalance.Text = "RM " + r.account_balance.ToString("F");
                            lblFullName.Text = r.full_name;
                            //lblUserRole.Text = "Parent";
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

                        await CartCount();
                    }
                    else
                    {
                        SnackB.Message = response.Message;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    SnackB.Message = AppResources.SomethingWrongText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally 
                {
                    IsBusy = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
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
                        else {
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
            await Navigation.PushAsync(new EditProfilePage("parent"));
        }

        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage());
        }

        //private async void OnChangePinClicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new VerifyAccountPage2(Settings.userName));
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
                imgStudent.Source = "ic_student_grey.png";
                imgWallet.Source = "ic_wallet_grey.png";
                imgTrophy.Source = "ic_trophy_grey.png";
            }
            else if (currentItemPosition == 1)
            {
                MessageView.LoadChatChannel.Execute(null);

                imgSchool.Source = "ic_school_grey.png";
                imgMessage.Source = "ic_chat.png";
                imgStudent.Source = "ic_student_grey.png";
                imgWallet.Source = "ic_wallet_grey.png";
                imgTrophy.Source = "ic_trophy_grey.png";
            }
            else if (currentItemPosition == 2)
            {
                StudentView.LoadParentStudentRelationship.Execute(null);

                imgSchool.Source = "ic_school_grey.png";
                imgMessage.Source = "ic_chat_grey.png";
                imgStudent.Source = "ic_student.png";
                imgWallet.Source = "ic_wallet_grey.png";
                imgTrophy.Source = "ic_trophy_grey.png";
            }
            else if (currentItemPosition == 3)
            {
                WalletView.LoadParentStudentRelationship.Execute(null);

                imgSchool.Source = "ic_school_grey.png";
                imgMessage.Source = "ic_chat_grey.png";
                imgStudent.Source = "ic_student_grey.png";
                imgWallet.Source = "ic_wallet.png";
                imgTrophy.Source = "ic_trophy_grey.png";
            }
            else if (currentItemPosition == 4)
            {
                ClubView.LoadClubRelationship.Execute(null);

                imgSchool.Source = "ic_school_grey.png";
                imgMessage.Source = "ic_chat_grey.png";
                imgStudent.Source = "ic_student_grey.png";
                imgWallet.Source = "ic_wallet_grey.png";
                imgTrophy.Source = "ic_trophy.png";
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
        //void OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        //{
        //    var previousItem = e.PreviousItem as View;
        //    var currentItem = e.CurrentItem as View;

        //    if (previousItem != null)
        //    {

        //    }
        //    else
        //    {
        //        school.LoadSchoolRelationship();
        //    }


        //}
    }
}
