using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffOutingRequestFormPage : ContentPage
    {
        DateTimeFormatInfo myDtfi = new CultureInfo("en-MY", false).DateTimeFormat;
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        StudentOuting outing = new StudentOuting();
        public string requestUrl = Settings.requestUrl;
        public string _action = string.Empty;
        public int _profile_id;
        public int _student_id;
        public int _school_id;
        public int _outing_id;
        public DateTime dtNow = DateTime.Now;
        public DateTime dtCheckout;
        public DateTime dtCheckin;
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
        public StaffOutingRequestFormPage(StudentOuting value)
        {
            InitializeComponent();
            BindingContext = this;

            outing = value;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            lblTitle.Text = "Outing Apllication Form";

            _outing_id = outing.outing_id;
            _student_id = outing.student_id;
            _profile_id = outing.profile_id;
            _school_id = outing.school_id;

            StudentProfile(_profile_id);

            txtOutingType.Text = outing.outing_type;
            txtCheckoutDate.Text = outing.check_out_date.ToString("dd-MM-yyyy");
            txtCheckoutTime.Text = outing.check_out_date.ToString("HH:mm:ss");

            txtCheckinDate.Text = outing.check_in_date.ToString("dd-MM-yyyy");
            txtCheckinTime.Text = outing.check_in_date.ToString("HH:mm:ss");

            txtReason.Text = outing.outing_reason;
        }
        public async void StudentProfile(int profile_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostStudentProfile(profile_id);
                    string jsonStr = await t;
                    StudentProfileProperty response = JsonConvert.DeserializeObject<StudentProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        string photo_url = string.Empty;

                        foreach (StudentProfile r in response.Data)
                        {
                            photo_url = r.photo_url;
                            lblFullName.Text = r.full_name;
                            lblSchoolName.Text = r.school_name;
                            lblClassName.Text = r.class_name;
                        }

                        if (!string.IsNullOrEmpty(photo_url))
                        {
                            userInitial.IsVisible = false;
                            userImg.IsVisible = true;
                            userImg.Source = requestUrl + photo_url;
                        }
                        else
                        {
                            userInitial.IsVisible = true;
                        }
                    }
                    else
                    {
                        SnackB.Message = response.Message;
                        SnackB.IsOpen = !SnackB.IsOpen;
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
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        async void OnApproveClicked(object sender, EventArgs args)
        {
            await ApproveStudentOuting();
        }
        async void OnRejectClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtComment.Text))
            {
                await RejectStudentOuting();
            }
            else 
            {
                SnackB.Message = "Comment is required.";
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async Task ApproveStudentOuting()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffApproveOutingRequest(_outing_id, _student_id, _school_id, Settings.profileId, txtComment.Text, Settings.fullName);
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
        async Task RejectStudentOuting()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffRejectOutingRequest(_outing_id, _student_id, _school_id, Settings.profileId, txtComment.Text, Settings.fullName);
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