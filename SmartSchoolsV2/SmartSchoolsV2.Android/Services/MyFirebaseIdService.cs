using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Iid;
using Xamarin.Forms;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using Newtonsoft.Json;
using System;
using SmartSchoolsV2.Services;
using Android.Gms.Extensions;
using System.Threading.Tasks;

namespace SmartSchoolsV2.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIdService : FirebaseInstanceIdService
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public int devicePlatformId = 0;
        const string TAG = "MyFirebaseIIDService";

        [Obsolete]
        public override void OnTokenRefresh()
        {
            Task.Run(async () =>
            {
                var refreshedToken = await FirebaseInstanceId.Instance.GetInstanceId().AsAsync<IInstanceIdResult>();
                Log.Debug(TAG, "Refreshed token: " + refreshedToken);
                Settings.deviceToken = refreshedToken.ToString();
                if (Settings.profileId > 0)
                {
                    UpdateDeviceToken(refreshedToken.ToString());
                }
            });
        }

        public async void UpdateDeviceToken(string token)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        devicePlatformId = 1;
                    }
                    else if (Device.RuntimePlatform == Device.iOS)
                    {
                        devicePlatformId = 2;
                    }
                    var t = srvc.PostUserUpdateDeviceToken(Settings.profileId, Settings.deviceToken, devicePlatformId, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        //do nothing
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                }
            }
            else
            {

            }

        }
    }
}