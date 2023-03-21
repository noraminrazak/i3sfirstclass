using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Views;
using System;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaveClubPopupPage : PopupPage
    {
        public int _club_id;
        public string _club_name;
        public string _school_name;
        public delegate void SetDetailEventHandler(object source, EventArgs args);
        public event SetDetailEventHandler DetailSet;
        protected virtual void OnDetailSet()
        {
            if (DetailSet != null)
            {
                DetailSet(this, EventArgs.Empty);
            }
        }
        public LeaveClubPopupPage(int club_id, string club_name, string school_name)
        {
            InitializeComponent();
            BindingContext = this;

            _club_id = club_id;
            _club_name = club_name;
            _school_name = school_name;

            lblClubName.Text = club_name;
            lblSchoolName.Text = school_name;
        }

        void OnCancelClicked(object sender, EventArgs args)
        {
            CloseAllPopup();
        }
        void OnLeaveClicked(object sender, EventArgs args)
        {
            CloseAllPopup();
            ClubPage.Back = "Y";
            OnDetailSet();
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}