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
    public partial class VerifyMobileNoPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _account_id;
        public int _card_type_id;
        public int _nationality_id;
        public string _full_name;
        public string _mobile_number;
        public string _identity_number;
        public string _email;
        public int hour = 0;
        public int counter = 0;
        public int mins = 5;
        public int isTimerCancel = 0;
        public ICommand TapResend => new Command(async () => await ResendOTP());
        public VerifyMobileNoPage(int account_id, string full_name, int card_type_id, int nationality_id,
            string identity_number, string mobile_number, string email)
        {
            InitializeComponent();
            BindingContext = this;

            _account_id = account_id;
            _full_name = full_name;
            _card_type_id = card_type_id;
            _nationality_id = nationality_id;
            _identity_number = identity_number;
            _mobile_number = mobile_number;
            _email = email;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            txtNum1.Focus();

            StartTimer(hour, mins, counter);
        }

        public void StartTimer(int h, int m, int sec)
        {
            hour = h;
            mins = m;
            counter = sec;
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if (isTimerCancel == 1)
                {
                    return false;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        counter = counter - 1;
                        if (counter < 0)
                        {
                            counter = 59;
                            mins = mins - 1;
                            if (mins < 0)
                            {
                                mins = 59;
                                hour = hour - 1;
                                if (hour < 0)
                                {
                                    hour = 0;
                                    mins = 0;
                                    counter = 0;
                                }
                            }
                        }

                        lblTimer.Text = string.Format("{0:00}:{1:00}:{2:00}", hour, mins, counter);
                    });
                    if (hour == 0 && mins == 0 && counter == 0)
                    {
                        //Code to call next form
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            lblTimer.IsVisible = false;
                            lblResend.IsVisible = true;
                        });
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            });

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
                btnVerify.IsEnabled = true;
            }
            else
            {
                btnVerify.IsEnabled = false;
                txtNum5.Focus();
            }
        }

        async Task ResendOTP()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostAccountRegister(_full_name,_card_type_id, _nationality_id, _identity_number,_mobile_number,_email);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            lblTimer.IsVisible = true;
                            lblResend.IsVisible = false;
                        });

                        StartTimer(hour, mins, counter);
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

        async void OnVerifyClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtNum1.Text) && !string.IsNullOrEmpty(txtNum2.Text)
                && !string.IsNullOrEmpty(txtNum3.Text) && !string.IsNullOrEmpty(txtNum4.Text)
                   && !string.IsNullOrEmpty(txtNum5.Text) && !string.IsNullOrEmpty(txtNum6.Text))
            {
                string otp = txtNum1.Text + txtNum2.Text + txtNum3.Text + txtNum4.Text + txtNum5.Text + txtNum6.Text;
                await VerifyAccount(otp);
            }
            else
            {
                SnackB.Message = AppResources.OTPRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async Task VerifyAccount(string otp)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostAccountVerify(_account_id, otp);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        if (response.Code == "verify_success")
                        {
                            await Navigation.PushAsync(new VerifyAccountPage2(_username));
                        }
                        else
                        {
                            await Navigation.PopToRootAsync();
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