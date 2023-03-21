using System;
using FFImageLoading;
using FFImageLoading.Forms.Platform;
using Firebase.CloudMessaging;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using Naxam.Controls.Platform.iOS;
using Plugin.HybridWebView.iOS;
using Prism;
using Prism.Ioc;
using SmartSchoolsV2.Class;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace SmartSchoolsV2.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags("Expander_Experimental");
            Forms.SetFlags("RadioButton_Experimental"); // Add here
            Forms.SetFlags("SwipeView_Experimental"); // Add here
            Forms.SetFlags("FastRenderers_Experimental");
            Rg.Plugins.Popup.Popup.Init();

            Firebase.Core.App.Configure();
            HybridWebViewRenderer.Initialize();
            global::Xamarin.Forms.Forms.Init();
            XamForms.Controls.iOS.Calendar.Init();
            TopTabbedRenderer.Init();

            CachedImageRenderer.Init();
            CachedImageRenderer.InitImageSourceHandler();

            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false,
                Logger = new CustomLogger(),
            };
            ImageService.Instance.Initialize(config);
            ImageService.Instance.InvalidateCacheAsync(FFImageLoading.Cache.CacheType.All);

            ImageCircleRenderer.Init();
            Xamarin.KeyboardHelper.Platform.iOS.Effects.Init();

            LoadApplication(new App(new iOSInitializer()));

            RegisterForRemoteNotifications();

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                                   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                                   new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
            //FirebasePushNotificationManager.Initialize(options, new NotificationUserCategory[]
            //{
            //    new NotificationUserCategory("message",new List<NotificationUserAction> {
            //        new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground)
            //    }),
            //    new NotificationUserCategory("request",new List<NotificationUserAction> {
            //        new NotificationUserAction("Accept","Accept"),
            //        new NotificationUserAction("Reject","Reject",NotificationActionType.Destructive)
            //    })

            //});

            return base.FinishedLaunching(app, options);
        }

        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            // show an alert
            new UIAlertView(notification.AlertAction, notification.AlertBody, null, "OK", null).Show();

            // reset our badge
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
            if (null != options && options.ContainsKey(new NSString("aps")))
            {
                //Get the aps dictionary
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                string alert = string.Empty;

                if (aps.ContainsKey(new NSString("alert")))
                    alert = (aps[new NSString("alert")] as NSString).ToString();

                if (!fromFinishedLaunching)
                {
                    //Manually show an alert
                    if (!string.IsNullOrEmpty(alert))
                    {
                        UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);
                        avAlert.Show();
                    }
                }
            }
        }

        private void RegisterForRemoteNotifications()
        {
            // Register your app for remote notifications.

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;

                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;

                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, async (granted, error) =>
                {
                    Console.WriteLine(granted);
                    await System.Threading.Tasks.Task.Delay(500);
                });
            }
            else
            {
                // iOS 9 or before

                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;

                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Messaging.SharedInstance.ApnsToken = deviceToken;
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            base.FailedToRegisterForRemoteNotifications(application, error);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)

        {

            completionHandler(UIBackgroundFetchResult.NewData);

        }

        [Export("messaging: didReceiveRegistrationToken:")]

        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {

            Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = Messaging.SharedInstance.FcmToken ?? "";

            Xamarin.Forms.Application.Current.SavePropertiesAsync();

            System.Diagnostics.Debug.WriteLine($"######Token######  :  {fcmToken}");

            Settings.deviceToken = fcmToken;

            Console.WriteLine(fcmToken);
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            switch (Device.Idiom)
            {
                case TargetIdiom.Phone:
                    return UIInterfaceOrientationMask.Portrait;
                case TargetIdiom.Tablet:
                    return UIInterfaceOrientationMask.Landscape;
                default:
                    return UIInterfaceOrientationMask.Portrait;
            }
        }

        //// To receive notifications in foregroung on iOS 9 and below.
        //// To receive notifications in background in any iOS version
        //public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        //{
        //    // If you are receiving a notification message while your app is in the background,
        //    // this callback will not be fired 'till the user taps on the notification launching the application.

        //    // If you disable method swizzling, you'll need to call this method. 
        //    // This lets FCM track message delivery and analytics, which is performed
        //    // automatically with method swizzling enabled.
        //    FirebasePushNotificationManager.DidReceiveMessage(userInfo);
        //    // Do your magic to handle the notification data
        //    System.Console.WriteLine(userInfo);

        //    completionHandler(UIBackgroundFetchResult.NewData);
        //}

        public class CustomLogger : FFImageLoading.Helpers.IMiniLogger
        {
            public void Debug(string message)
            {
                Console.WriteLine(message);
            }

            public void Error(string errorMessage)
            {
                Console.WriteLine(errorMessage);
            }

            public void Error(string errorMessage, Exception ex)
            {
                Error(errorMessage + System.Environment.NewLine + ex.ToString());
            }
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}
