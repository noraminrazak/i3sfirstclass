using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentOutingRequestViewPage : ContentPage
    {
        public StudentOuting outing = new StudentOuting();
        DateTime dtMin = DateTime.MinValue;
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _profile_id;
        public StudentOutingRequestViewPage(int profile_id, StudentOuting value)
        {
            InitializeComponent();
            BindingContext = this;

            _profile_id = profile_id;

            outing = value;

            lblTitle.Text = "Outing Application";

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            StudentProfile(_profile_id);

            txtOutingType.Text = outing.outing_type;
            txtCheckoutDate.Text = outing.check_out_date.ToString("dd-MM-yyyy");
            txtCheckoutTime.Text = outing.check_out_date.ToString("HH:mm:ss");

            txtCheckinDate.Text = outing.check_in_date.ToString("dd-MM-yyyy");
            txtCheckinTime.Text = outing.check_in_date.ToString("HH:mm:ss");

            txtReason.Text = outing.outing_reason;

            txtOutingStatus.Text = outing.outing_status;

            if (outing.outing_status_id == 2)
            {
                btnCancel.IsVisible = true;
            }
            else if (outing.outing_status_id == 4) 
            {
                bvApp.IsVisible = true;
                appDetail.IsVisible = true;
                bvExact.IsVisible = true;
                exactDt.IsVisible = true;

                txtApproveBy.Text = outing.approve_by;
                txtApproveAt.Text = outing.approve_at.ToString("dd-MM-yyyy HH:mm tt");
                txtComment.Text = outing.approve_comment;

                if (outing.check_out_at != dtMin)
                {
                    txtExactCheckoutDate.Text = outing.check_out_at.ToString("dd-MM-yyyy");
                    txtExactCheckoutTime.Text = outing.check_out_at.ToString("HH:mm:ss");
                }
                else {
                    txtExactCheckoutDate.Text = "N/A";
                    txtExactCheckoutTime.Text = "N/A";
                }

                if (outing.check_in_at != dtMin)
                {
                    txtExactCheckinDate.Text = outing.check_in_at.ToString("dd-MM-yyyy");
                    txtExactCheckinTime.Text = outing.check_in_at.ToString("HH:mm:ss");
                }
                else {
                    txtExactCheckinDate.Text = "N/A";
                    txtExactCheckinTime.Text = "N/A";
                }

            }
            else if (outing.outing_status_id == 5)
            {
                bvApp.IsVisible = true;
                appDetail.IsVisible = true;

                txtApproveBy.Text = outing.approve_by;
                txtApproveAt.Text = outing.approve_at.ToString("dd-MM-yyyy HH:mm tt");
                txtComment.Text = outing.approve_comment;
            }
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
        async void OnCancelClicked(object sender, EventArgs args)
        {
            bool answer = await DisplayAlert("Confirm cancel", "Do you really want to cancel " + outing.outing_type + " for " + outing.full_name + "?", AppResources.YesText, AppResources.CloseText);
            if (answer)
            {
                await StudentCancelOutingRequest(outing);
            }
        }

        async Task StudentCancelOutingRequest(StudentOuting value)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStudentCancelOutingRequest(value.outing_id, value.student_id, value.school_id, Settings.fullName);
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
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
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