using System.Collections.Generic;


namespace SmartSchoolsV2.Models
{
    public class Transaction
    {
        public string Create_month { get; set; }
        public List<TransactionHistory> TxnHistory { get; set; }

        public bool IsVisible { get; set; } = false;

        public Transaction()
        {
        }

        public Transaction(string create_month, List<TransactionHistory> txnhistory)
        {
            Create_month = create_month;
            TxnHistory = txnhistory;
        }
    }
}
