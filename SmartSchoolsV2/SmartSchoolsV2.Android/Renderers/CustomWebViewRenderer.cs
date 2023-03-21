using Android.Content;
using SmartSchoolsV2.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WebView), typeof(CustomWebViewRenderer))]
namespace SmartSchoolsV2.Droid.Renderers
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        public CustomWebViewRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var pdfView = Element as WebView;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                Control.Settings.SetSupportZoom(true);
                Control.Settings.BuiltInZoomControls = true;
                Control.Settings.DisplayZoomControls = true;
            }
        }
    }
}