using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage1 : ContentPage
    {
        readonly Connection conn = new Connection();
        readonly ServiceWrapper srvc = new ServiceWrapper();
        public int _remember_me;
        public ICommand TapRegister => new Command(async () => await Navigation.PushAsync(new RegisterAccountPage(9, "")));
        //public ICommand TapRegister => new Command(async () => await Navigation.PushAsync(new EdgeDetectionPage()));
        //public ICommand TapRegister => new Command(() => GetRegistrationNotice());
        public LoginPage1 ()
		{
			InitializeComponent ();
            BindingContext = this;
        }

        public async void GetRegistrationNotice() 
        {
            await DisplayAlert(AppResources.SorryText, AppResources.RegisterNotAvailableText, "OK");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _remember_me = Settings.rememberMe;
            if (_remember_me == 0)
            {
                rememberChk.IsChecked = false;
                txtUsername.Text = "";
            }
            else
            {
                rememberChk.IsChecked = true;
                txtUsername.Text = Settings.userName;
            }

            CheckVersion();

            flagLanguage.WidthRequest = 35;
            flagLanguage.HeightRequest = 35;

            if (Settings.cultureInfo == "en-US")
            {
                flagLanguage.Source = (Device.RuntimePlatform == Device.Android) ? "malaysia_flag.png" : "malaysia.png";

            }
            else if (Settings.cultureInfo == "ms-MY")
            {
                flagLanguage.Source = (Device.RuntimePlatform == Device.Android) ? "uk_flag.png" : "uk.png";
            }
            else
            {
                flagLanguage.Source = (Device.RuntimePlatform == Device.Android) ? "malaysia_flag.png" : "malaysia.png";
            }
        }
        void OnNextClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtUsername.Text))
            {
                Settings.userName = txtUsername.Text.Trim();
                UserInitLogin();
            }
            else 
            {
                SnackB.Message = AppResources.UsernameRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }
        public async void CheckVersion() 
        {
            string _platform = string.Empty;
            string _version = string.Empty;
            string _build = string.Empty;
            string _release = string.Empty;

            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostLookupPlatformVersion(Settings.devicePlatform);
                    string jsonStr = await t;
                    PlatformVersionProperty response = JsonConvert.DeserializeObject<PlatformVersionProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        foreach (PlatformVersion sl in response.Data)
                        {
                            _platform = sl.platform_name;
                            _version = sl.version_number;
                            _build = sl.build_number;
                            _release = sl.release;
                        }

                        if (_release == "Y") 
                        {
                            if (_version != Settings.currentVersion)
                            {
                                string url = "";
                                bool result = await DisplayAlert(AppResources.UpdateAvailableText, AppResources.PleaseUpdateText, AppResources.UpdateButtonText, AppResources.CancelText);

                                if (result == true)
                                {
                                    if (Device.RuntimePlatform == Device.Android)
                                    {
                                        url = "https://play.google.com/store/apps/details?id=com.emerging.smartschool";
                                        await Browser.OpenAsync(url, BrowserLaunchMode.External);
                                    }
                                    else if (Device.RuntimePlatform == Device.iOS)
                                    {
                                        //var location = RegionInfo.CurrentRegion.Name.ToLower();
                                        url = "https://apps.apple.com/my/app/i-3s-first-class/id1378062742";
                                        await Browser.OpenAsync(url, BrowserLaunchMode.External);
                                    }
                                }
                            }
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
                    HideLoadingPopup();
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }
        public async void UserInitLogin()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostUserInitLogin(txtUsername.Text.Trim());
                    string jsonStr = await t;
                    ResponseProperty response = JsonConvert.DeserializeObject<ResponseProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        if (response.Code == "auth_login")
                        {
                            await Navigation.PushAsync(new LoginPage2(txtUsername.Text.Trim()));
                        }
                        else if (response.Code == "create_password")
                        {
                            await Navigation.PushAsync(new CreatePasswordPage(txtUsername.Text.Trim()));
                        }
                        else if (response.Code == "register_account")
                        {
                            var code = await RegisterCheckStaff(8);

                            if (code == "auth_login")
                            {
                                await Navigation.PushAsync(new LoginPage2(txtUsername.Text.Trim()));
                            }
                            else if (code == "register_account")
                            {
                                await Navigation.PushAsync(new RegisterAccountPage(8, txtUsername.Text.Trim()));
                            }
                            else if (code == "no_record_found")
                            {
                                SnackB.Message = response.Message;
                                SnackB.IsOpen = !SnackB.IsOpen;
                            }
                            //await Navigation.PushAsync(new RegisterPage(8, txtUsername.Text.Trim()));
                        }
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
                    HideLoadingPopup();
                }

            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }

        }

        public async Task<string> RegisterCheckStaff(int user_role_id)
        {
            string code = string.Empty;
            if (conn.IsConnected() == true)
            {
                try
                {
                    //ShowLoadingPopup();
                    var t = srvc.PostAccountRegisterCheck(txtUsername.Text.Trim(), user_role_id);
                    string jsonStr = await t;
                    ResponseProperty response = JsonConvert.DeserializeObject<ResponseProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        code = response.Code;
                    }
                    else
                    {
                        code = response.Code;
                    }
                    //IsBusy = false;
                }
                catch (Exception)
                {
                    SnackB.Message = AppResources.SomethingWrongText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
                // HideLoadingPopup();
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }

            return code;
        }

        private void OnSelectLanguageClicked(object sender, EventArgs e)
        {
            if (Settings.cultureInfo == "en-US") 
            {
                Settings.cultureInfo = "ms-MY";
                var language = new CultureInfo("ms-MY");
                Thread.CurrentThread.CurrentUICulture = language;
                AppResources.Culture = language;
                Application.Current.MainPage = new NavigationPage(new LoginPage1())
                {
                    BarBackgroundColor = Color.FromHex("#5F625B"),
                    BarTextColor = Color.FromHex("#FFD612")
                };
            } 
            else if (Settings.cultureInfo == "ms-MY") 
            {
                Settings.cultureInfo = "en-US";
                var language = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = language;
                AppResources.Culture = language;
                Application.Current.MainPage = new NavigationPage(new LoginPage1())
                {
                    BarBackgroundColor = Color.FromHex("#5F625B"),
                    BarTextColor = Color.FromHex("#FFD612"),
                };
            }

            base.OnAppearing();
        }

        private void CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!e.Value)
            {
                Settings.rememberMe = 0;
            }
            else
            {
                Settings.rememberMe = 1;
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        readonly LoadingPopupPage loadingPage = new LoadingPopupPage();
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