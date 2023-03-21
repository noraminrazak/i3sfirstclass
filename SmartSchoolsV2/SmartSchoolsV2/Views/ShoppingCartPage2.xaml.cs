using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingCartPage2 : ContentPage, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public string _wallet_number;
        public static Command LoadPurchaseOrderHistory { get; set; }

        private bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ShoppingCartPage2()
        {
            InitializeComponent();
            BindingContext = this;
            LoadPurchaseOrderHistory = new Command(async () => await PurchaseOrderHistory());

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await PurchaseOrderHistory();
        }

        async Task PurchaseOrderHistory() 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostPurchaseOrderHistory(Settings.profileId, Settings.walletId);
                    string jsonStr = await t;
                    OrderHistoryMasterProperty response = JsonConvert.DeserializeObject<OrderHistoryMasterProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<OrderHistoryMaster> list = new List<OrderHistoryMaster>();
                        foreach (OrderHistoryMaster sl in response.Data)
                        {
                            OrderHistoryMaster post = new OrderHistoryMaster();
                            post.order_id = sl.order_id;
                            post.profile_id = sl.profile_id;
                            post.wallet_id = sl.wallet_id;
                            post.wallet_number = sl.wallet_number;
                            post.reference_number = sl.reference_number;
                            post.full_name = sl.full_name;
                            post.transaction_method = AppResources.OrderNoText + sl.reference_number;
                            post.transaction_type = AppResources.OnlineOrderText;
                            post.total_amount = "RM " + sl.total_amount;
                            post.amount_color = Color.Red.ToHex();
                            post.order_status_id = sl.order_status_id;
                            if (sl.order_status_id == 3) 
                            {
                                post.status_color = "#0080FF";
                            } 
                            else if (sl.order_status_id == 4) 
                            {
                                post.status_color = "#FFAD00";
                            } 
                            else if (sl.order_status_id == 5) 
                            {
                                post.status_color = "#008000";
                            }
                            if (Settings.cultureInfo == "en-US")
                            {
                                post.order_status = sl.order_status;
                                post.payment_method = sl.payment_method;
                            }
                            else if (Settings.cultureInfo == "ms-MY")
                            {
                                post.order_status = sl.order_status_bm;
                                post.payment_method = sl.payment_method_bm;
                            }

                            DateTime _ceate_at = Convert.ToDateTime(sl.create_at);
                            post.create_at = _ceate_at.ToString("dd/MM/yyyy HH:mm tt");
                            list.Add(post);
                        }
                        RowCount = list.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvOrderHistory.Footer = l;
                        lvOrderHistory.ItemsSource = list;
                    }
                    else
                    {
                        List<OrderHistoryMaster> list = new List<OrderHistoryMaster>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvOrderHistory.Footer = l;
                        lvOrderHistory.ItemsSource = list;
                    }
                    //IsBusy = false;
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
                //HideLoadingPopup();
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await PurchaseOrderHistory();
                });
            }
        }

        public OrderHistoryMaster history;
        async void OnOrderSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as OrderHistoryMaster;
            if (data == null) return;
            history = data;

            if (history.order_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...

                await Navigation.PushAsync(new TransactionDetailPage2(data.order_id, data.profile_id,data.wallet_id,data.order_status_id,data.wallet_number, data.reference_number));

                ((ListView)sender).SelectedItem = null;
            }
        }

        async void Delete_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            OrderHistoryMaster master = item.BindingContext as OrderHistoryMaster;

            if (master.order_status_id == 2)
            {
                bool answer = await App.Current.MainPage.DisplayAlert(AppResources.ConfirmDeleteText, AppResources.DoYouReallyWantToRemoveOrderText + "?", AppResources.YesText, AppResources.CancelText);
                if (answer)
                {
                    await DeleteOrderMaster(master.order_id, master.profile_id, master.wallet_id, master.full_name);
                }
            }
            else {
                await App.Current.MainPage.DisplayAlert(AppResources.SorryText, AppResources.SuccessOrderCannotDeleteText, "OK");
            }

        }

        public async Task DeleteOrderMaster(int order_id, int profile_id, int wallet_id, string full_name)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostPurchaseRemoveOrderMaster(order_id, profile_id, wallet_id, full_name);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await PurchaseOrderHistory();
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