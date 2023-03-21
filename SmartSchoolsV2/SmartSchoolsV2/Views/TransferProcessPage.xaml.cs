using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferProcessPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        private int _wallet_id;
        private string _wallet_number;
        private int _recipient_id;
        private string _recipient_wallet;
        private string _recipient_name;
        private decimal _transfer_amount;
        public TransferProcessPage(int wallet_id, string wallet_number, int recipient_id, string recipient_wallet, string recipient_name, decimal transfer_amount)
        {
            InitializeComponent();
            BindingContext = this;
            _wallet_id = wallet_id;
            _wallet_number = wallet_number;
            _recipient_id = recipient_id;
            _recipient_wallet = recipient_wallet;
            _recipient_name = recipient_name;
            _transfer_amount = transfer_amount;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            WalletTopup();
        }

        public async void WalletTopup()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostWalletTransfer(_wallet_id, _wallet_number, _recipient_id, _recipient_wallet, _transfer_amount, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.ThankYouText, response.Message, "OK");
                        await Navigation.PopToRootAsync();
                    }
                    else
                    {
                        await DisplayAlert(AppResources.SorryText, response.Message, "OK");
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