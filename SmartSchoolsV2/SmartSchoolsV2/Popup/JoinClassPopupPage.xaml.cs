using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Views;
using System;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinClassPopupPage : PopupPage
    {
        public int _class_id;
        public string _class_name;
        public string _session_code;
        public delegate void SetDetailEventHandler(object source, EventArgs args);
        public event SetDetailEventHandler DetailSet;
        protected virtual void OnDetailSet()
        {
            if (DetailSet != null)
            {
                DetailSet(this, EventArgs.Empty);
            }
        }
        public JoinClassPopupPage(int class_id, string class_name, string session_code)
        {
            InitializeComponent();
            BindingContext = this;

            _class_id = class_id;
            _class_name = class_name;
            _session_code = session_code;

            lblClassName.Text = class_name;
            lblSessionCode.Text = session_code;
        }

        void OnCancelClicked(object sender, EventArgs args)
        {
            CloseAllPopup();
        }
        void OnJoinClicked(object sender, EventArgs args)
        {
            CloseAllPopup();
            ClassView2.Back = "X";
            OnDetailSet();
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

    }
}