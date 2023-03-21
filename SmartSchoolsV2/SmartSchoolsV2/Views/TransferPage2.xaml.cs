using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferPage2 : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();

        public List<SchoolStaff> listStaff = new List<SchoolStaff>();
        public List<Student> listStudent = new List<Student>();
        public SchoolStaff stf = new SchoolStaff();
        public Student stud = new Student();
        private int _user_role_id;
        private int _school_id;
        public static string _back;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }
        public static string _option;
        public static string Option
        {
            get { return _option; }
            set { _option = value; }
        }
        public void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
                if (Option == "parent")
                {
                    _school_id = Settings.selectedSchoolId;
                    txtSchoolName.Text = Settings.selectedSchoolName;
                    //await MerchantProductCategory();
                }
            }
        }

        public TransferPage2()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _user_role_id = Settings.userRoleId;

            if (_user_role_id == 9) //parent
            {
                lblSearchName.Text = AppResources.StaffNameText;
            }
            else if (_user_role_id == 8) //staff
            {
                _school_id = Settings.schoolId;
                stackSchool.IsVisible = false;
                stackSchool.HeightRequest = 0;
                lblSearchName.Text = AppResources.StudentNameText;
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        async void StartCall(object sender, EventArgs args)
        {
            var page = new SearchListPage2(AppResources.SelectSchoolText, "parent", Settings.parentId);
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }
        void OnSearchClicked(object sender, EventArgs args)
        {
            if (_user_role_id == 8)
            {
                if (!string.IsNullOrEmpty(txtSearchName.Text))
                {
                    if (txtSearchName.Text.Length > 2)
                    {
                        SearchStudent();
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
            else if (_user_role_id == 9)
            {
                if (!string.IsNullOrEmpty(txtSchoolName.Text))
                {
                    if (!string.IsNullOrEmpty(txtSearchName.Text))
                    {
                        if (txtSearchName.Text.Length > 2)
                        {
                            SearchStaff();
                        }
                        else
                        {
                            SnackB.Message = AppResources.PleaseEnterMoreCharText;
                            SnackB.IsOpen = !SnackB.IsOpen;
                        }
                    }
                    else
                    {
                        SnackB.Message = AppResources.StaffNameRequiredText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                else
                {
                    SnackB.Message = AppResources.SchoolNameRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }

        }

        public async void SearchStaff() 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    lvSearch.IsVisible = true;
                    IsBusy = true;
                    var t = srvc.PostParentSearchStaff(_school_id, txtSearchName.Text);
                    string jsonStr = await t;
                    SchoolStaffProperty response = JsonConvert.DeserializeObject<SchoolStaffProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listStaff = new List<SchoolStaff>();
                        foreach (SchoolStaff r in response.Data)
                        {
                            SchoolStaff prop = new SchoolStaff();
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
                            prop.staff_id = r.staff_id;
                            prop.full_name = r.full_name;
                            prop.wallet_id = r.wallet_id;
                            prop.wallet_number = r.wallet_number;
                            prop.search_name = r.full_name;
                            prop.shift_id = r.shift_id;
                            if (!string.IsNullOrEmpty(r.shift_code))
                            {
                                prop.shift_code = r.shift_code;
                                prop.search_name2 = r.shift_code;
                            }
                            else
                            {
                                prop.shift_code = AppResources.WorkingSessionNotSpecifiedText;
                                prop.search_name2 = AppResources.WorkingSessionNotSpecifiedText;
                            }
                            listStaff.Add(prop);
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
                        lvSearch.Footer = l;
                        lvSearch.ItemsSource = listStaff;
                    }
                    else
                    {
                        List<School> list = new List<School>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvSearch.Footer = l;
                        lvSearch.ItemsSource = list;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
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

        public async void SearchStudent()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    lvSearch.IsVisible = true;
                    IsBusy = true;
                    var t = srvc.PostStaffSearchStudentWallet(_school_id, txtSearchName.Text);
                    string jsonStr = await t;
                    StudentProperty response = JsonConvert.DeserializeObject<StudentProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listStudent = new List<Student>();
                        foreach (Student r in response.Data)
                        {
                            Student prop = new Student();
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
                            prop.full_name = r.full_name;
                            prop.wallet_id = r.wallet_id;
                            prop.wallet_number = r.wallet_number;
                            prop.search_name = r.full_name;

                            if (!string.IsNullOrEmpty(r.class_name))
                            {
                                prop.class_name = r.class_name;
                                prop.search_name2 = r.class_name;
                            }
                            else {
                                prop.class_name = AppResources.ClassNotSpecifiedText;
                                prop.search_name2 = AppResources.ClassNotSpecifiedText;
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
                        lvSearch.Footer = l;
                        lvSearch.ItemsSource = listStudent;
                    }
                    else
                    {
                        List<School> list = new List<School>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvSearch.Footer = l;
                        lvSearch.ItemsSource = list;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
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

        async void OnSearchSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (_user_role_id == 8)
            {
                var data = e.SelectedItem as Student;
                if (data == null) return;
                stud = data;

                if (stud.wallet_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    await Navigation.PushAsync(new TransferPage(2,Settings.walletId,Settings.walletNumber,stud.wallet_id,stud.wallet_number,stud.full_name,stud.class_name,stud.photo_url));

                    ((ListView)sender).SelectedItem = null;
                }
            }
            else if (_user_role_id == 9) 
            {
                var data = e.SelectedItem as SchoolStaff;
                if (data == null) return;
                stf = data;

                if (stf.wallet_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    await Navigation.PushAsync(new TransferPage(2,Settings.walletId, Settings.walletNumber, stf.wallet_id, stf.wallet_number, stf.full_name, stf.shift_code, stf.photo_url));

                    ((ListView)sender).SelectedItem = null;
                }
            }
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}