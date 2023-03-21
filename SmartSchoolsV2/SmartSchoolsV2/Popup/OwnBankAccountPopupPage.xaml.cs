using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OwnBankAccountPopupPage : PopupPage
    {
        public decimal _amount;
        public bool _iWill = false;
        public OwnBankAccountPopupPage(decimal amount)
        {
            InitializeComponent();
            _amount = amount;
        }

        private void CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                _iWill = true;
            }
            else
            {
                _iWill = false;
            }
        }
        async void OnContinueClicked(object sender, EventArgs args)
        {
            if (_iWill == true)
            {
                CloseAllPopup();
                await Navigation.PushAsync(new PaymentPage(_amount));
            }
            else 
            {
                lbliWill.TextColor = Color.Red;
            }
        }
        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}