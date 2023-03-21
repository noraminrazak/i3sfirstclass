using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Views;
using System;
using SmartSchoolsV2.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int devicePlatformId = 0;
        public SplashPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //DependencyService.Get<IStatusBar>().HideStatusBar();
            ScaleIcon();
        }

        private async void ScaleIcon()
        {
            // wait until the UI is present
            await Task.Delay(300);

            // animate the splash logo
            await SplashIcon.ScaleTo(0.5, 500, Easing.CubicInOut);
            var animationTasks = new[]{
                SplashIcon.ScaleTo(100.0, 1000, Easing.CubicInOut),
                SplashIcon.FadeTo(0, 700, Easing.CubicInOut)
            };
            await Task.WhenAll(animationTasks);

            if (Settings.isLogin == true)
            {
                await UpdateDeviceToken();

                if (Settings.userRoleId == 9)
                {
                    //Navigation.InsertPageBefore(new MainPage(), Navigation.NavigationStack[0]);
                    Application.Current.MainPage = new NavigationPage(new MainPage())
                    {
                        BarBackgroundColor = Color.FromHex("#5E625A"),
                        BarTextColor = Color.FromHex("#FFD612")
                    };
                }
                else if (Settings.userRoleId == 8)
                {
                    if (Settings.staffTypeId == 1 || Settings.staffTypeId == 6)
                    {
                        //Navigation.InsertPageBefore(new StaffPage1(), Navigation.NavigationStack[0]);
                        Application.Current.MainPage = new NavigationPage(new StaffPage1())
                        {
                            BarBackgroundColor = Color.FromHex("#5E625A"),
                            BarTextColor = Color.FromHex("#FFD612")
                        };
                    }
                    else if (Settings.staffTypeId == 2)
                    {
                        //Navigation.InsertPageBefore(new StaffPage2(), Navigation.NavigationStack[0]);
                        Application.Current.MainPage = new NavigationPage(new StaffPage2())
                        {
                            BarBackgroundColor = Color.FromHex("#5E625A"),
                            BarTextColor = Color.FromHex("#FFD612")
                        };
                    }
                    else if (Settings.staffTypeId == 3)
                    {
                        //Navigation.InsertPageBefore(new StaffPage3(), Navigation.NavigationStack[0]);
                    Application.Current.MainPage = new NavigationPage(new StaffPage3())
                    {
                        BarBackgroundColor = Color.FromHex("#5E625A"),
                        BarTextColor = Color.FromHex("#FFD612")
                    };
                    }
                    else if (Settings.staffTypeId == 4)
                    {
                        //Navigation.InsertPageBefore(new StaffPage4(), Navigation.NavigationStack[0]);
                        Application.Current.MainPage = new NavigationPage(new StaffPage4())
                        {
                            BarBackgroundColor = Color.FromHex("#5E625A"),
                            BarTextColor = Color.FromHex("#FFD612")
                        };
                    }
                    else if (Settings.staffTypeId == 5)
                    {
                        //Navigation.InsertPageBefore(new StaffPage4(), Navigation.NavigationStack[0]);
                        Application.Current.MainPage = new NavigationPage(new StaffPage5())
                        {
                            BarBackgroundColor = Color.FromHex("#5E625A"),
                            BarTextColor = Color.FromHex("#FFD612")
                        };
                    }
                }
                else if (Settings.userRoleId == 7)
                {
                    //Navigation.InsertPageBefore(new MerchantPage(), Navigation.NavigationStack[0]);
                    Application.Current.MainPage = new NavigationPage(new MerchantPage())
                    {
                        BarBackgroundColor = Color.FromHex("#5E625A"),
                        BarTextColor = Color.FromHex("#FFD612")
                    };
                }
            }
            else
            {
                //Navigation.InsertPageBefore(new LoginPage1(), Navigation.NavigationStack[0]);
                Application.Current.MainPage = new NavigationPage(new LoginPage1()) 
                { 
                    BarBackgroundColor = Color.FromHex("#5F625B"), 
                    BarTextColor = Color.FromHex("#FFD612") 
                };
            }

            await Navigation.PopToRootAsync(false);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        public async Task UpdateDeviceToken()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    if (Device.RuntimePlatform == Device.Android) 
                    {
                        devicePlatformId = 1;
                    } 
                    else if (Device.RuntimePlatform == Device.iOS) 
                    {
                        devicePlatformId = 2;
                    }
                    var t = srvc.PostUserUpdateDeviceToken(Settings.profileId, Settings.deviceToken, devicePlatformId, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await GetLastLogin();
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
    }
}