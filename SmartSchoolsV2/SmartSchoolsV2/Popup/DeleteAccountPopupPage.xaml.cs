using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Views;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace SmartSchoolsV2.Popup
{
    public partial class DeleteAccountPopupPage : PopupPage
    {
        public delegate void SetDetailEventHandler(object source, EventArgs args);
        public event SetDetailEventHandler DetailSet;
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _user_role_id;
        public int _staff_type_id;
        protected virtual void OnDetailSet()
        {
            if (DetailSet != null)
            {
                DetailSet(this, EventArgs.Empty);
            }
        }
        public DeleteAccountPopupPage(int user_role_id)
        {
            InitializeComponent();
            BindingContext = this;

            _user_role_id = user_role_id;
        }
        void OnCancelClicked(object sender, EventArgs args)
        {
            CloseAllPopup();
        }

        async void OnConfirmClicked(object sender, EventArgs args)
        {
            CloseAllPopup();

            if (conn.IsConnected() == true)
            {
                try
                {
                    //ShowLoadingPopup();

                    var t = srvc.PostUserDeleteAccount(Settings.userName);
                    string jsonStr = await t;
                    AccStatusProperty response = JsonConvert.DeserializeObject<AccStatusProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert("", response.Message, "OK");

                        Settings.isLogin = false;
                        Settings.profileId = 0;
                        Settings.fullName = string.Empty;
                        Settings.userRoleId = 0;
                        Settings.parentId = 0;
                        Settings.staffId = 0;
                        Settings.merchantId = 0;
                        Settings.schoolId = 0;
                        Settings.classId = 0;
                        Settings.className = string.Empty;
                        Settings.sessionCode = string.Empty;
                        Settings.lastLogin = string.Empty;

                        AccountStatusPage.Back = "Y";

                        OnDetailSet();


                    }
                    else
                    {
                        await DisplayAlert("", response.Message, "OK");
                        //SnackB.Message = response.Message;
                        //SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    //HideLoadingPopup();
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

