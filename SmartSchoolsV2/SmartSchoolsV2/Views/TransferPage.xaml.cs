using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferPage : ContentPage
    {
        //Connection conn = new Connection();
        //ServiceWrapper srvc = new ServiceWrapper();
        //public string requestUrl = Settings.requestUrl;
        private int _wallet_id;
        private string _wallet_number;
        private int _recipient_id;
        private int _mode;
        private string _recipient_wallet;
        private string _recipient_name;
        private string _transfer_amount;
        private string _second_info;
        private string _photo_url;
        public TransferPage(int mode, int wallet_id, string wallet_number, int recipient_id, string recipient_wallet, string recipient_name, string second_info, string photo_url)
        {
            InitializeComponent();
            BindingContext = this;
            _mode = mode;
            _wallet_id = wallet_id;
            _wallet_number = wallet_number;
            _recipient_id = recipient_id;
            _recipient_wallet = recipient_wallet;
            _recipient_name = recipient_name;
            _second_info = second_info;
            _photo_url = photo_url;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!string.IsNullOrEmpty(_photo_url))
            {
                userImage.Source = _photo_url;
                userImage.IsVisible = true;
                userInitial.IsVisible = false;
            }
            else
            {
                userImage.IsVisible = false;
                userInitial.IsVisible = true;
            }
            lblFullName.Text = _recipient_name;
            lblSecondInfo.Text = _second_info;
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
            else
            {
                _transfer_amount = e.NewTextValue;
            }
        }
        void On5Clicked(object sender, EventArgs args)
        {
            _transfer_amount = "5";
            txtAmount.Text = _transfer_amount;
        }

        void On10Clicked(object sender, EventArgs args)
        {
            _transfer_amount = "10";
            txtAmount.Text = _transfer_amount;
        }

        void On20Clicked(object sender, EventArgs args)
        {
            _transfer_amount = "20";
            txtAmount.Text = _transfer_amount;
        }

        void On50Clicked(object sender, EventArgs args)
        {
            _transfer_amount = "50";
            txtAmount.Text = _transfer_amount;
        }

        void On100Clicked(object sender, EventArgs args)
        {
            _transfer_amount = "100";
            txtAmount.Text = _transfer_amount;
        }

        void On150Clicked(object sender, EventArgs args)
        {
            _transfer_amount = "150";
            txtAmount.Text = _transfer_amount;
        }

        //void On200Clicked(object sender, EventArgs args)
        //{
        //    _transfer_amount = "200";
        //    txtAmount.Text = _transfer_amount;
        //}

        async void OnTransferClicked(object sender, EventArgs args)
        {
            decimal amount = Convert.ToDecimal(txtAmount.Text);
            if (amount >= 1)
            {
                await Navigation.PushAsync(new TransferProcessPage(_wallet_id,_wallet_number,_recipient_id,_recipient_wallet,_recipient_name,Convert.ToDecimal(txtAmount.Text)));
            }
            else
            {
                SnackB.Message = AppResources.AmountLessThanTransferText;
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