using GalaSoft.MvvmLight.Command;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using SmartSchoolsV2.Resources;
using System.Globalization;

namespace SmartSchoolsV2.ViewModels
{
    public class OrderGroupViewModel : BaseViewModel
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
        public string requestUrl = Settings.requestUrl;
        private OrderViewModel _oldOrder;
        public ObservableCollection<OrderViewModel> filteredlistOrder;
        private ObservableCollection<OrderViewModel> listOrder;
        public ObservableCollection<OrderViewModel> ListOrders
        {
            get => listOrder;

            set => SetProperty(ref listOrder, value);
        }
        public Command LoadOrderCommand { get; set; }
        public Command<OrderViewModel> RefreshOrderCommand { get; set; }
        public RelayCommand<OrderCartViewModel> SelectedOrderCommand { get; set; }
        public OrderGroupViewModel()
        {
            listOrder = new ObservableCollection<OrderViewModel>();
            ListOrders = new ObservableCollection<OrderViewModel>();
            filteredlistOrder = new ObservableCollection<OrderViewModel>();
            LoadOrderCommand = new Command(async () => await ExecuteLoadOrderCommand());
            RefreshOrderCommand = new Command<OrderViewModel>((items) => ExecuteRefreshOrderCommand(items));

            SelectedOrderCommand = new RelayCommand<OrderCartViewModel>(GetOrderHistoryPage);
        }
        public async void GetOrderHistoryPage(OrderCartViewModel person)
        {
            Settings.pickupDate = person.order.pickup_date;
            Settings.selectedSchoolId = person.order.school_id;
            Settings.selectedClassId = person.order.class_id;

            if (person.order.class_id > 0) 
            {
                await App.Current.MainPage.Navigation.PushAsync(new MerchantOrderHistoryPage(person.order));
            }
            else
            {
                await App.Current.MainPage.Navigation.PushAsync(new MerchantOrderHistoryPage2(person.order));
            }

        }
        public bool isExpanded = false;
        private void ExecuteRefreshOrderCommand(OrderViewModel item)
        {
            if (item != null) 
            {
                if (_oldOrder == item)
                {
                    // click twice on the same item will hide it
                    item.Expanded = !item.Expanded;
                }
                else
                {
                    if (_oldOrder != null)
                    {
                        // hide previous selected item
                        _oldOrder.Expanded = false;
                    }
                    // show selected item
                    item.Expanded = true;
                }

                _oldOrder = item;
            }
        }
        async Task ExecuteLoadOrderCommand()
        {
            try
            {
                IsBusy = true;

                var t = srvc.PostMerchantOrderHistoryGroup(Settings.merchantSchoolId, Settings.merchantId);
                string jsonStr = await t;
                OrderHistoryGroupProperty response = JsonConvert.DeserializeObject<OrderHistoryGroupProperty>(jsonStr);
                if (response.Success == true)
                {
                    ListOrders.Clear();
                    int _count = 0;
                    if (response.Data.Count > 0)
                    {
                        foreach (OrderHistoryGroup g in response.Data)
                        {
                            var h = srvc.PostMerchantOrderHistory(Settings.merchantSchoolId, Settings.merchantId, g.pickup_date);
                            string jsonStr2 = await h;
                            OrderHistoryProperty response2 = JsonConvert.DeserializeObject<OrderHistoryProperty>(jsonStr2);
                            if (response2.Success == true)
                            {
                                string str_pickup_date = string.Empty;
                                string str_day_of_week = string.Empty;
                                string str_total_amount = string.Empty;
                                decimal total_amount = 0;
                                int total_order = 0;
                                List<OrderHistory> listOrder = new List<OrderHistory>();
                                foreach (OrderHistory n in response2.Data)
                                {
                                    OrderHistory cart = new OrderHistory();
                                    cart.pickup_date = n.pickup_date;
                                    cart.pickup_time = n.pickup_time;
                                    cart.total_amount = n.total_amount;
                                    cart.total_order = n.total_order;
                                    cart.merchant_id = n.merchant_id;
                                    cart.class_id = n.class_id;

                                    if (n.class_id > 0)
                                    {
                                        cart.image_visible = false;
                                        cart.initial_visible = true;
                                        cart.class_name = n.class_name;
                                    }
                                    else
                                    {
                                        cart.initial_visible = false;
                                        cart.image_visible = true;
                                        cart.class_name = n.full_name;
                                        cart.photo_url = requestUrl + n.photo_url;
                                    }
                                    cart.school_id = n.school_id;
                                    cart.school_name = n.school_name;
                                    cart.service_method_id = n.service_method_id;
                                    cart.delivery_location = n.delivery_location;
                                    cart.order_id = n.order_id;
                                    cart.order_status_id = n.order_status_id;
                                    if (Settings.cultureInfo == "en-US")
                                    {
                                        cart.order_status = n.order_status;
                                    }
                                    else if (Settings.cultureInfo == "ms-MY")
                                    {
                                        cart.order_status = n.order_status_bm;
                                    }
                                    listOrder.Add(cart);

                                    str_pickup_date = g.pickup_date.ToString("dddd, dd/MM/yyyy", ci);
                                    total_amount = total_amount + n.total_amount;
                                    str_total_amount = "RM " + total_amount.ToString("N2");
                                    total_order = total_order + n.total_order; // by date
                                }
                                _count++;

                                Order order = new Order(str_pickup_date, total_order.ToString(), str_total_amount, listOrder, _count.ToString() + AppResources.RecordText);
                                ListOrders.Add(new OrderViewModel(order));
                            }
                        }
                    }
                    else
                    {
                        Order order = new Order("", "", "", null, AppResources.NoRecordFoundText);
                        ListOrders.Add(new OrderViewModel(order));
                    }

                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
            finally 
            {
                IsBusy = false;
            }
        }


        public int PublicOrderCart => ListOrders.Count;

        private String LocalOrderString;
        public String PublicOrderString
        {
            get
            {
                return LocalOrderString;
            }
            set
            {
                SetProperty(ref LocalOrderString, value);
            }
        }
    }
}
