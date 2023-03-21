using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using SmartSchoolsV2.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantProductView : ContentView
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<ProductCategory> listProduct { get; set; }
        public static Command LoadMerchantProduct { get; set; }
        ViewCell lastCell;
        public int _school_id;
        public int _merchant_id;
        public string _receipt_date;
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
        public MerchantProductView()
        {
            InitializeComponent();
            BindingContext = this;

            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) => {
                Navigation.PushAsync(new ProductCategoryPage("C", Settings.merchantSchoolId, Settings.merchantId, 0, string.Empty, string.Empty));
            };
            btnAddCategory.GestureRecognizers.Add(tapGestureRecognizerTxn);

            DateTime dt = DateTime.Now;
            _receipt_date = dt.ToString("yyyy-MM-dd");

            LoadMerchantProduct = new Command(async () => await MerchantProductCategory());
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

        async void Edit_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            ProductCategory product = item.BindingContext as ProductCategory;

            await Navigation.PushAsync(new ProductCategoryPage("U", Settings.merchantSchoolId, Settings.merchantId, product.category_id,product.category_name,product.category_description));
        }

        async void Delete_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            ProductCategory product = item.BindingContext as ProductCategory;

            bool answer = await App.Current.MainPage.DisplayAlert(AppResources.ConfirmDeleteText, AppResources.DoYouReallyWantToDeleteText + product.category_name + "?", AppResources.YesText, AppResources.CancelText);
            if (answer) 
            {
                await DeleteProductCategory(Settings.merchantSchoolId, Settings.merchantId, product.category_id);
            }
        }

        public async Task DeleteProductCategory(int school_id, int merchant_id, int category_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostMerchantDeleteProductCategory(school_id, merchant_id, category_id, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await MerchantProductCategory();
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

        public async Task MerchantProductCategory()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvProduct.ItemsSource = null;
                    var t = srvc.PostMerchantProductCategory(Settings.merchantSchoolId, Settings.merchantId);
                    string jsonStr = await t;
                    ProductCategoryProperty response = JsonConvert.DeserializeObject<ProductCategoryProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listProduct = new ObservableCollection<ProductCategory>();
                        foreach (ProductCategory sl in response.Data)
                        {
                            ProductCategory post = new ProductCategory();
                            post.category_id = sl.category_id;
                            post.school_id = sl.school_id;
                            post.merchant_id = sl.merchant_id;
                            post.category_name = sl.category_name;
                            post.category_description = sl.category_description;
                            post.total_product = sl.total_product + AppResources._ProductText;
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
                        listProduct = new ObservableCollection<ProductCategory>();
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
        public ProductCategory product;
        async void OnProductSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as ProductCategory;
            if (data == null) return;
            product = data;

            if (product.category_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...

                await Navigation.PushAsync(new MerchantProductPage(product.merchant_id, product.school_id, product.category_id, product.category_name));
                ((ListView)sender).SelectedItem = null;
            }
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