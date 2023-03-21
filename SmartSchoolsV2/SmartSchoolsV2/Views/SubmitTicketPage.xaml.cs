using Newtonsoft.Json;
using Plugin.HybridWebView.Shared.Delegates;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubmitTicketPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        string _full_name;
        string _email;
        string _school_name;
        string _location;
        string _url;
        int _view = 1; //1=new ticket,2 = my tickets

        public SubmitTicketPage()
        {
            InitializeComponent();
            BindingContext = this;

            LekirWeb();
        }

        private void OnLoadFinished(object sender, EventArgs args)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Load Complete: {0}", args));
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await activity_indicator.ProgressTo(0.9, 900, Easing.SpringIn);
        }

        void OnBtnTicketClicked(object sender, EventArgs args)
        {

            if (_view == 1)
            {
                _view = 2;
                Device.BeginInvokeOnMainThread(() =>
                {
                    var OnPlatformDic = (OnPlatform<string>)App.Current.Resources["FontAwesomeSolid"];

                    var fontFamily = OnPlatformDic.Platforms.FirstOrDefault((arg) => arg.Platform.FirstOrDefault() == Device.RuntimePlatform).Value;

                    btnTicket.FontFamily = fontFamily.ToString();
                    lblTitle.Text = AppResources.MyTicketsText;
                    btnTicket.Text = "\uf067";
                });

                if (Settings.userRoleId == 9)
                {
                    _url = "https://i3s.lekir.tech/get-all-tickets-webview?name=" + _full_name + "&email=" + _email + "&role=9";
                }
                else
                {
                    _url = "https://i3s.lekir.tech/get-all-tickets-webview?name=" + _full_name + "&email=" + _email + "&school_name=" + _school_name + "&location=" + _location;
                }
            }
            else if (_view == 2)
            {
                _view = 1;
                Device.BeginInvokeOnMainThread(() =>
                {
                    var OnPlatformDic = (OnPlatform<string>)App.Current.Resources["FontAwesomeSolid"];

                    var fontFamily = OnPlatformDic.Platforms.FirstOrDefault((arg) => arg.Platform.FirstOrDefault() == Device.RuntimePlatform).Value;

                    btnTicket.FontFamily = fontFamily.ToString();
                    lblTitle.Text = AppResources.SubmitTicketText;
                    btnTicket.Text = "\uf0ca";
                });

                if (Settings.userRoleId == 9)
                {
                    _url = "https://i3s.lekir.tech/open-ticket-webview?name=" + _full_name + "&email=" + _email + "&role=9";
                }
                else
                {
                    _url = "https://i3s.lekir.tech/open-ticket-webview?name=" + _full_name + "&email=" + _email + "&school_name=" + _school_name + "&location=" + _location;
                }
            }

            webview.Source = _url;
        }

        public async void LekirWeb()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostAccountInfo(Settings.profileId, Settings.userRoleId);
                    string jsonStr = await t;
                    AccountCSInfoProperty response = JsonConvert.DeserializeObject<AccountCSInfoProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        _view = 1;
                        foreach (AccountCSInfo r in response.Data)
                        {
                            _full_name = r.full_name;
                            _email = r.email;
                            _school_name = r.school_name;
                            _location = r.coordinate;
                        }

                        if (Settings.userRoleId == 9)
                        {
                            _url = "https://i3s.lekir.tech/open-ticket-webview?name=" + _full_name + "&email=" + _email + "&role=9";
                        }
                        else {
                            _url = "https://i3s.lekir.tech/open-ticket-webview?name=" + _full_name + "&email=" + _email + "&school_name=" + _school_name + "&location=" + _location;
                        }

                        webview.Source = _url;
                    }
                    else
                    {
                        await DisplayAlert(AppResources.SorryText, response.Message, "OK");
                        await Navigation.PopAsync();
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

        public void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            activity_indicator.IsVisible = true;
        }

        public void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            activity_indicator.IsVisible = false;
        }

        //private void FormsWebView_OnNavigationStarted(object sender, DecisionHandlerDelegate e)
        //{
        //    activity_indicator.IsVisible = true;
        //    System.Diagnostics.Debug.WriteLine("Navigation has started");
        //}

        //private void FormsWebView_OnNavigationCompleted(object sender, System.EventArgs e)
        //{
        //    activity_indicator.IsVisible = false;
        //    System.Diagnostics.Debug.WriteLine("Navigation has completed");
        //}

        //private void FormsWebView_OnContentLoaded(object sender, System.EventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine("Content has loaded");
        //}

        //private void FormsWebView_OnNavigationError(object sender, int e)
        //{
        //    System.Diagnostics.Debug.WriteLine($"An error was thrown with code: {e}");
        //}

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