using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantOrderStudentDetailPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ProductOrderHistory param;
        public MerchantOrderStudentDetailPage(ProductOrderHistory value)
        {
            InitializeComponent();
            BindingContext = this;
            param = value;
            lblTitle.Text = value.product_name;
            lblDeliveryDate.Text = "Delivery on " + value.pickup_date.ToString("dddd, dd/MM/yyyy");
            lblSchoolName.Text = value.school_name;
            lblClassName.Text = value.class_name;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MerchantProductOrderHistoryDetail();
        }
        public ObservableCollection<StudentOrderHistory> listStudentOrder { get; set; }
        public async void MerchantProductOrderHistoryDetail()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvStudent.ItemsSource = null;
                    var t = srvc.PostMerchantProductOrderHistoryDetail(Settings.merchantId, param.school_id, param.class_id, param.pickup_date, param.product_id);
                    string jsonStr = await t;
                    StudentOrderHistoryProperty response = JsonConvert.DeserializeObject<StudentOrderHistoryProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listStudentOrder = new ObservableCollection<StudentOrderHistory>();
                        foreach (StudentOrderHistory sl in response.Data)
                        {
                            StudentOrderHistory post = new StudentOrderHistory();
                            post.pickup_date = sl.pickup_date;
                            post.recipient_id = sl.recipient_id;
                            post.full_name = sl.full_name;
                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url_student = sl.photo_url;
                                post.photo_url = requestUrl + sl.photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.image_visible = false;
                                post.initial_visible = true;
                            }
                            post.product_id = sl.product_id;
                            post.product_name = sl.product_name;
                            post.product_photo_url = sl.product_photo_url;
                            post.product_qty = sl.product_qty;
                            post.total_amount = sl.total_amount;
                            post.str_total_amount = "RM " + sl.total_amount;
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            post.class_id = sl.class_id;
                            post.class_name = sl.class_name;
                            listStudentOrder.Add(post);
                        }
                        RowCount = listStudentOrder.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvStudent.Footer = l;
                        lvStudent.ItemsSource = listStudentOrder;
                    }
                    else
                    {
                        listStudentOrder = new ObservableCollection<StudentOrderHistory>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvStudent.Footer = l;
                        lvStudent.ItemsSource = listStudentOrder;
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
            else
            {

            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
    }
}