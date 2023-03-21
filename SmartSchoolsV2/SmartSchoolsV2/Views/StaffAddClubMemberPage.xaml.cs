using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffAddClubMemberPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _school_id;
        public int _club_id;
        public string _school_name;
        public string _club_name;
        public int _user_role_id;
        public string _user_role;
        public StaffAddClubMemberPage(int school_id, string school_name , int club_id, string club_name)
        {
            InitializeComponent();
            BindingContext = this;

            _school_id = school_id;
            _school_name = school_name;
            _club_id = club_id;
            _club_name = club_name;

            lblSchoolName.Text = _school_name;
            lblClubName.Text = _club_name;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            pckrUserRole.Items.Add(AppResources.MerchantText);
            pckrUserRole.Items.Add(AppResources.StaffText);
            pckrUserRole.Items.Add(AppResources.ParentText);
            pckrUserRole.Items.Add(AppResources.StudentText);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                switch (selectedIndex)
                {
                    case 0:
                        _user_role_id = 7;
                        _user_role = "Merchant";
                        lblSearchName.Text = AppResources.MerchantNameText;
                        break;
                    case 1:
                        _user_role_id = 8;
                        _user_role = AppResources.StaffText;
                        lblSearchName.Text = AppResources.StaffNameText;
                        break;
                    case 2:
                        _user_role_id = 9;
                        _user_role = "Parent";
                        lblSearchName.Text = AppResources.ParentNameText;
                        break;
                    case 3:
                        _user_role_id = 10;
                        _user_role = AppResources.StudentText;
                        lblSearchName.Text = AppResources.StudentNameText;
                        break;
                    default:
                        break;
                }
                txtSearchName.Text = string.Empty;
            }
        }

        async void OnSearchClicked(object sender, EventArgs args)
        {
            if (_user_role_id >= 0)
            {
                if (!string.IsNullOrEmpty(txtSearchName.Text))
                {
                    if (txtSearchName.Text.Length > 2)
                    {
                        if (_user_role_id == 7) 
                        {
                            await StaffSearchMerchant();
                        }
                        else if (_user_role_id == 8)
                        {
                            await StaffSearchStaff();
                        }
                        else if (_user_role_id == 9)
                        {
                            await StaffSearchParent();
                        }
                        else if (_user_role_id == 10)
                        {
                            await StaffSearchStudent();
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
                    SnackB.Message =  _user_role + AppResources._NameIsRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                SnackB.Message = AppResources.UserRoleRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        private async Task StaffSearchStaff() 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    lvMember.IsVisible = true;
                    IsBusy = true;
                    var t = srvc.PostStaffSearchStaff(_school_id, txtSearchName.Text);
                    string jsonStr = await t;
                    ClubMemberProperty response = JsonConvert.DeserializeObject<ClubMemberProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<ClubMember> list = new List<ClubMember>();
                        foreach (ClubMember r in response.Data)
                        {
                            ClubMember prop = new ClubMember();
                            prop.profile_id = r.profile_id;
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
                            list.Add(prop);
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
                        lvMember.Footer = l;
                        lvMember.ItemsSource = list;
                    }
                    else
                    {
                        List<ClubMember> list = new List<ClubMember>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvMember.Footer = l;
                        lvMember.ItemsSource = list;
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

        private async Task StaffSearchMerchant()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    lvMember.IsVisible = true;
                    IsBusy = true;
                    var t = srvc.PostStaffSearchMerchant(_school_id, txtSearchName.Text);
                    string jsonStr = await t;
                    ClubMemberProperty response = JsonConvert.DeserializeObject<ClubMemberProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<ClubMember> list = new List<ClubMember>();
                        foreach (ClubMember r in response.Data)
                        {
                            ClubMember prop = new ClubMember();
                            prop.profile_id = r.profile_id;
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
                            list.Add(prop);
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
                        lvMember.Footer = l;
                        lvMember.ItemsSource = list;
                    }
                    else
                    {
                        List<ClubMember> list = new List<ClubMember>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvMember.Footer = l;
                        lvMember.ItemsSource = list;
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

        private async Task StaffSearchStudent()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    lvMember.IsVisible = true;
                    IsBusy = true;
                    var t = srvc.PostStaffSearchStudent(_school_id, txtSearchName.Text);
                    string jsonStr = await t;
                    ClubMemberProperty response = JsonConvert.DeserializeObject<ClubMemberProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<ClubMember> list = new List<ClubMember>();
                        foreach (ClubMember r in response.Data)
                        {
                            ClubMember prop = new ClubMember();
                            prop.profile_id = r.profile_id;
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
                            list.Add(prop);
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
                        lvMember.Footer = l;
                        lvMember.ItemsSource = list;
                    }
                    else
                    {
                        List<ClubMember> list = new List<ClubMember>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvMember.Footer = l;
                        lvMember.ItemsSource = list;
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

        private async Task StaffSearchParent()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    lvMember.IsVisible = true;
                    IsBusy = true;
                    var t = srvc.PostStaffSearchParent(_school_id, txtSearchName.Text);
                    string jsonStr = await t;
                    ClubMemberProperty response = JsonConvert.DeserializeObject<ClubMemberProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<ClubMember> list = new List<ClubMember>();
                        foreach (ClubMember r in response.Data)
                        {
                            ClubMember prop = new ClubMember();
                            prop.profile_id = r.profile_id;
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
                            list.Add(prop);
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
                        lvMember.Footer = l;
                        lvMember.ItemsSource = list;
                    }
                    else
                    {
                        List<ClubMember> list = new List<ClubMember>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvMember.Footer = l;
                        lvMember.ItemsSource = list;
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

        public ClubMember member = new ClubMember();

        async void OnMemberSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as ClubMember;
            if (data == null) return;
            member = data;

            if (member.profile_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;

                bool result = await DisplayAlert(AppResources.AddNewMemberText,AppResources.DoYouReallyWantToAddText + member.full_name + AppResources._ToThisClubText, AppResources.YesText, AppResources.CancelText);
                if (result == true) 
                {
                    await StaffAddClubMember(member.profile_id);
                }
                ((ListView)sender).SelectedItem = null;
            }
        }

        async Task StaffAddClubMember(int profile_id) 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffAddClubMember(_club_id, profile_id, _user_role_id, Settings.fullName);
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