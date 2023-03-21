using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceiptDetailPopupPage : PopupPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;

        public delegate void SetDetailEventHandler(object source, EventArgs args);

        public event SetDetailEventHandler DetailSet;
        protected virtual void OnDetailSet()
        {
            if (DetailSet != null)
            {
                DetailSet(this, EventArgs.Empty);
            }
        }
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _rcpt_id;
        public string _wallet_number;
        public string _reference_number;
        public string _full_name;
        public string _total_amount;
        public string _receipt_date;
        bool isBusy;
        public bool IsBusy2
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }

        public ReceiptDetailPopupPage(int rcpt_id, string wallet_number, string reference_number, string full_name,
            string total_amount, string receipt_date)
        {
            InitializeComponent();
            BindingContext = this;

            _rcpt_id = rcpt_id;
            _wallet_number = wallet_number;
            _reference_number = reference_number;
            _full_name = full_name;
            _total_amount = total_amount;
            _receipt_date = receipt_date;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            lblRcptRow0Col1.Text = _full_name;
            lblRcptRow1Col1.Text = _reference_number;
            lblRcptRow2Col1.Text = _total_amount;
            lblRcptRow3Col1.Text = _receipt_date;

            TransactionDetail(_wallet_number, _reference_number);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
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
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        private void OnClose_Tapped(object sender, System.EventArgs e)
        {
            MerchantTerminalPage.Back = "Y";
            CloseAllPopup();
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}