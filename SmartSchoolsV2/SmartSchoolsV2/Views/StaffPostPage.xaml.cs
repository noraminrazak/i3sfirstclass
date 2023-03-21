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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffPostPage : ContentPage
    {
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        SchoolPost _data = new SchoolPost();
        public string requestUrl = Settings.requestUrl;
        public string _full_name;
        public string _mode;
        public string _file_name;
        public string _photo_base64;
        public int _post_id;
        public int _school_id;
        public int _class_id = 0;
        public int _club_id = 0;
        public int _post_group_id;
        public int _staff_id;
        public DateTime _date_from;
        public DateTime _date_to;
        public StaffPostPage(string mode, int school_id, int post_group_id, int group_id, SchoolPost data = null)
        {
            InitializeComponent();
            BindingContext = this;
            _mode = mode;
            _data = data;
            _post_id = data.post_id;
            _school_id = school_id;
            _post_group_id = post_group_id;
            if (_post_group_id == 1)
            {
                lblSchoolName.IsVisible = true;
                lblSchoolName.Text = Settings.schoolName;
            }
            else if (_post_group_id == 2)
            {
                lblGroupGrid.IsVisible = true;
                lblSchoolName2.Text = Settings.schoolName;
                lblGroupName.Text = Settings.selectedClassName;
                _class_id = group_id;
            }
            else if (_post_group_id == 3)
            {
                lblGroupGrid.IsVisible = true;
                lblSchoolName2.Text = Settings.schoolName;
                lblGroupName.Text = Settings.selectedClubName;
                _club_id = group_id;
            }

            _staff_id = Settings.staffId;
            _full_name = Settings.fullName;

            if (_mode == "C")
            {
                lblPost.Text = AppResources.NewBulletinText;

                txtDateFrom.Text = DateTime.Today.ToString("dd-MM-yyyy");
                _date_from = DateTime.Today;
            }
            else if (_mode == "U")
            {
                lblPost.Text = AppResources.EditBulletinText;
                txtMessage.Text = data.post_message;
                if (!string.IsNullOrEmpty(data.date_from))
                {
                    txtDateFrom.Text = Convert.ToDateTime(data.date_from).ToString("dd-MM-yyyy");
                    _date_from = Convert.ToDateTime(data.date_from);
                }

                if (!string.IsNullOrEmpty(data.date_to))
                {
                    txtDateTo.Text = Convert.ToDateTime(data.date_to).ToString("dd-MM-yyyy");
                    _date_to = Convert.ToDateTime(data.date_to);
                }

                if (!string.IsNullOrEmpty(data.post_photo_url))
                {
                    LoadPhotoUri(data.post_photo_url);
                }
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //imagePhoto.Source = new UriImageSource()
            //{
            //    CachingEnabled = false,
            //};
        }

        void LoadPhotoUri(string photo_url)
        {
            _file_name = photo_url.Split('/').Last();
            imgFrame.IsVisible = true;
            btnRemove.IsVisible = true;

            imagePhoto.Source = ImageSource.FromUri(new Uri(photo_url));

            ConvertImageToBase64(photo_url);
        }
        private void ConvertImageToBase64(string url)
        {
            using (var client = new WebClient())
            {
                var bytes = client.DownloadData(url);
                _photo_base64 = Convert.ToBase64String(bytes);
            }
        }
        async void OnPickPhotoClicked(object sender, EventArgs args)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("", AppResources.PermissionGalleryText, "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
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
            lblFileSize.IsVisible = true;
            lblFileSize.Text = file_size;

            imagePhoto.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                imgFrame.IsVisible = true;
                btnRemove.IsVisible = true;
                byte[] imgByte = ReadAllBytes(stream);
                _photo_base64 = Convert.ToBase64String(imgByte);
                file.Dispose();
                return stream;
            });
        }
        async void OnTakePhotoClicked(object sender, EventArgs args)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("", AppResources.NoCameraText, "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "I3S",
                AllowCropping = false,
                SaveToAlbum = true,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.Medium,
                MaxWidthHeight = 1000,
                DefaultCamera = CameraDevice.Rear
            });

            if (file == null)
                return;

            _file_name = file.Path.Split('/').Last();
            var fileLength = new FileInfo(file.Path).Length;
            string file_size = GetFileSize(fileLength);
            lblFileSize.IsVisible = true;
            lblFileSize.Text = file_size;
            //await DisplayAlert("File Location", file.Path, "OK");

            imagePhoto.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                imgFrame.IsVisible = true;
                btnRemove.IsVisible = true;
                byte[] imgByte = ReadAllBytes(stream);
                _photo_base64 = Convert.ToBase64String(imgByte);
                file.Dispose();
                return stream;
            });

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

        void StartCallFrom(object sender, EventArgs args)
        {
            datePickerFrom.Focus();
        }

        private void OnFromDateSelected(object sender, DateChangedEventArgs e)
        {
            txtDateFrom.Text = e.NewDate.ToString("dd-MM-yyyy");
            _date_from = e.NewDate;
        }
        void StartCallTo(object sender, EventArgs args)
        {
            datePickerTo.Focus();
        }

        private async void OnToDateSelected(object sender, DateChangedEventArgs e)
        {
            if (e.NewDate > DateTime.Now)
            {
                txtDateTo.Text = e.NewDate.ToString("dd-MM-yyyy");
                _date_to = e.NewDate;
            }
            else {
                await DisplayAlert("", AppResources.DisplayDateGreaterText, "OK");
            }
        }
        void OnClearClicked(object sender, EventArgs args)
        {
            txtMessage.Text = string.Empty;
            txtDateTo.Text = string.Empty;
            _date_to = DateTime.MinValue;
        }
        void OnSubmitClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
                if (!string.IsNullOrEmpty(txtDateTo.Text) || _date_to != DateTime.MinValue)
                {
                    if (_mode == "C")
                    {
                        StaffCreatePost();
                    }
                    else if(_mode == "U") 
                    {
                        StaffUpdatePost();
                    }
                }
                else
                {
                    SnackB.Message = AppResources.DisplayDateToRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                SnackB.Message = AppResources.MessageRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen; 
            }
        }
        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }

        static public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes  = System.Convert.FromBase64String(encodedData);

            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;
        }


        async void StaffCreatePost() 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffCreatePost(_school_id, _post_group_id, _class_id, _club_id, _staff_id, txtMessage.Text, 
                        _file_name, _photo_base64, _date_from, _date_to, _full_name);
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

        async void StaffUpdatePost()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffUpdatePost(_data.post_id, _school_id, _post_group_id, _class_id, _club_id, _staff_id, txtMessage.Text,
                        _file_name, _photo_base64, _date_from, _date_to, _full_name);
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

        async void OnRemoveClicked(object sender, EventArgs args)
        {
            if (imagePhoto.Source != null)
            {
                if (_mode == "U")
                {
                    await RemovePostPhoto();
                }
                else 
                {
                    imagePhoto.Source = null;
                    imgFrame.IsVisible = false;
                    btnRemove.IsVisible = false;
                    lblFileSize.IsVisible = false;
                    lblFileSize.Text = "";
                    _file_name = "";
                    _photo_base64 = "";
                }
            }
        }

        async void OnDeleteClicked(object sender, EventArgs args)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffRemovePost(_post_id, _school_id, _post_group_id, _class_id, _club_id, _staff_id, _full_name);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await Navigation.PopAsync();
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

        public async Task RemovePostPhoto()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffRemovePostPhoto(_post_id,_school_id, _post_group_id,_class_id,_club_id, _staff_id, _full_name);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");
                        imagePhoto.Source = null;
                        imgFrame.IsVisible = false;
                        btnRemove.IsVisible = false;
                        lblFileSize.IsVisible = false;
                        lblFileSize.Text = "";
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