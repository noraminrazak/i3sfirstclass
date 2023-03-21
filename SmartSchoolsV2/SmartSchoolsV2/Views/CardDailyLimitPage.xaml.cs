using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardDailyLimitPage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _card_id;
        public int _card_status_id;
        public int _wallet_id;
        public int _student_id;
        public int _profile_id;
        public int _school_id;
        public int _school_type_id;
        public int _class_id;
        public string _card_number;
        public string _full_name;
        public string _photo_url;
        public string _school_name;
        public string _class_name;
        public string _wallet_number;
        public string _account_balance;
        public string _amount;
        bool isBusy;
        public bool IsBusy1
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        string _daily_limit;
        public string daily_limit
        {
            get
            {
                return _daily_limit;
            }
            set
            {
                if (_daily_limit != value)
                {
                    _daily_limit = value;
                    OnPropertyChanged("daily_limit");
                }
            }
        }
        public CardDailyLimitPage(int student_id, int profile_id, string full_name, string wallet_number, string account_balance, string photo_url, int school_id, string school_name, int school_type_id,
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
                    IsBusy1 = true;
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
                            _card_status_id = r.card_status_id;
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

                        if (_card_id > 0)
                        {
                            await CardDailyLimit(_card_id);
                        }
                        else 
                        {
                            daily_limit = AppResources.CardNotAssignedText;
                            btnSet.IsEnabled = false;
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
                    IsBusy1 = false;
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task CardDailyLimit(int card_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostCardDailyLimit(card_id);
                    string jsonStr = await t;
                    CardProperty response = JsonConvert.DeserializeObject<CardProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        string photo_url = string.Empty;
                        if (response.Data.Count > 0)
                        {
                            foreach (Card r in response.Data)
                            {
                                daily_limit = "RM " + r.daily_limit.ToString();
                            }
                        }
                        else 
                        {
                            daily_limit = AppResources.NoLimitText;
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
                    HideLoadingPopup();
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task CardUpdateDailyLimit(int card_id, int school_id, decimal limit,string update_by)
        {
            if (!string.IsNullOrEmpty(txtAmount.Text))
            {
                if (conn.IsConnected() == true)
                {
                    try
                    {
                        ShowLoadingPopup();
                        var t = srvc.PostCardUpdateDailyLimit(card_id, school_id, limit, update_by);
                        string jsonStr = await t;
                        CardProperty response = JsonConvert.DeserializeObject<CardProperty>(jsonStr);
                        if (response.Success == true)
                        {
                            if (limit > 0)
                            {
                                daily_limit = "RM " + limit.ToString("F");
                            }
                            else
                            {
                                daily_limit = AppResources.NoLimitText;
                            }

                            await DisplayAlert(AppResources.DoneText, response.Message, "OK");
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
                SnackB.Message = AppResources.CardDailyLimitRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }

        }

        void OnAmountTextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtAmount.Text, "[^0123456789.]"))
            {
                SnackB.Message = AppResources.PleaseEnterNoOnlytext;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
            else
            {
                _amount = e.NewTextValue;
            }
        }

        void On5Clicked(object sender, EventArgs args)
        {
            _amount = "5";
            txtAmount.Text = _amount;
        }

        void On10Clicked(object sender, EventArgs args)
        {
            _amount = "10";
            txtAmount.Text = _amount;
        }

        void On15Clicked(object sender, EventArgs args)
        {
            _amount = "15";
            txtAmount.Text = _amount;
        }

        void On20Clicked(object sender, EventArgs args)
        {
            _amount = "20";
            txtAmount.Text = _amount;
        }

        void On30Clicked(object sender, EventArgs args)
        {
            _amount = "30";
            txtAmount.Text = _amount;
        }

        void On50Clicked(object sender, EventArgs args)
        {
            _amount = "50";
            txtAmount.Text = _amount;
        }

        async void OnSetClicked(object sender, EventArgs args)
        {
            decimal amount = Convert.ToDecimal(txtAmount.Text);
            if (amount >= 0)
            {
                if (amount <= 50)
                {
                    await CardUpdateDailyLimit(_card_id, _school_id, Convert.ToDecimal(txtAmount.Text), Settings.fullName);
                }
                else
                {
                    SnackB.Message = AppResources.EnteredAmountMoreThanText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else
            {
                SnackB.Message = AppResources.EnteredAmountCannotLessText;
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