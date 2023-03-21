using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using SmartSchoolsV2.Customs;
using SmartSchoolsV2.Droid.Customs;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

#pragma warning disable CS0612
[assembly: ExportRenderer(typeof(CurrencyEntry), typeof(CurrencyEntryRenderer))]
#pragma warning restore CS0612
namespace SmartSchoolsV2.Droid.Customs
{
    [System.Obsolete]
    public class CurrencyEntryRenderer : EntryRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var nativeEditText = (EditText)Control;
                nativeEditText.SetPadding(110, 5, 10, 50);
            }
        }
    }
}