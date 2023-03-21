using SmartSchoolsV2.Models;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarEvent : ContentView
    {
        public static BindableProperty CalenderEventCommandProperty =
                    BindableProperty.Create(nameof(CalenderEventCommand), typeof(ICommand), typeof(CalendarEvent), null);

        public CalendarEvent()
        {
            InitializeComponent();
        }

        public ICommand CalenderEventCommand
        {
            get => (ICommand)GetValue(CalenderEventCommandProperty);
            set => SetValue(CalenderEventCommandProperty, value);
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            if (BindingContext is AdvancedEventModel eventModel)
            {
                CalenderEventCommand?.Execute(eventModel);
            }
        }
    }
}