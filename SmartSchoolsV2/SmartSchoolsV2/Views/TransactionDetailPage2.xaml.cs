using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
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
    public partial class TransactionDetailPage2 : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public TransactionDetailViewModel vm;
        public string _wallet_number;
        public string _reference_number;
        public int _order_id;
        public int _profile_id;
        public int _wallet_id;
        public int _status_id;

        public TransactionDetailPage2(int order_id, int profile_id, int wallet_id, int status_id, string wallet_number, string reference_number)
        {
            InitializeComponent();
            vm = new TransactionDetailViewModel();
            this.BindingContext = vm;
            _order_id = order_id;
            _profile_id = profile_id;
            _wallet_id = wallet_id;
            _status_id = status_id;
            _wallet_number = wallet_number;
            _reference_number = reference_number;

            lblTitle.Text = AppResources.OrderNoText + _reference_number;

            if (_status_id == 2) 
            {
                btnContinueOrder.IsVisible = true;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.PurchaseOrderHistoryDetailGroupDate(_wallet_number, _reference_number);
        }
        public class TransactionDetailViewModel : BaseViewModel
        {
            Connection conn = new Connection();
            ServiceWrapper srvc = new ServiceWrapper();
            public string requestUrl = Settings.requestUrl;
            //public ObservableCollection<ShoppingCartModel> templistCart;
            public ObservableCollection<OrderDetailModel> listOrder;
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
            public ObservableCollection<OrderGroup> Orders { get; private set; } = new ObservableCollection<OrderGroup>();

            public class OrderDetailModelProperty
            {
                public bool Success { get; set; }
                public string Code { get; set; }
                public string Message { get; set; }
                public List<OrderDetailModel> Data { get; set; }
            }
            public class OrderDetailModel : INotifyPropertyChanged
            {
                public int order_detail_id { get; set; }
                public int order_id { get; set; }
                public string reference_number { get; set; }
                public int wallet_id { get; set; }
                public string wallet_number { get; set; }
                public int recipient_id { get; set; }
                public string full_name { get; set; }
                public string photo_url { get; set; }
                public int merchant_id { get; set; }
                public string company_name { get; set; }
                public int school_id { get; set; }
                public string school_name { get; set; }
                public int class_id { get; set; }
                public string class_name { get; set; }
                public DateTime pickup_date { get; set; }
                public TimeSpan pickup_time { get; set; }
                public int service_method_id { get; set; }
                public string delivery_location { get; set; }
                public int product_id { get; set; }
                public string product_qty { get; set; }
                public string product_photo_url { get; set; }
                public string product_name { get; set; }
                public string str_unit_price { get; set; }
                public decimal unit_price { get; set; }
                public decimal sub_total_amount { get; set; }
                public decimal total_amount { get; set; }
                public decimal discount_amount { get; set; }
                public int order_status_id { get; set; }
                public string order_status { get; set; }
                public bool dot_visible { get; set; }

                #region INotifyPropertyChanged

                public event PropertyChangedEventHandler PropertyChanged;
                void OnPropertyChanged([CallerMemberName] string propertyName = null)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
                #endregion
            }
            public class OrderGroup : ObservableCollection<OrderDetailModel>
            {
                public string pickup_date { get; private set; }

                public OrderGroup(string pickupDate, ObservableCollection<OrderDetailModel> carts) : base(carts)
                {
                    pickup_date = pickupDate;
                }
            }

            public async void PurchaseOrderHistoryDetailGroupDate(string wallet_number, string reference_number)
            {
                CultureInfo ci = new CultureInfo(Settings.cultureInfo);

                if (conn.IsConnected() == true)
                {
                    try
                    {
                        var t = srvc.PostPurchaseOrderHistoryDetailGroupDate(wallet_number, reference_number);
                        string jsonStr = await t;
                        OrderDetailModelProperty response = JsonConvert.DeserializeObject<OrderDetailModelProperty>(jsonStr);
                        if (response.Success == true)
                        {
                            if (response.Data.Count > 0)
                            {
                                Orders.Clear();

                                foreach (OrderDetailModel sl in response.Data)
                                {
                                    await PurchaseOrderHistoryDetailDate(wallet_number, reference_number, sl.pickup_date);

                                    Orders.Add(new OrderGroup(AppResources.DeliveryOnText + sl.pickup_date.ToString("dddd, dd/MM/yyyy", ci), listOrder));
                                }

                                await PurchaseOrderHistoryTotal(wallet_number, reference_number);
                            }
                            else
                            {
                                Orders.Clear();
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

            public async Task PurchaseOrderHistoryDetailDate(string wallet_number, string reference_number, DateTime pickup_date)
            {
                if (conn.IsConnected() == true)
                {
                    try
                    {
                        var t = srvc.PostPurchaseOrderHistoryDetailDate(wallet_number, reference_number, pickup_date);
                        string jsonStr = await t;
                        OrderDetailModelProperty response = JsonConvert.DeserializeObject<OrderDetailModelProperty>(jsonStr);
                        if (response.Success == true)
                        {
                            listOrder = new ObservableCollection<OrderDetailModel>();
                            foreach (OrderDetailModel sl in response.Data)
                            {
                                OrderDetailModel post = new OrderDetailModel();
                                post.order_detail_id = sl.order_detail_id;
                                post.order_id = sl.order_id;
                                post.wallet_id = sl.wallet_id;
                                post.wallet_number = sl.wallet_number;
                                post.merchant_id = sl.merchant_id;
                                post.company_name = AppResources.SoldByText + sl.company_name;
                                post.school_id = sl.school_id;
                                post.class_id = sl.class_id;
                                post.recipient_id = sl.recipient_id;
                                post.full_name = sl.full_name;
                                post.pickup_date = sl.pickup_date;
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
                                        post.class_name = sl.delivery_location;
                                        post.dot_visible = true;
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

                                post.product_id = sl.product_id;
                                post.product_qty = "x " + sl.product_qty;
                                post.product_name = sl.product_name;
                                post.unit_price = sl.unit_price;
                                post.str_unit_price = "RM " + sl.unit_price;
                                post.sub_total_amount = sl.sub_total_amount;
                                post.total_amount = sl.total_amount;
                                listOrder.Add(post);
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
            public async Task PurchaseOrderHistoryTotal(string wallet_number, string reference_number)
            {
                if (conn.IsConnected() == true)
                {
                    try
                    {
                        var t = srvc.PostPurchaseOrderHistoryTotal(wallet_number, reference_number);
                        string jsonStr = await t;
                        CartTotalProperty response = JsonConvert.DeserializeObject<CartTotalProperty>(jsonStr);
                        if (response.Success == true)
                        {
                            foreach (CartTotal r in response.Data)
                            {
                                str_sub_total_amount = "RM " + r.sub_total_amount.ToString();
                                tax_amount = r.tax_amount;
                                if (r.sub_total_amount.ToString().Length >= 5)
                                {
                                    str_tax_amount = "RM   " + r.tax_amount.ToString();
                                }
                                else
                                {
                                    str_tax_amount = "RM  " + r.tax_amount.ToString();
                                }
                                sub_total_amount = r.sub_total_amount;
                                str_tax_rate = AppResources.ServiceChargeText + r.tax_rate.ToString() + "% : ";
                                str_total_amount = "RM " + r.total_amount.ToString();
                                total_amount = r.total_amount;
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

        async void OnContinueClicked(object sender, EventArgs args)
        {
            bool answer = await App.Current.MainPage.DisplayAlert(AppResources.ContinueOrderText, AppResources.CurrentOrderDiscardText, AppResources.YesText, AppResources.CancelText);
            if (answer)
            {
                await ContinueOrderMaster(_order_id, _profile_id, _wallet_id);
            }
        }

        public async Task ContinueOrderMaster(int order_id, int profile_id, int wallet_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostPurchaseContinueOrderMaster(order_id, profile_id, wallet_id, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        //await MerchantProductCategory();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.SorryText, response.Message, "OK");
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    Settings.cartMode = "all";
                    await Navigation.PopAsync();
                    HideLoadingPopup();
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
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
    }
}