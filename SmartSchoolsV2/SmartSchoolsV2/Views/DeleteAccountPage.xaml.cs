using SmartSchoolsV2.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteAccountPage : ContentPage
    {
        public ICommand TapEmail => new Command(async () => await Launcher.OpenAsync($"mailto:{"cs@emerging.com.my"}?subject={AppResources.AccountClosureText}"));
        public DeleteAccountPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}