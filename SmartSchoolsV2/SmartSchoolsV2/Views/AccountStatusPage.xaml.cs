using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
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
    public partial class AccountStatusPage : ContentPage
    {
        public int _account_status_id;
        public int _kyc_status_id;
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public static string _back;

        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }
        public void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
                GetLoginPage();
            }
        }
        public AccountStatusPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            AccountStatus();
        }

        public async void AccountStatus()
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
                            //lblUid.Text = r.mpay_uid;
                            _account_status_id = Convert.ToInt32(r.account_status_id);
                            lblAccStatus.Text = r.account_status;
                            _kyc_status_id = Convert.ToInt32(r.kyc_status_id);
                            lblKYCStatus.Text = r.kyc_status;
                        }

                        if (_kyc_status_id == 0 || _kyc_status_id == 10)
                        {
                            btnContinue.IsVisible = true;

                            if (_kyc_status_id == 0)
                            {
                                btnContinue.Text = AppResources.NewCustVerifyText;
                                lblMessage.IsVisible = true;
                                txtMessage.Text = AppResources.DocumentVerifyMessageText;
                            }
                            else if (_kyc_status_id == 10)
                            {
                                btnContinue.Text = AppResources.ResubmitDocumentText;
                            }
                        } else if (_kyc_status_id == 11) 
                        { 
                            btnDelete.IsVisible = true;
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

        async void OnDeleteClicked(object sender, EventArgs args)
        {
            var page = new DeleteAccountPopupPage(Settings.userRoleId);
            page.DetailSet += this.OnDetailSet;
            await PopupNavigation.Instance.PushAsync(page);
        }

        async void OnContinueClicked(object sender, EventArgs args)
        {
            if (_kyc_status_id == 0) 
            {
                await Navigation.PushAsync(new NewKYCPage(_kyc_status_id));
            } 
            else if (_kyc_status_id == 10) 
            {
                await Navigation.PushAsync(new ResubmitKYCPage(_kyc_status_id));
            }

        }

        public static Page GetLoginPage()
        {
            Application.Current.MainPage = new NavigationPage(new LoginPage1())
            {
                BarBackgroundColor = Color.FromHex("#5E625A"),
                BarTextColor = Color.FromHex("#FFD612")
            };
            return Application.Current.MainPage;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        readonly LoadingPopupPage loadingPage = new LoadingPopupPage();
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