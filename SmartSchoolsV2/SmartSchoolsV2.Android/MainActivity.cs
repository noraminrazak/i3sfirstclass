using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism.Ioc;
using Prism;
using ImageCircle.Forms.Plugin.Droid;
using FFImageLoading.Forms.Platform;
using Xamarin.Forms;
using FFImageLoading;
using System;
using SmartSchoolsV2.Class;
using Android.Gms.Common;
using Firebase.Iid;
using Firebase;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using Firebase.Messaging;
using Android.Support.V4.App;
using Android;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Plugin.HybridWebView.Droid;

namespace SmartSchoolsV2.Droid
{
    [Activity(Label = "SmartSchoolsV2", Icon = "@drawable/icon_512",  Theme = "@style/MainTheme",  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            InitFirebaseAuth();
            HybridWebViewRenderer.Initialize();
            Rg.Plugins.Popup.Popup.Init(this);

            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

            Forms.SetFlags("Expander_Experimental");
            Forms.SetFlags("RadioButton_Experimental");
            Forms.SetFlags("SwipeView_Experimental");
            Forms.SetFlags("FastRenderers_Experimental");

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

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            XamForms.Controls.Droid.Calendar.Init();
            ImageCircleRenderer.Init();
            //Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);
            Xamarin.KeyboardHelper.Platform.Droid.Effects.Init(this);
            XamEffects.Droid.Effects.Init();
            CachedImageRenderer.Init(true);
            CachedImageRenderer.InitImageViewHandler();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                if (!(CheckPermissionGranted(Manifest.Permission.ReadExternalStorage) && !CheckPermissionGranted(Manifest.Permission.WriteExternalStorage)))
                {
                    RequestPermission();
                }
            }

            //if (Intent.Extras != null)
            //{
            //    foreach (var key in Intent.Extras.KeySet())
            //    {
            //        if (key != null)
            //        {
            //            var value = Intent.Extras.GetString(key);
            //            //string push_type = Intent.Extras.GetString("push_type");
            //            LoadApplication(new App());
            //        }
            //    }
            //}
            //else 
            //{
                LoadApplication(new App());
            //}

            IsPlayServicesAvailable();

            CreateNotificationChannel();

            //if (!GetString(Resource.String.google_app_id).Equals("1:459292767069:android:28d1a0e374b52121"))
            //    throw new SystemException("Invalid Json file");

            GetTokenFcm();

        }

        private void RequestPermission()
        {
            ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage }, 0);
        }

        public bool CheckPermissionGranted(string Permissions)
        {
            // Check if the permission is already available.
            if (ActivityCompat.CheckSelfPermission(this, Permissions) != Permission.Granted)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //protected override void OnNewIntent(Intent intent)
        //{
        //    base.OnNewIntent(intent);
        //    FirebasePushNotificationManager.ProcessIntent(this, intent);
        //}
        static readonly string TAG = "MainActivity";

        [Obsolete]
        private void GetTokenFcm()
        {
            Task.Run(async() =>
            {
                var instanceIdResult = await FirebaseInstanceId.Instance.GetInstanceId().AsAsync<IInstanceIdResult>();
                Settings.deviceToken = instanceIdResult.Token;
                // FirebaseSubscribeToTopic
                await FirebaseMessaging.Instance.SubscribeToTopic("admin");
            });
        }
        protected override void OnStart()
        {
            base.OnStart();
            App.IsActive = true;
        }

        protected override void OnStop()
        {
            base.OnStop();
            App.IsActive = false;
        }
        public static FirebaseApp app;
        private void InitFirebaseAuth()
        {
            var options = new FirebaseOptions.Builder()
            .SetApplicationId("1:754750697278:android:02dd4a3d95f29e4536466f")
            .SetApiKey("AIzaSyBiv4JZp2VufdtUcOWT_-jULOi-HdfKkys")
            .SetDatabaseUrl("https://i3sv2-1f5dd-default-rtdb.asia-southeast1.firebasedatabase.app")
            .Build();

            if (app == null)
                app = FirebaseApp.InitializeApp(this, options, "SmartSchoolsV2");

        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    //msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }
                else
                {
                    //msgText.Text = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                //msgText.Text = "Google Play Services is available.";
                return true;
            }
        }


        internal static readonly string CHANNEL_ID = "general";
        internal static readonly int NOTIFICATION_ID = 100;
        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

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
    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }

}