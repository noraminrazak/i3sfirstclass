using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;
using System.Globalization;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(false)]
    public partial class AddToCartPage2 : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _school_id;
        public int _profile_id;
        public int _recipient_id;
        public int _recipient_role_id;
        public int _wallet_id;
        public int _merchant_type_id;
        public int _merchant_id;
        public int _user_role_id;
        public int _service_method_id;
        public string _str_service_method;
        public string _delivery_location;
        public string _company_name;
        public string _special_flag;
        public string _school_name;
        public string _class_name;
        public string _full_name;
        public string _dayOfWeek;
        public DateTime _pickup_date;
        public TimeSpan _pickup_time;
        private bool isBusy;
        public bool IsBusy2
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ProductDetail> listProduct { get; set; }
        public ObservableCollection<ProductCategory> Products { get; private set; } = new ObservableCollection<ProductCategory>();
        public class ProductCategory : ObservableCollection<ProductDetail>
        {
            public string category_name { get; private set; }

            public ProductCategory(string categoryName, ObservableCollection<ProductDetail> products) : base(products)
            {
                category_name = categoryName;
            }
        }
        public static string _back;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }

        public static string _option;
        public static string Option
        {
            get { return _option; }
            set { _option = value; }
        }
        public async void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
                //if (Option == "merchant")
                //{
                //    _merchant_id = Settings.selectMerchantId;
                //    txtCompanyName.Text = Settings.selectMerchantType;
                //    await MerchantProductCategory();
                //}
                //else
                if (Option == "add-to-cart")
                {
                    CartCount();
                }
            }
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await MerchantProductCategory();
                });
            }
        }
        
        public AddToCartPage2(int school_id, string school_name, string class_name, int recipient_id, string full_name, 
            DateTime pickup_date, TimeSpan pickup_time, int service_method_id, string delivery_location, int merchant_id)
        {
            InitializeComponent();
            BindingContext = this;

            _user_role_id = Settings.userRoleId;

            _service_method_id = service_method_id;

            if (_service_method_id == 1)
            {
                _str_service_method = AppResources.DeliveryBtnText;
            }
            else if (_service_method_id == 2) 
            {
                _str_service_method = AppResources.TakeAwayBtnText;
            }
            _profile_id = Settings.profileId;
            _wallet_id = Settings.walletId;
            _school_id = school_id;
            _school_name = school_name;
            _class_name = class_name;
            _recipient_id = recipient_id;
            _full_name = full_name;
            _pickup_date = pickup_date;
            _merchant_id = merchant_id;
            CultureInfo ci = new CultureInfo(Settings.cultureInfo);
            lblPickupDate.Text = pickup_date.ToString("dddd, dd/MM/yyyy", ci);
            _dayOfWeek = pickup_date.DayOfWeek.ToString();

            GetDayOfWeekEnum(_dayOfWeek);

            if (_user_role_id == 9)
            {
                _recipient_role_id = 10;
                _special_flag = "N";
                lblPickupTime.IsVisible = false;
                _pickup_time = DateTime.MinValue.TimeOfDay;
            }
            else
            {
                _recipient_role_id = _user_role_id;
                _special_flag = "Y";
                lblPickupTime.IsVisible = true;

                if (pickup_time != DateTime.MinValue.TimeOfDay)
                {
                    DateTime time = pickup_date.Add(pickup_time);
                    lblPickupTime.Text = _str_service_method + " @ " + time.ToString("hh:mm tt");
                    _pickup_time = pickup_time;
                }
            }
            _service_method_id = service_method_id;
            _delivery_location = delivery_location;
            lblFullName.Text = full_name;
            lblClassName.Text = class_name;
            lblSchoolName.Text = school_name;

            cvProduct.SelectionChanged += OnCollectionViewSelectionChanged;

            var tapGestureRecognizerCart = new TapGestureRecognizer();
            tapGestureRecognizerCart.Tapped += (s, e) => {
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                Navigation.PopAsync();
            };
            imgCart.GestureRecognizers.Add(tapGestureRecognizerCart);


        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            CartCount();
            await MerchantProductCategory();
        }

        //async void StartCall(object sender, EventArgs args)
        //{
        //    var page = new SearchListPage2(AppResources.SelectMerchantTypeText, "merchant", _school_id);
        //    page.DetailSet += this.OnDetailSet;
        //    await Navigation.PushAsync(page);
        //}
        void GetDayOfWeekEnum(string myDay) 
        {
            switch (myDay)
            {
                case "Sunday":
                    _dayOfWeek = "0";
                    break;
                case "Monday":
                    _dayOfWeek = "1";
                    break;
                case "Tuesday":
                    _dayOfWeek = "2";
                    break;
                case "Wednesday":
                    _dayOfWeek = "3";
                    break;
                case "Thursday":
                    _dayOfWeek = "4";
                    break;
                case "Friday":
                    _dayOfWeek = "5";
                    break;
                case "Saturday":
                    _dayOfWeek = "6";
                    break;
                default:
                    //Console.WriteLine("Nothing");
                    break;
            }
        }
        public async void CartCount()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostPurchaseCartCount(Settings.profileId, Settings.walletId);
                    string jsonStr = await t;
                    CartProperty response = JsonConvert.DeserializeObject<CartProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        if (response.Total > 0)
                        {
                            badgeCart.IsVisible = true;
                            badgeCart.Text = response.Total.ToString();
                        }
                        else
                        {
                            badgeCart.IsVisible = false;
                            badgeCart.Text = string.Empty;
                        }
                    }
                    else
                    {
                        //SnackB.Message = response.Message;
                        //SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {

                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task MerchantProductCategory()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy2 = true;

                    var t = srvc.PostPurchaseProductCategory(_school_id, _merchant_id, _special_flag);
                    string jsonStr = await t;
                    ProductCategoryProperty response = JsonConvert.DeserializeObject<ProductCategoryProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        Products.Clear();

                        foreach (var sl in response.Data)
                        {
                            await GetProductDetail(sl.category_id);

                            Products.Add(new ProductCategory(sl.category_name, listProduct));
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                    SnackB.Message = AppResources.SomethingWrongText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy2 = false;
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }
        public async Task GetProductDetail(int category_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostPurchaseProductDetail(_school_id, _merchant_id, category_id, _special_flag, _dayOfWeek);
                    string jsonStr = await t;
                    ProductDetailProperty response = JsonConvert.DeserializeObject<ProductDetailProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listProduct = new ObservableCollection<ProductDetail>();
                        foreach (ProductDetail sl in response.Data)
                        {
                            ProductDetail post = new ProductDetail();
                            post.product_id = sl.product_id;
                            post.category_id = sl.category_id;
                            post.school_id = sl.school_id;
                            post.merchant_id = sl.merchant_id;
                            post.category_name = sl.category_name;
                            post.product_name = sl.product_name;
                            post.product_description = sl.product_description;
                            post.unit_price = sl.unit_price;
                            post.str_unit_price = "RM " + sl.unit_price;
                            post.cost_price = sl.cost_price;
                            post.discount_price = sl.discount_price;
                            post.text_color = sl.text_color;
                            post.background_color = sl.background_color;
                            post.product_weight = sl.product_weight;
                            post.special_flag = sl.special_flag;
                            post.product_ingredient = sl.product_ingredient;
                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url = requestUrl + sl.photo_url;
                                post.product_photo_url = sl.photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.photo_url = requestUrl + "/images/sorry-no-image.jpg";
                                post.product_photo_url = sl.photo_url;
                                post.image_visible = false;
                                post.initial_visible = true;
                            }
                            listProduct.Add(post);
                        }
                        RowCount = listProduct.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                    }
                    else
                    {
                        listProduct = new ObservableCollection<ProductDetail>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        //cvProduct.Footer = l;
                        //cvProduct.ItemsSource = listProduct;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                    SnackB.Message = AppResources.SomethingWrongText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    //IsBusy = false;
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }
        //public ProductDetail detail;
        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var previous = e.PreviousSelection.FirstOrDefault() as ProductDetail;
            var current = e.CurrentSelection.FirstOrDefault() as ProductDetail;
            if (current == null) return;

            if (current.product_id > 0) 
            {
                if (((CollectionView)sender).SelectedItem == null)
                    return;

                var page = new AddToCartPopupPage(_profile_id, _wallet_id, _school_id, _merchant_id, _user_role_id, _recipient_id, _recipient_role_id, _pickup_date,_pickup_time,_service_method_id,_delivery_location,current);
                page.DetailSet += this.OnDetailSet;
                await PopupNavigation.Instance.PushAsync(page);

                ((CollectionView)sender).SelectedItem = null;
            }
        }
        //void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var picker = (Picker)sender;
        //    int selectedIndex = picker.SelectedIndex;

        //    if (selectedIndex != -1)
        //    {
        //        switch (selectedIndex)
        //        {
        //            case 0:
        //                _merchant_type_id = 1;
        //                break;
        //            case 1:
        //                _merchant_type_id = 2;
        //                break;
        //            case 2:
        //                _merchant_type_id = 3;
        //                break;
        //            default:
        //                // code block
        //                break;
        //        }
        //        txtCompanyName.Text = string.Empty;
        //    }
        //}
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}