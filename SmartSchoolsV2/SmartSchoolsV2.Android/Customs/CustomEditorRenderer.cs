
using Android.Widget;
using SmartSchoolsV2.Customs;
using SmartSchoolsV2.Droid.Customs;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
#pragma warning disable CS0612
[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
#pragma warning restore CS0612
namespace SmartSchoolsV2.Droid.Customs
{
    [System.Obsolete]
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var nativeEditText = (EditText)Control;
                nativeEditText.SetPadding(10, 5, 10, 50);
            }
        }
    }
}