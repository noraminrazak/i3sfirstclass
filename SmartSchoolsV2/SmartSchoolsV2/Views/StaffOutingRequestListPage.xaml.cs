using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffOutingRequestListPage : ContentPage, INotifyPropertyChanged
    {
        private ObservableCollection<StudentOuting> _listOuting;
        public ObservableCollection<StudentOuting> listOuting
        {
            get 
            { 
                return _listOuting; 
            }
            set 
            { 
                _listOuting = value; 
                OnPropertyChanged("listOuting"); 
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public DateTime _outing_month;
        public int _outing_type_id;
        public int RowCount { get; set; }
        public static Label l = new Label();
        //private ObservableCollection<StudentOuting> listOuting;

        public StaffOutingRequestListPage(string outing_status, int outing_type_id, string outing_type, DateTime outing_month)
        {
            //BindingContext = this;
            InitializeComponent();

            _outing_type_id = outing_type_id;
            _outing_month = outing_month;
            lblTitle.Text = outing_status + " - " + outing_type;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            PostStaffOutingRequest(2, Settings.schoolId, _outing_month, _outing_type_id);
        }
        public async void PostStaffOutingRequest(int outing_status_id, int school_id, DateTime outing_month, int outing_type_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostStaffOutingRequest(outing_status_id, school_id, outing_month, outing_type_id);
                    string jsonStr = await t;
                    StudentOutingProperty response = JsonConvert.DeserializeObject<StudentOutingProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listOuting = new ObservableCollection<StudentOuting>();
                        foreach (StudentOuting sl in response.Data)
                        {
                            StudentOuting post = new StudentOuting();
                            post.outing_id = sl.outing_id;
                            post.student_id = sl.student_id;
                            post.profile_id = sl.profile_id;
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
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            post.outing_type_id = sl.outing_type_id;
                            post.outing_type = sl.outing_type;
                            post.outing_status_id = sl.outing_status_id;
                            post.outing_status = sl.outing_status;
                            post.check_out_date = sl.check_out_date;
                            post.check_in_date = sl.check_in_date;
                            post.outing_reason = sl.outing_reason;
                            post.img_checkbox = "ic_uncheck.png";
                            post.is_check = false;
                            if (sl.outing_type_id == 1) 
                            {
                                post.outing_date = "Date : " + sl.check_out_date.ToString("dd/MM/yyyy");
                            } 
                            else if (sl.outing_type_id == 2) 
                            {
                                post.outing_date = "Date : " + sl.check_out_date.ToString("dd/MM/yyyy") + " - " + sl.check_in_date.ToString("dd/MM/yyyy");
                            }

                            listOuting.Add(post);
                        }
                        RowCount = listOuting.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvOuting.Footer = l;
                        lvOuting.ItemsSource = listOuting;
                    }
                    else
                    {
                        listOuting = new ObservableCollection<StudentOuting>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvOuting.Footer = l;
                        lvOuting.ItemsSource = listOuting;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {

                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        //private int _selectedItemIndex;

        //void OnCheckboxTapped(object sender, EventArgs e)
        //{
        //    if (e == null) return; // has been set to null, do not 'process' tapped event
        //    //_selectedItemIndex = listOuting.IndexOf((StudentOuting)sender);
        //    var args = (TappedEventArgs)e;
        //    _selectedItemIndex = (lvOuting.ItemsSource as ObservableCollection<StudentOuting>).IndexOf((StudentOuting)args.Parameter);
        //    listOuting[_selectedItemIndex].img_checkbox = listOuting[_selectedItemIndex].img_checkbox
        //        .Equals("ic_uncheck.png")
        //        ? "ic_check.png"
        //        : "ic_uncheck.png";

        //}
        private void OnCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;

            var ob = checkbox.BindingContext as StudentOuting;

            if (ob != null)
            {
                //AddOrUpdatetheResult(ob, checkbox);
            }

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        public StudentOuting student;
        async void OnOutingSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as StudentOuting;
            if (data == null) return;
            student = data;

            if(student.outing_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;

                await Navigation.PushAsync(new StaffOutingRequestFormPage(student));

                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}