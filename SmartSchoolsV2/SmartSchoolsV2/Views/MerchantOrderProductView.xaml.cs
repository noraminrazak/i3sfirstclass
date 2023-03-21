using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using System;
using System.Collections.ObjectModel;
using SmartSchoolsV2.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantOrderProductView : ContentView
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<ProductOrderHistory> listProductOrder { get; set; }
        public static Command LoadProductOrder { get; set; }

        public string _photo_url;
        ViewCell lastCell;
        bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public MerchantOrderProductView()
        {
            InitializeComponent();
            this.BindingContext = this;

            LoadProductOrder = new Command(async () => await ProductOrderHistory());
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await ProductOrderHistory();
                });
            }
        }
        public async Task ProductOrderHistory()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvStudent.ItemsSource = null;
                    var t = srvc.PostMerchantProductOrderHistory(Settings.merchantId, Settings.selectedSchoolId, Settings.selectedClassId, Settings.pickupDate.AddDays(1));
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
                        lvStudent.Footer = l;
                        lvStudent.ItemsSource = listProductOrder;
                    }
                    else
                    {
                        listProductOrder = new ObservableCollection<ProductOrderHistory>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvStudent.Footer = l;
                        lvStudent.ItemsSource = listProductOrder;
                    }
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
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.LightGoldenrodYellow;
                lastCell = viewCell;
            }
        }
        public ProductOrderHistory prod;
        void OnProductSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as ProductOrderHistory;
            if (data == null) return;
            prod = data;

            if (prod.product_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //await Navigation.PushAsync(new MerchantOrderStudentDetailPage(prod));
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}