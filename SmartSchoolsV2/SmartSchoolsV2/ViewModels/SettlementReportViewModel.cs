using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartSchoolsV2.ViewModels
{
    public class SettlementReportViewModel
    {
        private SettlementReport _settleReport;
        public SettlementReportViewModel(SettlementReport report)
        {
            this._settleReport = report;
        }
        public int merchant_id { get { return _settleReport.merchant_id; } }
        public int school_id { get { return _settleReport.school_id; } }
        public string school_name { get { return _settleReport.school_name; } }
        public string total_amount { get { return _settleReport.total_amount; } }
        public string settlement_amount { get { return _settleReport.settlement_amount; } }
        public string fee_amount { get { return _settleReport.fee_amount; } }
        public string net_amount { get { return _settleReport.net_amount; } }
        public string sales_method { get { return _settleReport.sales_method; } }
        public string sales_method_bm { get { return _settleReport.sales_method_bm; } }
        public string status { get { return _settleReport.status; } }
        public string status_bm { get { return _settleReport.status_bm; } }
        public string reference_number { get { return _settleReport.status_bm; } }

        //public TextDecorations decoration { get { return TextDecorations.Strikethrough; } }
        public SettlementReport report
        {
            get => _settleReport;
        }
    }
}
