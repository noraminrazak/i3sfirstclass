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
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttendanceDownloadPopupPage : PopupPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public string filePath = string.Empty;
        IDownloader downloader = DependencyService.Get<IDownloader>();
        IDocumentViewer viewer = DependencyService.Get<IDocumentViewer>();
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
        public int _shift_id;
        public int _class_id;
        public int _school_id;
        public DateTime _entry_month;
        public delegate void SetDetailEventHandler(object source, EventArgs args);
        public event SetDetailEventHandler DetailSet;
        protected virtual void OnDetailSet()
        {
            if (DetailSet != null)
            {
                DetailSet(this, EventArgs.Empty);
            }
        }

        [Obsolete]
        public AttendanceDownloadPopupPage(int school_id, string school_name, int class_id, string class_name, DateTime entry_month, int shift_id = 0, string shift_code = "")
        {
            InitializeComponent();
            BindingContext = this;

            _school_id = school_id;
            _class_id = class_id;
            _shift_id = shift_id;
            _entry_month = entry_month;

            if (class_id > 0 && shift_id == 0)
            {
                txtMessage.Text = AppResources.DownloadStudentAttendanceForText.Replace("class_name", class_name).Replace("MMM_yyyy", entry_month.ToString("MMM yyyy", ci));
            }
            else if (class_id == 0 && shift_id > 0) 
            {
                txtMessage.Text = AppResources.DownloadStaffAttendanceForText.Replace("shift_code", shift_code).Replace("MMM_yyyy", entry_month.ToString("MMM yyyy", ci));
            }

            downloader.OnFileDownloaded += OnFileDownloaded;
        }

        [Obsolete]
        private async void OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                //DisplayAlert(AppResources.DownloadText, AppResources.FileSaveSuccessText, AppResources.CloseText);
                //bool result = await DisplayAlert(AppResources.DownloadText, filePath, AppResources.OpenText, AppResources.CloseText);

                //if (result == true)
                //{
                    if (File.Exists(filePath))
                    {
                    // Note: In the second run of this method, the file exists
                    //await Launcher.OpenAsync(filePath);
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    SnackB.Message = AppResources.SomethingWrongText;
                    //    SnackB.IsOpen = !SnackB.IsOpen;
                    //});
                    }
                //}
            }
            else
            {
                await DisplayAlert(AppResources.DownloadText, AppResources.ErrorWhileSavingFileText, AppResources.CloseText);
            }

            //CloseAllPopup();
        }

        void OnCancelClicked(object sender, EventArgs args)
        {
            CloseAllPopup();
        }
        void OnDownloadClicked(object sender, EventArgs args)
        {
            if (_class_id > 0 && _shift_id == 0)
            {
                DownloadAndViewClassAttReport();
            }
            else if (_class_id == 0 && _shift_id > 0)
            {
                DownloadAndViewStaffAttReport();
            }
        }

        async void DownloadAndViewClassAttReport()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostReportClassAttendance(_school_id, _class_id, _entry_month, Settings.fullName);
                    string jsonStr = await t;
                    ReportProperty response = JsonConvert.DeserializeObject<ReportProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        if (response.Message.Contains(".xlsx"))
                        {
                            DownloadAndViewExcel(response.Message);
                        }
                        else 
                        {
                            await DisplayAlert(AppResources.SorryText, response.Message, "OK");
                            CloseAllPopup();
                        }
                    }
                    else
                    {
                        await DisplayAlert(AppResources.SorryText, response.Message, "OK");
                        CloseAllPopup();
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
                    //CloseAllPopup();
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void DownloadAndViewStaffAttReport()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostReportStaffAttendance(_school_id, _shift_id, _entry_month, Settings.fullName);
                    string jsonStr = await t;
                    ReportProperty response = JsonConvert.DeserializeObject<ReportProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        if (response.Message.Contains(".xlsx"))
                        {
                            DownloadAndViewExcel(response.Message);
                        }
                        else
                        {
                            await DisplayAlert(AppResources.SorryText, response.Message, "OK");
                        }
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

        //async void DownloadAndViewPDF(string file_path)
        //{
        //    string _uri = requestUrl + file_path;

        //    HttpClient client = new HttpClient();

        //    var uri = new Uri(_uri);

        //    Stream content;
        //    MemoryStream stream = new MemoryStream();

        //    var response = await client.GetAsync(uri);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        content = await response.Content.ReadAsStreamAsync();
        //        content.CopyTo(stream);
        //    }

        //    await CrossXamarinFormsSaveOpenPDFPackage.Current.SaveAndView(Guid.NewGuid() + ".pdf", "application/pdf", stream, PDFOpenContext.ChooseApp);
        //}

        void DownloadAndViewExcel(string file_path)
        {
            string _uri = requestUrl + file_path;
            filePath = downloader.DownloadFile(_uri, "downloads");
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