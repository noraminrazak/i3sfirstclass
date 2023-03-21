using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.ViewModels;
using SmartSchoolsV2.Class;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantSettlementPage : ContentPage
    {
        ViewCell lastCell;

        bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public MerchantSettlementPage()
        {
            InitializeComponent();
            SettlementGroupViewModel viewModel = new SettlementGroupViewModel();
            viewModel.LoadReportCommand.Execute(null);
            this.ViewModel = viewModel;

            lblSchoolName.Text = Settings.selectedSchoolName;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public SettlementGroupViewModel ViewModel
        {
            get { return (SettlementGroupViewModel)BindingContext; }
            set { BindingContext = value; }
        }
        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.Transparent;
                lastCell = viewCell;
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
    }
}