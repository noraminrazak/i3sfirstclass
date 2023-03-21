
using Firebase.Messaging;
using SmartSchoolsV2.Droid.Services;
using SmartSchoolsV2.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseSubscribeAndroid))]
namespace SmartSchoolsV2.Droid.Services
{
    public class FirebaseSubscribeAndroid : IFirebaseSubscribe
    {
        public void SubscribeToTopic(string channel)
        {
            FirebaseMessaging.Instance.SubscribeToTopic(channel);
        }

        public void UnsubscribeFromTopic(string channel)
        {
            FirebaseMessaging.Instance.UnsubscribeFromTopic(channel);
        }
    }
}