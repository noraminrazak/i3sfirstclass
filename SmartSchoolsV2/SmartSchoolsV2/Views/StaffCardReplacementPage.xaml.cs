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
    public partial class StaffCardReplacementPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _shift_id;
        public int _card_id;
        public int _old_card_status_id;
        public int _card_status_id;
        public int _student_id;
        public int _staff_id;
        public int _profile_id;
        public int _school_id;
        public int _school_type_id;
        public int _class_id;
        public int _user_role_id;
        public string _full_name;
        public string _photo_url;
        public string _school_name;
        public string _class_name;
        public string _wallet_number;
        public string _account_balance;
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

        public StaffCardReplacementPage()
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

                        if (_card_id == 0)
                        {
                            btnReplace.IsEnabled = false;
                        }
                        else
                        {
                            btnReplace.IsEnabled = true;
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
                            btnReplace.IsEnabled = false;
                        }
                        else 
                        {
                            btnReplace.IsEnabled = true;
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
                    var page = new SearchListPage3(AppResources.SelectStudentText, "replacement", _user_role_id, _school_id, txtSearchStudent.Text);
                    page.DetailSet += this.OnDetailSet;
                    await Navigation.PushAsync(page);
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

        void OnRadio1Clicked(object sender, EventArgs e)
        {
            _old_card_status_id = 3;
        }

        void OnRadio2Clicked(object sender, EventArgs e)
        {
            _old_card_status_id = 4;
        }

        void OnRadio3Clicked(object sender, EventArgs e)
        {
            _old_card_status_id = 6;
        }

        async void OnReplaceClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtCardNumber.Text))
            {
                if (_old_card_status_id > 0)
                {
                    if (!string.IsNullOrEmpty(txtNewCardNumber.Text))
                    {
                        await CardReplacement();
                    }
                    else
                    {
                        SnackB.Message = AppResources.NewCardNoRequiredText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                else
                {
                    SnackB.Message = AppResources.SelectReasonForReplacementText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                SnackB.Message = AppResources.CardNoRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }

        }

        public ICommand RemoveCommand => new Command(async (item) => await ExecuteRemoveCommand(item));
        private async Task ExecuteRemoveCommand(object item)
        {
            txtSearchStudent.Text = string.Empty;
            searchBar.IsVisible = true;
            cardDetail.IsVisible = false;
            gridProfile.IsVisible = false;
            _old_card_status_id = 0;
            radio1.IsChecked = false;
            radio2.IsChecked = false;
            radio3.IsChecked = false;
        }

        async Task CardReplacement()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostCardReplacement(_user_role_id, _school_id, _card_id, _old_card_status_id, txtNewCardNumber.Text.Replace(" ", string.Empty), Settings.fullName);
                    string jsonStr = await t;
                    StudentProfileProperty response = JsonConvert.DeserializeObject<StudentProfileProperty>(jsonStr);
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
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}