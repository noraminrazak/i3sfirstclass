using SmartSchoolsV2.Class;
using SmartSchoolsV2.Resources;
using System;
using System.Globalization;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class SelectLanguagePage : ContentPage
    {
        public int _mode;
        public SelectLanguagePage(int mode)
        {
            InitializeComponent();
            BindingContext = this;

            _mode = mode;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_mode == 1)
            {
                //lblWelcome.IsVisible = true;
            }
            else {
                //lblWelcome.IsVisible = false;
            }
        }
        void OnEnglishClicked(object sender, EventArgs args)
        {
            Settings.cultureInfo = "English";
            var language = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = language;
            AppResources.Culture = language;
            Application.Current.MainPage = new NavigationPage(new LoginPage1());
        }
        void OnMalayClicked(object sender, EventArgs args)
        {
            Settings.cultureInfo = "Malay";
            var language = new CultureInfo("ms-MY");
            Thread.CurrentThread.CurrentUICulture = language;
            AppResources.Culture = language;
            Application.Current.MainPage = new NavigationPage(new LoginPage1());
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}