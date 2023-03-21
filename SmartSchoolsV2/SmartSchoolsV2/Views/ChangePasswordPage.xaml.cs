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
	public partial class ChangePasswordPage : ContentPage
	{
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public bool _tick1 = false;
        public bool _tick2 = false;
        public bool _tick3 = false;
        public bool _tick4 = false;
        public ChangePasswordPage ()
		{
			InitializeComponent ();
            BindingContext = this;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
        private void OnNewPswdTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= 8)
            {
                _tick1 = true;
            }
            else 
            {
                _tick1 = false;
            }

            bool UpperLower = HasUpperLower(e.NewTextValue);
            if (UpperLower == true)
            {
                _tick2 = true;
            }
            else {
                _tick3 = false;
            }

            bool special = HasDigit(e.NewTextValue);
            if (special == true)
            {
                _tick3 = true;
            }
            else
            {
                _tick3 = false;
            }

            if (_tick1 && _tick2 && _tick3)
            {
                //lblRule.TextColor = Color.Green;
            }
            else
            {
                //lblRule.TextColor = Color.Red;
            }
        }

        private void OnConfirmNewPswdTextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtConfirmNewPswd.Text.Length > 0)
            {
                if (txtNewPswd.Text == e.NewTextValue && !string.IsNullOrEmpty(e.NewTextValue))
                {
                    lblMatch.TextColor = Color.Green;
                    lblMatch.IsVisible = false;
                    _tick4 = true;
                }
                else
                {
                    lblMatch.TextColor = Color.Red;
                    lblMatch.IsVisible = true;
                    _tick4 = false;
                }
            }
            else
            {
                lblMatch.TextColor = Color.Red;
                lblMatch.IsVisible = false;
                _tick4 = false;
            }
        }
        public static bool HasUpperLower(string text)
        {
            bool hasUpper = false; bool hasLower = false;
            for (int i = 0; i < text.Length && !(hasUpper && hasLower); i++)
            {
                char c = text[i];
                if (!hasUpper) hasUpper = char.IsUpper(c);
                if (!hasLower) hasLower = char.IsLower(c);
            }
            return hasUpper&&hasLower;
        }
        public static bool HasDigit(string text)
        {
            bool hasDigit = false;
            for (int i = 0; i < text.Length && !(hasDigit); i++)
            {
                char c = text[i];
                if (!hasDigit) hasDigit = char.IsDigit(c);
            }
            return hasDigit;
        }
        async void OnChangePswdClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(txtNewPswd.Text))
            {
                if (!string.IsNullOrWhiteSpace(txtConfirmNewPswd.Text))
                {
                    if (_tick1 && _tick2 && _tick3)
                    {
                        if (_tick4)
                        {
                            await ChangePassword();
                        }
                        else
                        {
                            SnackB.Message = AppResources.PasswordNotMatchText;
                            SnackB.IsOpen = !SnackB.IsOpen;
                        }
                    }
                    else
                    {
                        SnackB.Message = AppResources.PasswordRuleMetText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                else
                {
                    SnackB.Message = AppResources.ConfirmNewPasswordRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else {
                SnackB.Message = AppResources.NewPasswordRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }

        }

        async Task ChangePassword()
        {

            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostUserChangePassword(Settings.profileId, Settings.userName, txtNewPswd.Text, txtConfirmNewPswd.Text, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert(AppResources.SorryText, response.Message, "OK");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
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