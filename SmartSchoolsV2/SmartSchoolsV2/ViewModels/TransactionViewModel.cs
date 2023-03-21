using MvvmHelpers;
using SmartSchoolsV2.Models;
using System.ComponentModel;

namespace SmartSchoolsV2.ViewModels
{
    public class TransactionViewModel : ObservableRangeCollection<TransactionHistoryViewModel>, INotifyPropertyChanged
    {
        private ObservableRangeCollection<TransactionHistoryViewModel> tranReceipts = new ObservableRangeCollection<TransactionHistoryViewModel>();

        public TransactionViewModel(Transaction histroy, bool expanded = true) //expanded = false by default
        {
            this.TxnHistory = histroy;
            this._expanded = expanded;

            foreach (TransactionHistory history in histroy.TxnHistory)
            {
                tranReceipts.Add(new TransactionHistoryViewModel(history));
            }
            if (expanded)
                this.AddRange(tranReceipts);
        }

        public TransactionViewModel()
        {
        }

        private bool _expanded;
        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Expanded"));
                    OnPropertyChanged(new PropertyChangedEventArgs("StateIcon"));
                    if (_expanded)
                    {
                        this.AddRange(tranReceipts);
                    }
                    else
                    {
                        this.Clear();
                    }
                }
            }
        }

        public string StateIcon
        {
            get
            {
                if (Expanded)
                {
                    return "arrow_a.png";
                }
                else
                { return "arrow_b.png"; }
            }
        }
        public string create_month { get { return TxnHistory.Create_month; } }
        public Transaction TxnHistory { get; set; }
    }
}
