using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TopupPage : ContentPage
	{
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public string _amount;
        public string _acc_bal;
        public int _account_status_id;
        public int _kyc_status_id;
        public string _kyc_status;
        public string _mpay_uid;
        public TopupPage ()
		{
			InitializeComponent ();
            BindingContext = this;

            var tapGestureRecognizerTransfer = new TapGestureRecognizer();
            tapGestureRecognizerTransfer.Tapped += (s, e) =>
            {
                AccountStatus();
            };
            imgTransfer.GestureRecognizers.Add(tapGestureRecognizerTransfer);
        }

        private async void GetTransferPage()
        {
            await Navigation.PushAsync(new TransferPage2());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!string.IsNullOrEmpty(Settings.topupRefId)) 
            {
                GetTopupStatus();
            }
        }

        public async void AccountStatus()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostAccountStatus(Settings.profileId, Settings.walletNumber);
                    string jsonStr = await t;
                    AccStatusProperty response = JsonConvert.DeserializeObject<AccStatusProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        foreach (AccStatus r in response.Data)
                        {
                            _mpay_uid = r.mpay_uid;
                            _account_status_id = Convert.ToInt32(r.account_status_id);
                            _kyc_status_id = Convert.ToInt32(r.kyc_status_id);
                            _kyc_status = r.kyc_status;
                        }

                        if (_kyc_status_id == 11)
                        {
                            GetTransferPage();
                        }
                        else
                        {
                            await DisplayAlert(AppResources.KYCStatusText, _kyc_status + "." + AppResources.PleaseContactSupportText, "OK");
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

        public async void GetTopupStatus()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostAccountTopupStatus(Settings.userName, Settings.topupRefId, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        Settings.topupRefId = string.Empty;
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

        void OnAmountTextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtAmount.Text, "[^0123456789.]"))
            {
                SnackB.Message = AppResources.EnterNumberOnlyText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
            else { 
                _amount = e.NewTextValue;
            }
        }

        //void On10Clicked(object sender, EventArgs args)
        //{
        //    _amount = "10";
        //    txtAmount.Text = _amount;
        //}

        void On20Clicked(object sender, EventArgs args)
        {
            _amount = "20";
            txtAmount.Text = _amount;
        }

        void On50Clicked(object sender, EventArgs args)
        {
            _amount = "50";
            txtAmount.Text = _amount;
        }

        void On100Clicked(object sender, EventArgs args)
        {
            _amount = "100";
            txtAmount.Text = _amount;
        }

        void On150Clicked(object sender, EventArgs args)
        {
            _amount = "150";
            txtAmount.Text = _amount;
        }

        void On200Clicked(object sender, EventArgs args)
        {
            _amount = "200";
            txtAmount.Text = _amount;
        }

        void On250Clicked(object sender, EventArgs args)
        {
            _amount = "250";
            txtAmount.Text = _amount;
        }

        async void OnTopupClicked(object sender, EventArgs args)
        {
            decimal amount = Convert.ToDecimal(txtAmount.Text);
            if (amount >= 20)
            {
                //await Navigation.PushAsync(new PaymentPage(amount));
                await PopupNavigation.Instance.PushAsync(new OwnBankAccountPopupPage(amount));
            }
            else
            {
                SnackB.Message = AppResources.AmountLessThanTopupText;
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