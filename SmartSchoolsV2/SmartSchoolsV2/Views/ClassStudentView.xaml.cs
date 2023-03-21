using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassStudentView : ContentView
    {

        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _school_id;
        public int _class_id;
        public string _school_name;
        public string _class_name;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<SchoolClassStudent> listStudent { get; set; }
        public static Command LoadSchoolClassStudent { get; set; }
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
        public ClassStudentView()
        {
            InitializeComponent();
            this.BindingContext = this;

            _school_id = Settings.selectedSchoolId;
            _school_name = Settings.selectedSchoolName;
            _class_id = Settings.selectedClassId;
            _class_name = Settings.selectedClassName;


            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new StaffAddClassStudentPage(_school_id, _school_name, _class_id,_class_name));
            };
            btnAddStudent.GestureRecognizers.Add(tapGestureRecognizerTxn);

            LoadSchoolClassStudent = new Command(async () => await SchoolClassStudent());
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await SchoolClassStudent();
                });
            }
        }
        public async Task SchoolClassStudent()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvStudent.ItemsSource = null;
                    var t = srvc.PostSchoolClassStudent(Settings.selectedSchoolId, Settings.selectedClassId);
                    string jsonStr = await t;
                    SchoolClassStudentProperty response = JsonConvert.DeserializeObject<SchoolClassStudentProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listStudent = new ObservableCollection<SchoolClassStudent>();
                        foreach (SchoolClassStudent sl in response.Data)
                        {
                            SchoolClassStudent post = new SchoolClassStudent();
                            post.student_id = sl.student_id;
                            post.profile_id = sl.profile_id;
                            post.student_number = sl.student_number;
                            post.full_name = sl.full_name;
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
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
                            if (sl.class_id > 0)
                            {
                                post.class_id = sl.class_id;
                                post.class_name = sl.class_name;
                                post.dot_visible = true;
                            }
                            else
                            {
                                post.dot_visible = false;
                            }

                            post.school_type_id = sl.school_type_id;
                            post.school_type = sl.school_type;
                            listStudent.Add(post);
                        }
                        RowCount = listStudent.Count;
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
                        lvStudent.ItemsSource = listStudent;
                    }
                    else
                    {
                        listStudent = new ObservableCollection<SchoolClassStudent>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvStudent.Footer = l;
                        lvStudent.ItemsSource = listStudent;
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
        public SchoolClassStudent student;
        async void OnStudentSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as SchoolClassStudent;
            if (data == null) return;
            student = data;

            if (student.student_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                _school_id = student.school_id;
                _school_name = student.school_name;
                _class_id = student.class_id;
                _class_name = student.class_name;

                await Navigation.PushAsync(new StudentProfilePage(student.profile_id, student.student_id, 
                    student.photo_url_student, student.full_name, student.school_id, student.class_id, student.class_name));
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}