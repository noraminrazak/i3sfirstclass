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
    public partial class RegisterAccountPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _account_id;
        public int _user_role_id;
        public string _username;
        public int _nationality_id;
        public string _date_of_birth;
        public int _card_type_id;
        public bool _tick1 = false;
        public bool _tick2 = false;
        public bool _tick3 = false;
        public bool _tick4 = false;
        public bool _iAgree = false;
        public int _marketing_flag;
        public static string _back;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }

        public static string _option;
        public static string Option
        {
            get { return _option; }
            set { _option = value; }
        }
        public ICommand TapLogin => new Command(async () => await Navigation.PopAsync());
        public ICommand TapCommand => new Command(async () => await Navigation.PushAsync(new TermsConditionsPage()));
        public void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
                if (Option == "country")
                {
                    _nationality_id = Settings.countryId;
                    txtNationality.Text = Settings.countryName;
                    Device.BeginInvokeOnMainThread(() => {
                        txtNationality.Unfocus();
                    });
                }

                Option = string.Empty;
                Back = "N";
            }
        }
        public RegisterAccountPage(int user_role_id, string username = "")
        {
            InitializeComponent();
            BindingContext = this;

            _user_role_id = user_role_id;
            _username = username;

            if (_user_role_id == 8)
            {
                lblFullName.Text = AppResources.FullNameText;
            }
            else if (_user_role_id == 9)
            {
                lblFullName.Text = AppResources.ParentFullNameText;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!string.IsNullOrEmpty(_username)) 
            {
                txtIDNo.Text = _username.Trim();
            }
        }

        void OnRadio1Clicked(object sender, EventArgs e)
        {
            gridNationality.IsVisible = false;
            gridDOB.IsVisible = false;
            _nationality_id = 130;
            _card_type_id = 1;
        }

        void OnRadio2Clicked(object sender, EventArgs e)
        {
            gridNationality.IsVisible = true;
            gridDOB.IsVisible = true;
            _card_type_id = 2;
        }

        async void StartCallNationality(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectCountryText, "country", "registeracc");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        void StartCallDob(object sender, EventArgs args)
        {
            Device.BeginInvokeOnMainThread(() => {
                datePicker.Focus();
            });
        }

        private void OnDoBDateSelected(object sender, DateChangedEventArgs e)
        {
            txtDoB.Text = e.NewDate.ToString("dd-MM-yyyy");
            _date_of_birth = e.NewDate.ToString("yyyy-MM-dd");
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
                lblRule.TextColor = Color.Green;
            }
            else
            {
                lblRule.TextColor = Color.Red;
            }
        }

        private void OnConfirmNewPswdTextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtPswd.Text == e.NewTextValue && !string.IsNullOrEmpty(e.NewTextValue))
            {
                lblMatch.TextColor = Color.Green;
                _tick4 = true;
            }
            else
            {
                lblMatch.TextColor = Color.Red;
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
        private void CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!e.Value)
            {
                _marketing_flag = 0;
            }
            else
            {
                _marketing_flag = 1;
            }
        }

        private void AgreeChecked(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
            {
                _iAgree = true;
            }
            else
            {
                _iAgree = false;
            }
        }

        async void OnNextClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtFullName.Text))
            {
                Settings.fullName = txtFullName.Text.Trim();

                if (_card_type_id > 0)
                {
                    if (!string.IsNullOrEmpty(txtIDNo.Text))
                    {
                        if (!string.IsNullOrEmpty(txtMobileNo.Text))
                        {
                            if (!string.IsNullOrEmpty(txtEmail.Text))
                            {
                                if ((_card_type_id == 2 && !string.IsNullOrEmpty(txtNationality.Text)) || (_card_type_id == 1 && string.IsNullOrEmpty(txtNationality.Text)))
                                {
                                    if ((_card_type_id == 2 && !string.IsNullOrEmpty(txtDoB.Text)) || (_card_type_id == 1 && string.IsNullOrEmpty(txtDoB.Text)))
                                    {
                                        if (!string.IsNullOrEmpty(txtPswd.Text))
                                        {
                                            if (!string.IsNullOrWhiteSpace(txtConfirmPswd.Text))
                                            {
                                                if (_tick1 && _tick2 && _tick3)
                                                {
                                                    if (_tick4)
                                                    {
                                                        if (_iAgree == true)
                                                        {
                                                            await RegisterUser();
                                                        }
                                                        else
                                                        {
                                                            SnackB.Message = AppResources.PleaseAgreeText;
                                                            SnackB.IsOpen = !SnackB.IsOpen;
                                                        }
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
                                        SnackB.Message = AppResources.DOBRequiredText;
                                        SnackB.IsOpen = !SnackB.IsOpen;
                                    }
                                }
                                else
                                {
                                    SnackB.Message = AppResources.NationalityRequiredText;
                                    SnackB.IsOpen = !SnackB.IsOpen;
                                }
                            }
                            else
                            {
                                SnackB.Message = AppResources.EmailRequiredText;
                                SnackB.IsOpen = !SnackB.IsOpen;
                            }
                        }
                        else
                        {
                            SnackB.Message = AppResources.MobileNoRequiredText;
                            SnackB.IsOpen = !SnackB.IsOpen;
                        }
                    }
                    else
                    {
                        SnackB.Message = AppResources.MyKadPassportNoRequiredText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                else
                {
                    SnackB.Message = AppResources.IDTypeRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                SnackB.Message = AppResources.FullNameRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async Task RegisterUser()
        {
            bool zeroStart = txtMobileNo.Text.StartsWith("0");
            string mobileNum = string.Empty;

            if (zeroStart == true)
            {
                mobileNum = txtCountryCode.Text + txtMobileNo.Text.Remove(0, 1);
            }
            else
            {
                mobileNum = txtCountryCode.Text + txtMobileNo.Text;
            }

            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostAccountRegister(_user_role_id, txtFullName.Text.Trim(), _card_type_id, _nationality_id, txtDoB.Text, txtIDNo.Text.Trim(),
                        mobileNum, txtEmail.Text.Trim(), txtConfirmPswd.Text.Trim(), _marketing_flag);
                    string jsonStr = await t;
                    RegisterAccountProperty response = JsonConvert.DeserializeObject<RegisterAccountProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        if (response.Code == "verify_account")
                        {
                            await Navigation.PushAsync(new VerifyAccountPage(mobileNum, txtIDNo.Text.Trim(), txtPswd.Text.Trim()));
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