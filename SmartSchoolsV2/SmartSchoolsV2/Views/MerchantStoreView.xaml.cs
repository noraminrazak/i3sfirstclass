using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchantStoreView : ContentView, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public int _merchant_id;
        public static Label l = new Label();
        public ObservableCollection<MerchantSchoolRelationship> listStore { get; set; }
        public static Command LoadMerchantSchoolRelationship { get; set; }
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
        public MerchantStoreView()
        {
            InitializeComponent();
            this.BindingContext = this;
            _merchant_id = Settings.merchantId;
            LoadMerchantSchoolRelationship = new Command(async () => await MerchantSchoolRelationship());

            //MerchantSchoolRelationship();
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await MerchantSchoolRelationship();
                });
            }
        }

        public async Task MerchantSchoolRelationship()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvStore.ItemsSource = null;
                    var t = srvc.PostMerchantSchoolRelationship(_merchant_id);
                    string jsonStr = await t;
                    MerchantSchoolRelationshipProperty response = JsonConvert.DeserializeObject<MerchantSchoolRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listStore = new ObservableCollection<MerchantSchoolRelationship>();
                        foreach (MerchantSchoolRelationship sl in response.Data)
                        {
                            MerchantSchoolRelationship post = new MerchantSchoolRelationship();
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            post.school_type_id = sl.school_type_id;
                            post.school_type = sl.school_type;
                            post.state_name = sl.state_name;
                            post.city = sl.city;
                            post.country_name = sl.country_name;
                            post.school_type_id = sl.school_type_id;
                            post.school_type = sl.school_type;
                            listStore.Add(post);
                        }
                        RowCount = listStore.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvStore.Footer = l;
                        lvStore.ItemsSource = listStore;
                    }
                    else
                    {
                        listStore = new ObservableCollection<MerchantSchoolRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvStore.Footer = l;
                        lvStore.ItemsSource = listStore;
                    }
                }
                catch (Exception)
                {
                    SnackB.Message = AppResources.SomethingWrongText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
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
        public MerchantSchoolRelationship store;
        async void OnStoreSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as MerchantSchoolRelationship;
            if (data == null) return;
            store = data;

            if (store.school_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                Settings.merchantSchoolId = store.school_id;
                Settings.merchantSchoolName = store.school_name;
                await Navigation.PushAsync(new MerchantStorePage(store.school_id, store.school_name, store.city));
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}