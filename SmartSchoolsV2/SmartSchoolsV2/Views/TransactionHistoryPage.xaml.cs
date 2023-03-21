using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionHistoryPage : ContentPage, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
        public int RowCount { get; set; }
        public static Label l = new Label();
        public string _wallet_number;
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
        public TransactionHistoryPage()
        {
            InitializeComponent();
            BindingContext = this;
            //TransactionGroupViewModel viewModel = new TransactionGroupViewModel();
            //this.ViewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                _wallet_number = Settings.walletNumber;
                TransactionHistory(_wallet_number);
                //GetSalesDetails();
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex.Message);
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        public async void TransactionHistory(string wallet_number)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostWalletTransactionHistory(wallet_number);
                    string jsonStr = await t;
                    TransactionHistoryProperty response = JsonConvert.DeserializeObject<TransactionHistoryProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<TransactionHistory> list = new List<TransactionHistory>();
                        foreach (TransactionHistory sl in response.Data)
                        {
                            TransactionHistory post = new TransactionHistory();
                            post.transaction_id = sl.transaction_id;
                            post.transaction_type_id = sl.transaction_type_id;
                            post.transaction_type = sl.transaction_type;
                            post.reference_number = sl.reference_number;
                            post.wallet_number = sl.wallet_number;
                            post.wallet_number_reference = sl.wallet_number_reference;
                            post.full_name = sl.full_name;
                            post.full_name_reference = sl.full_name_reference;
                            if (sl.transaction_type_id == 1)
                            {
                                post.transaction_method = AppResources.TopupEWalletText;
                                post.transaction_type = AppResources.TopupModeText;
                                post.amount = "+RM " + sl.amount;
                                post.amount_color = Color.Green.ToHex();
                            }
                            else if (sl.transaction_type_id == 2)
                            {
                                post.transaction_method = AppResources.TransferToText + sl.full_name_reference;
                                post.transaction_type = AppResources.TransferToEWallet;
                                post.amount = "-RM " + sl.amount;
                                post.amount_color = Color.Red.ToHex();
                            }
                            else if (sl.transaction_type_id == 3)
                            {
                                post.transaction_method = AppResources.ReceivedFromText + sl.full_name_reference;
                                post.transaction_type = AppResources.ReceivedFromEWallet;
                                post.amount = "+RM " + sl.amount;
                                post.amount_color = Color.Green.ToHex();
                            }
                            else if (sl.transaction_type_id == 4)
                            {
                                post.transaction_method = AppResources.PaymentToText + sl.full_name_reference;
                                post.transaction_type = AppResources.PaymentText;
                                post.amount = "-RM " + sl.amount;
                                post.amount_color = Color.Red.ToHex();
                            }
                            else if (sl.transaction_type_id == 5)
                            {
                                post.transaction_method = AppResources.OrderNoText + sl.reference_number;
                                post.transaction_type = AppResources.OnlineOrderText;
                                post.amount = "-RM " + sl.amount;
                                post.amount_color = Color.Red.ToHex();
                            }
                            post.status_id = sl.status_id;
                            if (Settings.cultureInfo == "en-US")
                            {
                                post.status_code = sl.status_code;
                            }
                            else if (Settings.cultureInfo == "ms-MY")
                            {
                                post.status_code = sl.status_code_bm;
                            }
                            DateTime _ceate_at = Convert.ToDateTime(sl.create_at);
                            post.create_at = _ceate_at.ToString("dd/MM/yyyy HH:mm tt");
                            post.create_month = _ceate_at.ToString("MMM yyyy", ci);
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
                        lvTxnHistory.Footer = l;
                        lvTxnHistory.ItemsSource = list;
                    }
                    else
                    {
                        List<TransactionHistory> list = new List<TransactionHistory>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvTxnHistory.Footer = l;
                        lvTxnHistory.ItemsSource = list;
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
        public TransactionHistory trans;
        async void OnTxnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as TransactionHistory;
            if (data == null) return;
            trans = data;

            if (trans.transaction_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                if (trans.transaction_type_id != 5)
                {
                    await Navigation.PushAsync(new TransactionDetailPage(data.transaction_id, data.transaction_type_id,
                        data.reference_number, data.wallet_number, data.full_name, data.wallet_number_reference, data.full_name_reference));
                }
                else {
                    await Navigation.PushAsync(new TransactionDetailPage3(data.transaction_id, data.wallet_number, data.reference_number, data.full_name, data.create_at,data.status_code));
                }


                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}