using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartSchoolsV2.ViewModels
{
    public class SettlementGroupViewModel : BaseViewModel
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
        public string requestUrl = Settings.requestUrl;
        private SettlementViewModel _oldOrder;
        public ObservableCollection<SettlementViewModel> filteredlistReport;
        private ObservableCollection<SettlementViewModel> listReport;
        public ObservableCollection<SettlementViewModel> ListReports
        {
            get => listReport;

            set => SetProperty(ref listReport, value);
        }
        public Command LoadReportCommand { get; set; }
        public Command<SettlementViewModel> RefreshReportCommand { get; set; }
        public RelayCommand<SettlementReportViewModel> SelectedReportCommand { get; set; }
        public SettlementGroupViewModel()
        {
            listReport = new ObservableCollection<SettlementViewModel>();
            ListReports = new ObservableCollection<SettlementViewModel>();
            filteredlistReport = new ObservableCollection<SettlementViewModel>();
            LoadReportCommand = new Command(async () => await ExecuteLoadReportCommand());
            RefreshReportCommand = new Command<SettlementViewModel>((items) => ExecuteRefreshOrderCommand(items));

            //SelectedReportCommand = new RelayCommand<SettlementReportViewModel>(GetOrderHistoryPage);
        }

        public async void GetSettlementReportPage(SettlementReportViewModel settle)
        {
            Settings.receiptDate = settle.report.receipt_date;
            //Settings.merchantId = settle.report.merchant_id;
            Settings.selectedSchoolId = settle.report.school_id;

            //if (settle.report.class_id > 0)
            //{
            //    await App.Current.MainPage.Navigation.PushAsync(new MerchantOrderHistoryPage(settle.report));
            //}
            //else
            //{
            //    await App.Current.MainPage.Navigation.PushAsync(new MerchantOrderHistoryPage2(settle.order));
            //}
        }
        public bool isExpanded = false;
        private void ExecuteRefreshOrderCommand(SettlementViewModel item)
        {
            if (item != null)
            {
                if (_oldOrder == item)
                {
                    // click twice on the same item will hide it
                    item.Expanded = !item.Expanded;
                }
                else
                {
                    if (_oldOrder != null)
                    {
                        // hide previous selected item
                        _oldOrder.Expanded = false;
                    }
                    // show selected item
                    item.Expanded = true;
                }

                _oldOrder = item;
            }
        }
        async Task ExecuteLoadReportCommand()
        {
            try
            {
                IsBusy = true;

                var t = srvc.PostMerchantSettlementGroup(Settings.merchantId, Settings.merchantSchoolId);
                string jsonStr = await t;
                SettlementReportGroupProperty response = JsonConvert.DeserializeObject<SettlementReportGroupProperty>(jsonStr);
                if (response.Success == true)
                {
                    ListReports.Clear();
                    int _count = 0;
                    if (response.Data.Count > 0)
                    {
                        foreach (SettlementReportGroup g in response.Data)
                        {
                            var h = srvc.PostMerchantSettlement(Settings.merchantId, Settings.merchantSchoolId, g.receipt_date);
                            string jsonStr2 = await h;
                            SettlementReportProperty response2 = JsonConvert.DeserializeObject<SettlementReportProperty>(jsonStr2);
                            if (response2.Success == true)
                            {
                                string str_total_amount = string.Empty;
                                decimal total_amount = 0;
                                string str_settlement_amount = string.Empty;
                                decimal settlement_amount = 0;
                                string str_receipt_date = string.Empty;
                                List<SettlementReport> listReport = new List<SettlementReport>();
                                foreach (SettlementReport n in response2.Data)
                                {
                                    SettlementReport rpt = new SettlementReport();
                                    rpt.merchant_id = n.merchant_id;
                                    rpt.school_id = n.school_id;
                                    rpt.school_name = n.school_name;
                                    rpt.receipt_date = n.receipt_date;
                                    rpt.total_amount = "RM " + n.total_amount;
                                    rpt.settlement_amount = "RM " + Math.Round(Convert.ToDecimal(n.settlement_amount), 2);
                                    rpt.fee_amount = "RM " + Math.Round(Convert.ToDecimal(n.fee_amount), 2);
                                    rpt.net_amount = "RM " + Math.Round(Convert.ToDecimal(n.net_amount), 2);
                                    rpt.sales_method_id = n.sales_method_id;
                                    if (Settings.cultureInfo == "en-US")
                                    {
                                        rpt.sales_method = n.sales_method;
                                    }
                                    else if (Settings.cultureInfo == "ms-MY")
                                    {
                                        rpt.sales_method = n.sales_method_bm;
                                    }
                                    //rpt.status_id = n.status_id;
                                    if (Settings.cultureInfo == "en-US")
                                    {
                                        rpt.status = n.status;
                                    }
                                    else if (Settings.cultureInfo == "ms-MY")
                                    {
                                        rpt.status = n.status_bm;
                                    }
                                    if (n.status == "00")
                                    {
                                        rpt.status_color = Color.Green.ToHex();
                                    }
                                    else {
                                        rpt.status_color = Color.Red.ToHex();
                                    }
                                    //rpt.payment_date = n.payment_date;
                                    //rpt.reference_number = n.reference_number;
                                    listReport.Add(rpt);

                                    str_receipt_date = g.receipt_date.ToString("dddd, dd/MM/yyyy", ci);
                                    total_amount = total_amount + Convert.ToDecimal(n.total_amount);
                                    str_total_amount = "RM " + total_amount.ToString("N2");
                                    settlement_amount = settlement_amount + Convert.ToDecimal(n.settlement_amount);
                                    str_settlement_amount = "RM " + Math.Round(settlement_amount, 2).ToString();

                                }
                                _count++;

                                Settlement settle = new Settlement(str_receipt_date, str_total_amount, str_settlement_amount, listReport, _count.ToString() + AppResources.RecordText);
                                ListReports.Add(new SettlementViewModel(settle));
                            }
                        }
                    }
                    else
                    {
                        Settlement settle = new Settlement("", "", "", null, AppResources.NoRecordFoundText);
                        ListReports.Add(new SettlementViewModel(settle));
                    }

                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }


        public int PublicSettlementReport => ListReports.Count;

        private String LocalReportString;
        public String PublicReportString
        {
            get
            {
                return LocalReportString;
            }
            set
            {
                SetProperty(ref LocalReportString, value);
            }
        }
    }
}
