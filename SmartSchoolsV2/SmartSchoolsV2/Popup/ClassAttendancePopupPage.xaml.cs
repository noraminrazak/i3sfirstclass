using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassAttendancePopupPage : PopupPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public DateTime _selectedDate;
        public int _school_id;
        public int _class_id;

        public ObservableCollection<ClassDailyAttendance> listAttendance { get; set; }
        public ClassAttendancePopupPage(int school_id, int class_id, DateTime selected_date)
        {
            InitializeComponent();
            BindingContext = this;

            _school_id = school_id;
            _class_id = class_id;
            _selectedDate = selected_date;


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ClassDailyAttendance();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        public async void ClassDailyAttendance()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvAttendance.ItemsSource = null;
                    var t = srvc.PostSchoolClassDailyAttendance(_school_id, _class_id, _selectedDate.ToString("dd-MM-yyyy"));
                    string jsonStr = await t;
                    ClassDailyAttendanceProperty response = JsonConvert.DeserializeObject<ClassDailyAttendanceProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<ClassDailyAttendance> list = new List<ClassDailyAttendance>();
                        foreach (ClassDailyAttendance sl in response.Data)
                        {
                            ClassDailyAttendance post = new ClassDailyAttendance();

                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url = requestUrl + sl.photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.photo_url = "";
                                post.initial_visible = true;
                                post.image_visible = false;
                            }

                            if (sl.reason_id > 0)
                            {
                                post.dot_visible = true;
                            }
                            else
                            {
                                post.dot_visible = false;
                            }

                            post.report_id = sl.report_id;
                            post.student_id = sl.student_id;
                            post.full_name = sl.full_name;

                            post.attendance_id = sl.attendance_id;
                            post.attendance = sl.attendance;
                            post.reason_id = sl.reason_id;
                            post.reason_for_absent = sl.reason_for_absent;

                        }
                        RowCount = listAttendance.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = "Showing " + RowCount + " records";
                        }
                        else
                        {
                            l.Text = "No records for selected date";
                        }
                        lvAttendance.Footer = l;
                        lvAttendance.ItemsSource = listAttendance;
                    }
                    else
                    {
                        listAttendance = new ObservableCollection<ClassDailyAttendance>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = "No records for selected date";
                        lvAttendance.Footer = l;
                        lvAttendance.ItemsSource = listAttendance;
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {

            }
        }
        ViewCell lastCell;
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
        private void OnClose_Tapped(object sender, System.EventArgs e)
        {
            CloseAllPopup();
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}