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
    public partial class PaymentOrderPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public decimal _sub_total_amount;
        public decimal _tax_amount;
        public decimal _total_amount;
        public PaymentOrderPage(decimal sub_total_amount, decimal tax_amount, decimal total_amount)
        {
            InitializeComponent();
            BindingContext = this;
            _sub_total_amount = sub_total_amount;
            _tax_amount = tax_amount;
            _total_amount = total_amount;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            PlaceOrder();
        }
        public async void PlaceOrder()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostPurchasePlaceOrder(Settings.profileId, Settings.walletId, 1, Settings.userRoleId, _sub_total_amount, _tax_amount, _total_amount, 1, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.ThankYouText, response.Message, "OK");
                        //await Navigation.PopToRootAsync();
                    }
                    else
                    {
                        await DisplayAlert(AppResources.SorryText, response.Message, "OK");
                        //await Navigation.PopToRootAsync();
                    }
                }
                catch (Exception)
                {
                    SnackB.Message = AppResources.SomethingWrongText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    for (var counter = 1; counter < 2; counter++)
                    {
                        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    }
                    Settings.cartMode = "history";
                    await Navigation.PopAsync();

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