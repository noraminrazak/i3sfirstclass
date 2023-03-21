using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using Firebase.Messaging;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Services;
using System;
using System.Globalization;

namespace SmartSchoolsV2.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    class MyFirebaseMessagingService : FirebaseMessagingService
    {
        // id message, if you remove messages will be replaced by new ones
        private static int idPush = 1;

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            try
            {
                if (message.GetNotification() != null)
                {
                    var notification = new PushNotification()
                    {
                        Title = message.GetNotification().Title,
                        Body = message.GetNotification().Body,
                    };

                    SendNotification(notification);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void SendNotification(PushNotification message)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Drawable.icon_60)   // Display this icon
                .SetContentTitle(message.Title)      // Set its title
                .SetContentText(message.Body)           // The message to display.
                .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
                .SetSound(defaultSoundUri)              // Sound of message
                .SetContentIntent(pendingIntent);


            if (!App.IsActive)
            {
                var notificationManager = NotificationManager.FromContext(this);
                notificationManager.Notify(idPush++, notificationBuilder.Build());
            }
            else
            {
                var chatService = ViewModelLocator.Instance.Resolve(typeof(ChatService)) as IChatService;
                chatService.OnMessageReceived(message);
            }
        }
    }
}