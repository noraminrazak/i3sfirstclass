using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffCardLostReportPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _card_id;
        public int _shift_id;
        public int _card_status_id;
        public int _student_id;
        public int _staff_id;
        public int _profile_id;
        public int _school_id;
        public int _user_role_id;
        public string _full_name;
        public string _photo_url;
        public string _message;
        public static string _back;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }
        public async void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
                await StudentProfile(Settings.assignProfileId);
                cardDetail.IsVisible = true;
                gridProfile.IsVisible = true;
                searchBar.IsVisible = false;
                btnRemove.IsVisible = true;
            }
        }

        public StaffCardLostReportPage()
        {
            InitializeComponent();
            BindingContext = this;

            _school_id = Settings.schoolId;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        async void OnRadio8Clicked(object sender, EventArgs e)
        {
            _user_role_id = 8;

            await StaffProfile(Settings.profileId);

            cardDetail.IsVisible = true;
            gridProfile.IsVisible = true;
            searchBar.IsVisible = false;
            btnRemove.IsVisible = false;
        }

        void OnRadio10Clicked(object sender, EventArgs e)
        {
            _user_role_id = 10;
            txtSearchStudent.Text = string.Empty;
            searchBar.IsVisible = true;
            cardDetail.IsVisible = false;
            gridProfile.IsVisible = false;
        }
        public async Task StaffProfile(int profile_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostStaffProfile(profile_id);
                    string jsonStr = await t;
                    StaffProfileProperty response = JsonConvert.DeserializeObject<StaffProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        foreach (StaffProfile r in response.Data)
                        {
                            _card_id = r.card_id;
                            txtCardNumber.Text = CardNumberFormat(r.card_number, 4, " ");
                            _card_status_id = r.card_status_id;
                            txtCardStatus.Text = r.card_status;
                            lblFullName.Text = r.full_name;
                            lblSchoolName.Text = r.school_name;
                            lblClassName.Text = r.shift_code;
                            _shift_id = r.shift_id;
                            _photo_url = r.photo_url;
                        }

                        if (!string.IsNullOrEmpty(_photo_url))
                        {
                            userInitial.IsVisible = false;
                            userImage.IsVisible = true;
                            userImage.Source = requestUrl + _photo_url;
                        }
                        else
                        {
                            userInitial.IsVisible = true;
                        }

                        if (_shift_id > 0)
                        {
                            dotImage.IsVisible = true;
                        }
                        else
                        {
                            dotImage.IsVisible = false;
                        }

                        if (_card_status_id == 2)
                        {
                            _message = AppResources.CardStatus2Text;
                        }
                        else if (_card_status_id == 5)
                        {
                            btnReport.IsEnabled = false;
                            _message = AppResources.CardStatus5Text;
                        }

                        if (_card_id == 0)
                        {
                            btnReport.IsEnabled = false;
                            _message = AppResources.CardNotAssignedText;
                        }

                        lblMessage.Text = _message;
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
                    HideLoadingPopup();
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task StudentProfile(int profile_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostStudentProfile(profile_id);
                    string jsonStr = await t;
                    StudentProfileProperty response = JsonConvert.DeserializeObject<StudentProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        foreach (StudentProfile r in response.Data)
                        {
                            _card_id = r.card_id;
                            txtCardNumber.Text = CardNumberFormat(r.card_number, 4, " ");
                            _card_status_id = r.card_status_id;
                            txtCardStatus.Text = r.card_status;
                            lblFullName.Text = r.full_name;
                            lblSchoolName.Text = r.school_name;
                            lblClassName.Text = r.class_name;
                            _photo_url = r.photo_url;
                        }

                        if (!string.IsNullOrEmpty(_photo_url))
                        {
                            userInitial.IsVisible = false;
                            userImage.IsVisible = true;
                            userImage.Source = requestUrl + _photo_url;
                        }
                        else
                        {
                            userInitial.IsVisible = true;
                        }
                        dotImage.IsVisible = true;

                        if (_card_id == 0)
                        {
                            btnReport.IsEnabled = false;
                            _message = AppResources.CardNotAssignedText;
                        }
                        else 
                        {
                            if (_card_status_id == 2)
                            {
                                _message = AppResources.CardStatus2Text;
                            }
                            else if (_card_status_id == 5)
                            {
                                btnReport.IsEnabled = false;
                                _message = AppResources.CardStatus5Text;
                            }
                        }

                        lblMessage.Text = _message;
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
                    HideLoadingPopup();
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void OnSearchClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(txtSearchStudent.Text))
            {
                if (txtSearchStudent.Text.Length > 2)
                {
                    var page = new SearchListPage3(AppResources.SelectStudentText,"report", _user_role_id, _school_id, txtSearchStudent.Text);
                    page.DetailSet += this.OnDetailSet;
                    await Navigation.PushAsync(page);
                }
                else {
                    SnackB.Message = AppResources.PleaseEnterMoreCharText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else {
                SnackB.Message = AppResources.StudentNameRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }

        }

        public static string CardNumberFormat(string cardNumber, int batchSize, string separator)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= cardNumber.Length / batchSize; i++)
            {
                if (i > 0) sb.Append(separator);
                int currentIndex = i * batchSize;
                sb.Append(cardNumber.Substring(currentIndex,
                          Math.Min(batchSize, cardNumber.Length - currentIndex)));
            }
            return sb.ToString();
        }

        public ICommand RemoveCommand => new Command(async (item) => await ExecuteRemoveCommand(item));
        private async Task ExecuteRemoveCommand(object item)
        {
            txtSearchStudent.Text = string.Empty;
            searchBar.IsVisible = true;
            cardDetail.IsVisible = false;
            gridProfile.IsVisible = false;
        }

        public async void UpdateCardStatus(int profile_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    //ShowLoadingPopup();
                    var t = srvc.PostStudentProfile(profile_id);
                    string jsonStr = await t;
                    StudentProfileProperty response = JsonConvert.DeserializeObject<StudentProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        string photo_url = string.Empty;
                        foreach (StudentProfile r in response.Data)
                        {
                            _card_status_id = r.card_status_id;
                            txtCardStatus.Text = r.card_status;
                        }

                        if (_card_status_id == 2)
                        {
                            _message = AppResources.CardStatus2Text;
                        }
                        else if (_card_status_id == 5)
                        {
                            btnReport.IsEnabled = false;
                            _message = AppResources.CardStatus5Text;
                        }

                        lblMessage.Text = _message;
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
                    //HideLoadingPopup();
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void OnReportClicked(object sender, EventArgs args)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostCardUpdateStatusBlacklist(_card_id, _school_id, Settings.fullName);
                    string jsonStr = await t;
                    StudentProfileProperty response = JsonConvert.DeserializeObject<StudentProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        UpdateCardStatus(_profile_id);
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
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}