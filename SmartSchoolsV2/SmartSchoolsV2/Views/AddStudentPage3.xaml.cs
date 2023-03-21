using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStudentPage3 : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _student_id;
        public string _full_name;
        public string _photo_url;
        public int _school_id;
        public string _school_name;
        public int _class_id;
        public string _class_name;
        public string _nric;
        public static string _back;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }
        public AddStudentPage3(int student_id, string full_name, string photo_url, int school_id, string school_name, 
            int class_id, string class_name, string nric)
        {
            InitializeComponent();
            BindingContext = this;

            _student_id = student_id;
            _full_name = full_name;
            _photo_url = photo_url;
            _school_id = school_id;
            _school_name = school_name;
            _class_id = class_id;
            _class_name = class_name;
            _nric = nric;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!string.IsNullOrEmpty(_photo_url))
            {
                userImg.IsVisible = true;
                userImg.Source = _photo_url;
            }
            else
            {
                userInitial.IsVisible = true;
            }

            lblFullName.Text = _full_name;
            lblSchoolName.Text = _school_name;

            if (_class_id != 0) 
            {
                Settings.classId = _class_id;
                txtClass.Text = _class_name;
            }

            txtNum1.Focus();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        async void Num1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                await Task.Delay(50);
                txtNum2.Focus();
            }
            else
            {
                txtNum1.Focus();
            }
        }

        async void Num2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                await Task.Delay(50);
                txtNum3.Focus();
            }
            else
            {
                txtNum1.Focus();
            }
        }

        async void Num3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                await Task.Delay(50);
                txtNum4.Focus();
            }
            else
            {
                txtNum2.Focus();
            }
        }
        void Num4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                btnVerify.IsEnabled = true;
            }
            else
            {
                btnVerify.IsEnabled = false;
                txtNum3.Focus();
            }
        }

        public string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        async void OnVerifyClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtNum1.Text) && !string.IsNullOrEmpty(txtNum2.Text)
                && !string.IsNullOrEmpty(txtNum3.Text) && !string.IsNullOrEmpty(txtNum4.Text))
            {
                string nricFiltered = RemoveSpecialCharacters(_nric);
                string last4 = nricFiltered.Substring(nricFiltered.Length - 4);

                string enter4 = string.Concat(txtNum1.Text, txtNum2.Text, txtNum3.Text, txtNum4.Text);
                if (last4 == enter4)
                {
                    lbl4digit.IsVisible = false;
                    grid4digit.IsVisible = false;
                    btnVerify.IsVisible = false;
                    lblVerify.IsVisible = true;
                    imgTick2.IsVisible = true;
                    imgTick2.IsAnimationPlaying = true;
                    await Task.Delay(5000);
                    lblVerify.Text = string.Empty;
                    imgTick2.IsVisible = false;
                    imgTick2.IsAnimationPlaying = false;
                    lblClass.IsVisible = true;
                    txtClass.IsVisible = true;
                    btnAdd.IsVisible = true;

                }
                else
                {
                    SnackB.Message = AppResources.VerificationFailedText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                SnackB.Message = AppResources.Last4DigitRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void StartCall(object sender, EventArgs args)
        {
            var page = new SearchListPage2(AppResources.SelectClassNameText, "class", _school_id);
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }
        public void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
                txtClass.Text = Settings.className;
            }
        }

        async void OnAddClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtClass.Text))
            {
                if (conn.IsConnected() == true)
                {
                    try
                    {
                        ShowLoadingPopup();

                        var t = srvc.PostAccountAddVirtualBalance(Settings.parentId, _student_id, _school_id, Settings.classId, Settings.fullName);
                        string jsonStr = await t;
                        CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                        if (response.Success == true)
                        {
                            await DisplayAlert(AppResources.DoneText, response.Message,"OK");

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
            else {
                SnackB.Message = AppResources.ClassNameRequired;
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