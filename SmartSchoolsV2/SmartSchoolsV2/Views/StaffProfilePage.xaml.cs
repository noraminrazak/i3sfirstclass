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
    public partial class StaffProfilePage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<StaffClassRelationship> listClass { get; set; }
        public string _photo_url;
        public string _full_name;
        public int _profile_id;
        public int _staff_id;
        public int _school_id;
        public int _shift_id;
        public int _new_shift_id;
        public string _shift_code;
        public static string _back;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }

        public void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
                txtShift.Text = Settings.shiftCode;
                _new_shift_id = Settings.shiftId;
            }
        }
        public StaffProfilePage(int profile_id, int staff_id, string photo_url, string full_name, int school_id, int shift_id, string shift_code)
        {
            InitializeComponent();
            BindingContext = this;

            _profile_id = profile_id;
            _staff_id = staff_id;
            _photo_url = photo_url;
            _full_name = full_name;
            _school_id = school_id;
            _shift_id = shift_id;
            _shift_code = shift_code;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            txtFullName.Text = _full_name;
            txtShift.Text = _shift_code;

            if (!string.IsNullOrEmpty(_photo_url))
            {
                userInitial.IsVisible = false;
                imagePhoto.IsVisible = true;
                imagePhoto.Source = requestUrl + _photo_url;
            }
            else
            {
                userInitial.IsVisible = true;
            }

            StaffClassRelationship();
        }

        async void StartCall(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectWorkingShiftText, "shift", "profile");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        private async void OnRemoveStaffClicked(object sender, EventArgs e)
        {
            bool action = await DisplayAlert(AppResources.RemoveStaffMenuText, AppResources.DoYouReallyWantToRemoveText + _full_name + "?", AppResources.YesText, AppResources.CancelText);
            if (action)
            {
                await RemoveStaff();
            }
        }

        public async Task RemoveStaff()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffRemoveStaff(_school_id, _staff_id, Settings.fullName);
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
        async void OnUpdateClicked(object sender, EventArgs args)
        {
            if (txtShift.Text != _shift_code)
            {
                if (conn.IsConnected() == true)
                {
                    try
                    {
                        ShowLoadingPopup();

                        var t = srvc.PostStaffUpdateStaffShift(_school_id, _staff_id, _new_shift_id, Settings.fullName);
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
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
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
            else
            {
                SnackB.Message = AppResources.PleaseSelectDifferShiftText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async void StaffClassRelationship()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvClass.ItemsSource = null;
                    var t = srvc.PostStaffClassRelationship(_school_id, _staff_id);
                    string jsonStr = await t;
                    StaffClassRelationshipProperty response = JsonConvert.DeserializeObject<StaffClassRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listClass = new ObservableCollection<StaffClassRelationship>();
                        foreach (StaffClassRelationship sl in response.Data)
                        {
                            StaffClassRelationship post = new StaffClassRelationship();
                            post.relationship_id = sl.relationship_id;
                            post.class_id = sl.class_id;
                            post.class_name = sl.class_name;
                            post.image_visible = false;
                            post.initial_visible = true;
                            listClass.Add(post);
                        }
                        RowCount = listClass.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            lvClass.HeightRequest = (30 + (75 * RowCount));
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            lvClass.HeightRequest = 30;
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvClass.Footer = l;
                        lvClass.ItemsSource = listClass;
                    }
                    else
                    {
                        lvClass.HeightRequest = 30;
                        listClass = new ObservableCollection<StaffClassRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvClass.Footer = l;
                        lvClass.ItemsSource = listClass;
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
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}