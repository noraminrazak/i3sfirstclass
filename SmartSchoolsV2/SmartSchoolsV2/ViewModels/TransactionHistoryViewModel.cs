using SmartSchoolsV2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.ViewModels
{
    public class TransactionHistoryViewModel
    {
        private TransactionHistory _txnHistory;

        public TransactionHistoryViewModel(TransactionHistory txnHistory)
        {
            this._txnHistory = txnHistory;
        }

        public int transaction_id { get { return _txnHistory.transaction_id; } }
        public string full_name { get { return _txnHistory.full_name; } }
        public string full_name_reference { get { return _txnHistory.full_name_reference; } }
        public string amount { get { return _txnHistory.amount; } }
        public string amount_color { get { return _txnHistory.amount_color; } }
        public int transaction_type_id { get { return _txnHistory.transaction_type_id; } }
        public string transaction_type { get { return _txnHistory.transaction_type; } }
        public string create_at { get { return _txnHistory.create_at; } }
        public TransactionHistory txnHistory
        {
            get => _txnHistory;
        }
    }
}
