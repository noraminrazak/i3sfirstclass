using Newtonsoft.Json;
using Plugin.XamarinFormsSaveOpenPDFPackage;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDownloadPopupPage : PopupPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public DateTime _pickup_date;
        public int _report_by_id = 0;
        public int _school_id;
        public int _merchant_id;
        public int _class_id;
        public string _class_name;
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
        public delegate void SetDetailEventHandler(object source, EventArgs args);
        public event SetDetailEventHandler DetailSet;
        protected virtual void OnDetailSet()
        {
            if (DetailSet != null)
            {
                DetailSet(this, EventArgs.Empty);
            }
        }
        public OrderDownloadPopupPage(int merchant_id, int school_id, int class_id = 0, string class_name = "")
        {
            InitializeComponent();
            BindingContext = this;

            _school_id = school_id;
            _merchant_id = merchant_id;
            _class_id = class_id;
            _class_name = class_name;
            if (_class_id > 0)
            {
                rbStack.IsVisible = false;
                lblTitle.Text = AppResources.DownloadOrderText + " : " + _class_name;
            }
            else 
            {
                rbStack.IsVisible = true;
            }
            txtPickupDate.Text = DateTime.Now.ToString("dddd, dd MMM yyyy", ci);
            _pickup_date = DateTime.Today;
        }
        void StartCall(object sender, EventArgs args)
        {
            txtPickupDate.Unfocus();
            Device.BeginInvokeOnMainThread(() => {
                datePicker.Focus();
            });
        }
        private void OnDeliveryDateSelected(object sender, DateChangedEventArgs e)
        {
            txtPickupDate.Text = e.NewDate.ToString("dddd, dd MMM yyyy", ci);
            _pickup_date = e.NewDate;
        }

        void OnRadio1Clicked(object sender, EventArgs e)
        {
            _report_by_id = 1;
        }

        void OnRadio2Clicked(object sender, EventArgs e)
        {
            _report_by_id = 2;
        }

        void OnRadio3Clicked(object sender, EventArgs e)
        {
            _report_by_id = 3;
        }

        void OnCancelClicked(object sender, EventArgs args)
        {
            CloseAllPopup();
        }
        void OnDownloadClicked(object sender, EventArgs args)
        {
            errMessage1.IsVisible = false;
            errMessage2.IsVisible = false;

            if (!string.IsNullOrEmpty(txtPickupDate.Text))
            {
                if (_class_id == 0)
                {
                    if (_report_by_id > 0)
                    {
                        CloseAllPopup();

                        if (_report_by_id == 1)
                        {
                            DownloadAndViewProductOrderReport();
                        }
                        else if (_report_by_id == 2)
                        {
                            DownloadAndViewClassOrderReport();
                        }
                        else if (_report_by_id == 3)
                        {
                            DownloadAndViewStaffOrderReport();
                        }
                    }
                    else
                    {
                        errMessage2.IsVisible = true;
                    }
                }
                else 
                {
                    CloseAllPopup();

                    DownloadAndViewStudentOrderReport();
                }
            }
            else {
                errMessage1.IsVisible = true;
            }
        }

        async void DownloadAndViewProductOrderReport() 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostReportProductOrder(_merchant_id, _school_id, _pickup_date, Settings.fullName);
                    string jsonStr = await t;
                    ReportProperty response = JsonConvert.DeserializeObject<ReportProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        DownloadAndViewPDF(response.Message);
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
                    HideLoadingPopup();
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void DownloadAndViewClassOrderReport()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostReportClassOrder(_merchant_id,_school_id,_pickup_date,Settings.fullName);
                    string jsonStr = await t;
                    ReportProperty response = JsonConvert.DeserializeObject<ReportProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        DownloadAndViewPDF(response.Message);
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
                    HideLoadingPopup();
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void DownloadAndViewStaffOrderReport()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostReportStaffOrder(_merchant_id, _school_id, _pickup_date, Settings.fullName);
                    string jsonStr = await t;
                    ReportProperty response = JsonConvert.DeserializeObject<ReportProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        DownloadAndViewPDF(response.Message);
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
                    HideLoadingPopup();
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void DownloadAndViewStudentOrderReport()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostReportStudentOrder(_merchant_id, _school_id, _class_id, _pickup_date, Settings.fullName);
                    string jsonStr = await t;
                    ReportProperty response = JsonConvert.DeserializeObject<ReportProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        DownloadAndViewPDF(response.Message);
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
                    HideLoadingPopup();
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void DownloadAndViewPDF(string file_path) 
        {
            string _uri = requestUrl + file_path;

            HttpClient client = new HttpClient();

            var uri = new Uri(_uri);

            Stream content;
            MemoryStream stream = new MemoryStream();

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStreamAsync();
                content.CopyTo(stream);
            }

            await CrossXamarinFormsSaveOpenPDFPackage.Current.SaveAndView(Guid.NewGuid() + ".pdf", "application/pdf", stream, PDFOpenContext.ChooseApp);
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
        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}