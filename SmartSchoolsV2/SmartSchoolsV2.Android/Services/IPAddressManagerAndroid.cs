using SmartSchoolsV2.Droid.Services;
using SmartSchoolsV2.Services;
using System.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(IPAddressManagerAndroid))]
namespace SmartSchoolsV2.Droid.Services
{
    public class IPAddressManagerAndroid : IIPAddressManager
    {
        public string GetIPAddress()
        {
            IPAddress[] adresses = Dns.GetHostAddresses(Dns.GetHostName());

            if (adresses != null && adresses[0] != null)
            {
                return adresses[0].ToString();
            }
            else
            {
                return null;
            }
        }
    }
}