using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentMethodPage : ContentPage
    {
        public delegate void SetDetailEventHandler(object source, EventArgs args);
        public event SetDetailEventHandler DetailSet;
        protected virtual void OnDetailSet()
        {
            if (DetailSet != null)
            {
                DetailSet(this, EventArgs.Empty);
            }
        }
        public PaymentMethodPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

    }
}