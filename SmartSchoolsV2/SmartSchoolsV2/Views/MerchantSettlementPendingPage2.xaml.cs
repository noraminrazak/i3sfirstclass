using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantSettlementPendingPage2 : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _merchant_id;
        public string _receipt_date;
        private bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }
        public MerchantSettlementPendingPage2(int merchant_id, string receipt_date)
        {
            InitializeComponent();
            BindingContext = this;

            _merchant_id = merchant_id;
            _receipt_date = receipt_date;

            Title = AppResources.PendingText + " (" + receipt_date + ")";
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                MerchantSalesMethod();
                //GetSalesDetails();
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex.Message);
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        public async void MerchantSalesMethod()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostMerchantSalesMethod(_merchant_id, Convert.ToDateTime(_receipt_date));
                    string jsonStr = await t;
                    MerchantSalesMethodProperty response = JsonConvert.DeserializeObject<MerchantSalesMethodProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<MerchantSalesMethod> list = new List<MerchantSalesMethod>();
                        foreach (MerchantSalesMethod sl in response.Data)
                        {
                            MerchantSalesMethod post = new MerchantSalesMethod();
                            post.merchant_id = sl.merchant_id;
                            post.sales_method = sl.sales_method;
                            post.total_amount = "RM " + sl.total_amount;

                            DateTime _ceate_at = Convert.ToDateTime(sl.receipt_date);
                            post.receipt_date = _ceate_at.ToString("dd/MM/yyyy");
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
                        lvSales.Footer = l;
                        lvSales.ItemsSource = list;
                    }
                    else
                    {
                        List<MerchantSalesMethod> list = new List<MerchantSalesMethod>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvSales.Footer = l;
                        lvSales.ItemsSource = list;
                    }
                    //IsBusy = false;
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy = false;
                }
                //HideLoadingPopup();
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }
    }
}