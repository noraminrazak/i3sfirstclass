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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantOrderHistoryPage2 : ContentPage, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public event PropertyChangedEventHandler PropertyChanged1;
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
        public int _order_id;
        public int _order_status_id;
        public int _school_id;
        public DateTime _pickup_date;
        bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }
        public ObservableCollection<ProductOrderHistory> _listProductOrder;
        public ObservableCollection<ProductOrderHistory> listProductOrder
        {
            get
            {
                return _listProductOrder;
            }
            set
            {
                _listProductOrder = value;
                PropertyChanged1?.Invoke(this, new PropertyChangedEventArgs("listProductOrder"));
            }
        }
        public int RowCount { get; set; }
        public static Label l = new Label();
        public MerchantOrderHistoryPage2(OrderHistory prop)
        {
            InitializeComponent();
            BindingContext = this;

            _order_id = Convert.ToInt16(prop.order_id);
            _order_status_id = prop.order_status_id;
            _school_id = prop.school_id;
            _pickup_date = prop.pickup_date;

            lblTitle.Text = AppResources.OrderForText + prop.pickup_date.ToString("dddd, dd/MM/yyyy", ci);
            lblClassName.Text = prop.class_name;
            if (prop.service_method_id == 1) 
            {
                lblServiceMethod.Text = AppResources.DeliveryBtnText + " (" + prop.delivery_location + ") @ " + prop.pickup_time.ToString("hh:mm tt");
            } 
            else if (prop.service_method_id == 2) 
            {
                lblServiceMethod.Text = AppResources.TakeAwayBtnText + " @ " + prop.pickup_time.ToString("hh:mm tt");
            }
            lblOrderStatus.Text = prop.order_status;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ProductOrderHistory();
        }

        public async Task ProductOrderHistory()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvStudent.ItemsSource = null;
                    var t = srvc.PostMerchantProductOrderHistoryStaff(_order_id, _school_id, _pickup_date);
                    string jsonStr = await t;
                    ProductOrderHistoryProperty response = JsonConvert.DeserializeObject<ProductOrderHistoryProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listProductOrder = new ObservableCollection<ProductOrderHistory>();
                        foreach (ProductOrderHistory sl in response.Data)
                        {
                            ProductOrderHistory post = new ProductOrderHistory();
                            post.pickup_date = sl.pickup_date;
                            post.product_id = sl.product_id;
                            post.product_name = sl.product_name;
                            if (!string.IsNullOrEmpty(sl.product_photo_url))
                            {
                                post.product_photo_url = requestUrl + sl.product_photo_url;
                            }
                            else
                            {
                                post.product_photo_url = requestUrl + "/images/sorry-no-image.jpg";
                            }
                            post.product_qty = sl.product_qty;
                            post.product_total = sl.product_qty.ToString() + "  X  " + sl.product_name;
                            post.total_amount = sl.total_amount;
                            post.str_total_amount = "RM " + sl.total_amount;
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            post.class_id = sl.class_id;
                            post.class_name = sl.class_name;
                            listProductOrder.Add(post);
                        }
                        RowCount = listProductOrder.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvStudent.Footer = l;
                        lvStudent.ItemsSource = listProductOrder;
                    }
                    else
                    {
                        listProductOrder = new ObservableCollection<ProductOrderHistory>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvStudent.Footer = l;
                        lvStudent.ItemsSource = listProductOrder;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        ViewCell lastCell;
        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.LightGoldenrodYellow;
                lastCell = viewCell;
            }
        }
        public ProductOrderHistory prod;
        void OnProductSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as ProductOrderHistory;
            if (data == null) return;
            prod = data;

            if (prod.product_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //await Navigation.PushAsync(new MerchantOrderStudentDetailPage(prod));
                ((ListView)sender).SelectedItem = null;
            }
        }
        async void OnOrderStatusClicked(object sender, EventArgs args)
        {
            if (_order_status_id == 3)
            {
                bool result = await DisplayAlert(AppResources.StatusUpdateText, AppResources.OrderStatusPreparingText, AppResources.YesText, AppResources.CancelText);

                if (result == true)
                {
                    await UpdateOrderStatus(4);
                }
            }
            else if (_order_status_id == 4)
            {
                bool result = await DisplayAlert(AppResources.StatusUpdateText, AppResources.OrderStatusDeliveredText, AppResources.YesText, AppResources.CancelText);

                if (result == true)
                {
                    await UpdateOrderStatus(5);
                }
            }
        }

        public async Task UpdateOrderStatus(int order_status_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostMerchantUpdateOrderStatus(_school_id, Settings.merchantId, _order_id.ToString(), order_status_id, Settings.fullName, _pickup_date);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        if (order_status_id == 4)
                        {
                            lblOrderStatus.Text = AppResources.PreparingText;
                        }
                        else if (order_status_id == 5)
                        {
                            OrderGroupViewModel viewModel = new OrderGroupViewModel();
                            viewModel.LoadOrderCommand.Execute(null);
                            await Navigation.PopAsync();
                        }
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
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
    }
}