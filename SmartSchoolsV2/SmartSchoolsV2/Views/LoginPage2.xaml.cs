using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage2 : ContentPage
    {
        readonly Connection conn = new Connection();
        readonly ServiceWrapper srvc = new ServiceWrapper();
        public string _username;

        public ICommand TapForgot => new Command(async () => await Navigation.PushAsync(new ForgetPasswordPage(_username)));
        public LoginPage2 (string username)
		{
			InitializeComponent ();
            BindingContext = this;
            _username = username;
        }
        void OnNextClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtPswd.Text))
            {
                UserAuthLogin();
            }
            else
            {
                SnackB.Message = AppResources.PasswordRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
            //await Navigation.PushAsync(new SelectRolePage());
        }
        public async void UserAuthLogin()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostUserAuthLogin(_username, txtPswd.Text);
                    string jsonStr = await t;
                    ResponseProperty response = JsonConvert.DeserializeObject<ResponseProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        Settings.isLogin = true;
                        Settings.userName = _username;
                        Settings.accessToken = response.Message;
                        Settings.lastLogin = response.Code;
                        await Navigation.PushAsync(new SelectRolePage(_username));
                    }
                    else
                    {

                        SnackB.Message = AppResources.PasswordIncorrectText;
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