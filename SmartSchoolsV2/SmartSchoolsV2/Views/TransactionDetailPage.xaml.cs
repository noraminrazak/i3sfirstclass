using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionDetailPage : ContentPage, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();

        public int _transaction_id;
        public int _transaction_type_id;
        public string _reference_number;
        public string _wallet_number;
        public string _wallet_reference_number;
        public string _full_name;
        public string _full_name_reference;
        public string _status_code;
        bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        bool isBusy2;
        public bool IsBusy2
        {
            get => isBusy2;
            set
            {
                isBusy2 = value;
                OnPropertyChanged();
            }
        }
        public TransactionDetailPage(int transaction_id, int transaction_type_id, string reference_number, 
            string wallet_number, string full_name, string wallet_number_reference, string full_name_reference)
        {
            InitializeComponent();
            BindingContext = this;
            _transaction_id = transaction_id;
            _transaction_type_id = transaction_type_id;
            _reference_number = reference_number;
            _wallet_number = wallet_number;
            _full_name = full_name;
            _full_name_reference = full_name_reference;
            _wallet_reference_number = wallet_number_reference;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            TransactionReference(_transaction_id, _reference_number);
            if (_transaction_type_id == 4)
            {
                lblTxnRow2Col0.IsVisible = false;
                lblTxnRow2Col1.IsVisible = false;
                bvTxnRow2.IsVisible = false;
                stackTotal.IsVisible = true;
                //stackHeader.IsVisible = true;
                lvTxnDetail.IsVisible = true;
                TransactionDetail(_wallet_number, _reference_number);
                TransactionMaster(_wallet_number, _reference_number);
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        public async void TransactionReference(int transaction_id, string reference_number) 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostWalletTransactionReference(transaction_id,reference_number);
                    string jsonStr = await t;
                    TransactionHistoryProperty response = JsonConvert.DeserializeObject<TransactionHistoryProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        foreach (TransactionHistory sl in response.Data)
                        {
                            if (Settings.cultureInfo == "en-US")
                            {
                                lblTxnRow0Col1.Text = sl.transaction_type;
                            }
                            else if (Settings.cultureInfo == "ms-MY")
                            {
                                lblTxnRow0Col1.Text = sl.transaction_type_bm;
                            }

                            if (_transaction_type_id == 1) 
                            {
                                lblTxnRow1Col0.Text = AppResources.TopupToText;
                                lblTxnRow1Col1.Text = _full_name;
                            }
                            else if (_transaction_type_id == 2)
                            {
                                lblTxnRow1Col0.Text = AppResources.TransferToText;
                                lblTxnRow1Col1.Text = _full_name_reference;
                            }
                            else if (_transaction_type_id == 3)
                            {
                                lblTxnRow1Col0.Text = AppResources.ReceivedFromText;
                                lblTxnRow1Col1.Text = _full_name_reference;
                            }
                            else if (_transaction_type_id == 4)
                            {
                                lblTxnRow1Col0.Text = AppResources.PaymentToText;
                                lblTxnRow1Col1.Text = _full_name_reference;
                            }
                            lblTxnRow2Col1.Text = "RM " + sl.amount;
                            DateTime _ceate_at = Convert.ToDateTime(sl.create_at);
                            lblTxnRow3Col1.Text = _ceate_at.ToString("dd/MM/yyyy HH:mm tt");
                            if (Settings.cultureInfo == "en-US")
                            {
                                lblTxnRow4Col1.Text = sl.status_code;
                            }
                            else if (Settings.cultureInfo == "ms-MY")
                            {
                                lblTxnRow4Col1.Text = sl.status_code_bm;
                            }

                            lblTxnRow5Col1.Text = sl.reference_number;
                        }
                    }
                    else
                    {

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

        public async void TransactionDetail(string wallet_number, string reference_number)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy2 = true;
                    var t = srvc.PostWalletTransactionDetail(wallet_number, reference_number);
                    string jsonStr = await t;
                    TransactionDetailProperty response = JsonConvert.DeserializeObject<TransactionDetailProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<TransactionDetail> list = new List<TransactionDetail>();
                        foreach (TransactionDetail sl in response.Data)
                        {
                            TransactionDetail post = new TransactionDetail();
                            post.rcpt_detail_id = sl.rcpt_detail_id;
                            post.product_id = sl.product_id;
                            post.product_name = sl.product_name;
                            post.product_qty = sl.product_qty.ToString() + " X ";
                            post.unit_price = "RM " + sl.unit_price;
                            post.sub_total_amount = "RM " + sl.sub_total_amount;
                            post.total_amount = "RM " + sl.total_amount;
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
                        lvTxnDetail.Footer = l;
                        lvTxnDetail.ItemsSource = list;
                    }
                    else
                    {
                        List<TransactionHistory> list = new List<TransactionHistory>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvTxnDetail.Footer = l;
                        lvTxnDetail.ItemsSource = list;
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
                    IsBusy2 = false;
                }
                //HideLoadingPopup();
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async void TransactionMaster(string wallet_number, string reference_number)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostWalletTransactionMaster(wallet_number, reference_number);
                    string jsonStr = await t;
                    CartTotalProperty response = JsonConvert.DeserializeObject<CartTotalProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        foreach (CartTotal r in response.Data)
                        {
                            lblSubTotal.Text = "RM " + r.sub_total_amount.ToString();
                            if (r.sub_total_amount.ToString().Length >= 5)
                            {
                                lblTaxAmount.Text = "RM   " + r.tax_amount.ToString();
                            }
                            else
                            {
                                lblTaxAmount.Text = "RM    " + r.tax_amount.ToString();
                            }
                            lblTaxrate.Text = AppResources.ServiceChargeText + r.tax_rate.ToString() + "% : ";
                            lblTotal.Text = "RM " + r.total_amount.ToString();
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
}