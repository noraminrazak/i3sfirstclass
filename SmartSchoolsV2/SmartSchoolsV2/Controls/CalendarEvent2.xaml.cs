using SmartSchoolsV2.Models;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarEvent2 : ContentView
    {
        public static BindableProperty CalenderEventCommandProperty =
                    BindableProperty.Create(nameof(CalenderEventCommand), typeof(ICommand), typeof(CalendarEvent2), null);

        public CalendarEvent2()
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
            if (BindingContext is AdvancedEventModel2 eventModel)
            {
                CalenderEventCommand?.Execute(eventModel);
            }
        }
    }
}