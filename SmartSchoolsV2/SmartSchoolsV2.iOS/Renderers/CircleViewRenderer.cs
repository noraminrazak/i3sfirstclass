using SmartSchoolsV2.Customs;
using SmartSchoolsV2.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CircleView), typeof(CircleViewRenderer))]
namespace SmartSchoolsV2.iOS.Renderers
{
    public class CircleViewRenderer : BoxRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            if (Element == null)
                return;

            Layer.MasksToBounds = true;
            Layer.CornerRadius = (float)((CircleView)Element).CornerRadius / 2.0f;
        }

    }
}