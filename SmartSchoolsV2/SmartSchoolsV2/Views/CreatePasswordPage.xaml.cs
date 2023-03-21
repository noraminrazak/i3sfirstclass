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
    public partial class CreatePasswordPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public bool _tick1 = false;
        public bool _tick2 = false;
        public bool _tick3 = false;
        public bool _tick4 = false;
        public static string _back;
        public string _username;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }
        public void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
                txtCountryCode.Text = Settings.countryCode;
            }
        }
        public CreatePasswordPage(string username)
        {
            InitializeComponent();
            BindingContext = this;

            _username = username;
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
            else
            {
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
            return hasUpper && hasLower;
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

        async void StartCall1(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectCountryCodeText, "country", "create-password");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }
        async void OnChangePswdClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtCountryCode.Text) && !string.IsNullOrEmpty(txtMobileNo.Text))
            {
                if (!string.IsNullOrWhiteSpace(txtNewPswd.Text))
                {
                    if (!string.IsNullOrWhiteSpace(txtConfirmNewPswd.Text))
                    {
                        if (_tick1 && _tick2 && _tick3)
                        {
                            if (_tick4)
                            {
                                await CreatePassword();
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
                else
                {
                    SnackB.Message = AppResources.NewPasswordRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else 
            {
                SnackB.Message = AppResources.MobileNoRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }

            

        }

        async Task CreatePassword()
        {
            bool zeroStart = txtMobileNo.Text.StartsWith("0");
            string mobileNum = string.Empty;

            if (zeroStart == true) 
            {
                mobileNum = txtCountryCode.Text + txtMobileNo.Text.Remove(0, 1);
            }
            else {
                mobileNum = txtCountryCode.Text + txtMobileNo.Text;
            }


            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostUserCreatePassword(mobileNum, _username, txtNewPswd.Text.Trim(), txtConfirmNewPswd.Text.Trim());
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        if (response.Code == "verify_account")
                        {
                            await Navigation.PushAsync(new VerifyAccountPage(mobileNum, _username, txtConfirmNewPswd.Text.Trim()));
                        }
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