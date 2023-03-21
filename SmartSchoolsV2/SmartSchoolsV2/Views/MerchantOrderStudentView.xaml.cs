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
    public partial class MerchantOrderStudentView : ContentView, INotifyPropertyChanged
    {

        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<StudentOrderHistory> listStudentOrder { get; set; }
        public static Command LoadStudentOrder { get; set; }

        public string _photo_url;
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
        public MerchantOrderStudentView()
        {
            InitializeComponent();
            this.BindingContext = this;

            LoadStudentOrder = new Command(async () => await StudentOrderHistory());
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await StudentOrderHistory();
                });
            }
        }
        public async Task StudentOrderHistory()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvStudent.ItemsSource = null;
                    var t = srvc.PostMerchantStudentOrderHistory(Settings.merchantId, Settings.selectedSchoolId, Settings.selectedClassId, Settings.pickupDate.AddDays(1));
                    string jsonStr = await t;
                    StudentOrderHistoryProperty response = JsonConvert.DeserializeObject<StudentOrderHistoryProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listStudentOrder = new ObservableCollection<StudentOrderHistory>();
                        foreach (StudentOrderHistory sl in response.Data)
                        {
                            StudentOrderHistory post = new StudentOrderHistory();
                            post.pickup_date = sl.pickup_date;
                            post.recipient_id = sl.recipient_id;
                            post.full_name = sl.full_name;
                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url_student = sl.photo_url;
                                post.photo_url = requestUrl + sl.photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.image_visible = false;
                                post.initial_visible = true;
                            }
                            post.product_id = sl.product_id;
                            post.product_name = sl.product_name;
                            post.product_photo_url = sl.product_photo_url;
                            post.product_qty = sl.product_qty;
                            post.total_amount = sl.total_amount;
                            post.str_total_amount = "RM " + sl.total_amount;
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            post.class_id = sl.class_id;
                            post.class_name = sl.class_name;
                            listStudentOrder.Add(post);
                        }
                        RowCount = listStudentOrder.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvStudent.Footer = l;
                        lvStudent.ItemsSource = listStudentOrder;
                    }
                    else
                    {
                        listStudentOrder = new ObservableCollection<StudentOrderHistory>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvStudent.Footer = l;
                        lvStudent.ItemsSource = listStudentOrder;
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
        public StudentOrderHistory student;
        async void OnStudentSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as StudentOrderHistory;
            if (data == null) return;
            student = data;

            if (student.recipient_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                await Navigation.PushAsync(new MerchantOrderProductDetailPage(student));
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}