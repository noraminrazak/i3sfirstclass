using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using SmartSchoolsV2.Customs;
using SmartSchoolsV2.Droid.Customs;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

#pragma warning disable CS0612
[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
#pragma warning restore CS0612
namespace SmartSchoolsV2.Droid.Customs
{
    [System.Obsolete]
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var nativeEditText = (EditText)Control;
                nativeEditText.SetPadding(10,5,10,50);
            }
        }
    }
}