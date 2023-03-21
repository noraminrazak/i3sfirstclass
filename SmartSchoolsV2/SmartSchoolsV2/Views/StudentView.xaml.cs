using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StudentView : ContentView, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<ParentStudentRelationship> listStudent { get; set; }
        public static Command LoadParentStudentRelationship { get; set; }
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
        public StudentView ()
		{
			InitializeComponent ();
            this.BindingContext = this;

            LoadParentStudentRelationship = new Command(async () => await ParentStudentRelationship());

            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) => {
                Navigation.PushAsync(new AddStudentPage1());
            };
            btnAddStudent.GestureRecognizers.Add(tapGestureRecognizerTxn);
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await ParentStudentRelationship();
                });
            }
        }

        public async Task ParentStudentRelationship()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvStudent.ItemsSource = null;
                    var t = srvc.PostParentStudentRelationship(Settings.parentId);
                    string jsonStr = await t;
                    ParentStudentRelationshipProperty response = JsonConvert.DeserializeObject<ParentStudentRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listStudent = new ObservableCollection<ParentStudentRelationship>();
                        foreach (ParentStudentRelationship sl in response.Data)
                        {
                            ParentStudentRelationship post = new ParentStudentRelationship();
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
                        listStudent = new ObservableCollection<ParentStudentRelationship>();
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
        public ParentStudentRelationship student;
        async void OnStudentSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as ParentStudentRelationship;
            if (data == null) return;
            student = data;

            if (student.student_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                Settings.studentSchoolId = data.school_id;
                Settings.studentSchoolName = data.school_name;
                Settings.studentClassId = data.class_id;
                Settings.studentClassName = data.class_name;
                Settings.studentId = data.student_id;
                Settings.studentProfileId = data.profile_id;
                Settings.studentFullName = data.full_name;
                Settings.studentPhotoUrl = data.photo_url_student;

                if (student.school_type_id == 1)
                {
                    await Navigation.PushAsync(new StudentPage1(data.student_id,
                        data.profile_id,
                        data.full_name,
                        data.photo_url_student,
                        data.school_id,
                        data.school_name,
                        data.school_type_id,
                        data.class_id,
                        data.class_name));
                }
                else if (student.school_type_id == 2)
                {
                    await Navigation.PushAsync(new StudentPage2(data.student_id,
                        data.profile_id,
                        data.full_name,
                        data.photo_url_student,
                        data.school_id,
                        data.school_name,
                        data.school_type_id,
                        data.class_id,
                        data.class_name));
                }

                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}