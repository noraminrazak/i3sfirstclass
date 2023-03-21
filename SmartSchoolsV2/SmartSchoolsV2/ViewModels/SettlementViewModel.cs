using MvvmHelpers;
using SmartSchoolsV2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SmartSchoolsV2.ViewModels
{
    public class SettlementViewModel : ObservableRangeCollection<SettlementReportViewModel>, INotifyPropertyChanged
    {
        private ObservableRangeCollection<SettlementReportViewModel> settleReport = new ObservableRangeCollection<SettlementReportViewModel>();
        public SettlementViewModel(Settlement settle, bool expanded = true) //expanded = false by default
        {
            this.settlement = settle;
            this._expanded = expanded;

            foreach (SettlementReport sr in settle.listReport)
            {
                settleReport.Add(new SettlementReportViewModel(sr));
            }
            if (expanded)
                this.AddRange(settleReport);
        }

        public SettlementViewModel()
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
                        this.AddRange(settleReport);
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
        public string receipt_date { get { return settlement.receipt_date; } }
        public string hdr_total_amount { get { return settlement.hdr_total_amount; } }
        public string hdr_settlement_amount { get { return settlement.hdr_settlement_amount; } }
        public string footer { get { return settlement.footer; } }
        public Settlement settlement { get; set; }
    }
}
