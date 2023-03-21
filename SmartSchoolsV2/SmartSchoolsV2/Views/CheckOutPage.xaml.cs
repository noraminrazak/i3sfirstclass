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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckOutPage : ContentPage
    {
        public CheckOutViewModel vm;
        public int _payment_method_id = 1;
        public int _bank_id;
        public static string _back;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }
        public CheckOutPage()
        {
            InitializeComponent();
            vm = new CheckOutViewModel();
            this.BindingContext = vm;
        }

        public void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            vm.PurchaseCartGroup(Settings.profileId, Settings.walletId, 1);
            await vm.GetWalletBalance(Settings.userName);
        }
        public class CheckOutViewModel : BaseViewModel
        {
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
                public int service_method_id { get; set; }
                public int product_id { get; set; }
                public string delivery_location { get; set; }

                string _product_qty;
                public string product_qty
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

                public bool dot_visible { get; set; }
                public int order_status_id { get; set; }
                public string order_status { get; set; }
                public string order_status_bm { get; set; }
                public bool is_check { get; set; }

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
                CultureInfo ci = new CultureInfo(Settings.cultureInfo);

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

                                await PurchaseCartTotal(profile_id, wallet_id);
                            }
                            else
                            {
                                Carts.Clear();
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
                                post.pickup_date = sl.pickup_date;
                                post.product_id = sl.product_id;
                                post.product_qty = "x " + sl.product_qty;
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
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
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
                            CartTotal list = new CartTotal();
                            string tax_rate_no_trim = string.Empty;
                            foreach (CartTotal sl in response.Data)
                            {
                                sub_total_amount = sub_total_amount + sl.sub_total_amount;
                                str_sub_total_amount = "RM " + sub_total_amount;
                                tax_amount = tax_amount + sl.tax_amount;
                                if (str_sub_total_amount.Length >= 5) 
                                {
                                    str_tax_amount = "RM  " + tax_amount.ToString();
                                } 
                                else 
                                {
                                    str_tax_amount = "RM " + tax_amount.ToString();
                                }

                                str_tax_rate = sl.tax_rate.ToString() + "-";

                                if (!string.IsNullOrEmpty(tax_rate_no_trim) && !tax_rate_no_trim.Contains(sl.tax_rate.ToString()))
                                {
                                    tax_rate_no_trim = str_tax_rate + sl.tax_rate.ToString() + "-";
                                }
                                else 
                                {
                                    tax_rate_no_trim = sl.tax_rate.ToString() + "-";
                                }
                                total_amount = total_amount + sl.total_amount;
                                str_total_amount = "RM " + total_amount;
                            }
                            str_tax_rate = AppResources.ServiceChargeText + tax_rate_no_trim.Remove(tax_rate_no_trim.Length - 1) + "% :";
                        }
                        else
                        {
                            //SnackB.Message = response.Message;
                            //SnackB.IsOpen = !SnackB.IsOpen;
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

                    }
                }
                else
                {
                    //SnackB.Message = AppResources.CheckInternetText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }

            }

            public async Task GetWalletBalance(string username)
            {
                if (conn.IsConnected() == true)
                {
                    try
                    {
                        var t = srvc.PostAccountCardInfo(username);
                        string jsonStr = await t;
                        CardInfoProperty response = JsonConvert.DeserializeObject<CardInfoProperty>(jsonStr);
                        if (response.Success == true)
                        {
                            CardInfo list = new CardInfo();

                            foreach (CardInfo sl in response.Data) 
                            {
                                wallet_balance =  AppResources.i3seWalletText + "\n(" + AppResources.BalanceText + ": RM " + sl.balance + ")".Replace("\n", System.Environment.NewLine);
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
                        System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
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

        async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            //var page = new PaymentMethodPage();
            //page.DetailSet += this.OnDetailSet;
            //await Navigation.PushAsync(page);
        }

        async void OnPlaceOrderClicked(object sender, EventArgs args)
        {
            if (_payment_method_id > 0)
            {
                await Navigation.PushAsync(new PaymentOrderPage(vm.sub_total_amount,vm.tax_amount,vm.total_amount));
            }
            else 
            {
                await DisplayAlert("", AppResources.PleaseSelectPaymentMethodText, "OK");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
    }
}