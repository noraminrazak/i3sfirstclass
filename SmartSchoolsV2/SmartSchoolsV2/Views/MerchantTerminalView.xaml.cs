using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantTerminalView : ContentView, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<MerchantTerminal> listPOS { get; set; }
        public static Command LoadMerchantTerminal { get; set; }
        ViewCell lastCell;
        public int _school_id;
        public int _merchant_id;
        public string _receipt_date;
        bool isBusy;
        DateTime dtToday = DateTime.Today;
        DateTime dtSelected = DateTime.Today;
        DateTime dtCurrent = DateTime.Today;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public MerchantTerminalView()
        {
            InitializeComponent();
            BindingContext = this;

            _receipt_date = dtToday.ToString("yyyy-MM-dd");
            lblDay.Text = dtSelected.ToString("dd MMMM yyyy", ci);
            lblTotalFor.Text = AppResources.TotalForText + dtSelected.ToString("dd MMMM yyyy", ci);
            LoadMerchantTerminal = new Command(async () => await MerchantTerminal());
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await MerchantTerminal();
                });
            }
        }

        public async Task MerchantTerminal()
        {
            _school_id = Settings.merchantSchoolId;
            _merchant_id = Settings.merchantId;

            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvTerminal.ItemsSource = null;
                    var t = srvc.PostMerchantTerminal(_school_id, _merchant_id, _receipt_date);
                    string jsonStr = await t;
                    MerchantTerminalProperty response = JsonConvert.DeserializeObject<MerchantTerminalProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        decimal amount = 0;
                        int totalTxn = 0;
                        listPOS = new ObservableCollection<MerchantTerminal>();
                        foreach (MerchantTerminal sl in response.Data)
                        {
                            MerchantTerminal post = new MerchantTerminal();
                            post.terminal_id = sl.terminal_id;
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            post.tag_number = sl.tag_number;
                            post.serial_number = sl.serial_number;
                            post.hardware_status = sl.hardware_status;
                            post.total_amount = "RM " + sl.total_amount;
                            post.total_transaction = sl.total_transaction;
                            listPOS.Add(post);

                            amount += Convert.ToDecimal(sl.total_amount);
                            totalTxn += sl.total_transaction;
                        }
                        RowCount = listPOS.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvTerminal.Footer = l;
                        lvTerminal.ItemsSource = listPOS;
                        lblTotalAmount.Text = "RM " + amount.ToString("F") + AppResources._FromText_ + totalTxn.ToString() + AppResources._SalesText;
                    }
                    else
                    {
                        listPOS = new ObservableCollection<MerchantTerminal>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvTerminal.Footer = l;
                        lvTerminal.ItemsSource = listPOS;
                        lblTotalAmount.Text = "RM 0.00" + AppResources._FromText_ + "0" + AppResources._SalesText;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public ICommand PrevDayCommand
        {
            get
            {
                return new Command(async () =>
                {
                    dtSelected = dtCurrent.AddDays(-1);
                    dtCurrent = dtSelected;
                    _receipt_date = dtSelected.ToString("yyyy-MM-dd");
                    lblDay.Text = dtSelected.ToString("dd MMMM yyyy", ci);
                    lblTotalFor.Text = AppResources.TotalForText + dtSelected.ToString("dd MMMM yyyy", ci);
                    await MerchantTerminal();
                });
            }
        }

        public ICommand NextDayCommand
        {
            get
            {
                return new Command(async () =>
                {
                    dtSelected = dtCurrent.AddDays(1);
                    dtCurrent = dtSelected;
                    _receipt_date = dtSelected.ToString("yyyy-MM-dd");
                    lblDay.Text = dtSelected.ToString("dd MMMM yyyy", ci);
                    lblTotalFor.Text = AppResources.TotalForText + dtSelected.ToString("dd MMMM yyyy", ci);
                    await MerchantTerminal();
                });
            }
        }

        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.LightGoldenrodYellow;
                lastCell = viewCell;
            }
        }

        public MerchantTerminal pos;
        async void OnTerminalSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as MerchantTerminal;
            if (data == null) return;
            pos = data;

            if (pos.terminal_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...

                await Navigation.PushAsync(new MerchantTerminalPage(_merchant_id, pos.school_id, pos.school_name, _receipt_date, pos.terminal_id, pos.tag_number, pos.serial_number, pos.total_amount));
                ((ListView)sender).SelectedItem = null;
            }
        }

        async void OnStatementClicked(object sender, EventArgs args) 
        {
            await Navigation.PushAsync(new MerchantSettlementPage());
        }
    }
}