using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffAddClassStudentPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _school_id;
        public int _class_id;
        public int _student_id;
        public string _school_name;
        public string _class_name;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<SchoolClassStudent> listStudent { get; set; }
        public static Command LoadSchoolStudent { get; set; }
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

        public StaffAddClassStudentPage(int school_id, string school_name, int class_id, string class_name)
        {
            InitializeComponent();
            this.BindingContext = this;

            _school_id = school_id;
            _class_id = class_id;
            lblSchoolName.Text = school_name;
            lblClassName.Text = class_name;
        }

        async void OnSearchClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtSearchName.Text))
            {
                if (txtSearchName.Text.Length > 2)
                {
                    await StaffSearchStudent();
                }
                else
                {
                    SnackB.Message = AppResources.PleaseEnterMoreCharText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                SnackB.Message = AppResources.StudentNameRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        private async Task StaffSearchStudent()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    lvStudent.IsVisible = true;
                    IsBusy = true;
                    var t = srvc.PostStaffSearchStudent(_school_id, txtSearchName.Text);
                    string jsonStr = await t;
                    SchoolClassStudentProperty response = JsonConvert.DeserializeObject<SchoolClassStudentProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listStudent = new ObservableCollection<SchoolClassStudent>();
                        foreach (SchoolClassStudent r in response.Data)
                        {
                            SchoolClassStudent prop = new SchoolClassStudent();
                            prop.profile_id = r.profile_id;
                            prop.student_id = r.student_id;
                            prop.student_number = r.student_number;
                            prop.class_id = r.class_id;
                            prop.class_name = r.class_name;
                            prop.full_name = r.full_name;
                            if (!string.IsNullOrEmpty(r.photo_url))
                            {
                                prop.photo_url = requestUrl + r.photo_url;
                                prop.image_visible = true;
                                prop.initial_visible = false;
                            }
                            else
                            {
                                prop.image_visible = false;
                                prop.initial_visible = true;
                            }
                            listStudent.Add(prop);
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
                        //listStudent = new ObservableCollection<SchoolClassStudent>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvStudent.Footer = l;
                        lvStudent.ItemsSource = listStudent;
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

        public SchoolClassStudent stud = new SchoolClassStudent();
        async void OnStudentSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as SchoolClassStudent;
            if (data == null) return;
            stud = data;

            if (stud.student_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;

                bool result = await DisplayAlert(AppResources.EnrollmentConfirmText, AppResources.DoYouReallyWantToEnrollText + stud.full_name + AppResources._ToThisClassText, AppResources.YesText, AppResources.CancelText);
                if (result == true)
                {
                    await StaffAddClassStudent(stud.student_id);
                }
                ((ListView)sender).SelectedItem = null;
            }
        }

        async Task StaffAddClassStudent(int student_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffAddClassStudent(_school_id, _class_id, student_id, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert(AppResources.SorryText, response.Message, "OK");
                    }
                }
                catch (Exception)
                {
                    SnackB.Message = AppResources.SomethingWrongText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    HideLoadingPopup();
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        LoadingPopupPage loadingPage = new LoadingPopupPage();
        async void ShowLoadingPopup()
        {
            await Navigation.PushPopupAsync(loadingPage);
        }

        async void HideLoadingPopup()
        {
            await Task.Delay(500);
            await Navigation.RemovePopupPageAsync(loadingPage);
        }
    }
}