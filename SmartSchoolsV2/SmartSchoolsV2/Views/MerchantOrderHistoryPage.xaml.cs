using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantOrderHistoryPage : ContentPage, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public event PropertyChangedEventHandler PropertyChanged1;
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);

        public string _order_id;
        public int _order_status_id;
        public int _school_id;
        public DateTime _pickup_date;

        public ObservableCollection<StudentOrderHistory> _listStudentOrder;
        public ObservableCollection<StudentOrderHistory> listStudentOrder
        {
            get
            {
                return _listStudentOrder;
            }
            set
            {
                _listStudentOrder = value;
                PropertyChanged1?.Invoke(this, new PropertyChangedEventArgs("listStudentOrder"));
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
        public string _display_by;
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
        public MerchantOrderHistoryPage(OrderHistory prop)
        {
            InitializeComponent();
            BindingContext = this;

            _order_id = prop.order_id;
            _order_status_id = prop.order_status_id;
            _school_id = prop.school_id;
            _pickup_date = prop.pickup_date;

            var tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += (s, e) => {
                carouselView.Position = 0;
            };
            imgProduct.GestureRecognizers.Add(tapGestureRecognizer1);

            var tapGestureRecognizer0 = new TapGestureRecognizer();
            tapGestureRecognizer0.Tapped += (s, e) => {
                carouselView.Position = 1;
            };
            imgStudent.GestureRecognizers.Add(tapGestureRecognizer0);

            lblTitle.Text = AppResources.OrderForText + prop.pickup_date.ToString("dddd, dd/MM/yyyy", ci);
            lblClassName.Text = prop.class_name;
            //if (Settings.cultureInfo == "en-US")
            //{
                lblOrderStatus.Text = prop.order_status;
            //}
            //else if (Settings.cultureInfo == "ms-MY")
            //{
            //    lblOrderStatus.Text = prop.order_status_bm;
            //}

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (carouselView.Position == 0)
            {
                MerchantOrderProductView.LoadProductOrder.Execute(null);
            }
            else if (carouselView.Position == 1)
            {
                MerchantOrderStudentView.LoadStudentOrder.Execute(null);
            }
        }
        void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            int currentItemPosition = e.CurrentPosition;

            if (currentItemPosition == 0)
            {
                MerchantOrderProductView.LoadProductOrder.Execute(null);

                imgProduct.Source = "ic_product.png";
                imgStudent.Source = "ic_student_grey.png";
            }
            else if (currentItemPosition == 1)
            {
                MerchantOrderStudentView.LoadStudentOrder.Execute(null);

                imgProduct.Source = "ic_product_grey.png";
                imgStudent.Source = "ic_student.png";
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

                    var t = srvc.PostMerchantUpdateOrderStatus(_school_id, Settings.merchantId, _order_id, order_status_id, Settings.fullName, _pickup_date);
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