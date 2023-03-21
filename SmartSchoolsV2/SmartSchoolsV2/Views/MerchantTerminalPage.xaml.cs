using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using System.Collections.Generic;
using SmartSchoolsV2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantTerminalPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _terminal_id;
        public int _school_id;
        public int _merchant_id;
        public string _tag_number;
        public string _serial_number;
        public string _total_amount;
        public string _school_name;
        public string _receipt_date;
        bool isBusy;
        public bool IsBusy1
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public static string _back;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }
        public void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {

            }
        }
        public MerchantTerminalPage(int merchant_id, int school_id, string school_name, string receipt_date, int terminal_id, string tag_number, string serial_number, string total_amount)
        {
            InitializeComponent();
            BindingContext = this;
            _school_id = school_id;
            _merchant_id = merchant_id;
            _school_name = school_name;
            _receipt_date = receipt_date;
            _terminal_id = terminal_id;
            _tag_number = tag_number;
            _serial_number = serial_number;
            _total_amount = total_amount;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblTitleView.Text = _school_name + " - " + _receipt_date.Split('-')[2] + "/" + _receipt_date.Split('-')[1] + "/" + _receipt_date.Split('-')[0];
            lblTagNumber.Text = _tag_number;
            lblSerialNumber.Text = _serial_number;
            lblTotal.Text = _total_amount;

            TerminalReceipt();
        }

        public async void TerminalReceipt()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy1 = true;
                    var t = srvc.PostMerchantTerminalReceipt(_school_id,_merchant_id,_terminal_id,_receipt_date);
                    string jsonStr = await t;
                    TerminalReceiptProperty response = JsonConvert.DeserializeObject<TerminalReceiptProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<TerminalReceipt> list = new List<TerminalReceipt>();
                        foreach (TerminalReceipt sl in response.Data)
                        {
                            TerminalReceipt post = new TerminalReceipt();
                            post.rcpt_id = sl.rcpt_id;
                            post.wallet_id = sl.wallet_id;
                            post.wallet_number = sl.wallet_number;
                            post.full_name = sl.full_name;
                            post.reference_number = sl.reference_number;
                            post.total_amount = "RM " + sl.total_amount;
                            DateTime _ceate_at = Convert.ToDateTime(sl.receipt_date);
                            post.receipt_time = _ceate_at.ToString("HH:mm tt");
                            post.receipt_date = _ceate_at.ToString("dd/MM/yyyy HH:mm tt");
                            if (Settings.cultureInfo == "en-US")
                            {
                                post.payment_method = sl.payment_method;
                            }
                            else if (Settings.cultureInfo == "ms-MY")
                            {
                                post.payment_method = sl.payment_method_bm;
                            }

                            post.company_name = sl.company_name;
                            list.Add(post);
                        }
                        RowCount = list.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.FontFamily = "Roboto-Regular";
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvReceipt.Footer = l;
                        lvReceipt.ItemsSource = list;
                    }
                    else
                    {
                        List<TerminalReceipt> list = new List<TerminalReceipt>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.FontFamily = "Roboto-Regular";
                        l.Text = AppResources.NoRecordFoundText;
                        lvReceipt.Footer = l;
                        lvReceipt.ItemsSource = list;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy1 = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public TerminalReceipt rcpt;
        async void OnReceiptSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as TerminalReceipt;
            if (data == null) return;
            rcpt = data;

            if (rcpt.rcpt_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                ReceiptDetailPopupPage page = new ReceiptDetailPopupPage(rcpt.rcpt_id,rcpt.wallet_number,rcpt.reference_number,rcpt.full_name,
                    rcpt.total_amount,rcpt.receipt_date);
                page.DetailSet += this.OnDetailSet;
                await PopupNavigation.Instance.PushAsync(page);

                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}