using SmartSchoolsV2.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarHeader : DataTemplate
    {
        public CalendarHeader()
        {
            InitializeComponent();
        }
        private void PrevMonthCommand_Tapped(object sender, System.EventArgs e)
        {
            StudentAttendanceView.LoadPrevMonth.Execute(null);
        }
        private void NextMonthCommand_Tapped(object sender, System.EventArgs e)
        {
            StudentAttendanceView.LoadNextMonth.Execute(null);
        }
    }
}