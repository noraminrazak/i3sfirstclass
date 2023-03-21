using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using System;
using System.Collections.Generic;
using SmartSchoolsV2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Popup;
using Rg.Plugins.Popup.Extensions;
using System.Threading.Tasks;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WalletPage : ContentPage
	{
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _wallet_id;
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
        public int _account_status_id_sender;
        public int _kyc_status_id_sender;
        public string _kyc_status_sender;
        public string _mpay_uid_sender;

        public int _account_status_id_recipient;
        public int _kyc_status_id_recipient;
        public string _kyc_status_recipient;
        public string _mpay_uid_recipient;
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
        public WalletPage (int student_id, int profile_id, string full_name, string wallet_number, string account_balance, string photo_url, int school_id, string school_name, int school_type_id,
            int class_id, string class_name)
		{
			InitializeComponent ();
            BindingContext = this;

            var tapGestureRecognizerTopup = new TapGestureRecognizer();
            tapGestureRecognizerTopup.Tapped += (s, e) =>

            {
                //DisplayAlert(AppResources.SorryText, AppResources.EWalletFunctionText, "OK");
                TransferActionSheet();
            };
            imgTopup.GestureRecognizers.Add(tapGestureRecognizerTopup);

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
            TransactionHistory(_wallet_number);
            AccountStatusSender();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        public async void AccountStatusSender()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostAccountStatus(Settings.profileId, Settings.walletNumber);
                    string jsonStr = await t;
                    AccStatusProperty response = JsonConvert.DeserializeObject<AccStatusProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        foreach (AccStatus r in response.Data)
                        {
                            _mpay_uid_sender = r.mpay_uid;
                            _account_status_id_sender = Convert.ToInt32(r.account_status_id);
                            _kyc_status_id_sender = Convert.ToInt32(r.kyc_status_id);
                            _kyc_status_sender = r.kyc_status;
                        }

                        if (_kyc_status_id_sender == 11)
                        {
                            await AccountStatusRecipient(Settings.profileId);
                        }
                        else
                        {
                            await DisplayAlert(AppResources.KYCStatusText, _kyc_status_sender + "." + AppResources.PleaseContactSupportText, "OK");
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

        public async Task AccountStatusRecipient(int parent_profile_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    //ShowLoadingPopup();

                    var t = srvc.PostAccountStatus(_profile_id, _wallet_number);
                    string jsonStr = await t;
                    AccStatusProperty response = JsonConvert.DeserializeObject<AccStatusProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        foreach (AccStatus r in response.Data)
                        {
                            _mpay_uid_recipient = r.mpay_uid;
                            _account_status_id_recipient = Convert.ToInt32(r.account_status_id);
                            _kyc_status_id_recipient = Convert.ToInt32(r.kyc_status_id);
                            _kyc_status_recipient = r.kyc_status;
                        }

                        if (_mpay_uid_recipient == "0")
                        {
                            await UpdateVirtualBalance(parent_profile_id, _profile_id);
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
                    //HideLoadingPopup();
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task UpdateVirtualBalance(int parent_profile_id, int student_profile_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    //ShowLoadingPopup();
                    var t = srvc.PostAccountUpdateVirtualBalance(parent_profile_id, student_profile_id, Settings.fullName);
                    string jsonStr = await t;
                    ResponseProperty response = JsonConvert.DeserializeObject<ResponseProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await DisplayAlert(AppResources.DoneText, response.Message, "OK");
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
                    //HideLoadingPopup();
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
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
                    IsBusy1 = false;
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }

        }

        private async void TransferActionSheet()
        {
            //var actionSheet = await DisplayActionSheet("Transfer to?", "Cancel", null, "My eWallet", "Student eWallet");

            //switch (actionSheet)
            //{
            //    case "Cancel":

            //        // Do Something when 'Cancel' Button is pressed

            //        break;

            //    case "My eWallet":

            //        await Navigation.PushAsync(new TransferPage(_wallet_id, _wallet_number, Settings.walletId, Settings.walletNumber,Settings.fullName));

            //        break;

            //    case "Student eWallet":

                    await Navigation.PushAsync(new TransferPage(1,Settings.walletId, Settings.walletNumber,_wallet_id, _wallet_number,lblFullName.Text,_class_name, _photo_url));

            //        break;
            //}
        }

        public async void TransactionHistory(string wallet_number)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostWalletTransactionHistory(wallet_number);
                    string jsonStr = await t;
                    TransactionHistoryProperty response = JsonConvert.DeserializeObject<TransactionHistoryProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<TransactionHistory> list = new List<TransactionHistory>();
                        foreach (TransactionHistory sl in response.Data)
                        {
                            TransactionHistory post = new TransactionHistory();
                            post.transaction_id = sl.transaction_id;
                            post.transaction_type_id = sl.transaction_type_id;
                            post.transaction_type = sl.transaction_type;
                            post.reference_number = sl.reference_number;
                            post.wallet_number = sl.wallet_number;
                            post.wallet_number_reference = sl.wallet_number_reference;
                            post.full_name = sl.full_name;
                            post.full_name_reference = sl.full_name_reference;
                            if (sl.transaction_type_id == 1)
                            {
                                post.transaction_method = AppResources.TopupEWalletText;
                                post.transaction_type = AppResources.TopupModeText;
                                post.amount = "+RM " + sl.amount;
                                post.amount_color = Color.Green.ToHex();
                            }
                            else if (sl.transaction_type_id == 2)
                            {
                                post.transaction_method = AppResources.TransferToText + sl.full_name_reference;
                                post.transaction_type = AppResources.TransferToEWallet;
                                post.amount = "-RM " + sl.amount;
                                post.amount_color = Color.Red.ToHex();
                            }
                            else if (sl.transaction_type_id == 3)
                            {
                                post.transaction_method = AppResources.ReceivedFromText + sl.full_name_reference;
                                post.transaction_type = AppResources.ReceivedFromEWallet;
                                post.amount = "+RM " + sl.amount;
                                post.amount_color = Color.Green.ToHex();
                            }
                            else if (sl.transaction_type_id == 4)
                            {
                                post.transaction_method = AppResources.PaymentToText + sl.full_name_reference;
                                post.transaction_type = AppResources.PurchaseText;
                                post.amount = "-RM " + sl.amount;
                                post.amount_color = Color.Red.ToHex();
                            }
                            DateTime _ceate_at = Convert.ToDateTime(sl.create_at);
                            post.create_at = _ceate_at.ToString("dd/MM/yyyy, HH:mm tt");
                            post.create_month = _ceate_at.ToString("MMM yyyy");
                            list.Add(post);
                        }
                        RowCount = list.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvTxnHistory.Footer = l;
                        lvTxnHistory.ItemsSource = list;
                    }
                    else
                    {
                        List<TransactionHistory> list = new List<TransactionHistory>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvTxnHistory.Footer = l;
                        lvTxnHistory.ItemsSource = list;
                    }
                    //IsBusy = false;
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy = false;
                }
                //HideLoadingPopup();
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public TransactionHistory trans;
        async void OnTxnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as TransactionHistory;
            if (data == null) return;
            trans = data;

            if (trans.transaction_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...

                await Navigation.PushAsync(new TransactionDetailPage(data.transaction_id, data.transaction_type_id,
                    data.reference_number, data.wallet_number, data.full_name,data.wallet_number_reference,data.full_name_reference));

                ((ListView)sender).SelectedItem = null;
            }
        }

        async void OnDailyLimitClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new CardDailyLimitPage(_student_id,_profile_id,_full_name,_wallet_number,_account_balance,_photo_url,
                _school_id,_school_name,_school_type_id,_class_id,_class_name));
        }
        async void OnCardLostLimitClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new CardLostReportPage(_student_id, _profile_id, _full_name, _wallet_number, _account_balance, _photo_url,
                _school_id, _school_name, _school_type_id, _class_id, _class_name));
        }
        async void OnCardReplacementClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new CardReplacementPage(_student_id, _profile_id, _full_name, _wallet_number, _account_balance, _photo_url,
                _school_id, _school_name, _school_type_id, _class_id, _class_name));
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