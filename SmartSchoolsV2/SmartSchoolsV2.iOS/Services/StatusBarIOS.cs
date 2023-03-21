using SmartSchoolsV2.iOS.Services;
using SmartSchoolsV2.Services;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(StatusBarIOS))]
namespace SmartSchoolsV2.iOS.Services
{
    public class StatusBarIOS : IStatusBar
    {
        public StatusBarIOS()
        {
        }

        #region IStatusBar implementation

        public void HideStatusBar()
        {
            UIApplication.SharedApplication.StatusBarHidden = true;
        }

        public void ShowStatusBar()
        {
            UIApplication.SharedApplication.StatusBarHidden = false;
        }

        #endregion
    }
}