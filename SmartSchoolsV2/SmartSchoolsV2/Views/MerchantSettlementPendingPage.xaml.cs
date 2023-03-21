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
    public partial class MerchantSettlementPendingPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _merchant_id;

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
        public MerchantSettlementPendingPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                _merchant_id = Settings.merchantId;
                MerchantSales(_merchant_id);
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

        public async void MerchantSales(int merchant_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostMerchantSales(merchant_id);
                    string jsonStr = await t;
                    MerchantSalesProperty response = JsonConvert.DeserializeObject<MerchantSalesProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<MerchantSales> list = new List<MerchantSales>();
                        foreach (MerchantSales sl in response.Data)
                        {
                            MerchantSales post = new MerchantSales();
                            post.merchant_id = sl.merchant_id;
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
                        List<MerchantSales> list = new List<MerchantSales>();
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
        public MerchantSales trans;
        async void OnSalesSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as MerchantSales;
            if (data == null) return;
            trans = data;

            if (trans.merchant_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                await Navigation.PushAsync(new MerchantSettlementPendingPage2(data.merchant_id, data.receipt_date));

                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}