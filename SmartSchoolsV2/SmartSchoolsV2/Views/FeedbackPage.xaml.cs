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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;

        public ObservableCollection<AttachmentFile> attachmentFile { get; set; }
        public class AttachmentFile
        {
            public string file_name { get; set; }
            public string photo_base64 { get; set; }
        }

        public int _problem_type_id;
        public int _ticket_id;
        public string _file_name;
        public string _photo_base64;
        public string _file_size;
        public bool _isLoading;

        public FeedbackPage()
        {
            InitializeComponent();
            BindingContext = this;
            attachmentFile = new ObservableCollection<AttachmentFile>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        async void OnSubmitClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtSubject.Text))
            {
                if (!string.IsNullOrEmpty(txtDescription.Text))
                {
                    if (conn.IsConnected() == true)
                    {
                        try
                        {
                            ShowLoadingPopup();

                            var t = srvc.PostReportCustomerFeedback(4, 11, 4, txtSubject.Text.Trim(), txtDescription.Text.Trim(), 1, Settings.fullName); //10=complaint
                            string jsonStr = await t;
                            CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                            if (response.Success == true)
                            {
                                _ticket_id = Convert.ToInt32(response.Code);
                                if (attachmentFile.Count > 0)
                                {
                                    foreach (AttachmentFile att in attachmentFile) 
                                    {
                                        await UploadAttachment(att);
                                    }
                                }
                                else
                                { 
                                    await DisplayAlert(AppResources.ThankYouText, response.Message, "OK");

                                    await Navigation.PopAsync();
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
                else
                {
                    SnackB.Message = AppResources.DescriptionRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else {
                SnackB.Message = AppResources.SubjectRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async Task UploadAttachment(AttachmentFile data)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostReportUploadAttachment(_ticket_id, data.file_name, data.photo_base64, Settings.fullName); //10=complaint
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {

                        await DisplayAlert(AppResources.ThankYouText, response.Message, "OK");

                        await Navigation.PopAsync();
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

                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void OnAddAttachmentClicked(object sender, EventArgs args)
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
            _file_size = file_size;

            var stream = file.GetStream();
            byte[] imgByte = ReadAllBytes(stream);
            _photo_base64 = Convert.ToBase64String(imgByte);

            bool valid = ValidateBase64EncodedString(_photo_base64);

            if (valid)
            {
                attachmentFile.Add(new AttachmentFile { file_name = _file_name, photo_base64 = _photo_base64});
                lvAttachment.ItemsSource = attachmentFile;
                if (attachmentFile.Count > 0)
                {
                    lvAttachment.HeightRequest = 30 + (attachmentFile.Count * 35);
                    lvAttachment.IsVisible = true;
                }
            }
        }

        void OnRemoveClicked(object sender, EventArgs e)
        {
            var value = sender as ImageButton;
            attachmentFile.Remove(value.CommandParameter as AttachmentFile);

            lvAttachment.ItemsSource = attachmentFile;
            if (attachmentFile.Count == 0)
            {
                lvAttachment.IsVisible = false;
            }
            else {
                lvAttachment.HeightRequest = 30 + (attachmentFile.Count * 35);
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

        LoadingPopupPage loadingPage = new LoadingPopupPage();
        async void ShowLoadingPopup()
        {

            await Navigation.PushPopupAsync(loadingPage);
            _isLoading = true;
        }
        async void HideLoadingPopup()
        {
            await Task.Delay(500);
            await Navigation.RemovePopupPageAsync(loadingPage);
            _isLoading = false;
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}