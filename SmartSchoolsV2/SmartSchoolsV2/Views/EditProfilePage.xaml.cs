using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;
using Xamarin.Essentials;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditProfilePage : ContentPage
	{
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public string _file_name;
        public string _photo_base64;
        public string _full_name;
        public string _date_of_birth;
        public string _user_role;
        public int _profile_id;
        public int _card_type_id;
        public int _user_race_id;
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
                else if (Option == "user-race")
                {
                    _user_race_id = Settings.userRaceId;
                    txtUserRace.Text = Settings.userRace;
                    Device.BeginInvokeOnMainThread(() => {
                        txtUserRace.Unfocus();
                    });
                }
                else if (Option == "card-type")
                {
                    _card_type_id = Settings.cardTypeId;
                    txtIDDocType.Text = Settings.cardType;
                    Device.BeginInvokeOnMainThread(() => {
                        txtIDDocType.Unfocus();
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
        public EditProfilePage (string user_role)
		{
			InitializeComponent ();
            BindingContext = this;

            _user_role = user_role;
            if (_user_role == "student")
            {
                _profile_id = Settings.studentProfileId;
            }
            else
            {
                _profile_id = Settings.profileId;
            }

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
                            if (!string.IsNullOrEmpty(r.photo_url))
                            {
                                userInitial.IsVisible = false;
                                imagePhoto.IsVisible = true;
                                btnRemove.IsVisible = true;
                                //imagePhoto.Source = requestUrl + r.photo_url;
                                imagePhoto.Source = ImageSource.FromUri(new Uri(requestUrl + r.photo_url));
                            }
                            else
                            {
                                userInitial.IsVisible = true;
                                btnRemove.IsVisible = false;
                            }
                            _profile_id = r.profile_id;
                            _full_name = r.full_name;
                            txtFullName.Text = r.full_name;
                            _card_type_id = r.card_type_id;
                            txtIDDocType.Text = r.card_type;
                            txtIDNo.Text = r.nric;
                            if (!string.IsNullOrEmpty(r.date_of_birth))
                            {
                                //_date_of_birth = Convert.ToDateTime(r.date_of_birth).ToString("dd-MM-yyyy");
                                txtDoB.Text = r.date_of_birth;
                            }
                            else 
                            {
                                txtDoB.Text = null;
                            }
                            _user_race_id = r.user_race_id;
                            txtUserRace.Text = r.user_race;
                            if (!string.IsNullOrEmpty(r.mobile_number)) 
                            {
                                txtMobileNo.Text = r.mobile_number.Substring(3);
                            }
                            txtEmail.Text = r.email;
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

        async void OnPickPhotoClicked(object sender, EventArgs args)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("", AppResources.PermissionGalleryText, "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Medium
                //CustomPhotoSize = 75,
            });


            if (file == null)
                return;

            _file_name = file.Path.Split('/').Last();
            userInitial.IsVisible = false;
            imagePhoto.IsVisible = true;
            btnRemove.IsVisible = true;

            imagePhoto.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                byte[] imgByte = ReadAllBytes(stream);
                _photo_base64 = Convert.ToBase64String(imgByte);
                file.Dispose();
                return stream;
            });

            System.Threading.Thread.Sleep(1000);

            await UpdateUserPhoto();
        }
        async void OnTakePhotoClicked(object sender, EventArgs args)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("", AppResources.NoCameraText, "OK");
                return;
            }

            var mediaOptions = new StoreCameraMediaOptions
            {
                Directory = "i3s",
                AllowCropping = false,
                SaveToAlbum = true,
                PhotoSize = PhotoSize.Medium,
                //CustomPhotoSize = 75,
                MaxWidthHeight = 1000,
                DefaultCamera = CameraDevice.Front
            };

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);

            if (file == null)
                return;

            _file_name = file.Path.Split('/').Last();
            userInitial.IsVisible = false;
            imagePhoto.IsVisible = true;
            btnRemove.IsVisible = true;

            imagePhoto.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                byte[] imgByte = ReadAllBytes(stream);
                _photo_base64 = Convert.ToBase64String(imgByte);
                file.Dispose();
                return stream;
            });

            System.Threading.Thread.Sleep(1000);

            await UpdateUserPhoto();
        }
        public static byte[] ReadAllBytes(Stream instream)
        {
            if (instream is MemoryStream)
                return ((MemoryStream)instream).ToArray();

            using (var memoryStream = new MemoryStream())
            {
                instream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        async void OnRemoveClicked(object sender, EventArgs args)
        {
            if (imagePhoto.Source != null)
            {
                await RemoveUserPhoto();
            }
        }

        public async Task RemoveUserPhoto()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostUserRemovePhoto(_profile_id, _full_name);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        //await DisplayAlert(AppResources.DoneText, response.Message, "OK");
                        userInitial.IsVisible = true;
                        imagePhoto.IsVisible = false;
                        btnRemove.IsVisible = false;
                        _file_name = "";
                        _photo_base64 = "";
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


        void StartCall(object sender, EventArgs args)
        {
            Device.BeginInvokeOnMainThread(() => {
                datePicker.Focus();
            });
        }

        private void OnDoBDateSelected(object sender, DateChangedEventArgs e) 
        {
            txtDoB.Text = e.NewDate.ToString("dd-MM-yyyy");
            _date_of_birth = e.NewDate.ToString("yyyy-MM-dd");
        }

        async void StartCall1(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectIDDocTypeText, "card-type", "profile");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void StartCall2(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectRaceText, "user-race", "profile");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void StartCallState(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectStateText, "state", "profile");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void StartCallCity(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectCityText, "city", "profile", _state_id);
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void StartCallCountry(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectCountryText, "country", "profile");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void StartCallOccupation(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectOccupationText, "occupation", "profile");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void OnSaveClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtDoB.Text))
            {
                if (!string.IsNullOrEmpty(txtUserRace.Text))
                {
                    if ((!string.IsNullOrEmpty(txtMobileNo.Text) && _user_role != "student") || 
                        ((string.IsNullOrEmpty(txtMobileNo.Text) || !string.IsNullOrEmpty(txtMobileNo.Text)) && _user_role == "student"))
                    {
                        if ((!string.IsNullOrEmpty(txtEmail.Text) && _user_role != "student") ||
                            ((string.IsNullOrEmpty(txtEmail.Text) || !string.IsNullOrEmpty(txtEmail.Text)) && _user_role == "student"))
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
                        else
                        {
                            SnackB.Message = AppResources.EmailRequiredText;
                            SnackB.IsOpen = !SnackB.IsOpen;
                        }
                    }
                    else
                    {
                        SnackB.Message = AppResources.MobileNoRequiredText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                else
                {
                    SnackB.Message = AppResources.UserRaceRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else 
            {
                SnackB.Message = AppResources.DOBRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task UpdateUserPhoto()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostUserUpdatePhoto(_profile_id, _file_name, _photo_base64, _full_name);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        //await DisplayAlert(AppResources.DoneText, "Profile updated successfully.", "OK");
                        //SnackB.Message = "Profile updated successfully.";
                        //SnackB.IsOpen = !SnackB.IsOpen;
                        //await Navigation.PopAsync();
                        //Settings.photoUrl = response.photo_url;
                    }
                    else
                    {
                        //await DisplayAlert(AppResources.SorryText, response.Message, "OK");
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

        public async Task UpdateUserProfile()
        {
            bool zeroStart = txtMobileNo.Text.StartsWith("0");
            string mobileNum = string.Empty;

            if (zeroStart == true)
            {
                mobileNum = txtCountryCode.Text + txtMobileNo.Text.Remove(0, 1);
            }
            else
            {
                mobileNum = txtCountryCode.Text + txtMobileNo.Text;
            }

            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    string iDate = txtDoB.Text.Split("-")[2] + "-" + txtDoB.Text.Split("-")[1] + "-" + txtDoB.Text.Split("-")[0];
                    DateTime oDate = DateTime.Parse(iDate);

                    var t = srvc.PostUserUpdateProfile(_profile_id, txtFullName.Text, _user_race_id, _card_type_id, txtIDNo.Text, oDate.ToString("yyyy-MM-dd"),
                        mobileNum, txtEmail.Text,txtAddress.Text,txtPostcode.Text,txtCity.Text,_state_id, txtState.Text, _country_id, txtMotherName.Text,
                        txtOccupation.Text,txtEmployerName.Text,txtFullName.Text);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        //await Navigation.PopAsync();
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