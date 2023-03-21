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
    public partial class NewKYCPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public string _nric;
        public string _full_name;
        public int _kyc_status_id;
        public string _user_role;
        public int _profile_id;
        public int _state_id;
        public int _city_id;
        public int _country_id;

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
                if (Option == "state")
                {
                    _state_id = Settings.stateId;
                    txtState.Text = Settings.stateName;
                    Device.BeginInvokeOnMainThread(() => {
                        txtState.Unfocus();
                    });
                }
                else if (Option == "country")
                {
                    _country_id = Settings.countryId;
                    txtCountry.Text = Settings.countryName;
                    Device.BeginInvokeOnMainThread(() => {
                        txtCountry.Unfocus();
                    });
                }
                else if (Option == "city")
                {
                    _city_id = Settings.cityId;
                    txtCity.Text = Settings.cityName;
                    Device.BeginInvokeOnMainThread(() => {
                        txtCity.Unfocus();
                    });

                }
                else if (Option == "occupation")
                {
                    txtOccupation.Text = Settings.occupationName;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        txtOccupation.Unfocus();
                    });
                }
            }
        }
        public NewKYCPage(int kyc_status_id)
        {
            InitializeComponent();
            BindingContext = this;

            _kyc_status_id = kyc_status_id;
            _profile_id = Settings.profileId;
            _nric = Settings.userName;
            _full_name = Settings.fullName;
            txtFullName.Text = _full_name;
            txtIDNo.Text = _nric;

            UserProfile();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public async void UserProfile()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostUserProfile(_profile_id);
                    string jsonStr = await t;
                    UserProfileProperty response = JsonConvert.DeserializeObject<UserProfileProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        foreach (UserProfile r in response.Data)
                        {
                            _profile_id = r.profile_id;
                            _full_name = r.full_name;
                            txtFullName.Text = r.full_name;
                            txtIDNo.Text = r.nric;
                            txtAddress.Text = r.address;
                            txtCity.Text = r.city;
                            txtPostcode.Text = r.postcode;
                            _state_id = r.state_id;
                            txtState.Text = r.state_name;
                            _country_id = r.country_id;
                            txtCountry.Text = r.country_name;
                            txtMotherName.Text = r.mother_maiden_name;
                            txtOccupation.Text = r.occupation;
                            txtEmployerName.Text = r.employer_name;
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

        async void StartCallState(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectStateText, "state", "kyc");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void StartCallCity(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectCityText, "city", "kyc", _state_id);
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void StartCallCountry(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectCountryText, "country", "kyc");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void StartCallOccupation(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectOccupationText, "occupation", "kyc");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void OnSaveClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtAddress.Text))
            {
                if (!string.IsNullOrEmpty(txtPostcode.Text))
                {
                    if (!string.IsNullOrEmpty(txtCity.Text))
                    {
                        if (!string.IsNullOrEmpty(txtState.Text))
                        {
                            if (!string.IsNullOrEmpty(txtCountry.Text))
                            {
                                if (!string.IsNullOrEmpty(txtMotherName.Text))
                                {
                                    if (!string.IsNullOrEmpty(txtOccupation.Text))
                                    {
                                        if (!string.IsNullOrEmpty(txtEmployerName.Text))
                                        {
                                            await UpdateUserProfile();
                                        }
                                        else
                                        {
                                            SnackB.Message = AppResources.EmployerNameRequiredText;
                                            SnackB.IsOpen = !SnackB.IsOpen;
                                        }
                                    }
                                    else
                                    {
                                        SnackB.Message = AppResources.OccupationRequiredText;
                                        SnackB.IsOpen = !SnackB.IsOpen;
                                    }
                                }
                                else
                                {
                                    SnackB.Message = AppResources.MotherNameRequiredText;
                                    SnackB.IsOpen = !SnackB.IsOpen;
                                }
                            }
                            else
                            {
                                SnackB.Message = AppResources.CountryRequiredText;
                                SnackB.IsOpen = !SnackB.IsOpen;
                            }
                        }
                        else
                        {
                            SnackB.Message = AppResources.StateRequiredText;
                            SnackB.IsOpen = !SnackB.IsOpen;
                        }
                    }
                    else
                    {
                        SnackB.Message = AppResources.CityRequiredText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                else
                {
                    SnackB.Message = AppResources.PostcodeText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                SnackB.Message = AppResources.AddressRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task UpdateUserProfile()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostAccountUpdateProfile(_profile_id, txtFullName.Text, txtIDNo.Text,
                        txtAddress.Text, txtPostcode.Text, txtCity.Text, _state_id, _country_id, txtMotherName.Text,
                        txtOccupation.Text, txtEmployerName.Text, txtFullName.Text);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await Navigation.PushAsync(new ResubmitKYCPage(_kyc_status_id));
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