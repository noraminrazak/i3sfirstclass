using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantProductPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<ProductDetail> listProduct { get; set; }
        public static Command LoadMerchantProduct { get; set; }
        ViewCell lastCell;
        public int _school_id;
        public int _merchant_id;
        public int _category_id;
        public string _category_name;
        public string _full_name;
        public string _school_name;
        public string _city;
        bool isBusy;
        public bool IsBusy2
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public MerchantProductPage(int merchant_id, int school_id, int category_id, string category_name)
        {
            InitializeComponent();
            BindingContext = this;

            _school_id = school_id;
            _merchant_id = merchant_id;
            _category_id = category_id;
            _category_name = category_name;
            _full_name = Settings.fullName;

            var tapGestureRecognizerTxn = new TapGestureRecognizer();

            tapGestureRecognizerTxn.Tapped += (s, e) => {
                ProductDetail product = new ProductDetail();
                Navigation.PushAsync(new ProductDetailPage("C", _school_id, _school_name, _category_id, _category_name, product));
            };
            btnAddProduct.GestureRecognizers.Add(tapGestureRecognizerTxn);

            LoadMerchantProduct = new Command(async () => await MerchantProductDetail(_school_id, _merchant_id, _category_id));

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            lblCategoryName.Text = _category_name;
            SchoolInfo();
            await MerchantProductDetail(_school_id,_merchant_id,_category_id);
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await MerchantProductDetail(_school_id, _merchant_id, _category_id);
                });
            }
        }
        async void Edit_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            ProductDetail product = item.BindingContext as ProductDetail;

            await Navigation.PushAsync(new ProductDetailPage("U", _school_id, _school_name, _category_id, _category_name, product));
        }

        async void Delete_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            ProductDetail product = item.BindingContext as ProductDetail;

            bool answer = await App.Current.MainPage.DisplayAlert(AppResources.ConfirmDeleteText, AppResources.DoYouReallyWantToDeleteText + product.product_name + "?", AppResources.YesText, AppResources.CancelText);
            if (answer)
            {
                await DeleteProductDetail(product.product_id, _merchant_id, product.category_id);
            }
        }

        public async Task DeleteProductDetail(int product_id, int merchant_id, int category_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostMerchantDeleteProductDetail(product_id, merchant_id, category_id, _full_name);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await MerchantProductDetail(_school_id, _merchant_id, _category_id);
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.SorryText, response.Message, "OK");
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    HideLoadingPopup();
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async void SchoolInfo()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy2 = true;
                    var t = srvc.PostSchoolInfo(_school_id);
                    string jsonStr = await t;
                    SchoolInfoProperty response = JsonConvert.DeserializeObject<SchoolInfoProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<SchoolInfo> list = new List<SchoolInfo>();
                        foreach (SchoolInfo sl in response.Data)
                        {
                            _school_name = sl.school_name;
                            _city = sl.city;
                        }
                        lblSchoolName.Text = _school_name;
                    }
                    else
                    {

                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy2 = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task MerchantProductDetail(int school_id, int merchant_id, int category_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy2 = true;
                    lvProduct.ItemsSource = null;
                    var t = srvc.PostMerchantProductDetail(school_id, merchant_id, category_id);
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
                            post.available_day = sl.available_day;
                            post.product_sku = sl.product_sku;
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
                        lvProduct.Footer = l;
                        lvProduct.ItemsSource = listProduct;
                    }
                    else
                    {
                        listProduct = new ObservableCollection<ProductDetail>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvProduct.Footer = l;
                        lvProduct.ItemsSource = listProduct;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy2 = false;
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        LoadingPopupPage loadingPage = new LoadingPopupPage();
        async void ShowLoadingPopup()
        {
            await Navigation.PushPopupAsync(loadingPage);
        }
        async void HideLoadingPopup()
        {
            await Task.Delay(500);
            await Navigation.RemovePopupPageAsync(loadingPage);
        }
    }
}