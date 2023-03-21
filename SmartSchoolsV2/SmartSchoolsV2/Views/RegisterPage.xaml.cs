using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public string _file_name;
        public string _photo_base64;
        public string _photo_id_base64;
        public string _full_name;
        public string _mother_maiden_name;
        public string _occupation;
        public string _employer_name;
        public string _date_of_birth;
        public string _user_role;
        public string _nric;
        public int _user_role_id;
        public int _profile_id;
        public int _card_type_id;
        public int _user_race_id;
        public int _state_id;
        public int _city_id;
        public int _country_id;
        public int _nationality_id;
        public int _marketing_flag;
        public bool _tick1 = false;
        public bool _tick2 = false;
        public bool _tick3 = false;
        public bool _tick4 = false;
        public bool _iAgree = false;
        public ICommand TapLogin => new Command(async () => await Navigation.PopAsync());
        public ICommand TapCommand => new Command(async () => await Navigation.PushAsync(new TermsConditionsPage()));
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
                if (Option == "card-type")
                {
                    _card_type_id = Settings.cardTypeId;
                    txtIDDocType.Text = Settings.cardType;
                    Device.BeginInvokeOnMainThread(() => {
                        txtIDDocType.Unfocus();
                    });

                }
                else if (Option == "nation")
                {
                    _nationality_id = Settings.countryId;
                    txtNationality.Text = Settings.countryName;
                    Device.BeginInvokeOnMainThread(() => {
                        txtNationality.Unfocus();
                    });

                }
                else if (Option == "state")
                {
                    _state_id = Settings.stateId;
                    txtState.Text = Settings.stateName;
                    Device.BeginInvokeOnMainThread(() => {
                        txtState.Unfocus();
                    });

                    if (_state_id > 0) 
                    {
                        txtCity.IsEnabled = true;
                    }
                }
                else if (Option == "city")
                {
                    _city_id = Settings.cityId;
                    txtCity.Text = Settings.cityName;
                    Device.BeginInvokeOnMainThread(() => {
                        txtCity.Unfocus();
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
                else if (Option == "occupation")
                {
                    //_occupation_id = Settings.occupationId;
                    txtOccupation.Text = Settings.occupationName;
                    Device.BeginInvokeOnMainThread(() => {
                        txtOccupation.Unfocus();
                    });
                }

                Option = string.Empty;
                Back = "N";
            }
        }

        private void AgreeChecked(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
            {
                _iAgree = true;
            }
            else
            {
                _iAgree = false;
            }
        }
        public RegisterPage(int user_role_id, string nric = "")
        {
            InitializeComponent();
            BindingContext = this;
            _nric = nric;
            _user_role_id = user_role_id;
            if (_user_role_id == 8) 
            {
                lblTitle.Text = AppResources.RegisterText;
                lblFullName.Text = AppResources.FullNameText;
            }
            else if (_user_role_id == 9)
            {
                lblTitle.Text = AppResources.RegisterParentText;
                lblFullName.Text = AppResources.ParentFullNameText;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!string.IsNullOrEmpty(_nric)) {
                txtIDNo.Text = _nric;
            }
        }

        async void OnRegisterClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtFullName.Text))
            {
                Settings.fullName = txtFullName.Text.Trim();
                if (!string.IsNullOrEmpty(txtNationality.Text))
                {
                    if (_card_type_id > 0)
                    {
                        if (!string.IsNullOrEmpty(txtIDNo.Text))
                        {
                            if (!string.IsNullOrEmpty(txtMobileNo.Text))
                            {
                                if (!string.IsNullOrEmpty(txtEmail.Text))
                                {

                                    if (!string.IsNullOrEmpty(txtAddress.Text))
                                    {
                                        if (!string.IsNullOrEmpty(txtPostcode.Text))
                                        {
                                            if (!string.IsNullOrEmpty(txtState.Text))
                                            {
                                                if (!string.IsNullOrEmpty(txtCity.Text))
                                                {
                                                    if (!string.IsNullOrEmpty(txtCountry.Text))
                                                    {
                                                        if (!string.IsNullOrEmpty(txtMotherName.Text))
                                                        {
                                                            if (!string.IsNullOrEmpty(txtOccupation.Text))
                                                            {
                                                                if (!string.IsNullOrEmpty(txtEmployerName.Text))
                                                                {
                                                                    if (!string.IsNullOrEmpty(txtPswd.Text))
                                                                    {
                                                                        if (!string.IsNullOrWhiteSpace(txtConfirmPswd.Text))
                                                                        {
                                                                            if (_tick1 && _tick2 && _tick3)
                                                                            {
                                                                                if (_tick4)
                                                                                {
                                                                                    if (!string.IsNullOrEmpty(txtSelfieImage.Text))
                                                                                    {
                                                                                        if (!string.IsNullOrEmpty(txtIDImage.Text))
                                                                                        {
                                                                                            if (_iAgree == true)
                                                                                            {
                                                                                                await RegisterUser();
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                SnackB.Message = AppResources.PleaseAgreeText;
                                                                                                SnackB.IsOpen = !SnackB.IsOpen;
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            SnackB.Message = AppResources.IDImageRequiredText;
                                                                                            SnackB.IsOpen = !SnackB.IsOpen;
                                                                                        }
                                                                                    }
                                                                                    else {
                                                                                        SnackB.Message = AppResources.SelfieImageRequiredText;
                                                                                        SnackB.IsOpen = !SnackB.IsOpen;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    SnackB.Message = AppResources.PasswordNotMatchText;
                                                                                    SnackB.IsOpen = !SnackB.IsOpen;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                SnackB.Message = AppResources.PasswordRuleMetText;
                                                                                SnackB.IsOpen = !SnackB.IsOpen;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            SnackB.Message = AppResources.ConfirmNewPasswordRequiredText;
                                                                            SnackB.IsOpen = !SnackB.IsOpen;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        SnackB.Message = AppResources.NewPasswordRequiredText;
                                                                        SnackB.IsOpen = !SnackB.IsOpen;
                                                                    }
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
                                                    SnackB.Message = AppResources.CityRequiredText;
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
                                            SnackB.Message = AppResources.PostcodeRequiredText;
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
                            SnackB.Message = AppResources.MyKadPassportNoRequiredText;
                            SnackB.IsOpen = !SnackB.IsOpen;
                        }
                    }
                    else
                    {
                        SnackB.Message = AppResources.IDTypeRequiredText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                else
                {
                    SnackB.Message = AppResources.NationalityRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                SnackB.Message = AppResources.FullNameRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async Task RegisterUser()
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

                    var t = srvc.PostUserRegister(txtFullName.Text.Trim(),_nationality_id, _card_type_id, txtIDNo.Text.Trim(), 
                        _date_of_birth, mobileNum, txtEmail.Text.Trim(),txtAddress.Text,txtPostcode.Text,txtCity.Text,_state_id,
                        _country_id, txtMotherName.Text,txtOccupation.Text,txtEmployerName.Text,txtPswd.Text.Trim(),_marketing_flag,
                        _user_role_id);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        if (response.Code == "verify_account")
                        {
                            await Navigation.PushAsync(new VerifyAccountPage(mobileNum, txtIDNo.Text.Trim(), txtPswd.Text.Trim()));
                        }
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

        async void StartCallDocType(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectIDDocTypeText, "card-type", "register");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        void StartCallDob(object sender, EventArgs args)
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

        async void StartCallState(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectStateText, "state", "register");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void StartCallCity(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectCityText, "city", "register", _state_id);
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void StartCallCountry(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectCountryText, "country", "register");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void StartCallNationality(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectNationalityText, "nation", "register");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void StartCallOccupation(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectOccupationText, "occupation", "register");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        private void OnNewPswdTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length >= 8)
            {
                _tick1 = true;
            }
            else
            {
                _tick1 = false;
            }

            bool UpperLower = HasUpperLower(e.NewTextValue);
            if (UpperLower == true)
            {
                _tick2 = true;
            }
            else
            {
                _tick3 = false;
            }

            bool special = HasDigit(e.NewTextValue);
            if (special == true)
            {
                _tick3 = true;
            }
            else
            {
                _tick3 = false;
            }

            if (_tick1 && _tick2 && _tick3)
            {
                lblRule.TextColor = Color.Green;
            }
            else
            {
                lblRule.TextColor = Color.Red;
            }
        }

        private void OnConfirmNewPswdTextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtPswd.Text == e.NewTextValue && !string.IsNullOrEmpty(e.NewTextValue))
            {
                lblMatch.TextColor = Color.Green;
                _tick4 = true;
            }
            else
            {
                lblMatch.TextColor = Color.Red;
                _tick4 = false;
            }
        }
        public static bool HasUpperLower(string text)
        {
            bool hasUpper = false; bool hasLower = false;
            for (int i = 0; i < text.Length && !(hasUpper && hasLower); i++)
            {
                char c = text[i];
                if (!hasUpper) hasUpper = char.IsUpper(c);
                if (!hasLower) hasLower = char.IsLower(c);
            }
            return hasUpper && hasLower;
        }
        public static bool HasDigit(string text)
        {
            bool hasDigit = false;
            for (int i = 0; i < text.Length && !(hasDigit); i++)
            {
                char c = text[i];
                if (!hasDigit) hasDigit = char.IsDigit(c);
            }
            return hasDigit;
        }

        async void OnButtonSelfieUploadClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtFullName.Text))
            {
                if (!string.IsNullOrEmpty(txtIDNo.Text))
                {
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("", AppResources.PermissionGalleryText, "OK");
                        return;
                    }
                    var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        CustomPhotoSize = 50,
                        PhotoSize = PhotoSize.Medium,
                        CompressionQuality = 75
                    });


                    if (file == null)
                        return;

                    _file_name = file.Path.Split('/').Last();
                    var fileLength = new FileInfo(file.Path).Length;
                    string file_size = GetFileSize(fileLength);
                    txtSelfieImage.Text = file_size;

                    var stream = file.GetStream();
                    byte[] imgByte = ReadAllBytes(stream);
                    _photo_base64 = Convert.ToBase64String(imgByte);

                    bool valid = ValidateBase64EncodedString(_photo_base64);

                    if (valid)
                    {
                        try
                        {
                            ShowLoadingPopup();

                            SetControls(true, 1, _file_name + " (" + file_size + ")", false);

                            var t = srvc.PostUserUploadImage(txtIDNo.Text, 1, _file_name, _photo_base64, txtFullName.Text);
                            string jsonStr = await t;
                            CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);

                            if (response.Success == true)
                            {
                                SetControls(false, 1, _file_name + " (" + file_size + ")", true);
                            }
                            else
                            {
                                SetControls(false, 1, _file_name + " (" + file_size + ")", false);
                                SnackB.Message = AppResources.ErrorUploadImageText;
                                SnackB.IsOpen = !SnackB.IsOpen;
                            }
                        }
                        catch (Exception)
                        {
                            SetControls(false, 2, _file_name + " (" + file_size + ")", false);
                            SnackB.Message = AppResources.ErrorUploadImageText;
                            SnackB.IsOpen = !SnackB.IsOpen;
                        }
                        finally {
                            HideLoadingPopup();
                        }
                    }
                    else 
                    {
                        SetControls(false, 1, _file_name + " (" + file_size + ")", false);
                        SnackB.Message = AppResources.InvalidImageText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                else
                {
                    SnackB.Message = AppResources.MyKadPassportNoRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                SnackB.Message = AppResources.FullNameRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        private static bool ValidateBase64EncodedString(string inputText)
        {
            string stringToValidate = inputText;
            stringToValidate = stringToValidate.Replace('-', '+'); // 62nd char of encoding
            stringToValidate = stringToValidate.Replace('_', '/'); // 63rd char of encoding
            switch (stringToValidate.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: stringToValidate += "=="; break; // Two pad chars
                case 3: stringToValidate += "="; break; // One pad char
                default:
                    return false;
            }

            return true;
        }

        public static string GetFileSize(long fileLength)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (fileLength >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                fileLength = fileLength / 1024;
            }
            string result = String.Format("{0:0.##} {1}", fileLength, sizes[order]);
            return result;
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

        async void OnButtonIDUploadClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtFullName.Text))
            {
                if (!string.IsNullOrEmpty(txtIDNo.Text))
                {
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("", AppResources.PermissionGalleryText, "OK");
                        return;
                    }
                    var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        CustomPhotoSize = 50,
                        PhotoSize = PhotoSize.Medium,
                        CompressionQuality = 75
                    });


                    if (file == null)
                        return;

                    _file_name = file.Path.Split('/').Last();
                    var fileLength = new FileInfo(file.Path).Length;
                    string file_size = GetFileSize(fileLength);
                    txtIDImage.Text = file_size;

                    var stream = file.GetStream();
                    byte[] imgByte = ReadAllBytes(stream);
                    _photo_base64 = Convert.ToBase64String(imgByte);

                    bool valid = ValidateBase64EncodedString(_photo_base64);

                    if (valid)
                    {
                        try
                        {
                            ShowLoadingPopup();

                            SetControls(true, 2, _file_name + " (" + file_size + ")", false);

                            var t = srvc.PostUserUploadImage(txtIDNo.Text, 2, _file_name, _photo_base64, txtFullName.Text);
                            string jsonStr = await t;
                            CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);

                            if (response.Success == true)
                            {
                                SetControls(false, 2, _file_name + " (" + file_size + ")", true);
                            }
                            else
                            {
                                SetControls(false, 2, _file_name + " (" + file_size + ")", false);
                                SnackB.Message = AppResources.ErrorUploadImageText;
                                SnackB.IsOpen = !SnackB.IsOpen;
                            }
                        }
                        catch (Exception)
                        {
                            SetControls(false, 2, _file_name + " (" + file_size + ")", false);
                            SnackB.Message = AppResources.ErrorUploadImageText;
                            SnackB.IsOpen = !SnackB.IsOpen;
                        }
                        finally {
                            HideLoadingPopup();
                        }
                    }
                    else
                    {
                        SetControls(false, 2, _file_name + " (" + file_size + ")", false);
                        SnackB.Message = AppResources.InvalidImageText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }

                }
                else
                {
                    SnackB.Message = AppResources.MyKadPassportNoRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                SnackB.Message = AppResources.FullNameRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public ICommand RemoveSelfie => new Command((item) => ExecuteRemoveSelfie(item));
        private void ExecuteRemoveSelfie(object item)
        {
            btnRemoveSelfie.IsVisible = false;
            txtSelfieImage.IsVisible = false;
            txtSelfieImage.Text = "";
            btnUploadSelfie.IsEnabled = true;
        }

        public ICommand RemoveID => new Command((item) =>  ExecuteRemoveID(item));
        private void ExecuteRemoveID(object item)
        {
            btnRemoveID.IsVisible = false;
            txtIDImage.IsVisible = false;
            txtIDImage.Text = "";
            btnUploadID.IsEnabled = true;
        }

        private void SetControls(bool startUpload, int imageType, string fileName, bool success)
        {
            if (startUpload)
            {
                if (imageType == 1)
                {
                    txtSelfieImage.IsVisible = true;
                    txtSelfieImage.Text = AppResources.UploadingText;
                    //btnUploadSelfie.IsEnabled = false;
                }
                else 
                {
                    txtIDImage.IsVisible = true;
                    txtIDImage.Text = AppResources.UploadingText;
                    //btnUploadID.IsEnabled = false;
                }
            }
            else
            {
                if (success)
                {
                    if (imageType == 1)
                    {
                        txtSelfieImage.IsVisible = true;
                        txtSelfieImage.Text = fileName;
                        btnRemoveSelfie.IsVisible = true;
                        btnUploadSelfie.IsEnabled = false;
                    }
                    else
                    {
                        txtIDImage.IsVisible = true;
                        txtIDImage.Text = fileName;
                        btnRemoveID.IsVisible = true;
                        btnUploadID.IsEnabled = false;
                    }
                }
                else 
                {
                    if (imageType == 1)
                    {
                        txtSelfieImage.IsVisible = true;
                        txtSelfieImage.Text = AppResources.UploadFailedtext;
                        btnRemoveSelfie.IsVisible = false;
                        btnUploadSelfie.IsEnabled = true;
                    }
                    else
                    {
                        txtIDImage.IsVisible = true;
                        txtIDImage.Text = AppResources.UploadFailedtext;
                        btnRemoveID.IsVisible = false;
                        btnUploadID.IsEnabled = true;
                    }
                }
            }
        }

        private void CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!e.Value)
            {
                _marketing_flag = 0;
            }
            else
            {
                _marketing_flag = 1;
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