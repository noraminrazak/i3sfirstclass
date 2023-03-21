using Newtonsoft.Json;
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
    public partial class ForgetPasswordPage : ContentPage
    {
        readonly Connection conn = new Connection();
        readonly ServiceWrapper srvc = new ServiceWrapper();
        public string _username;
        public ForgetPasswordPage(string username)
        {
            InitializeComponent();
            BindingContext = this;
            _username = username;
        }

        void OnRequestClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                RequestNewPassword();
            }
            else
            {
                SnackB.Message = AppResources.EmailRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
            //await Navigation.PushAsync(new SelectRolePage());
        }

        public async void RequestNewPassword()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostUserForgotPassword(_username, txtEmail.Text);
                    string jsonStr = await t;
                    ResponseProperty response = JsonConvert.DeserializeObject<ResponseProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText,response.Message,"OK");
                        await Navigation.PopAsync();
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
                HideLoadingPopup();
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
            return false;
        }
    }
}