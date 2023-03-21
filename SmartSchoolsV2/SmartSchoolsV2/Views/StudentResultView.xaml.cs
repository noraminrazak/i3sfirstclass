using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StudentResultView : ContentView
	{
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public static Command LoadSchoolInfo { get; set; }
        public string _school_website;
        public string _school_result_url;
		public StudentResultView ()
		{
			InitializeComponent ();
			this.BindingContext = this;

			LoadSchoolInfo = new Command(async () => await SchoolInfo());
		}

        public async Task SchoolInfo()
        {
            int school_id = Settings.studentSchoolId;
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostSchoolInfo(school_id);
                    string jsonStr = await t;
                    SchoolInfoProperty response = JsonConvert.DeserializeObject<SchoolInfoProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<SchoolInfo> list = new List<SchoolInfo>();
                        foreach (SchoolInfo sl in response.Data)
                        {
                            SchoolInfo post = new SchoolInfo();
                            _school_website = sl.school_website;
                            if (string.IsNullOrEmpty(sl.school_result_url)) 
                            {
                                btnOpenBrowser.IsEnabled = false;
                                await App.Current.MainPage.DisplayAlert("", "School result page is not available.", "OK");
                            }
                            else
                            {
                                _school_result_url = sl.school_result_url;
                            }
                        }
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
                    HideLoadingPopup();
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void OnOpenBrowserClicked(object sender, EventArgs args)
        {
            try
            {
                await Browser.OpenAsync(_school_result_url, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception)
            {
                // An unexpected error occured. No browser may be installed on the device.
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