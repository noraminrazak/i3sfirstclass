using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductCategoryPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();

        public string requestUrl = Settings.requestUrl;
        public string _mode;
        public int _school_id;
        public string _school_name;
        public string _city;
        public int _merchant_id;
        public int _category_id;
        public string _category_name;
        public string _category_description;
        public string _full_name;
        public ProductCategoryPage(string mode, int school_id, int merchant_id, int category_id, string category_name, string category_description)
        {
            InitializeComponent();
            BindingContext = this;

            _mode = mode;
            _school_id = school_id;
            _merchant_id = merchant_id;
            _category_id = category_id;
            _category_name = category_name;
            _category_description = category_description;
            _full_name = Settings.fullName;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblSchoolName.Text = _school_name;
            if (_mode == "C")
            {
                lblTitleView.Text = AppResources.NewProductCategoryText;
            }
            else {
                lblTitleView.Text = AppResources.EditProductCategoryText;
                txtCategoryName.Text = _category_name;
                txtCategoryDesc.Text = _category_description;
            }
            SchoolInfo();
            //MerchantProductDetail(_school_id, _merchant_id, _category_id);
        }

        public async void SchoolInfo()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
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
                        lblCity.Text = _city;
                    }
                    else
                    {

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

        async void OnSaveClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtCategoryName.Text))
            {
                if (_mode == "C")
                {
                    await CreateProductCategory();
                }
                else if (_mode == "U")
                {
                    await UpdateProductCategory();
                };
            }
            else
            {
                SnackB.Message = AppResources.CategoryNameRequired;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task CreateProductCategory() 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostMerchantCreateProductCategory(_school_id, _merchant_id, txtCategoryName.Text, txtCategoryDesc.Text, _full_name);
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

        public async Task UpdateProductCategory()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostMerchantUpdateProductCategory(_school_id, _merchant_id, _category_id, txtCategoryName.Text, txtCategoryDesc.Text, _full_name);
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