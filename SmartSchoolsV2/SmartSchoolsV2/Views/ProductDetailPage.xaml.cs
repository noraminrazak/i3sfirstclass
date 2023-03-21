using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using SmartSchoolsV2.Services;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;
using System.Collections.ObjectModel;
using System.Net;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public ProductDetail _product;
        public string _mode;
        public int _school_id;
        public string _school_name;
        public int _product_id;
        public int _category_id;
        public string _category_name;
        public string _file_name;
        public string _photo_base64;
        public string _full_name;
        public string _special_flag;
        public int _merchant_id;
        public int _merchant_type_id;
        public static string _back;
        public bool takePhoto = false;
        public bool pickPhoto = false;
        public string _dayOfWeek;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public string image_url { get; set; }
        public ObservableCollection<ProductNutrition> listNutrition { get; set; }
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }

        private bool isBusy;
        public new bool IsBusy
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
        public void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
                if (!string.IsNullOrEmpty(Settings.textColor))
                {
                    bvTextColor.BackgroundColor = ColorConverters.FromHex(Settings.textColor);
                }
                if (!string.IsNullOrEmpty(Settings.bgColor))
                {
                    bvBgColor.BackgroundColor = ColorConverters.FromHex(Settings.bgColor);
                }

                MerchantProductNutrition(_school_id,_merchant_id,_product_id);
            }
        }
        public ProductDetailPage(string mode, int school_id, string school_name, int category_id, string category_name, ProductDetail product)
        {
            InitializeComponent();
            this.BindingContext = this;
            _school_id = school_id;
            _school_name = school_name;
            _category_id = category_id;
            _category_name = category_name;
            _product = product;
            _product_id = product.product_id;
            _mode = mode;
            _merchant_id = Settings.merchantId;
            _merchant_type_id = Settings.merchantTypeId;
            _full_name = Settings.fullName;

            if (_merchant_type_id == 1) //canteen
            {
                //lblProductSKU.IsVisible = false;
                //txtProductSKU.IsVisible = false;
                lblProductSKU.Text = AppResources.ProductCodeText;
                txtProductSKU.Placeholder = AppResources.EnterProductCodeText;
                specialStack.IsVisible = true;
            }
            else if (_merchant_type_id == 2) //coop
            {
                lblProductSKU.Text = AppResources.ProductSKUText;
                txtProductSKU.Placeholder = AppResources.EnterProductSKUText;
                lblWeight.IsVisible = false;
                txtWeight.IsVisible = false;
                lblIngredient.IsVisible = false;
                txtIngredient.IsVisible = false;
                gridNutrition.IsVisible = false;
                specialStack.IsVisible = false;
            }

            if (_mode == "C")
            {
                lblTitleView.Text = AppResources.NewProductText;
            }
            else if (_mode == "U")
            {
                lblTitleView.Text = AppResources.EditProductText;
            }
            if (_mode == "C")
            {
                MerchantProductNutrition(_school_id, _merchant_id, 0);
            }
            else if (_mode == "U")
            {
                MerchantProductNutrition(_school_id, _merchant_id, _product_id);
            }

            if (!string.IsNullOrEmpty(_product.photo_url))
            {
                LoadPhotoUri(_product.photo_url);
            }
            txtSchoolName.Text = _school_name;
            _product_id = _product.product_id;
            txtCategoryName.Text = _category_name;
            txtProductName.Text = _product.product_name;
            txtProductSKU.Text = _product.product_sku;
            txtProductDesc.Text = _product.product_description;
            txtUnitPrice.Text = _product.unit_price;
            txtWeight.Text = _product.product_weight;
            txtIngredient.Text = _product.product_ingredient;

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                var page = new ProductNutritionPopupPage("C", _product.product_id, 0, string.Empty, string.Empty);
                page.DetailSet += this.OnDetailSet;
                PopupNavigation.Instance.PushAsync(page);
            };
            btnAdd.GestureRecognizers.Add(tapGestureRecognizer);

            if (_mode == "C")
            {
                _special_flag = "N";
            }
            else
            {
                _special_flag = _product.special_flag;
            }

            if (_special_flag == "Y")
            {
                specialSwitch.IsToggled = true;
            }
            else
            {
                specialSwitch.IsToggled = false;
            }

            if (!string.IsNullOrEmpty(_product.available_day)) 
            {
                ProvidedDayCheck(_product.available_day);
            }

            _dayOfWeek = _product.available_day;

            if (!string.IsNullOrEmpty(_product.text_color))
            {
                bvTextColor.BackgroundColor = ColorConverters.FromHex(_product.text_color);
            }
            else
            {
                bvTextColor.BackgroundColor = Color.FromHex("#FFFFFFFF");
            }

            if (!string.IsNullOrEmpty(_product.background_color))
            {
                bvBgColor.BackgroundColor = ColorConverters.FromHex(_product.background_color);
            }
            else
            {
                bvBgColor.BackgroundColor = Color.FromHex("#FFFF8C00");
            }

            ColorPickerPopupPage pageText = new ColorPickerPopupPage("Text");
            ColorPickerPopupPage pageBackground = new ColorPickerPopupPage("Background");

            var tapTextColor = new TapGestureRecognizer();
            pageText.DetailSet += this.OnDetailSet;
            tapTextColor.Tapped += async (s, e) => await PopupNavigation.Instance.PushAsync(pageText);
            bvTextColor.GestureRecognizers.Add(tapTextColor);

            var tapBgColor = new TapGestureRecognizer();
            pageBackground.DetailSet += this.OnDetailSet;
            tapBgColor.Tapped += async (s, e) => await PopupNavigation.Instance.PushAsync(pageBackground);
            bvBgColor.GestureRecognizers.Add(tapBgColor);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //imagePhoto.Source = new UriImageSource()
            //{
            //    CachingEnabled = false,
            //};

        }
        void OnToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
            {
                _special_flag = "Y";
            }
            else {
                _special_flag = "N";
            }
        }

        void LoadPhotoUri(string photo_url)
        {
            _file_name = photo_url.Split('/').Last();
            imgFrame.IsVisible = true;
            btnRemove.IsVisible = true;

            imagePhoto.Source = ImageSource.FromUri(new Uri(photo_url));

            ConvertImageToBase64(photo_url);
        }

        private void ConvertImageToBase64(string url)
        {
            using (var client = new WebClient())
            {
                var bytes = client.DownloadData(url);
                _photo_base64 = Convert.ToBase64String(bytes);
            }
        }

        void ProvidedDayCheck(string available_day)
        {
            int[] nums = available_day.Split(',').Select(int.Parse).ToArray();
            if (nums.Length == 7) 
            {
                Cb7.IsChecked = true;
            }
            foreach (int i in nums)
            {
                switch (i)
                {
                    case 0:
                        Cb0.IsChecked = true;
                        break;
                    case 1:
                        Cb1.IsChecked = true;
                        break;
                    case 2:
                        Cb2.IsChecked = true;
                        break;
                    case 3:
                        Cb3.IsChecked = true;
                        break;
                    case 4:
                        Cb4.IsChecked = true;
                        break;
                    case 5:
                        Cb5.IsChecked = true;
                        break;
                    case 6:
                        Cb6.IsChecked = true;
                        break;
                    default:
                        //Console.WriteLine("Nothing");
                        break;
                }
            }
        }
        async void StartCall(object sender, EventArgs args)
        {
            var page = new SearchListPage1("Select category", "category", "product");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }
        async void OnPickPhotoClicked(object sender, EventArgs args)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("", AppResources.PermissionGalleryText, "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 75
            });


            if (file == null)
                return;

            _file_name = file.Path.Split('/').Last();
            var fileLength = new FileInfo(file.Path).Length;
            string file_size = GetFileSize(fileLength);
            lblFileSize.Text = file_size;

            imagePhoto.Source = ImageSource.FromStream(() => 
            {
                var stream = file.GetStream();
                imgFrame.IsVisible = true;
                btnRemove.IsVisible = true;
                byte[] imgByte = ReadAllBytes(stream);
                _photo_base64 = Convert.ToBase64String(imgByte);
                file.Dispose();
                return stream;
            });

        }
        async void OnTakePhotoClicked(object sender, EventArgs args)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("", AppResources.NoCameraText, "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "i3s",
                AllowCropping = false,
                SaveToAlbum = true,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.Medium,
                MaxWidthHeight = 1000,
                DefaultCamera = CameraDevice.Rear
            });

            if (file == null)
                return;

            _file_name = file.Path.Split('/').Last();
            //await DisplayAlert("File Location", file.Path, "OK");
            var fileLength = new FileInfo(file.Path).Length;
            string file_size = GetFileSize(fileLength);
            lblFileSize.Text = file_size;

            imagePhoto.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                imgFrame.IsVisible = true;
                btnRemove.IsVisible = true;
                byte[] imgByte = ReadAllBytes(stream);
                _photo_base64 = Convert.ToBase64String(imgByte);
                file.Dispose();
                return stream;
            });
        }

        public static string GetFileSize(long fileLength)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (fileLength >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                fileLength = fileLength / 1024;
            }
            string result = String.Format("{0:0.##} {1}", fileLength, sizes[order]);
            return result;
        }

        public static byte[] ReadAllBytes(Stream instream)
        {
            if (instream is MemoryStream)
                return ((MemoryStream)instream).ToArray();

            using (var memoryStream = new MemoryStream())
            {
                instream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        async void OnRemoveClicked(object sender, EventArgs args)
        {
            if (imagePhoto.Source != null)
            {
                if (_mode == "U")
                {
                    await RemoveProductPhoto();
                }
                else 
                {
                    imagePhoto.Source = null;
                    imagePhoto.IsVisible = false;
                    btnRemove.IsVisible = false;
                    lblFileSize.IsVisible = false;
                    lblFileSize.Text = "";
                }
            }
        }

        async void OnSaveClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtCategoryName.Text))
            {
                if (!string.IsNullOrEmpty(txtProductName.Text))
                {
                    if (!string.IsNullOrEmpty(txtUnitPrice.Text))
                    {
                        if (!string.IsNullOrEmpty(_dayOfWeek))
                        {
                            if (_merchant_type_id == 1)
                            {
                                if (_mode == "C")
                                {
                                    await CreateProductDetail();
                                }
                                else if (_mode == "U")
                                {
                                    await UpdateProductDetail();
                                };
                            }
                            else if (_merchant_type_id == 2)
                            {
                                if (!string.IsNullOrEmpty(txtProductSKU.Text))
                                {
                                    if (_mode == "C")
                                    {
                                        await CreateProductDetail();
                                    }
                                    else if (_mode == "U")
                                    {
                                        await UpdateProductDetail();
                                    };
                                }
                                else
                                {
                                    SnackB.Message = AppResources.ProductSKURequiredText;
                                    SnackB.IsOpen = !SnackB.IsOpen;
                                }
                            }
                        }
                        else 
                        {
                            SnackB.Message = AppResources.ProvidedDayRequiredText;
                            SnackB.IsOpen = !SnackB.IsOpen;
                        }
                    }
                    else {
                        SnackB.Message = AppResources.UnitPriceRequiredText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                else {
                    SnackB.Message = AppResources.ProductNameRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }

            }
            else
            {
                SnackB.Message = AppResources.CategoryNameRequired;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task CreateProductDetail()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostMerchantCreateProductDetail(_merchant_id, _category_id, txtProductName.Text, txtProductSKU.Text, _photo_base64, _file_name,
                        Convert.ToDecimal(txtUnitPrice.Text), 0, 0, txtProductDesc.Text, txtWeight.Text, txtIngredient.Text, _special_flag, 
                        _dayOfWeek.TrimEnd(','), bvTextColor.BackgroundColor.ToHex(), bvBgColor.BackgroundColor.ToHex(), Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert(AppResources.SorryText, response.Message, "OK");
                    }
                }
                catch (Exception)
                {
                    SnackB.Message = AppResources.SomethingWrongText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    HideLoadingPopup();
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task UpdateProductDetail()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostMerchantUpdateProductDetail(_product_id, _merchant_id, _category_id, txtProductName.Text, txtProductSKU.Text, _photo_base64, _file_name,
                        Convert.ToDecimal(txtUnitPrice.Text), 0, 0, txtProductDesc.Text, txtWeight.Text, txtIngredient.Text, _special_flag, _dayOfWeek.TrimEnd(','), 
                        bvTextColor.BackgroundColor.ToHex(), bvBgColor.BackgroundColor.ToHex(), _full_name);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert(AppResources.SorryText, response.Message, "OK");
                    }
                }
                catch (Exception)
                {
                    SnackB.Message = AppResources.SomethingWrongText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    HideLoadingPopup();
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task RemoveProductPhoto()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostMerchantRemoveProductPhoto(_product_id, _merchant_id, _category_id, _full_name);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");
                        imagePhoto.Source = null;
                        imgFrame.IsVisible = false;
                        btnRemove.IsVisible = false;
                        lblFileSize.IsVisible = false;
                        lblFileSize.Text = "";
                        _file_name = "";
                        _photo_base64 = "";
                    }
                    else
                    {
                        SnackB.Message = response.Message;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                catch (Exception)
                {
                    SnackB.Message = AppResources.SomethingWrongText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    HideLoadingPopup();
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void Edit_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            ProductNutrition product = item.BindingContext as ProductNutrition;
            var page = new ProductNutritionPopupPage("U", product.product_id, product.info_id, product.nutrition_name, product.per_serving);
            page.DetailSet += this.OnDetailSet;
            await PopupNavigation.Instance.PushAsync(page);
        }

        async void Delete_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            ProductNutrition product = item.BindingContext as ProductNutrition;

            bool answer = await App.Current.MainPage.DisplayAlert(AppResources.ConfirmDeleteText, AppResources.DoYouReallyWantToRemoveText + product.nutrition_name + "?", AppResources.YesText, AppResources.CancelText);
            if (answer)
            {
                await DeleteProductNutrition(product.info_id, product.product_id);
            }
        }

        public async Task DeleteProductNutrition(int info_id, int product_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostMerchantDeleteProductNutrition(info_id, product_id, _full_name);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        MerchantProductNutrition(_school_id, _merchant_id, _category_id);
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

        public async void MerchantProductNutrition(int school_id, int merchant_id, int product_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    //IsBusy = true;
                    lvNutrition.ItemsSource = null;
                    var t = srvc.PostMerchantProductNutrition(school_id, merchant_id, product_id);
                    string jsonStr = await t;
                    ProductNutritionProperty response = JsonConvert.DeserializeObject<ProductNutritionProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listNutrition = new ObservableCollection<ProductNutrition>();
                        foreach (ProductNutrition sl in response.Data)
                        {
                            ProductNutrition post = new ProductNutrition();
                            post.info_id = sl.info_id;
                            post.product_id = sl.product_id;
                            post.school_id = sl.school_id;
                            post.merchant_id = sl.merchant_id;
                            post.nutrition_name = sl.nutrition_name;
                            post.per_serving = sl.per_serving;
                            listNutrition.Add(post);
                        }
                        RowCount = listNutrition.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            lvNutrition.HeightRequest = 35 + (35 * RowCount);
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            lvNutrition.HeightRequest = 35;
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvNutrition.Footer = l;
                        lvNutrition.ItemsSource = listNutrition;
                    }
                    else
                    {
                        lvNutrition.HeightRequest = 35;
                        listNutrition = new ObservableCollection<ProductNutrition>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvNutrition.Footer = l;
                        lvNutrition.ItemsSource = listNutrition;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    //IsBusy = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        void EveryDayChecked(object sender, EventArgs e)
        {
            if (Cb7.IsChecked == true)
            {
                Cb0.IsChecked = true;
                Cb1.IsChecked = true;
                Cb2.IsChecked = true;
                Cb3.IsChecked = true;
                Cb4.IsChecked = true;
                Cb5.IsChecked = true;
                Cb6.IsChecked = true;
                _dayOfWeek = "0,1,2,3,4,5,6,";
            }
            else 
            {
                Cb0.IsChecked = false;
                Cb1.IsChecked = false;
                Cb2.IsChecked = false;
                Cb3.IsChecked = false;
                Cb4.IsChecked = false;
                Cb5.IsChecked = false;
                Cb6.IsChecked = false;
                _dayOfWeek = "";
            }
        }
        void SundayChecked(object sender, EventArgs e)
        {
            if (Cb0.IsChecked == true)
            {
                _dayOfWeek = String.Concat(_dayOfWeek, "0,");
            }
            else {
                _dayOfWeek = _dayOfWeek.Replace("0,","");
            }

            if (_dayOfWeek.Contains("0") && _dayOfWeek.Contains("1") && _dayOfWeek.Contains("2")
    && _dayOfWeek.Contains("3") && _dayOfWeek.Contains("4") && _dayOfWeek.Contains("5")
    && _dayOfWeek.Contains("6"))
            {
                Cb7.IsChecked = true;
            }
            else {
                Cb7.IsChecked = false;
            }

        }
        void MondayChecked(object sender, EventArgs e)
        {
            if (Cb1.IsChecked == true)
            {
                _dayOfWeek = String.Concat(_dayOfWeek, "1,");
            }
            else
            {
                _dayOfWeek = _dayOfWeek.Replace("1,", "");
            }

            if (_dayOfWeek.Contains("0") && _dayOfWeek.Contains("1") && _dayOfWeek.Contains("2")
&& _dayOfWeek.Contains("3") && _dayOfWeek.Contains("4") && _dayOfWeek.Contains("5")
&& _dayOfWeek.Contains("6"))
            {
                Cb7.IsChecked = true;
            }
            else
            {
                Cb7.IsChecked = false;
            }
        }
        void TuesdayChecked(object sender, EventArgs e)
        {
            if (Cb2.IsChecked == true)
            {
                _dayOfWeek = String.Concat(_dayOfWeek, "2,");
            }
            else
            {
                _dayOfWeek = _dayOfWeek.Replace("2,", "");
            }

            if (_dayOfWeek.Contains("0") && _dayOfWeek.Contains("1") && _dayOfWeek.Contains("2")
&& _dayOfWeek.Contains("3") && _dayOfWeek.Contains("4") && _dayOfWeek.Contains("5")
&& _dayOfWeek.Contains("6"))
            {
                Cb7.IsChecked = true;
            }
            else
            {
                Cb7.IsChecked = false;
            }
        }
        void WednesdayChecked(object sender, EventArgs e)
        {
            if (Cb3.IsChecked == true)
            {
                _dayOfWeek = String.Concat(_dayOfWeek, "3,");
            }
            else
            {
                _dayOfWeek = _dayOfWeek.Replace("3,", "");
            }

            if (_dayOfWeek.Contains("0") && _dayOfWeek.Contains("1") && _dayOfWeek.Contains("2")
&& _dayOfWeek.Contains("3") && _dayOfWeek.Contains("4") && _dayOfWeek.Contains("5")
&& _dayOfWeek.Contains("6"))
            {
                Cb7.IsChecked = true;
            }
            else
            {
                Cb7.IsChecked = false;
            }
        }
        void ThursdayChecked(object sender, EventArgs e)
        {
            if (Cb4.IsChecked == true)
            {
                _dayOfWeek = String.Concat(_dayOfWeek, "4,");
            }
            else
            {
                _dayOfWeek = _dayOfWeek.Replace("4,", "");
            }

            if (_dayOfWeek.Contains("0") && _dayOfWeek.Contains("1") && _dayOfWeek.Contains("2")
&& _dayOfWeek.Contains("3") && _dayOfWeek.Contains("4") && _dayOfWeek.Contains("5")
&& _dayOfWeek.Contains("6"))
            {
                Cb7.IsChecked = true;
            }
            else
            {
                Cb7.IsChecked = false;
            }
        }
        void FridayChecked(object sender, EventArgs e)
        {
            if (Cb5.IsChecked == true)
            {
                _dayOfWeek = String.Concat(_dayOfWeek, "5,");
            }
            else
            {
                _dayOfWeek = _dayOfWeek.Replace("5,", "");
            }

            if (_dayOfWeek.Contains("0") && _dayOfWeek.Contains("1") && _dayOfWeek.Contains("2")
&& _dayOfWeek.Contains("3") && _dayOfWeek.Contains("4") && _dayOfWeek.Contains("5")
&& _dayOfWeek.Contains("6"))
            {
                Cb7.IsChecked = true;
            }
            else
            {
                Cb7.IsChecked = false;
            }
        }
        void SaturdayChecked(object sender, EventArgs e)
        {
            if (Cb6.IsChecked == true)
            {
                _dayOfWeek = String.Concat(_dayOfWeek, "6,");
            }
            else
            {
                _dayOfWeek = _dayOfWeek.Replace("6,", "");
            }

            if (_dayOfWeek.Contains("0") && _dayOfWeek.Contains("1") && _dayOfWeek.Contains("2")
&& _dayOfWeek.Contains("3") && _dayOfWeek.Contains("4") && _dayOfWeek.Contains("5")
&& _dayOfWeek.Contains("6"))
            {
                Cb7.IsChecked = true;
            }
            else
            {
                Cb7.IsChecked = false;
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
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}