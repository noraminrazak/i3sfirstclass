using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SmartSchoolsV2.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.ViewModels
{
    public class TransactionGroupViewModel : BaseViewModel
    {
        private TransactionViewModel _oldTransaction;

        //public TransactionPage page = new TransactionPage();

        public ObservableCollection<TransactionViewModel> filteredTxnReceipts;

        private ObservableCollection<TransactionViewModel> txnhistory;
        public ObservableCollection<TransactionViewModel> TxnHistory
        {
            get => txnhistory;

            set => SetProperty(ref txnhistory, value);
        }

        public Command LoadTxnHistoryCommand { get; set; }
        public Command<TransactionViewModel> RefreshTxnReceiptsCommand { get; set; }
        //public Command SearchTxnReceiptsCommand
        //{
        //    get
        //    {
        //        return new Command(() => ExecuteSearchReceiptsCommand());
        //    }
        //}
        public TransactionGroupViewModel()
        {
            txnhistory = new ObservableCollection<TransactionViewModel>();
            TxnHistory = new ObservableCollection<TransactionViewModel>();
            filteredTxnReceipts = new ObservableCollection<TransactionViewModel>();
            LoadTxnHistoryCommand = new Command(async () => await ExecuteLoadItemsCommandAsync());
            RefreshTxnReceiptsCommand = new Command<TransactionViewModel>((items) => ExecuteRefreshItemsCommand(items));
        }

        public bool isExpanded = false;
        private void ExecuteRefreshItemsCommand(TransactionViewModel item)
        {
            if (_oldTransaction == item)
            {
                // click twice on the same item will hide it
                item.Expanded = !item.Expanded;
            }
            else
            {
                if (_oldTransaction != null)
                {
                    // hide previous selected item
                    _oldTransaction.Expanded = false;
                }
                // show selected item
                item.Expanded = true;
            }

            _oldTransaction = item;
        }
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        async Task ExecuteLoadItemsCommandAsync()
        {
            try
            {
                //ShowLoadingPopup();
                List<TransactionHistory> list = new List<TransactionHistory>();
                string create_month = string.Empty;
                var t = srvc.PostWalletTransactionHistory(Settings.walletNumber);
                string jsonStr = await t;
                TransactionHistoryProperty response = JsonConvert.DeserializeObject<TransactionHistoryProperty>(jsonStr);
                if (response.Success == true)
                {

                    foreach (TransactionHistory sl in response.Data)
                    {
                        TransactionHistory post = new TransactionHistory();
                        post.transaction_id = sl.transaction_id;
                        post.transaction_type_id = sl.transaction_type_id;
                        post.transaction_type = sl.transaction_type;

                        if (sl.transaction_type_id == 1)
                        {
                            post.full_name = "Topup eWallet";
                            post.transaction_type = "Topup via {method}";
                            post.amount = "+RM " + sl.amount;
                            post.amount_color = Color.Green.ToHex();
                        }
                        else if (sl.transaction_type_id == 2)
                        {
                            post.full_name = "Transfer to " + sl.full_name_reference;
                            post.transaction_type = "Transfer to eWallet";
                            post.amount = "-RM " + sl.amount;
                            post.amount_color = Color.Red.ToHex();
                        }
                        else if (sl.transaction_type_id == 3)
                        {
                            post.full_name = "Received from " + sl.full_name_reference;
                            post.transaction_type = "Received from eWallet";
                            post.amount = "+RM " + sl.amount;
                            post.amount_color = Color.Green.ToHex();
                        }
                        else if (sl.transaction_type_id == 4)
                        {
                            post.full_name = "Payment to " + sl.full_name_reference;
                            post.transaction_type = "Purchased";
                            post.amount = "-RM " + sl.amount;
                            post.amount_color = Color.Red.ToHex();
                        }
                        DateTime _ceate_at = Convert.ToDateTime(sl.create_at);
                        post.create_at = _ceate_at.ToString("dd MMM, HH:mm tt");
                        post.create_month = _ceate_at.ToString("MMM yyyy");
                        create_month = _ceate_at.ToString("MMM yyyy");
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
                    //lvTxnHistory.Footer = l;
                    //lvTxnHistory.ItemsSource = list;
                }
                else
                {
                    list = new List<TransactionHistory>();
                    l.HorizontalTextAlignment = TextAlignment.Center;
                    l.Text = AppResources.NoRecordFoundText;
                    //lvTxnHistory.Footer = l;
                    //lvTxnHistory.ItemsSource = list;
                }
                Transaction transaction = new Transaction(create_month, list);
                TxnHistory.Add(new TransactionViewModel(transaction));
            }
            catch (Exception)
            {
                //SnackB.Message = AppResources.SomethingWrongText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }
        public int PublicReceiptCount => TxnHistory.Count;

        private String LocalReceiptString;
        public String PublicReceiptString
        {
            get
            {
                return LocalReceiptString;
            }
            set
            {
                SetProperty(ref LocalReceiptString, value);
            }
        }

    }
}
