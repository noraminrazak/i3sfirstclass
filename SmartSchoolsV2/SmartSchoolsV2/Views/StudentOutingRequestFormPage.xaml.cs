using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentOutingRequestFormPage : ContentPage
    {
        DateTimeFormatInfo myDtfi = new CultureInfo("en-MY", false).DateTimeFormat;
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        StudentOuting outing = new StudentOuting();
        public string requestUrl = Settings.requestUrl;
        public string _action = string.Empty;
        public int _outing_status_id = 0;
        public int _outing_type_id;
        public int _profile_id;
        public int _error = 0;
        public int _student_id;
        public int _school_id;
        public int _user_role_id;
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
        public StudentOutingRequestFormPage(int profile_id, string action, StudentOuting value)
        {
            InitializeComponent();
            BindingContext = this;

            outing = value;
            _profile_id = profile_id;
            _action = action;
            if (action == "new")
            {
                lblTitle.Text = "New Outing Application";
            }
            else if (action == "edit")
            {
                lblTitle.Text = "Edit Outing Application";
                _outing_status_id = 1;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _student_id = Settings.studentId;
            _school_id = Settings.studentSchoolId;
            StudentProfile(_profile_id);

            if (_action == "edit") 
            {
                _outing_id = outing.outing_id;
                _outing_type_id = outing.outing_type_id;

                if (outing.outing_type_id == 1)
                {
                    pckrOutingType.SelectedIndex = 0;
                }
                else if (outing.outing_type_id == 2)
                {
                    pckrOutingType.SelectedIndex = 1;
                }
                txtCheckoutDate.Text = outing.check_out_date.ToString("dd-MM-yyyy");
                txtCheckoutTime.Text = outing.check_out_date.ToString("HH:mm:ss");

                txtCheckinDate.Text = outing.check_in_date.ToString("dd-MM-yyyy");
                txtCheckinTime.Text = outing.check_in_date.ToString("HH:mm:ss");

                txtReason.Text = outing.outing_reason;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
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
        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {

            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                txtCheckoutDate.IsReadOnly = false;
                txtCheckoutTime.IsReadOnly = false;
                txtCheckinDate.IsReadOnly = false;
                txtCheckinTime.IsReadOnly = false;

                switch (selectedIndex)
                {
                    case 0:
                        _outing_type_id = 1;
                        txtCheckoutDate.Text = dtNow.ToString("dd-MM-yyyy");
                        txtCheckinDate.Text = dtNow.ToString("dd-MM-yyyy");
                        dtCheckout = dtNow;
                        dtCheckin = dtNow;
                        txtCheckinDate.IsReadOnly = true;
                        break;
                    case 1:
                        _outing_type_id = 2;
                        txtCheckoutDate.Text = dtNow.ToString("dd-MM-yyyy");
                        txtCheckinDate.Text = dtNow.AddDays(1).ToString("dd-MM-yyyy");
                        dtCheckout = dtNow;
                        dtCheckin = dtNow.AddDays(1);
                        txtCheckinDate.IsReadOnly = false;
                        break;
                    default:
                        break;
                }
            }
        }
        void StartCall40(object sender, EventArgs args)
        {
            checkoutDate.Focus();
        }
        void StartCall60(object sender, EventArgs args)
        {
            checkinDate.Focus();
        }
        private void OnCheckoutDateSelected(object sender, DateChangedEventArgs e)
        {
            if (_outing_type_id == 1)
            {
                txtCheckoutDate.Text = e.NewDate.ToString("dd-MM-yyyy");
                txtCheckinDate.Text = e.NewDate.ToString("dd-MM-yyyy");
                dtCheckout = e.NewDate;
                dtCheckin = e.NewDate;
            }
            else if (_outing_type_id == 2)
            {
                txtCheckoutDate.Text = e.NewDate.ToString("dd-MM-yyyy");
                txtCheckinDate.Text = e.NewDate.AddDays(1).ToString("dd-MM-yyyy");
                dtCheckout = e.NewDate;
                dtCheckin = e.NewDate.AddDays(1);
            }
        }

        private void OnCheckinDateSelected(object sender, DateChangedEventArgs e)
        {
            txtCheckinDate.Text = e.NewDate.ToString("dd-MM-yyyy");
            dtCheckin = e.NewDate;
        }
        void StartCall41(object sender, EventArgs args)
        {
            checkoutTime.Focus();
        }
        void StartCall61(object sender, EventArgs args)
        {
            checkinTime.Focus();
        }
        void OnCheckoutTimePickerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            txtCheckoutTime.Text = checkoutTime.Time.ToString();
        }
        void OnCheckinTimePickerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            txtCheckinTime.Text = checkinTime.Time.ToString();
        }
        void OnErrorChecked() 
        {
            _error = 0;

            if (_outing_type_id > 0)
            {
                if (!string.IsNullOrEmpty(txtCheckoutDate.Text) && !string.IsNullOrEmpty(txtCheckoutTime.Text))
                {
                    if (!string.IsNullOrEmpty(txtCheckinDate.Text) && !string.IsNullOrEmpty(txtCheckinTime.Text))
                    {
                        if (!string.IsNullOrEmpty(txtReason.Text))
                        {
                            //proceed
                        }
                        else
                        {
                            _error++;
                            SnackB.Message = "Reason is required.";
                            SnackB.IsOpen = !SnackB.IsOpen;
                        }
                    }
                    else
                    {
                        _error++;
                        SnackB.Message = "Check In Date & Time is required.";
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                else
                {
                    _error++;
                    SnackB.Message = "Check Out Date & Time is required.";
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                _error++;
                SnackB.Message = "Outing type is required.";
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }
        async void OnSaveClicked(object sender, EventArgs args)
        {
            OnErrorChecked();
            if (_error == 0)
            {
                if (_outing_status_id == 0) 
                {
                    await SaveStudentOuting();
                }
                else if (_outing_status_id == 1)
                {
                    await UpdateStudentOuting();
                }
            }
        }
        async void OnSubmitClicked(object sender, EventArgs args)
        {
            OnErrorChecked();
            if (_error == 0)
            {
                await SubmitStudentOuting();
            }
        }

        public async Task SaveStudentOuting() 
        {
            DateTime dtCheckout = Convert.ToDateTime(txtCheckoutDate.Text.Split('-')[2] + "-" + txtCheckoutDate.Text.Split('-')[1] + "-" + txtCheckoutDate.Text.Split('-')[0] + " " + txtCheckoutTime.Text, myDtfi);
            DateTime dtCheckin = Convert.ToDateTime(txtCheckinDate.Text.Split('-')[2] + "-" + txtCheckinDate.Text.Split('-')[1] + "-" + txtCheckinDate.Text.Split('-')[0] + " " + txtCheckinTime.Text, myDtfi);

            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStudentSaveOutingRequest(_student_id, _school_id, _outing_type_id, dtCheckout, dtCheckin, txtReason.Text, Settings.profileId, Settings.userRoleId, Settings.fullName);
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

        public async Task UpdateStudentOuting()
        {
            DateTime dtCheckout = Convert.ToDateTime(txtCheckoutDate.Text.Split('-')[2] + "-" + txtCheckoutDate.Text.Split('-')[1] + "-" + txtCheckoutDate.Text.Split('-')[0] + " " + txtCheckoutTime.Text, myDtfi);
            DateTime dtCheckin = Convert.ToDateTime(txtCheckinDate.Text.Split('-')[2] + "-" + txtCheckinDate.Text.Split('-')[1] + "-" + txtCheckinDate.Text.Split('-')[0] + " " + txtCheckinTime.Text, myDtfi);

            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStudentUpdateOutingRequest(_outing_id, _student_id, _school_id, _outing_type_id, dtCheckout, dtCheckin, txtReason.Text, Settings.profileId, Settings.userRoleId, Settings.fullName);
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

        public async Task SubmitStudentOuting()
        {
            DateTime dtCheckout = Convert.ToDateTime(txtCheckoutDate.Text.Split('-')[2] + "-" + txtCheckoutDate.Text.Split('-')[1] + "-" + txtCheckoutDate.Text.Split('-')[0] + " " + txtCheckoutTime.Text, myDtfi);
            DateTime dtCheckin = Convert.ToDateTime(txtCheckinDate.Text.Split('-')[2] + "-" + txtCheckinDate.Text.Split('-')[1] + "-" + txtCheckinDate.Text.Split('-')[0] + " " + txtCheckinTime.Text, myDtfi);

            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStudentSubmitOutingRequest(_outing_id, _student_id, _school_id, _outing_type_id, dtCheckout, dtCheckin, txtReason.Text, Settings.profileId, Settings.userRoleId, Settings.fullName);
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