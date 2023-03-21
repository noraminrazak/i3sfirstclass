using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using SmartSchoolsV2.Effects;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddToCartPopupPage : PopupPage, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public string product_ingredient { get; set; }

        public delegate void SetDetailEventHandler(object source, EventArgs args);
        public event SetDetailEventHandler DetailSet;
        public int _school_id;
        public int _profile_id;
        public int _recipient_id;
        public int _recipient_role_id;
        public int _wallet_id;
        public int _merchant_id;
        public int _user_role_id;
        public DateTime _pickup_date;
        public TimeSpan _pickup_time;
        public int _service_method_id;
        public string _delivery_location;
        public int _qty = 1;
        public ProductDetail prod = new ProductDetail();
        protected virtual void OnDetailSet()
        {
            if (DetailSet != null)
            {
                DetailSet(this, EventArgs.Empty);
            }
        }
        public AddToCartPopupPage(int profile_id, int wallet_id, int school_id, int merchant_id, int user_role_id, int recipient_id, int recipient_role_id, DateTime pickup_date,TimeSpan pickup_time,int service_method_id, string delivery_location,ProductDetail value)
        {
            InitializeComponent();
            BindingContext = this;

            _profile_id = profile_id;
            _wallet_id = wallet_id;
            _merchant_id = merchant_id;
            _school_id = school_id;
            _user_role_id = user_role_id;
            _recipient_id = recipient_id;
            _recipient_role_id = recipient_role_id;
            _pickup_date = pickup_date;
            _pickup_time = pickup_time;
            _service_method_id = service_method_id;
            _delivery_location = delivery_location;
            prod = value;
            productPhoto.Source = value.photo_url;
            lblProductName.Text = value.product_name;
            lblDescriptipn.Text = value.product_description;
            lblUnitPrice.Text = "RM " + value.unit_price;

            if (!string.IsNullOrEmpty(value.product_ingredient))
            {
                TooltipEffect.SetText(this, value.product_ingredient);
            }
            else {
                lblInfo.IsVisible = false;
            }


            var tapGestureRecognizerMinus = new TapGestureRecognizer();
            tapGestureRecognizerMinus.Tapped += (s, e) => {
                _qty--;
                if (_qty == 0)
                {
                    PopupNavigation.Instance.PopAllAsync();
                }
                else {
                    txtQty.Text = _qty.ToString();
                }
            };
            btnMinus.GestureRecognizers.Add(tapGestureRecognizerMinus);

            var tapGestureRecognizerPlus = new TapGestureRecognizer();
            tapGestureRecognizerPlus.Tapped += (s, e) => {
                _qty++;
                txtQty.Text = _qty.ToString();
            };
            btnPlus.GestureRecognizers.Add(tapGestureRecognizerPlus);

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        async void OnClicked(object sender, EventArgs args)
        {

            if (conn.IsConnected() == true)
            {
                try
                {
                    ButtonWithSpinner.IsBusy = !ButtonWithSpinner.IsBusy;

                    var t = srvc.PostPurchaseInsertCart(_profile_id, _wallet_id,_merchant_id,_school_id,_user_role_id,_recipient_id,
                        _recipient_role_id, _pickup_date,_pickup_time,_service_method_id,_delivery_location,prod.product_id,_qty,Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                        AddToCartPage2.Back = "Y";
                        AddToCartPage2.Option = "add-to-cart";
                        OnDetailSet();
                    }
                    else
                    {
                        await DisplayAlert(AppResources.SorryText, response.Message, "OK");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    ButtonWithSpinner.IsBusy = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }
        private void OnClose_Tapped(object sender, System.EventArgs e)
        {
            CloseAllPopup();
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}