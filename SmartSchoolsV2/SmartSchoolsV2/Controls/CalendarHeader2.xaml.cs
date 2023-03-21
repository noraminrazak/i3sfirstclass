using SmartSchoolsV2.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarHeader2 : DataTemplate
    {
        public CalendarHeader2()
        {
            InitializeComponent();
        }
        private void PrevMonthCommand_Tapped(object sender, System.EventArgs e)
        {
            ClassAttendanceView.LoadPrevMonth.Execute(null);
        }
        private void NextMonthCommand_Tapped(object sender, System.EventArgs e)
        {
            ClassAttendanceView.LoadNextMonth.Execute(null);
        }
    }
}