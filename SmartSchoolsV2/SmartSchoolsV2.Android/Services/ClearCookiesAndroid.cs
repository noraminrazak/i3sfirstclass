using Android.Webkit;
using SmartSchoolsV2.Droid.Services;
using SmartSchoolsV2.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(ClearCookiesAndroid))]
namespace SmartSchoolsV2.Droid.Services
{
    public class ClearCookiesAndroid : IClearCookies
    {
        public void Clear()
        {
            var cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookie();
        }
    }
}