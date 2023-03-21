using Xamarin.Forms;

namespace SmartSchoolsV2.Views
{
    public class XamChatNavigationPage : NavigationPage
    {
        public XamChatNavigationPage(Page page) : base(page)
        {

        }

        public XamChatNavigationPage() : base()
        {

        }

        void SetColor()
        {
            BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"];
            BarTextColor = (Color)Application.Current.Resources["PrimaryTextColor"];
        }
    }
}
