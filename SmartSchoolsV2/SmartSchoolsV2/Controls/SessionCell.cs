using SmartSchoolsV2.Models;
using Xamarin.Forms;

namespace SmartSchoolsV2.Controls
{
    public class SessionCell : ViewCell
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (Device.OS == TargetPlatform.iOS)
            {
                var session = (ChatMessage)BindingContext;
                this.Height = 80;

                if (session.Message.Length > 0)
                {
                    var len = session.Message.Length * 1.2;

                    if (len > 80)
                        this.Height = len;
                }
                if (session.PhotoUrl.Length > 0)
                {
                    this.Height = 160;
                }
            }
        }
    }
}
