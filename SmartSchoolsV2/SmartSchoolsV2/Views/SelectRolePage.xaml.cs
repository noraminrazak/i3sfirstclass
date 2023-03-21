using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using SmartSchoolsV2.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectRolePage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public string _username;
        public int _staff_type_id = 0;
        public int devicePlatformId = 0;
        public bool isLoading = false;
        bool isBusy;
        public bool IsBusy1
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public SelectRolePage(string username)
        {
            InitializeComponent();
            _username = username;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UserRoleAccount();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        public async void UserRoleAccount()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostUserRoleAccount(_username);
                    string jsonStr = await t;
                    RoleAccountProperty response = JsonConvert.DeserializeObject<RoleAccountProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        lblFullName.Text = response.full_name;
                        Settings.profileId = response.profile_id;
                        Settings.fullName = response.full_name;
                        Settings.photoUrl = response.photo_url;
                        if (!string.IsNullOrEmpty(response.photo_url))
                        {
                            userInitial.IsVisible = false;
                            userImg.IsVisible = true;
                            userImg.Source = ImageSource.FromUri(new Uri(requestUrl + response.photo_url));
                        }
                        else
                        {
                            userInitial.IsVisible = true;
                        }

                        if (response.Data.Count > 1)
                        {
                            foreach (RoleAccount r in response.Data)
                            {
                                if (r.user_role_id == 9)
                                {
                                    btnParent.IsVisible = true;
                                }
                                else if (r.user_role_id == 8)
                                {
                                    btnStaff.IsVisible = true;
                                }
                                else if (r.user_role_id == 7)
                                {
                                    btnMerchant.IsVisible = true;
                                }
                            }
                        }
                        else 
                        {
                            isLoading = true;

                            foreach (RoleAccount r in response.Data)
                            {
                                if (r.user_role_id == 9)
                                {
                                    btnParent.IsVisible = true;
                                    Settings.userRoleId = 9;
                                    int parent_id = await ParentProfile();
                                    if (parent_id > 0)
                                    {
                                        Application.Current.MainPage = new NavigationPage(new MainPage())
                                        {
                                            BarBackgroundColor = Color.FromHex("#5E625A"),
                                            BarTextColor = Color.FromHex("#FFD612")
                                        };
                                    }
                                }
                                else if (r.user_role_id == 8)
                                {
                                    btnStaff.IsVisible = true;
                                    Settings.userRoleId = 8;
                                    int staff_type_id = await StaffProfile();
                                    if (staff_type_id == 1 || staff_type_id == 6) //HM or School Admin
                                    {
                                        Application.Current.MainPage = new NavigationPage(new StaffPage1())
                                        {
                                            BarBackgroundColor = Color.FromHex("#5E625A"),
                                            BarTextColor = Color.FromHex("#FFD612")
                                        };
                                    }
                                    else if (staff_type_id == 2) //PK HEM
                                    {
                                        Application.Current.MainPage = new NavigationPage(new StaffPage2())
                                        {
                                            BarBackgroundColor = Color.FromHex("#5E625A"),
                                            BarTextColor = Color.FromHex("#FFD612")
                                        };
                                    }
                                    else if (staff_type_id == 3) //Teacher
                                    {
                                        Application.Current.MainPage = new NavigationPage(new StaffPage3())
                                        {
                                            BarBackgroundColor = Color.FromHex("#5E625A"),
                                            BarTextColor = Color.FromHex("#FFD612")
                                        };
                                    }
                                    else if (staff_type_id == 4) //Staff
                                    {
                                        Application.Current.MainPage = new NavigationPage(new StaffPage4())
                                        {
                                            BarBackgroundColor = Color.FromHex("#5E625A"),
                                            BarTextColor = Color.FromHex("#FFD612")
                                        };
                                    }
                                    else if (staff_type_id == 5) //Teacher or Warden
                                    {
                                        Application.Current.MainPage = new NavigationPage(new StaffPage5())
                                        {
                                            BarBackgroundColor = Color.FromHex("#5E625A"),
                                            BarTextColor = Color.FromHex("#FFD612")
                                        };
                                    }
                                }
                                else if (r.user_role_id == 7)
                                {
                                    btnMerchant.IsVisible = true;
                                    Settings.userRoleId = 7;
                                    int merchant_id = await MerchantProfile();
                                    if (merchant_id > 0)
                                    {
                                        Application.Current.MainPage = new NavigationPage(new MerchantPage())
                                        {
                                            BarBackgroundColor = Color.FromHex("#5E625A"),
                                            BarTextColor = Color.FromHex("#FFD612")
                                        };
                                    }
                                }
                            }
                        }

                        if (Settings.deviceToken != string.Empty) 
                        {
                            UpdateDeviceToken();
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

        public async void UpdateDeviceToken()
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
                        //do onthing
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

        async void OnParentClicked(object sender, EventArgs args)
        {
            Settings.userRoleId = 9;
            int parent_id = await ParentProfile();
            if (parent_id > 0)
            {
                Application.Current.MainPage = new NavigationPage(new MainPage())
                {
                    BarBackgroundColor = Color.FromHex("#5E625A"),
                    BarTextColor = Color.FromHex("#FFD612")
                };
            }
        }

        async void OnStaffClicked(object sender, EventArgs args)
        {
            Settings.userRoleId = 8;
            int staff_type_id =  await StaffProfile();
            if (staff_type_id == 1 || staff_type_id == 6) //HM
            {
                Application.Current.MainPage = new NavigationPage(new StaffPage1())
                {
                    BarBackgroundColor = Color.FromHex("#5E625A"),
                    BarTextColor = Color.FromHex("#FFD612")
                };
            }
            else if (staff_type_id == 2) //PK HEM
            {
                Application.Current.MainPage = new NavigationPage(new StaffPage2())
                {
                    BarBackgroundColor = Color.FromHex("#5E625A"),
                    BarTextColor = Color.FromHex("#FFD612")
                };
            }
            else if (staff_type_id == 3) //Teacher
            {
                Application.Current.MainPage = new NavigationPage(new StaffPage3())
                {
                    BarBackgroundColor = Color.FromHex("#5E625A"),
                    BarTextColor = Color.FromHex("#FFD612")
                };
            }
            else if (staff_type_id == 4) //Staff
            {
                Application.Current.MainPage = new NavigationPage(new StaffPage4())
                {
                    BarBackgroundColor = Color.FromHex("#5E625A"),
                    BarTextColor = Color.FromHex("#FFD612")
                };
            }
            else if (staff_type_id == 5) //Teacher/Warden
            {
                Application.Current.MainPage = new NavigationPage(new StaffPage5())
                {
                    BarBackgroundColor = Color.FromHex("#5E625A"),
                    BarTextColor = Color.FromHex("#FFD612")
                };
            }
        }

        async void OnMerchantClicked(object sender, EventArgs args)
        {
            Settings.userRoleId = 7;
            int merchant_id = await MerchantProfile();
            if (merchant_id > 0)
            {
                Application.Current.MainPage = new NavigationPage(new MerchantPage())
                {
                    BarBackgroundColor = Color.FromHex("#5E625A"),
                    BarTextColor = Color.FromHex("#FFD612")
                };
            }

        }

        public async Task<int> MerchantProfile()
        {
            int result = 0;
            if (conn.IsConnected() == true)
            {
                try
                {
                    if (isLoading == false)
                    {
                        ShowLoadingPopup();
                    }
                    var t = srvc.PostMerchantProfile(Settings.profileId);
                    string jsonStr = await t;
                    MerchantProfileProperty response = JsonConvert.DeserializeObject<MerchantProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        string photo_url = string.Empty;
                        foreach (MerchantProfile r in response.Data)
                        {
                            Settings.merchantId = r.merchant_id;
                            Settings.merchantTypeId = r.merchant_type_id;
                            result = r.merchant_id;
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
                    if (isLoading == false)
                    {
                        HideLoadingPopup();
                    }
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
            return result;
        }

        public async Task<int> ParentProfile()
        {
            int result = 0;
            if (conn.IsConnected() == true)
            {
                try
                {
                    if (isLoading == false) 
                    {
                        ShowLoadingPopup();
                    }

                    var t = srvc.PostParentProfile(Settings.profileId);
                    string jsonStr = await t;
                    ParentProfileProperty response = JsonConvert.DeserializeObject<ParentProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        string photo_url = string.Empty;
                        foreach (ParentProfile r in response.Data)
                        {
                            Settings.parentId = r.parent_id;
                            result = r.parent_id;
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
                    if (isLoading == false)
                    {
                        HideLoadingPopup();
                    }
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
            return result;
        }

        public async Task<int> StaffProfile()
        {
            int result = 0;
            if (conn.IsConnected() == true)
            {
                try
                {
                    if (isLoading == false)
                    {
                        ShowLoadingPopup();
                    }
                    var t = srvc.PostStaffProfile(Settings.profileId);
                    string jsonStr = await t;
                    StaffProfileProperty response = JsonConvert.DeserializeObject<StaffProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {

                        foreach (StaffProfile r in response.Data)
                        {
                            Settings.staffId = r.staff_id;
                            Settings.staffTypeId = r.staff_type_id;
                            Settings.schoolId = r.school_id;
                            Settings.schoolName = r.school_name;
                            result = r.staff_type_id;
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
                    if (isLoading == false)
                    {
                        HideLoadingPopup();
                    }
                }

            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
            return result;
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