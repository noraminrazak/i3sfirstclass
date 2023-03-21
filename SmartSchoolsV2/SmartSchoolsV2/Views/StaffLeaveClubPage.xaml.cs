using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartSchoolsV2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffLeaveClubPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _school_id;
        public int _club_id;
        public int _new_staff_id;
        public int _new_profile_id;
        public string _club_name;
        public StaffLeaveClubPage(int school_id, int club_id, string club_name)
        {
            InitializeComponent();
            BindingContext = this;

            _school_id = school_id;
            _club_id = club_id;
            _club_name = club_name;
            lblTitle.Text = AppResources.HandoverText_ + _club_name;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        async void OnSearchClicked(object sender, EventArgs args)
        {
        if (!string.IsNullOrEmpty(txtFullName.Text))
            {
                if (txtFullName.Text.Length > 2)
                {
                    if (conn.IsConnected() == true)
                    {
                        try
                        {
                            lvStaff.IsVisible = true;
                            IsBusy = true;
                            var t = srvc.PostStaffSearchStaff(_school_id, txtFullName.Text);
                            string jsonStr = await t;
                            SchoolStaffProperty response = JsonConvert.DeserializeObject<SchoolStaffProperty>(jsonStr);
                            if (response.Success == true)
                            {
                                List<SchoolStaff> list = new List<SchoolStaff>();
                                foreach (SchoolStaff r in response.Data)
                                {
                                    SchoolStaff prop = new SchoolStaff();
                                    prop.staff_id = r.staff_id;
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
                                    prop.school_id = r.school_id;
                                    prop.school_name = r.school_name;
                                    prop.school_type_id = r.school_type_id;
                                    prop.school_type = r.school_type;
                                    prop.staff_type_id = r.staff_type_id;
                                    prop.staff_type = r.staff_type;
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
                                lvStaff.Footer = l;
                                lvStaff.ItemsSource = list;
                            }
                            else
                            {
                                List<School> list = new List<School>();
                                l.HorizontalTextAlignment = TextAlignment.Center;
                                l.Text = AppResources.NoRecordFoundText;
                                lvStaff.Footer = l;
                                lvStaff.ItemsSource = list;
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
        public SchoolStaff staff = new SchoolStaff();
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
                _new_staff_id = staff.staff_id;
                _new_profile_id = staff.profile_id;
                bool result = await DisplayAlert(AppResources.HandoverText_ + _club_name, AppResources.DoYouReallyWantToHandoverText + staff.full_name + "?", "OK", AppResources.CancelText);
                if (result == true) 
                {
                    await StaffHandoverClub();
                }
                ((ListView)sender).SelectedItem = null;

            }
        }

        public async Task StaffHandoverClub() 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffHandoverClub(_school_id,_club_id,Settings.staffId,_new_staff_id, _new_profile_id, Settings.fullName);
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
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
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