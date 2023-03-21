using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using SmartSchoolsV2.iOS.Renderers;
using SmartSchoolsV2.Customs;
using System;

[assembly: ExportRenderer(typeof(TimePicker), typeof(CustomTimePickerRenderer))]
namespace SmartSchoolsV2.iOS.Renderers
{
	public class CustomTimePickerRenderer : TimePickerRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
		{
            base.OnElementChanged(e);

            if (e.NewElement != null && this.Control != null)
            {
                try
                {
                    if (UIDevice.CurrentDevice.CheckSystemVersion(14, 0))
                    {
                        UIDatePicker picker = (UIDatePicker)Control.InputView;
                        picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
                    }
                }
                catch (Exception ex)
                {
                    //Log.Error(ex, "Failed to set PreferredDatePickerStyle to be UIDatePickerStyle.Wheels for iOS 14.0+");
                }
            }
		}
	}
}