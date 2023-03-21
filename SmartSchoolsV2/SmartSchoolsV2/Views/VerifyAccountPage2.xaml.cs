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
    public partial class VerifyAccountPage2 : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public string new_pin = string.Empty;
        public string confirm_pin = string.Empty;
        public string _username;
        public VerifyAccountPage2(string username)
        {
            InitializeComponent();
            BindingContext = this;
            _username = username;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            btnChange.Text = AppResources.NextButtonText;
            lblEnterPIN.Text = AppResources.PleaseEnterCardPINText;
            lblChangePin.Text = AppResources.NewCardPINText;
            txtNum1.Focus();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        void Num1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                txtNum2.Focus();
            }
            else
            {
                txtNum1.Focus();
            }
        }

        void Num2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                txtNum3.Focus();
            }
            else
            {
                txtNum1.Focus();
            }
        }

        void Num3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                txtNum4.Focus();
            }
            else
            {
                txtNum2.Focus();
            }
        }
        void Num4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                txtNum5.Focus();
            }
            else
            {
                txtNum3.Focus();
            }
        }

        void Num5_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                txtNum6.Focus();
            }
            else
            {
                txtNum4.Focus();
            }
        }

        void Num6_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                btnChange.IsEnabled = true;
            }
            else
            {
                btnChange.IsEnabled = false;
                txtNum5.Focus();
            }
        }

        async void OnChangeClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtNum1.Text) && !string.IsNullOrEmpty(txtNum2.Text)
                && !string.IsNullOrEmpty(txtNum3.Text) && !string.IsNullOrEmpty(txtNum4.Text)
                   && !string.IsNullOrEmpty(txtNum5.Text) && !string.IsNullOrEmpty(txtNum6.Text))
            {
                if (string.IsNullOrEmpty(new_pin))
                {
                    new_pin = txtNum1.Text + txtNum2.Text + txtNum3.Text + txtNum4.Text + txtNum5.Text + txtNum6.Text;
                    lblChangePin.Text = AppResources.ConfirmCardPINText;
                    lblEnterPIN.Text = AppResources.PleaseConfirmCardPINText;
                    btnChange.Text = AppResources.ConfirmText;
                    txtNum1.Text = "";
                    txtNum2.Text = "";
                    txtNum3.Text = "";
                    txtNum4.Text = "";
                    txtNum5.Text = "";
                    txtNum6.Text = "";
                    txtNum1.Focus();
                }
                else 
                {
                    confirm_pin = txtNum1.Text + txtNum2.Text + txtNum3.Text + txtNum4.Text + txtNum5.Text + txtNum6.Text;

                    if (new_pin == confirm_pin)
                    {
                        await ChangePin(new_pin);
                    }
                    else 
                    {
                        SnackB.Message = AppResources.CardPINNotMatchText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
            }
            else
            {
                SnackB.Message = AppResources.CardPINRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async Task ChangePin(string new_pin)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostUserChangePin(_username, new_pin, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");
                        await Navigation.PopToRootAsync();
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