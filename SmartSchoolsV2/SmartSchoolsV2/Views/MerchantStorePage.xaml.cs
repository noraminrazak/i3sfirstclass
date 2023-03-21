using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantStorePage : ContentPage
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<SchoolPost> _listPOS;
        public ObservableCollection<SchoolPost> listPOS
        {
            get
            {
                return _listPOS;
            }
            set
            {
                _listPOS = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("listPOS"));
            }
        }

        public ObservableCollection<ParentStudentRelationship> _listProduct;
        public ObservableCollection<ParentStudentRelationship> listProduct
        {
            get
            {
                return _listProduct;
            }
            set
            {
                _listProduct = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("listProduct"));
            }
        }
        public ObservableCollection<ParentStudentRelationship> _listOrder;
        public ObservableCollection<ParentStudentRelationship> listOrder
        {
            get
            {
                return _listOrder;
            }
            set
            {
                _listOrder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("listOrder"));
            }
        }

        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _school_id;
        public string _school_name;
        public string _city;
        public MerchantStorePage(int school_id, string school_name , string city)
        {
            InitializeComponent();
            BindingContext = this;
            _school_id = school_id;
            _school_name = school_name;
            _city = city;

            var tapGestureRecognizer0 = new TapGestureRecognizer();
            tapGestureRecognizer0.Tapped += (s, e) => {
                carouselView.Position = 0;
            };
            imgPOS.GestureRecognizers.Add(tapGestureRecognizer0);

            var tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += (s, e) => {
                carouselView.Position = 1;
            };
            imgInvoice.GestureRecognizers.Add(tapGestureRecognizer1);

            var tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += (s, e) => {
                carouselView.Position = 2;
            };
            imgProduct.GestureRecognizers.Add(tapGestureRecognizer2);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblSchoolName.Text = _school_name;
            Settings.merchantSchoolId = _school_id;
            lblCity.Text = _city;

            if (carouselView.Position == 0)
            {
                MerchantTerminalView.LoadMerchantTerminal.Execute(null);
            }
            else if (carouselView.Position == 1)
            {
                OrderGroupViewModel viewModel = new OrderGroupViewModel();
                viewModel.LoadOrderCommand.Execute(null);
            }
            else if (carouselView.Position == 2)
            {
                MerchantProductView.LoadMerchantProduct.Execute(null);
            }

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }


        void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            int previousItemPosition = e.PreviousPosition;
            int currentItemPosition = e.CurrentPosition;
            var smallSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            var microSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
            if (currentItemPosition == 0)
            {
                MerchantTerminalView.LoadMerchantTerminal.Execute(null);

                imgPOS.Source = "ic_edc.png";
                imgInvoice.Source = "ic_invoice_grey.png";
                imgProduct.Source = "ic_product_grey.png";
            }
            else if (currentItemPosition == 1)
            {
                OrderGroupViewModel viewModel = new OrderGroupViewModel();
                viewModel.LoadOrderCommand.Execute(null);

                imgPOS.Source = "ic_edc_grey.png";
                imgInvoice.Source = "ic_invoice.png";
                imgProduct.Source = "ic_product_grey.png";
            }
            else if (currentItemPosition == 2)
            {

                MerchantProductView.LoadMerchantProduct.Execute(null);

                imgPOS.Source = "ic_edc_grey.png";
                imgInvoice.Source = "ic_invoice_grey.png";
                imgProduct.Source = "ic_product.png";
            }
        }
    }
}