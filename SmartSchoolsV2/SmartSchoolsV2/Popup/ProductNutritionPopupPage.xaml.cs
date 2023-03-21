using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductNutritionPopupPage : PopupPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;

        public delegate void SetDetailEventHandler(object source, EventArgs args);
        public event SetDetailEventHandler DetailSet;
        public int _product_id;
        public int _info_id;
        public string _mode;
        protected virtual void OnDetailSet()
        {
            if (DetailSet != null)
            {
                DetailSet(this, EventArgs.Empty);
            }
        }
        public ProductNutritionPopupPage(string mode, int product_id, int info_id, string nutrition_name, string per_serving)
        {
            InitializeComponent();
            BindingContext = this;
            _mode = mode;
            _product_id = product_id;
            _info_id = info_id;
            if (mode == "C")
            {
                txtComposition.Text = "";
                txtPerServing.Text = "";
                lblHeader.Text = AppResources.AddNutritionText;
            }
            else if (mode == "U") 
            {
                txtComposition.Text = nutrition_name;
                txtPerServing.Text = per_serving;
                lblHeader.Text = AppResources.EditNutritionText;
            }
        }

        void OnCloseClicked(object sender, EventArgs args)
        {
            CloseAllPopup();
        }
        void OnSaveClicked(object sender, EventArgs args)
        {
            int err = 0;
            if (string.IsNullOrEmpty(txtComposition.Text)) {
                txtComposition.Placeholder = AppResources.RequiredText;
                txtComposition.PlaceholderColor = Color.Red;
                txtComposition.FontAttributes = FontAttributes.Italic;
                err++;
            }

            if (string.IsNullOrEmpty(txtComposition.Text))
            {
                txtPerServing.Placeholder = AppResources.RequiredText;
                txtPerServing.PlaceholderColor = Color.Red;
                txtPerServing.FontAttributes = FontAttributes.Italic;
                err++;
            }

            if (err == 0) 
            {
                if (_mode == "C")
                {
                    CreateProductNutrition();
                }
                else if (_mode == "U")
                {
                    UpdateProductNutrition();
                }
            }
        }

        async void CreateProductNutrition() {
            if (conn.IsConnected() == true)
            {
                try
                {
                    //ButtonWithSpinner.IsBusy = !ButtonWithSpinner.IsBusy;

                    var t = srvc.PostMerchantCreateProductNutrition(_product_id, txtComposition.Text.Trim(), txtPerServing.Text.Trim(), Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                        ProductDetailPage.Back = "Y";
                        OnDetailSet();
                    }
                    else
                    {
                        await DisplayAlert(AppResources.SorryText, response.Message, "OK");
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
                    //ButtonWithSpinner.IsBusy = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void UpdateProductNutrition()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    //ButtonWithSpinner.IsBusy = !ButtonWithSpinner.IsBusy;

                    var t = srvc.PostMerchantUpdateProductNutrition(_info_id, _product_id, txtComposition.Text.Trim(), txtPerServing.Text.Trim(), Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await PopupNavigation.Instance.PopAllAsync();
                        ProductDetailPage.Back = "Y";
                        OnDetailSet();
                    }
                    else
                    {
                        await DisplayAlert(AppResources.SorryText, response.Message, "OK");
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
                    //ButtonWithSpinner.IsBusy = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}