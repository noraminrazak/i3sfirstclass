using Foundation;
using SmartSchoolsV2.iOS.Services;
using SmartSchoolsV2.Services;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Xamarin.Forms;

[assembly: Dependency(typeof(IPAddressManagerIOS))]
namespace SmartSchoolsV2.iOS.Services
{
    public class IPAddressManagerIOS : IIPAddressManager
    {
        public string GetIPAddress()
        {
            String ipAddress = "";

            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (var addrInfo in netInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddress = addrInfo.Address.ToString();

                        }
                    }
                }
            }

            return ipAddress;
        }
    }
}