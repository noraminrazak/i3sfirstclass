using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantOrderProductDetailPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);

        public int RowCount { get; set; }
        public static Label l = new Label();
        public StudentOrderHistory param;
        public MerchantOrderProductDetailPage(StudentOrderHistory value)
        {
            InitializeComponent();
            BindingContext = this;
            param = value;
            lblTitle.Text = value.full_name;
            lblDeliveryDate.Text = AppResources.DeliveryOnText + value.pickup_date.ToString("dddd, dd/MM/yyyy", ci);
            lblSchoolName.Text = value.school_name;
            lblClassName.Text = value.class_name;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MerchantProductOrderHistoryDetail();
        }
        public ObservableCollection<ProductOrderHistory> listProductOrder { get; set; }
        public async void MerchantProductOrderHistoryDetail()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvProduct.ItemsSource = null;
                    var t = srvc.PostMerchantStudentOrderHistoryDetail(param.school_id, param.class_id, param.pickup_date, param.recipient_id);
                    string jsonStr = await t;
                    ProductOrderHistoryProperty response = JsonConvert.DeserializeObject<ProductOrderHistoryProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listProductOrder = new ObservableCollection<ProductOrderHistory>();
                        foreach (ProductOrderHistory sl in response.Data)
                        {
                            ProductOrderHistory post = new ProductOrderHistory();
                            post.pickup_date = sl.pickup_date;
                            post.product_id = sl.product_id;
                            post.product_name = sl.product_name;
                            if (!string.IsNullOrEmpty(sl.product_photo_url))
                            {
                                post.product_photo_url = requestUrl + sl.product_photo_url;
                            }
                            else
                            {
                                post.product_photo_url = requestUrl + "/images/sorry-no-image.jpg";
                            }
                            post.product_qty = sl.product_qty;
                            post.product_total = sl.product_qty.ToString() + "  X  " + sl.product_name;
                            post.total_amount = sl.total_amount;
                            post.str_total_amount = "RM " + sl.total_amount;
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            post.class_id = sl.class_id;
                            post.class_name = sl.class_name;
                            listProductOrder.Add(post);
                        }
                        RowCount = listProductOrder.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvProduct.Footer = l;
                        lvProduct.ItemsSource = listProductOrder;
                    }
                    else
                    {
                        listProductOrder = new ObservableCollection<ProductOrderHistory>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvProduct.Footer = l;
                        lvProduct.ItemsSource = listProductOrder;
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    IsBusy = false;
                }
            }
            else { 
            
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
    }
}