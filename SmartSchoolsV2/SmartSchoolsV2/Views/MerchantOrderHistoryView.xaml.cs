using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantOrderHistoryView : ContentView
    {
        public static Command LoadMerchantOrderHistory { get; set; }
        //public static Command SelectedOrder { get; set; }
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
        public MerchantOrderHistoryView()
        {
            InitializeComponent();

            OrderGroupViewModel viewModel = new OrderGroupViewModel();
            viewModel.LoadOrderCommand.Execute(null);
            this.ViewModel = viewModel;

            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) => {
                PopupNavigation.Instance.PushAsync(new OrderDownloadPopupPage(Settings.merchantId, Settings.merchantSchoolId));
            };
            btnDownload.GestureRecognizers.Add(tapGestureRecognizerTxn);
        }

        public OrderGroupViewModel ViewModel
        {
            get { return (OrderGroupViewModel)BindingContext; }
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

    }
}