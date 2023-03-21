using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SmartSchoolsV2.Views.ShoppingCartPage1.ShoppingCartViewModel;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingCartPage1 : ContentPage
    {
        public ShoppingCartViewModel vm;
        public ShoppingCartPage1()
        {
            InitializeComponent();

            vm = new ShoppingCartViewModel();
            this.BindingContext = vm;

            //var tapGestureRecognizerClick = new TapGestureRecognizer();
            //tapGestureRecognizerClick.Tapped += (s, e) =>
            //{
            //    GetAddToCartPage();
            //};
            //lblClickHere.GestureRecognizers.Add(tapGestureRecognizerClick);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.PurchaseCartGroup(Settings.profileId, Settings.walletId, 1);
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

        public class ShoppingCartViewModel : BaseViewModel
        {
            CultureInfo ci = new CultureInfo(Settings.cultureInfo);
            Connection conn = new Connection();
            ServiceWrapper srvc = new ServiceWrapper();
            public string requestUrl = Settings.requestUrl;
            //public ObservableCollection<ShoppingCartModel> templistCart;
            public ObservableCollection<ShoppingCartModel> listCart;

            string _str_total_amount;
            public string str_total_amount
            {
                get
                {
                    return _str_total_amount;
                }
                set
                {
                    if (_str_total_amount != value)
                    {
                        _str_total_amount = value;
                        OnPropertyChanged("str_total_amount");
                    }
                }
            }
            string _wallet_balance;
            public string wallet_balance
            {
                get
                {
                    return _wallet_balance;
                }
                set
                {
                    if (_wallet_balance != value)
                    {
                        _wallet_balance = value;
                        OnPropertyChanged("wallet_balance");
                    }
                }
            }
            decimal _total_amount;
            public decimal total_amount
            {
                get
                {
                    return _total_amount;
                }
                set
                {
                    if (_total_amount != value)
                    {
                        _total_amount = value;
                        OnPropertyChanged("total_amount");
                    }
                }
            }

            string _str_sub_total_amount;
            public string str_sub_total_amount
            {
                get
                {
                    return _str_sub_total_amount;
                }
                set
                {
                    if (_str_sub_total_amount != value)
                    {
                        _str_sub_total_amount = value;
                        OnPropertyChanged("str_sub_total_amount");
                    }
                }
            }

            decimal _sub_total_amount;
            public decimal sub_total_amount
            {
                get
                {
                    return _sub_total_amount;
                }
                set
                {
                    if (_sub_total_amount != value)
                    {
                        _sub_total_amount = value;
                        OnPropertyChanged("sub_total_amount");
                    }
                }
            }

            string _str_tax_amount;
            public string str_tax_amount
            {
                get
                {
                    return _str_tax_amount;
                }
                set
                {
                    if (_str_tax_amount != value)
                    {
                        _str_tax_amount = value;
                        OnPropertyChanged("str_tax_amount");
                    }
                }
            }

            decimal _tax_amount;
            public decimal tax_amount
            {
                get
                {
                    return _tax_amount;
                }
                set
                {
                    if (_tax_amount != value)
                    {
                        _tax_amount = value;
                        OnPropertyChanged("tax_amount");
                    }
                }
            }

            string _str_tax_rate;
            public string str_tax_rate
            {
                get
                {
                    return _str_tax_rate;
                }
                set
                {
                    if (_str_tax_rate != value)
                    {
                        _str_tax_rate = value;
                        OnPropertyChanged("str_tax_rate");
                    }
                }
            }

            int _tax_rate;
            public int tax_rate
            {
                get
                {
                    return _tax_rate;
                }
                set
                {
                    if (_tax_rate != value)
                    {
                        _tax_rate = value;
                        OnPropertyChanged("tax_rate");
                    }
                }
            }

            bool _is_visible;
            public bool is_visible
            {
                get
                {
                    return _is_visible;
                }
                set
                {
                    if (_is_visible != value)
                    {
                        _is_visible = value;
                        OnPropertyChanged("is_visible");
                    }
                }
            }
            public int RowCount { get; set; }
            public static Label l = new Label();
            //public ICommand FindGroupAndUpdateProductQtyCommand { protected set; get; }
            public ObservableCollection<CartGroup> Carts { get; private set; } = new ObservableCollection<CartGroup>();

            public class ShoppingCartModelProperty
            {
                public bool Success { get; set; }
                public string Code { get; set; }
                public string Message { get; set; }
                public List<ShoppingCartModel> Data { get; set; }
            }
            public class ShoppingCartModel : INotifyPropertyChanged
            {
                public int cart_id { get; set; }
                public int profile_id { get; set; }
                public int wallet_id { get; set; }
                public string wallet_number { get; set; }
                public string full_name { get; set; }
                public int merchant_id { get; set; }
                public string company_name { get; set; }
                public int school_id { get; set; }
                public string school_name { get; set; }
                public int class_id { get; set; }
                public string class_name { get; set; }
                public int recipient_id { get; set; }
                public int recipient_role_id { get; set; }
                public int user_role_id { get; set; }
                public string recipient_role { get; set; }
                public string recipient_name { get; set; }
                public DateTime pickup_date { get; set; }
                public TimeSpan pickup_time { get; set; }
                public int product_id { get; set; }

                int _product_qty;
                public int product_qty
                {
                    get
                    {
                        return _product_qty;
                    }
                    set
                    {
                        if (_product_qty != value)
                        {
                            _product_qty = value;
                            OnPropertyChanged("product_qty");

                        }
                    }
                }
                public string product_photo_url { get; set; }
                public string product_name { get; set; }
                public string product_description { get; set; }
                public string str_unit_price { get; set; }
                public decimal unit_price { get; set; }
                public decimal sub_total_amount { get; set; }
                public int service_method_id { get; set; }
                public string delivery_location { get; set; }
                public int order_status_id { get; set; }
                public string order_status { get; set; }
                public string order_status_bm { get; set; }
                public bool is_check { get; set; }
                public bool dot_visible { get; set; }

                #region INotifyPropertyChanged

                public event PropertyChangedEventHandler PropertyChanged;
                void OnPropertyChanged([CallerMemberName] string propertyName = null)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
                #endregion
            }
            public class CartGroup : ObservableCollection<ShoppingCartModel>
            {
                public string pickup_date { get; private set; }

                public CartGroup(string pickupDate, ObservableCollection<ShoppingCartModel> carts) : base(carts)
                {
                    pickup_date = pickupDate;
                }
            }

            public async void PurchaseCartGroup(int profile_id, int wallet_id, int order_status_id)
            {
                if (conn.IsConnected() == true)
                {
                    try
                    {
                        var t = srvc.PostPurchaseCartGroupDate(profile_id, wallet_id, order_status_id);
                        string jsonStr = await t;
                        ShoppingCartModelProperty response = JsonConvert.DeserializeObject<ShoppingCartModelProperty>(jsonStr);
                        if (response.Success == true)
                        {
                            if (response.Data.Count > 0)
                            {
                                Carts.Clear();

                                foreach (ShoppingCartModel sl in response.Data)
                                {
                                    await PurchaseCartPickupDate(profile_id, wallet_id, order_status_id, sl.pickup_date);

                                    Carts.Add(new CartGroup(AppResources.DeliveryOnText + sl.pickup_date.ToString("dddd, dd/MM/yyyy", ci), listCart));
                                }

                                is_visible = true;
                                await PurchaseCartTotal(profile_id, wallet_id);
                            }
                            else
                            {
                                Carts.Clear();
                                is_visible = false;
                            }

                        }
                        else
                        {
                            //SnackB.Message = response.Message;
                            //SnackB.IsOpen = !SnackB.IsOpen;
                        }
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
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

            public async Task PurchaseCartPickupDate(int profile_id, int wallet_id, int order_status_id, DateTime pickup_date)
            {
                if (conn.IsConnected() == true)
                {
                    try
                    {
                        var t = srvc.PostPurchaseCartPickupDate(profile_id, wallet_id, order_status_id, pickup_date);
                        string jsonStr = await t;
                        ShoppingCartModelProperty response = JsonConvert.DeserializeObject<ShoppingCartModelProperty>(jsonStr);
                        if (response.Success == true)
                        {
                            listCart = new ObservableCollection<ShoppingCartModel>();
                            foreach (ShoppingCartModel sl in response.Data)
                            {
                                ShoppingCartModel post = new ShoppingCartModel();
                                post.cart_id = sl.cart_id;
                                post.profile_id = sl.profile_id;
                                post.wallet_id = sl.wallet_id;
                                post.wallet_number = sl.wallet_number;
                                post.merchant_id = sl.merchant_id;
                                post.company_name = AppResources.SoldByText + sl.company_name;
                                post.school_id = sl.school_id;
                                post.class_id = sl.class_id;
                                if (sl.recipient_id == Settings.profileId)
                                {
                                    if (sl.service_method_id == 1) 
                                    {
                                        DateTime time = sl.pickup_date.Add(sl.pickup_time);
                                        post.school_name = AppResources.DeliveryBtnText + " @ " + time.ToString("hh:mm tt");
                                        post.dot_visible = true;
                                        post.class_name = sl.delivery_location;
                                    } 
                                    else if (sl.service_method_id == 2) 
                                    {
                                        DateTime time = sl.pickup_date.Add(sl.pickup_time);
                                        post.school_name = AppResources.TakeAwayBtnText + " @ " + time.ToString("hh:mm tt");
                                        post.dot_visible = false;
                                    }
                                }
                                else 
                                {
                                    post.school_name = sl.school_name;
                                    post.dot_visible = true;
                                    post.class_name = sl.class_name;
                                }
                                post.recipient_role_id = sl.recipient_role_id;
                                post.recipient_id = sl.recipient_id;
                                post.recipient_name = sl.recipient_name;
                                if (!string.IsNullOrEmpty(sl.product_photo_url))
                                {
                                    post.product_photo_url = requestUrl + sl.product_photo_url;
                                }
                                else
                                {
                                    post.product_photo_url = requestUrl + "/images/sorry-no-image.jpg";
                                }
                                post.pickup_date = sl.pickup_date;
                                post.product_id = sl.product_id;
                                post.product_qty = sl.product_qty;
                                post.product_name = sl.product_name;
                                post.product_description = sl.product_description;
                                post.unit_price = sl.unit_price;
                                post.str_unit_price = "RM " + sl.unit_price;
                                post.sub_total_amount = sl.sub_total_amount;
                                post.is_check = false;
                                listCart.Add(post);
                            }
                        }
                        else
                        {

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

            public async void UpdateProductQty(string mode, ShoppingCartModel value)
            {
                //IEnumerable<ShoppingCartModel> items = templistCart.Where(x => x.cart_id == value.cart_id && x.pickup_date == value.pickup_date && x.product_id == value.product_id);
                //int productQty = 0;
                //foreach (var item in items)
                //{
                if (mode == "plus")
                {
                    value.product_qty++;

                }
                else if (mode == "minus")
                {
                    value.product_qty--;
                }
                //}

                if (value.product_qty == 0)
                {
                    await PurchaseDeleteCart(value);
                }
                else
                {
                    if (conn.IsConnected() == true)
                    {
                        try
                        {
                            var t = srvc.PostPurchaseUpdateCart(value.cart_id, value.profile_id, value.wallet_id, value.merchant_id, value.school_id,
                                value.recipient_id, value.pickup_date, value.product_id, value.product_qty, Settings.fullName);
                            string jsonStr = await t;
                            CartProperty response = JsonConvert.DeserializeObject<CartProperty>(jsonStr);
                            if (response.Success == true)
                            {
                                await PurchaseCartTotal(Settings.profileId, Settings.walletId);
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
            }

            public async Task PurchaseDeleteCart(ShoppingCartModel cart)
            {
                if (conn.IsConnected() == true)
                {
                    try
                    {
                        var t = srvc.PostPurchaseDeleteCart(cart.cart_id, cart.profile_id, cart.recipient_id, cart.product_id, Settings.fullName);
                        string jsonStr = await t;
                        CartProperty response = JsonConvert.DeserializeObject<CartProperty>(jsonStr);
                        if (response.Success == true)
                        {
                            PurchaseCartGroup(Settings.profileId, Settings.walletId, 1);
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

            public async Task PurchaseCartTotal(int profile_id, int wallet_id)
            {
                if (conn.IsConnected() == true)
                {
                    try
                    {
                        var t = srvc.PostPurchaseCartTotal(profile_id, wallet_id);
                        string jsonStr = await t;
                        CartTotalProperty response = JsonConvert.DeserializeObject<CartTotalProperty>(jsonStr);
                        if (response.Success == true)
                        {
                            sub_total_amount = 0;

                            foreach (CartTotal r in response.Data) 
                            {
                                sub_total_amount = sub_total_amount + r.sub_total_amount;
                                str_sub_total_amount = "RM " + sub_total_amount;
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
        }

        void OnStartOrderClicked(object sender, EventArgs args)
        {
            GetAddToCartPage();
        }

        async void Delete_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            ShoppingCartModel cart = item.BindingContext as ShoppingCartModel;

            await vm.PurchaseDeleteCart(cart);
        }
        void OnMinusTapped(object sender, EventArgs args)
        {
            var param = (TappedEventArgs)args;
            var value = param.Parameter as ShoppingCartModel;

            vm.UpdateProductQty("minus", value);
        }

        void OnPlusTapped(object sender, EventArgs args)
        {
            var param = (TappedEventArgs)args;
            var value = param.Parameter as ShoppingCartModel;

            vm.UpdateProductQty("plus", value);

            //cvCarts.ItemsSource = null;
            //cvCarts.ItemsSource = vm.Carts;
        }
        void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var previous = e.PreviousSelection.FirstOrDefault() as ShoppingCart;
            var current = e.CurrentSelection.FirstOrDefault() as ShoppingCartModel;

        }

        async void OnCheckOutClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new CheckOutPage());
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
    }
}