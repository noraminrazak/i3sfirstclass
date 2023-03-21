using Foundation;
using SmartSchoolsV2.iOS.Services;
using SmartSchoolsV2.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSClearCookies))]
namespace SmartSchoolsV2.iOS.Services
{
    public class iOSClearCookies : IClearCookies
    {
        public void Clear()
        {
            NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;
            foreach (var cookie in CookieStorage.Cookies)
                CookieStorage.DeleteCookie(cookie);
        }
    }
}