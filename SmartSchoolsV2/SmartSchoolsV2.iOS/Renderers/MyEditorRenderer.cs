using SmartSchoolsV2.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Editor), typeof(MyEditorRenderer))]
namespace SmartSchoolsV2.iOS.Renderers
{
    public class MyEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Layer.CornerRadius = 4;
                Control.Layer.BorderColor = Color.FromHex("#f4f4f4").ToCGColor();
                Control.Layer.BorderWidth = 1;
            }
        }
    }
}