using SmartSchoolsV2.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FAQPage : ContentPage
	{
		public string requestUrl = Settings.requestUrl;
		public FAQPage ()
		{
			InitializeComponent ();
			BindingContext = this;
			if (Settings.cultureInfo == "en-US")
			{
				webview.Source = requestUrl + "/FAQ.aspx";
			}
			else if (Settings.cultureInfo == "ms-MY")
			{
				webview.Source = requestUrl + "/FAQ_bm.aspx";
			}
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();
			await activity_indicator.ProgressTo(0.9, 900, Easing.SpringIn);
		}

		public void OnNavigating(object sender, WebNavigatingEventArgs e)
		{
			activity_indicator.IsVisible = true;
		}

		public void OnNavigated(object sender, WebNavigatedEventArgs e)
		{
			activity_indicator.IsVisible = false;
		}

	}
}