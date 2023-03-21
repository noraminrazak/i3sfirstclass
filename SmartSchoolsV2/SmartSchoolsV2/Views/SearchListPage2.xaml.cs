using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchListPage2 : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _school_id;
        public int _merchant_id;
        public int _parent_id;
        public int _class_id;
        public int _user_role_id;
        public string _title;
        public string _option;
        public string _class_teacher_flag;
        public List<School> listSchool;
        public List<SchoolClass> listClass;
        public List<SchoolClub> listClub;
        public List<SchoolMerchant> listMerchant;
        public delegate void SetDetailEventHandler(object source, EventArgs args);
        public event SetDetailEventHandler DetailSet;
        protected virtual void OnDetailSet()
        {
            if (DetailSet != null)
            {
                DetailSet(this, EventArgs.Empty);
            }
        }
        bool isBusy;
        public bool IsBusy2
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public SearchListPage2(string title, string option, int param1)
        {
            InitializeComponent();
            BindingContext = this;
            _title = title;
            _option = option;
            _school_id = param1;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _user_role_id = Settings.userRoleId;
            lblTitleView.Text = _title;

            int[] array = new int[1];

            if (_option == "class" || _option == "join" || _option == "student")
            {
                SchoolClass(_school_id);
            }
            else if (_option == "headmaster")
            {
                if (_user_role_id == 7) 
                {
                    MerchantSchoolRelationship();
                }
                else if (_user_role_id == 9)
                {
                    ParentSchoolRelationship();
                }
            }
            else if (_option == "teacher")
            {
                array[0] = _school_id;
                SchoolClub(array, 2);
            }
            else if (_option == "all")
            {
                array[0] = _school_id;
                SchoolClub(array, 3);
            }
            else if (_option == "merchant" || _option == "class-order")
            {
                SchoolMerchant(_school_id);
            }
            else if (_option == "parent")
            {
                ParentSchoolRelationship2();
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        async void SchoolClass(int school_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy2 = true;
                    var t = srvc.PostSchoolClass(school_id);
                    string jsonStr = await t;
                    SchoolClassProperty response = JsonConvert.DeserializeObject<SchoolClassProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listClass = new List<SchoolClass>();
                        foreach (SchoolClass r in response.Data)
                        {
                            SchoolClass prop = new SchoolClass();
                            prop.image_visible = false;
                            prop.initial_visible = true;
                            prop.class_id = r.class_id;
                            prop.class_name = r.class_name;
                            prop.session_code = r.session_code;
                            prop.search_id = r.class_id;
                            prop.search_name = r.class_name;
                            prop.search_name2 = r.session_code;
                            listClass.Add(prop);
                        }
                        RowCount = listClass.Count;
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
                        lvSearch.ItemsSource = listClass;
                    }
                    else
                    {
                        List<SchoolClass> list = new List<SchoolClass>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvSearch.Footer = l;
                        lvSearch.ItemsSource = list;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy2 = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void SchoolClub(int[] school_id, int create_by_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy2 = true;
                    var t = srvc.PostSchoolClub(school_id, create_by_id);
                    string jsonStr = await t;
                    SchoolClubProperty response = JsonConvert.DeserializeObject<SchoolClubProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<SchoolClub> listClass = new List<SchoolClub>();
                        foreach (SchoolClub r in response.Data)
                        {
                            SchoolClub prop = new SchoolClub();
                            prop.image_visible = false;
                            prop.initial_visible = true;
                            prop.club_id = r.club_id;
                            prop.club_name = r.club_name;
                            prop.full_name = r.full_name;
                            prop.search_id = r.club_id;
                            prop.search_name = r.club_name;
                            prop.search_name2 = r.school_name;
                            listClass.Add(prop);
                        }
                        RowCount = listClass.Count;
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
                        lvSearch.ItemsSource = listClass;
                    }
                    else
                    {
                        List<SchoolClub> list = new List<SchoolClub>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvSearch.Footer = l;
                        lvSearch.ItemsSource = list;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy2 = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void SchoolMerchant(int school_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy2 = true;
                    var t = srvc.PostSchoolMerchant(school_id);
                    string jsonStr = await t;
                    SchoolMerchantProperty response = JsonConvert.DeserializeObject<SchoolMerchantProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listMerchant = new List<SchoolMerchant>();
                        foreach (SchoolMerchant r in response.Data)
                        {
                            SchoolMerchant prop = new SchoolMerchant();
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
                            prop.merchant_id = r.merchant_id;
                            prop.company_name = r.company_name;
                            prop.full_name = r.full_name;
                            prop.merchant_type_id = r.merchant_type_id;
                            prop.merchant_type = r.merchant_type;
                            prop.search_name = r.company_name;
                            prop.search_name2 = r.merchant_type;
                            prop.search_name3 = r.full_name;
                            listMerchant.Add(prop);
                        }
                        RowCount = listMerchant.Count;
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
                        lvSearch.ItemsSource = listMerchant;
                    }
                    else
                    {
                        List<SchoolMerchant> list = new List<SchoolMerchant>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvSearch.Footer = l;
                        lvSearch.ItemsSource = list;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy2 = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //thats all you need to make a search  
            if (_option == "class" || _option == "join" || _option == "student")
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    lvSearch.ItemsSource = listClass;
                }

                else
                {
                    lvSearch.ItemsSource = listClass.Where(x => x.search_name.ToLower().Contains(e.NewTextValue));
                    RowCount = listClass.Where(x => x.search_name.ToLower().Contains(e.NewTextValue)).Count();
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
                }
            }
            else if (_option == "headmaster" || _option == "teacher" || _option == "all")
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    lvSearch.ItemsSource = listClub;
                }

                else
                {
                    lvSearch.ItemsSource = listClub.Where(x => x.search_name.ToLower().Contains(e.NewTextValue));
                    RowCount = listClub.Where(x => x.search_name.ToLower().Contains(e.NewTextValue)).Count();
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
                }
            }
            else if (_option == "merchant" || _option == "class-order")
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    lvSearch.ItemsSource = listMerchant;
                }
                else
                {
                    lvSearch.ItemsSource = listMerchant.Where(x => x.search_name.ToLower().Contains(e.NewTextValue));
                    RowCount = listMerchant.Where(x => x.search_name.ToLower().Contains(e.NewTextValue)).Count();
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
                }
            }
        }

        public async void ParentSchoolRelationship()
        {
            _parent_id = Settings.parentId;

            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostParentSchoolRelationship(_parent_id);
                    string jsonStr = await t;
                    ParentSchoolRelationshipProperty response = JsonConvert.DeserializeObject<ParentSchoolRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        int[] array = new int[response.Data.Count];
                        int i = 0;
                        foreach (ParentSchoolRelationship r in response.Data)
                        {
                            array[i++] = r.school_id;
                        }

                        SchoolClub(array, 1);
                    }
                    else
                    {
                        //SnackB.Message = response.Message;
                        //SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async void ParentSchoolRelationship2()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostParentSchoolRelationship(_school_id);
                    string jsonStr = await t;
                    SchoolProperty response = JsonConvert.DeserializeObject<SchoolProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listSchool = new List<School>();
                        foreach (School r in response.Data)
                        {
                            School prop = new School();
                            prop.image_visible = false;
                            prop.initial_visible = true;
                            prop.school_id = r.school_id;
                            prop.school_name = r.school_name;
                            prop.city = r.city;
                            prop.search_id = r.school_id;
                            prop.search_name = r.school_name;
                            prop.search_name2 = r.city;
                            listSchool.Add(prop);
                        }
                        RowCount = listSchool.Count;
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
                        lvSearch.ItemsSource = listSchool;
                    }
                    else
                    {
                        List<SchoolClass> list = new List<SchoolClass>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvSearch.Footer = l;
                        lvSearch.ItemsSource = list;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async void MerchantSchoolRelationship()
        {
            _merchant_id = Settings.merchantId;

            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostMerchantSchoolRelationship(_merchant_id);
                    string jsonStr = await t;
                    ParentSchoolRelationshipProperty response = JsonConvert.DeserializeObject<ParentSchoolRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        int[] array = new int[response.Data.Count];
                        int i = 0;
                        foreach (ParentSchoolRelationship r in response.Data)
                        {
                            array[i++] = r.school_id;
                        }

                        SchoolClub(array, 1);
                    }
                    else
                    {
                        //SnackB.Message = response.Message;
                        //SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }
        public School listSelectSchool;
        public SchoolClass listSelectClass;
        public SchoolClub listSelectClub;
        public SchoolMerchant listSelectMerchant;
        async void OnSearchSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (_option == "class" || _option == "join" || _option == "student")
            {
                var data = e.SelectedItem as SchoolClass;
                if (data == null) return;
                listSelectClass = data;

                if (listSelectClass.search_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.classId = listSelectClass.class_id;
                    Settings.className = listSelectClass.class_name;
                    Settings.sessionCode = listSelectClass.session_code;

                    ((ListView)sender).SelectedItem = null;

                    if (_option == "class")
                    {
                        await Navigation.PopAsync();
                        AddStudentPage3.Back = "Y";
                    }
                    else if (_option == "student")
                    {
                        await Navigation.PopAsync();
                        StudentProfilePage.Back = "Y";
                    }
                    else if (_option == "join")
                    {
                        bool result = await DisplayAlert(AppResources.JoinClassText, AppResources.DoYouReallyWantToJoinText + listSelectClass.class_name + "?",AppResources.YesText,AppResources.CancelText);
                        
                        if (result == true) 
                        {
                            string result2 = await DisplayActionSheet(AppResources.JoinClassAsText, AppResources.CancelText , null, AppResources.ClassTeacherText,AppResources.SubjectTeacherText);

                            if (result2 != AppResources.CancelText) 
                            {
                                if (result2 == AppResources.ClassTeacherText)
                                {
                                    _class_teacher_flag = "Y";
                                }
                                else if (result2 == AppResources.SubjectTeacherText)
                                {
                                    _class_teacher_flag = "N";
                                }
                                StaffJoinClass();
                            }
                        }
                    }
                    OnDetailSet();
                }
            }
            else if (_option == "headmaster" || _option == "teacher" || _option == "all")
            {
                var data = e.SelectedItem as SchoolClub;
                if (data == null) return;
                listSelectClub = data;

                if (listSelectClub.search_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.clubId = listSelectClub.club_id;
                    Settings.clubName = listSelectClub.club_name;
                    Settings.clubCreator = listSelectClub.full_name;
                    if (_option == "headmaster")
                    {
                        if (_user_role_id == 7)
                        {
                            bool result = await DisplayAlert(AppResources.JoinClubText, AppResources.DoYouReallyWantToJoinText + listSelectClub.club_name + "?", AppResources.YesText, AppResources.CancelText);
                            if (result == true)
                            {
                                await MerchantJoinClub();
                            }
                        }
                        else if (_user_role_id == 9)
                        {
                            bool result = await DisplayAlert(AppResources.JoinClubText, AppResources.DoYouReallyWantToJoinText + listSelectClub.club_name + "?", AppResources.YesText, AppResources.CancelText);
                            if (result == true)
                            {
                                await ParentJoinClub();
                            }
                        }
                    }
                    else if (_option == "teacher")
                    {
                        string student_name = Settings.studentFullName;
                        bool result = await DisplayAlert(AppResources.JoinClubText, AppResources.DoYouReallyWantText + student_name + AppResources._ToJoinText_ + listSelectClub.club_name + "?", AppResources.YesText, AppResources.CancelText);
                        if (result == true)
                        {
                            await StudentJoinClub();
                        }
                    }
                    else if (_option == "all")
                    {
                        bool result = await DisplayAlert(AppResources.JoinClubText, AppResources.DoYouReallyWantToJoinText + listSelectClub.club_name + "?", AppResources.YesText, AppResources.CancelText);
                        if (result == true)
                        {
                            await StaffJoinClub();
                        }
                    }
                    ((ListView)sender).SelectedItem = null;
                }
            }
            else if (_option == "merchant")
            {
                var data = e.SelectedItem as SchoolMerchant;
                if (data == null) return;
                listSelectMerchant = data;

                if (listSelectMerchant.merchant_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.selectMerchantId = listSelectMerchant.merchant_id;
                    Settings.selectCompanyName = listSelectMerchant.company_name;
                    Settings.selectMerchantType = listSelectMerchant.merchant_type;
                    Settings.selectMerchantTypeId = listSelectMerchant.merchant_type_id;
                    ((ListView)sender).SelectedItem = null;

                    await Navigation.PopAsync();
                    if (_user_role_id == 9)
                    {
                        AddToCartPage1.Back = "Y";
                        AddToCartPage1.Option = _option;
                    }
                    else if (_user_role_id == 8) 
                    {
                        AddToCartPage1b.Back = "Y";
                        AddToCartPage1b.Option = _option;
                    }

                    OnDetailSet();
                }
            }
            else if (_option == "class-order")
            {
                var data = e.SelectedItem as SchoolMerchant;
                if (data == null) return;
                listSelectMerchant = data;

                if (listSelectMerchant.merchant_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.selectMerchantId = listSelectMerchant.merchant_id;
                    Settings.selectCompanyName = listSelectMerchant.company_name;
                    Settings.selectMerchantType = listSelectMerchant.merchant_type;
                    ((ListView)sender).SelectedItem = null;

                    await Navigation.PopAsync();
                    ClassPage2.Back = "Y";
                    OnDetailSet();
                }
            }
            else if (_option == "parent")
            {
                var data = e.SelectedItem as School;
                if (data == null) return;
                listSelectSchool = data;

                if (listSelectSchool.school_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.selectedSchoolId = listSelectSchool.school_id;
                    Settings.selectedSchoolName = listSelectSchool.school_name;
                    ((ListView)sender).SelectedItem = null;

                    await Navigation.PopAsync();
                    TransferPage2.Back = "Y";
                    TransferPage2.Option = _option;
                    OnDetailSet();
                }
            }
        }

        async void StaffJoinClass()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffJoinClass(Settings.classId, Settings.staffId, _class_teacher_flag, Settings.fullName);
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

        async Task StaffJoinClub()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffJoinClub(Settings.staffId, Settings.clubId, Settings.fullName);
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

        async Task StudentJoinClub()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStudentJoinClub(Settings.studentId, Settings.clubId, Settings.fullName);
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

        async Task ParentJoinClub()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostParentJoinClub(Settings.parentId, Settings.clubId, Settings.fullName);
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

        async Task MerchantJoinClub()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostMerchantJoinClub(Settings.merchantId, Settings.clubId, Settings.fullName);
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