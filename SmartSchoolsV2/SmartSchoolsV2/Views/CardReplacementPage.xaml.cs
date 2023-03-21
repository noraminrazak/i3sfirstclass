using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardReplacementPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _wallet_id;
        public int _card_id;
        public int _old_card_status_id;
        public int _card_status_id;
        public int _student_id;
        public int _profile_id;
        public int _school_id;
        public int _school_type_id;
        public int _class_id;
        public string _full_name;
        public string _photo_url;
        public string _school_name;
        public string _class_name;
        public string _wallet_number;
        public string _account_balance;
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
        public CardReplacementPage(int student_id, int profile_id, string full_name, string wallet_number, string account_balance, string photo_url, int school_id, string school_name, int school_type_id,
            int class_id, string class_name)
        {
            InitializeComponent();
            BindingContext = this;

            _student_id = student_id;
            _profile_id = profile_id;
            _full_name = full_name;
            _wallet_number = wallet_number;
            _account_balance = account_balance;
            _school_id = school_id;
            _class_id = class_id;
            _school_type_id = school_type_id;
            _photo_url = photo_url;
            _school_name = school_name;
            _class_name = class_name;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            StudentProfile(_profile_id);
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
                            _wallet_id = r.wallet_id;
                            _card_id = r.card_id;
                            txtCardNumber.Text = CardNumberFormat(r.card_number, 4, " ");
                            _card_status_id = r.card_status_id;
                            lblWalletNumber.Text = r.wallet_number;
                            lblAccountBalance.Text = "RM " + r.account_balance.ToString("F");
                            lblFullName.Text = r.full_name;
                            photo_url = r.photo_url;
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

                        if (_card_id == 0)
                        {
                            btnReplace.IsEnabled = false;
                        }
                    }
                    else
                    {
                        SnackB.Message = response.Message;
                        SnackB.IsOpen = !SnackB.IsOpen;
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
                    IsBusy = false;
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
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
            else {
                SnackB.Message = AppResources.CardNoRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }

        }

        async Task CardReplacement()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();
                    var t = srvc.PostCardReplacement(10, _school_id, _card_id, _old_card_status_id, txtNewCardNumber.Text.Replace(" ", string.Empty), Settings.fullName);
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
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}