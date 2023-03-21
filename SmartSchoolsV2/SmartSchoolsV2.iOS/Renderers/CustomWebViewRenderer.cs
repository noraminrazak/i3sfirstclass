using SmartSchoolsV2.iOS.Renderers;
using System;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WebView), typeof(CustomWebViewRenderer))]
namespace SmartSchoolsV2.iOS.Renderers
{
    public class CustomWebViewRenderer : ViewRenderer<WebView, WKWebView>
    {
        WKWebView _wkWebView;
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                WKWebViewConfiguration config = new WKWebViewConfiguration();

                _wkWebView = new WKWebView(Frame, config);


                SetNativeControl(_wkWebView);
            }
            if (e.NewElement != null)
            {

                HtmlWebViewSource source = (Xamarin.Forms.HtmlWebViewSource)Element.Source;
                string headerString = "<header><meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0'></header>";
                string html = headerString + source.Html;
                Console.WriteLine("Height" + Element);
                _wkWebView.LoadHtmlString(html, baseUrl: null);
                _wkWebView.ScrollView.ScrollEnabled = false;
                _wkWebView.SizeToFit();
            }
        }
    }
}