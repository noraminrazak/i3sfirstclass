using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using System;
using SmartSchoolsV2.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffView : ContentView
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<SchoolStaff> listStaff { get; set; }
        public static Command LoadSchoolStaff { get; set; }
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
        public StaffView()
        {
            InitializeComponent();
            this.BindingContext = this;

            LoadSchoolStaff = new Command(async () => await SchoolStaff());

            //var tapGestureRecognizerTxn = new TapGestureRecognizer();
            //tapGestureRecognizerTxn.Tapped += (s, e) => {
            //    Navigation.PushAsync(new AddStudentPage1());
            //};
            //btnAddStudent.GestureRecognizers.Add(tapGestureRecognizerTxn);
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await SchoolStaff();
                });
            }
        }

        public async Task SchoolStaff()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvStaff.ItemsSource = null;
                    var t = srvc.PostSchoolStaff(Settings.schoolId);
                    string jsonStr = await t;
                    SchoolStaffProperty response = JsonConvert.DeserializeObject<SchoolStaffProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listStaff = new ObservableCollection<SchoolStaff>();
                        foreach (SchoolStaff sl in response.Data)
                        {
                            SchoolStaff post = new SchoolStaff();
                            post.staff_id = sl.staff_id;
                            post.profile_id = sl.profile_id;
                            post.staff_number = sl.staff_number;
                            post.full_name = sl.full_name;
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url_staff = sl.photo_url;
                                post.photo_url = requestUrl + sl.photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.image_visible = false;
                                post.initial_visible = true;
                            }
                            if (sl.shift_id > 0)
                            {
                                post.shift_id = sl.shift_id;
                                post.shift_code = sl.shift_code;
                                post.dot_visible = true;
                            }
                            else
                            {
                                post.dot_visible = false;
                            }
                            post.staff_type_id = sl.staff_type_id;
                            if (Settings.cultureInfo == "en-US")
                            {
                                post.staff_type = sl.staff_type;
                            }
                            else if (Settings.cultureInfo == "ms-MY")
                            {
                                post.staff_type = sl.staff_type_bm;
                            }

                            post.school_type_id = sl.school_type_id;
                            post.school_type = sl.school_type;
                            listStaff.Add(post);
                        }
                        RowCount = listStaff.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvStaff.Footer = l;
                        lvStaff.ItemsSource = listStaff;
                    }
                    else
                    {
                        listStaff = new ObservableCollection<SchoolStaff>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvStaff.Footer = l;
                        lvStaff.ItemsSource = listStaff;
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
        public SchoolStaff staff;
        async void OnStaffSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as SchoolStaff;
            if (data == null) return;
            staff = data;

            if (staff.staff_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...

                await Navigation.PushAsync(new StaffProfilePage(staff.profile_id,staff.staff_id,
                    staff.photo_url_staff,staff.full_name,staff.school_id,staff.shift_id,staff.shift_code));
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}