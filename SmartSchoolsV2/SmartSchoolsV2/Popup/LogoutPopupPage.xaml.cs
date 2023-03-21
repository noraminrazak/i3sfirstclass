using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogoutPopupPage : PopupPage
    {
        public delegate void SetDetailEventHandler(object source, EventArgs args);
        public event SetDetailEventHandler DetailSet;
        public int _user_role_id;
        public int _staff_type_id;
        protected virtual void OnDetailSet()
        {
            if (DetailSet != null)
            {
                DetailSet(this, EventArgs.Empty);
            }
        }
        public LogoutPopupPage(int user_role_id)
        {
            InitializeComponent();
            BindingContext = this;

            _user_role_id = user_role_id;
        }
        void OnCancelClicked(object sender, EventArgs args)
        {
            CloseAllPopup();
        }
        void OnYesClicked(object sender, EventArgs args)
        {
            CloseAllPopup();
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

            if (_user_role_id == 9)
            {
                MainPage.Back = "Y";
            }
            else if (_user_role_id == 8) 
            {
                int _staff_type_id = Settings.staffTypeId;
                if (_staff_type_id == 1 || _staff_type_id == 6) 
                {
                    StaffPage1.Back = "Y";
                } 
                else if (_staff_type_id == 2) 
                {
                    StaffPage2.Back = "Y";
                }
                else if (_staff_type_id == 3)
                {
                    StaffPage3.Back = "Y";
                }
                else if (_staff_type_id == 4)
                {
                    StaffPage4.Back = "Y";
                }
                else if (_staff_type_id == 5)
                {
                    StaffPage5.Back = "Y";
                }
                Settings.staffTypeId = 0;
            }
            else if (_user_role_id == 7) 
            {
                MerchantPage.Back = "Y";
            }

            OnDetailSet();
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}