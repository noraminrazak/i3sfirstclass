using Android.Content;
using Android.Widget;
using SmartSchoolsV2.Controls;
using SmartSchoolsV2.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessPicker), typeof(BorderlessPickerRenderer))]
namespace SmartSchoolsV2.Droid.Renderers
{
    public class BorderlessPickerRenderer : PickerRenderer
    {
        public BorderlessPickerRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                //Control.Background = null;

                var nativeEditText = (PickerEditText)Control;
                nativeEditText.SetPadding(10, 5, 10, 50);
            }
        }
    }
}