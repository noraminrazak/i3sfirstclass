using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingCartPage : TabbedPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public string _mode;
        public string cartCnt { get; set; }
        public ShoppingCartPage()
        {
            InitializeComponent();
            BindingContext = this;

            var tapGestureRecognizerCart = new TapGestureRecognizer();
            tapGestureRecognizerCart.Tapped += (s, e) => {
                GetAddToCartPage();
            };
            imgCartPlus.GestureRecognizers.Add(tapGestureRecognizerCart);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _mode = Settings.cartMode;
            if (_mode == "all")
            {
                CurrentPage = Children[0];
            }
            else if (_mode == "history")
            {
                CurrentPage = Children[1];
            }
        }

        public async void CartCount()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostPurchaseCartCount(Settings.profileId, Settings.walletId);
                    string jsonStr = await t;
                    CartProperty response = JsonConvert.DeserializeObject<CartProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        if (response.Total > 0)
                        {
                            //badgeCart.IsVisible = true;
                            //badgeCart.Text = response.Total.ToString();
                        }
                        else
                        {
                            //badgeCart.IsVisible = false;
                            //badgeCart.Text = string.Empty;
                        }
                    }
                    else
                    {
                        //SnackB.Message = response.Message;
                        //SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {

                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }
        private async void GetAddToCartPage()
        {
            int user_role_id = Settings.userRoleId;
            if (user_role_id == 8)
            {
                await Navigation.PushAsync(new AddToCartPage1b());
            }
            else if (user_role_id == 9)
            {
                await Navigation.PushAsync(new AddToCartPage1());
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
    }
}