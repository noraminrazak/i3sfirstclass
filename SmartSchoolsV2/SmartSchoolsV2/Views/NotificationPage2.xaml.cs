using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationPage2 : ContentPage
	{
		Connection conn = new Connection();
		ServiceWrapper srvc = new ServiceWrapper();
		public string requestUrl = Settings.requestUrl;
		UserNotify _data = new UserNotify();

        public ICommand TapCommand => new Command(async (url) => await Browser.OpenAsync(url.ToString(), BrowserLaunchMode.SystemPreferred));
        public NotificationPage2 (UserNotify data)
		{
			InitializeComponent ();
			BindingContext = this;

			_data = data;

			lblSubject.Text = _data.notify_subject;
			lblMessage.Text = _data.notify_message_full;
            if (!string.IsNullOrEmpty(_data.notify_photo_url)) {
                imgFrame.IsVisible = true;
                imagePhoto.Source = requestUrl + _data.notify_photo_url;
            }

            if (!string.IsNullOrEmpty(_data.notify_link)) 
            {
                lblNotify.IsVisible = true;
                lblNotifyMessage.Text = _data.notify_link;
                lblNotifyLink.Text = _data.notify_link_text;
                lblNotifyParam.CommandParameter = _data.notify_link_param;
            }
		}
		protected override async void OnAppearing()
		{
			base.OnAppearing();
            if (_data.read_flag == "N") 
            {
                await UpdateNotify(_data.notify_id);
            }
		}

        public async Task UpdateNotify(int notify_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    //ShowLoadingPopup();

                    var t = srvc.PostUserUpdateNotify(notify_id, Settings.profileId, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        //nothing
                    }
                    else
                    {
                        SnackB.Message = AppResources.SomethingWrongText;
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
                    //HideLoadingPopup();
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }
    }
}