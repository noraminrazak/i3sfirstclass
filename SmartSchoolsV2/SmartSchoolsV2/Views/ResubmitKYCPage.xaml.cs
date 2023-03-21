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
    public partial class ResubmitKYCPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public string _nric;
        public string _file_name;
        public string _photo_base64;
        public string _photo_id_base64;
        public string _full_name;
        public int _kyc_status_id;
        public ResubmitKYCPage(int kyc_status_id)
        {
            InitializeComponent();
            BindingContext = this;
            _nric = Settings.userName;
            _full_name = Settings.fullName;
            _kyc_status_id = kyc_status_id;
            txtFullName.Text = _full_name;
            txtIDNo.Text = _nric;

            if (_kyc_status_id == 0) 
            {
                lblTitle.Text = AppResources.DocumentVerifyText;
            }
            else if (_kyc_status_id == 10) 
            {
                lblTitle.Text = AppResources.ResubmitDocumentText;
            }
        }

        async void OnResubmitClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtFullName.Text))
            {
                if (!string.IsNullOrEmpty(txtIDNo.Text))
                {
                    if (!string.IsNullOrEmpty(txtIDImage.Text))
                    {
                        if (!string.IsNullOrEmpty(txtSelfieImage.Text))
                        {
                            if (_kyc_status_id == 0)
                            {
                                await VerifyKYC();
                            }
                            else if (_kyc_status_id == 10)
                            {
                                await ResubmitKYC();
                            }
                        }
                        else
                        {
                            SnackB.Message = AppResources.SelfieImageRequiredText;
                            SnackB.IsOpen = !SnackB.IsOpen;
                        }
                    }
                    else
                    {
                        SnackB.Message = AppResources.IDImageRequiredText;
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

        async Task ResubmitKYC()
        {

            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostAccountResubmitKYC(txtIDNo.Text.Trim());
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");
                        await Navigation.PopToRootAsync();
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

        async Task VerifyKYC()
        {

            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostAccountVerifyKYC(txtIDNo.Text.Trim());
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");
                        await Navigation.PopToRootAsync();
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

        async void OnButtonSelfieUploadClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtFullName.Text))
            {
                if (!string.IsNullOrEmpty(txtIDNo.Text))
                {
                    //if (!CrossMedia.Current.IsPickPhotoSupported)
                    //{
                    //    await DisplayAlert("", AppResources.PermissionGalleryText, "OK");
                    //    return;
                    //}
                    //var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    //{
                    //    CustomPhotoSize = 50,
                    //    PhotoSize = PhotoSize.Medium,
                    //    CompressionQuality = 75
                    //});


                    //if (file == null)
                    //    return;

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
                        PhotoSize = PhotoSize.Large,
                        CompressionQuality = 90,
                        MaxWidthHeight = 1000,
                        DefaultCamera = CameraDevice.Front,
                    };

                    MediaFile file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);

                    if (file == null)
                        return;

                    _file_name = file.Path.Split('/').Last();
                    var fileLength = new FileInfo(file.Path).Length;
                    string file_size = GetFileSize(fileLength);
                    txtSelfieImage.Text = file_size;

                    //var stream = file.GetStream();
                    //byte[] imgByte = ReadAllBytes(stream);
                    //_photo_base64 = Convert.ToBase64String(imgByte);

                    imageSelfie.Scale = 1.0;
                    imageSelfie.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        byte[] imgByte = ReadAllBytes(stream);
                        _photo_base64 = Convert.ToBase64String(imgByte);
                        file.Dispose();
                        return stream;
                    });

                    System.Threading.Thread.Sleep(1000);

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
                        finally 
                        {
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
                    //if (!CrossMedia.Current.IsPickPhotoSupported)
                    //{
                    //    await DisplayAlert("", AppResources.PermissionGalleryText, "OK");
                    //    return;
                    //}
                    //var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    //{
                    //    CustomPhotoSize = 50,
                    //    PhotoSize = PhotoSize.Medium,
                    //    CompressionQuality = 75
                    //});


                    //if (file == null)
                    //    return;

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
                        PhotoSize = PhotoSize.Large,
                        CompressionQuality = 90,
                        MaxWidthHeight = 1000,
                        DefaultCamera = CameraDevice.Rear
                    };

                    MediaFile file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);

                    if (file == null)
                        return;

                    _file_name = file.Path.Split('/').Last();
                    var fileLength = new FileInfo(file.Path).Length;
                    string file_size = GetFileSize(fileLength);
                    txtIDImage.Text = file_size;

                    //var stream = file.GetStream();
                    //byte[] imgByte = ReadAllBytes(stream);
                    //_photo_base64 = Convert.ToBase64String(imgByte);
                    imageID.Scale = 1.0;
                    imageID.Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        byte[] imgByte = ReadAllBytes(stream);
                        _photo_base64 = Convert.ToBase64String(imgByte);
                        file.Dispose();
                        return stream;
                    });

                    System.Threading.Thread.Sleep(1000);

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
                        finally 
                        {
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

        private void SetControls(bool startUpload, int imageType, string fileName, bool success)
        {
            if (startUpload)
            {
                if (imageType == 1)
                {
                    imageSelfie.IsVisible = true;
                    txtSelfieImage.IsVisible = true;
                    txtSelfieImage.Text = AppResources.UploadingText;
                    //btnUploadSelfie.IsEnabled = false;
                }
                else
                {
                    imageID.IsVisible = true;
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

        public ICommand RemoveSelfie => new Command(async (item) => await ExecuteRemoveSelfie(item));
        private async Task ExecuteRemoveSelfie(object item)
        {
            //imageSelfie.IsVisible = false;
            imageSelfie.Source = "image_Selfie.png";
            imageSelfie.Scale = 1.4;
            btnRemoveSelfie.IsVisible = false;
            txtSelfieImage.IsVisible = false;
            txtSelfieImage.Text = "";
            btnUploadSelfie.IsEnabled = true;
        }

        public ICommand RemoveID => new Command(async (item) => await ExecuteRemoveID(item));
        private async Task ExecuteRemoveID(object item)
        {
            //imageID.IsVisible = false;
            imageID.Source = "image_IC.png";
            imageID.Scale = 1.4;
            btnRemoveID.IsVisible = false;
            txtIDImage.IsVisible = false;
            txtIDImage.Text = "";
            btnUploadID.IsEnabled = true;
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