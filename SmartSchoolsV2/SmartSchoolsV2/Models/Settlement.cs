using System.Collections.Generic;

namespace SmartSchoolsV2.Models
{
    public class Settlement
    {
        public string receipt_date { get; set; }
        public string hdr_total_amount { get; set; }
        public string hdr_settlement_amount { get; set; }
        public string footer { get; set; }
        public List<SettlementReport> listReport { get; set; }

        public bool is_visible { get; set; } = false;

        public Settlement()
        {
        }

        public Settlement(string receiptDate, string totalAmt, string settlementAmt, List<SettlementReport> listReports, string fooTer)
        {
            receipt_date = receiptDate;
            hdr_total_amount = totalAmt;
            hdr_settlement_amount = settlementAmt;
            listReport = listReports;
            footer = fooTer;
        }
    }
}
