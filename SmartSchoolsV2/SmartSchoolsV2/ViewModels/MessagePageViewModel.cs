using SmartSchoolsV2.Services;
using SmartSchoolsV2.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartSchoolsV2.ViewModels
{
    public class MessagePageViewModel : BaseViewModel
    {
        public IFirebaseAuth _firebaseAuth = DependencyService.Get<IFirebaseAuth>();

        public MessagePageViewModel()
        {
            var user = _firebaseAuth.GetUser();
            if (!String.IsNullOrEmpty(user.Name))
            {
                MoveToChat();
            }
        }


        async Task MoveToChat()
        {
            await Task.Delay(500);
            App.Current.MainPage = new MessageTabbedPage();
        }


        private string _login = "admin@aa.aa";

        public string Login
        {
            get { return _login; }
            set { _login = value; OnPropertyChanged(); }
        }

        private string _password = "123456";

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private string _resultText;

        public string ResultText
        {
            get { return _resultText; }
            set { _resultText = value; OnPropertyChanged(); }
        }



        public ICommand LoginByPassCommand => new Command(LoginByPassword);
        //public ICommand OpenFacebookCommand => new Command(OpenFacebook);

        //private void OpenFacebook()
        //{
        //    App.Current.MainPage.Navigation.PushModalAsync(new FacebookProfileCsPage());
        //}


        private async void LoginByPassword()
        {
            try
            {
                var result = await _firebaseAuth.LoginAsync(Login, Password);
                ResultText = result;
                if (result != "")
                    App.Current.MainPage = new MessageTabbedPage();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("---> " + ex.Message);
                ResultText = ex.Message;
            }

        }


    }
}
