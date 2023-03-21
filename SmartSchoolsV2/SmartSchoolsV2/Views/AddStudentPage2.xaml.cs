using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using System;
using System.Collections.Generic;
using SmartSchoolsV2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStudentPage2 : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public static string _back;
        public List<Student> list = new List<Student>();
        public Student student = new Student();
        public int _school_id;
        public string _school_name;
        public AddStudentPage2(int school_id, string school_name)
        {
            InitializeComponent();
            BindingContext = this;
            _school_id = school_id;
            _school_name = school_name;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            lblTitleView.Text = _school_name;

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        async void OnSearchClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtSearchStudent.Text))
            {
                if (txtSearchStudent.Text.Length > 2)
                {
                    if (conn.IsConnected() == true)
                    {
                        try
                        {
                            IsBusy = true;
                            lvStudent.ItemsSource = null;
                            var t = srvc.PostParentSearchStudent(_school_id, txtSearchStudent.Text);
                            string jsonStr = await t;
                            StudentProperty response = JsonConvert.DeserializeObject<StudentProperty>(jsonStr);
                            if (response.Success == true)
                            {
                                List<Student> list = new List<Student>();
                                foreach (Student sl in response.Data)
                                {
                                    Student post = new Student();
                                    post.student_id = sl.student_id;
                                    post.student_number = sl.student_number;
                                    post.full_name = sl.full_name;
                                    post.nric = sl.nric;
                                    post.school_id = sl.school_id;
                                    post.school_name = sl.school_name;
                                    if (!string.IsNullOrEmpty(sl.photo_url))
                                    {
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
                                    }
                                    else
                                    {
                                        //post.dot_visible = false;
                                    }

                                    post.school_type_id = sl.school_type_id;
                                    post.school_type = sl.school_type;
                                    list.Add(post);
                                }
                                RowCount = list.Count;
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
                                lvStudent.ItemsSource = list;
                            }
                            else
                            {
                                List<Student> list = new List<Student>();
                                l.HorizontalTextAlignment = TextAlignment.Center;
                                l.Text = AppResources.NoRecordFoundText;
                                lvStudent.Footer = l;
                                lvStudent.ItemsSource = list;
                            }
                            //IsBusy = false;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                        }
                        finally
                        {
                            IsBusy = false;
                        }
                        //HideLoadingPopup();
                    }
                    else
                    {
                        //SnackB.Message = AppResources.CheckInternetText;
                        //SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                else
                {
                    SnackB.Message = AppResources.PleaseEnterMoreCharText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }

            }
            else
            {
                SnackB.Message = AppResources.SchoolNameRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void OnStudentSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as Student;
            if (data == null) return;
            student = data;

            if (student.student_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                await Navigation.PushAsync(new AddStudentPage3(student.student_id, 
                    student.full_name, student.photo_url, student.school_id,student.school_name, student.class_id, student.class_name, student.nric));

                ((ListView)sender).SelectedItem = null;

            }
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}