using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartSchoolsV2.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        #region Init

        public readonly IFirebaseDatabase _firebaseDatabase;
        public readonly IFirebaseStorage _firebaseStorage;
        public readonly IChatService _chatService;
        public readonly IFirebaseAuth _firebaseAuth;

        string nodePath = Settings.nodePath;

        public ChatViewModel(IChatService chatService)
        {
            _chatService = chatService;
            _firebaseAuth = DependencyService.Get<IFirebaseAuth>();
            _firebaseStorage = DependencyService.Get<IFirebaseStorage>();
            _firebaseDatabase = DependencyService.Get<IFirebaseDatabase>();

            //FakeData();
            _chatService.NewMessageReceived += ChatVM_NewMessageReceived;

            GetUser().ContinueWith(x => GetDataFromFirebase());
        }
        private void GetDataFromFirebase()
        {
            Action<Dictionary<string, ChatMessage>> onValueEvent = (Dictionary<string, ChatMessage> messages) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("---> EVENT GetDataFromFirebase ");
                    Action onSetValueSuccess = () =>
                    {

                    };

                    Action<string> onSetValueError = (string errorDesc) =>
                    {

                    };

                    if (messages == null)
                    {

                    }
                    else
                    {
                        if (messages.Count != 0 && ChatMessages.Count != messages.Count)
                        {
                            foreach (var message in messages.OrderBy(m => m.Value.SentAt))
                            {
                                message.Value.IsYourMessage = Settings.profileId == message.Value.SenderId;

                                if (ChatMessages.All(m => m.MessageId != message.Value.MessageId))
                                {
                                    ChatMessages.Add(message.Value);
                                    System.Diagnostics.Debug.WriteLine("---> add new -> " + message.Value.MessageId);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("---> error GetDataFromFirebase " + ex.Message);
                    throw;
                }
                finally 
                {
                    MessagingCenter.Send<ChatViewModel>(this, "ScrollToEnd");
                }
            };

            _firebaseDatabase.AddValueEvent(nodePath, onValueEvent);
        }


        private async Task GetUser()
        {
            await Task.Delay(1000);

            Device.BeginInvokeOnMainThread(() => {
                UserCurent = _firebaseAuth.GetUser();
                OnPropertyChanged("UserCurent");
            });
        }

        //private void FakeData()
        //{
        //    ChatMessages.Add(new ChatMessage()
        //    {
        //        IsYourMessage = true,
        //        //Text = "some mesage  asdasdasd asdklj lj jiopjop jopj opjo pj ioph ioh uiohuio hioio",
        //        FullName = "Admin",
        //        AttachImg = "alice"
        //    });

        //    for (int i = 0; i < 5; i++)
        //    {
        //        ChatMessages.Add(new ChatMessage()
        //        {
        //            IsYourMessage = (i % 2 == 0),
        //            Text = "some mesage " + i,
        //            FullName = (i % 2 == 0) ? "Admin" : "some friend"
        //        });
        //    }
        //}

        #endregion


        //public ICommand AttachFileCommand => new Command(AttachFile);
        public ICommand LogoutCommand => new Command(Logout);
        //public ICommand AddMessageCommand => new Command(AddMessage);

        #region Properties

        public ObservableCollection<ChatMessage> ChatMessages { get; set; } = new ObservableCollection<ChatMessage>();


        private UserModel _user = new UserModel();
        public UserModel UserCurent
        {
            get { return _user; }
            set
            {
                _user = value; OnPropertyChanged();
            }
        }

        private string _newMessageText;
        public string NewMessageText
        {
            get { return _newMessageText; }
            set { _newMessageText = value; OnPropertyChanged(); }
        }

        public object ConnectCommand { get; internal set; }

        #endregion


        #region Commands

        private void Logout()
        {
            _firebaseAuth.Logout();
            App.Current.MainPage = new MainPage();
        }
        //private async void AttachFile()
        //{
        //    IsBusy = true;

        //    var file = await _firebaseStorage.UploadFiles();
        //    var url = await _firebaseStorage.GetFileUrl(file);

        //    SendMessage(url);
        //    IsBusy = false;
        //}

        private async void AddMessage()
        {
            TimeSpan start = new TimeSpan(07, 0, 0);
            TimeSpan end = new TimeSpan(19, 0, 0);
            TimeSpan now = DateTime.Now.TimeOfDay;

            if (now >= start && now <= end)
            {
                IsBusy = true;

                if (!String.IsNullOrEmpty(NewMessageText))
                {
                    //SendMessage();
                }

                IsBusy = false;
            }
            else {

                await App.Current.MainPage.DisplayAlert(AppResources.SorryText, AppResources.CannotSendMessageNowText, "OK");
            }
        }
        public string requestUrl = Settings.requestUrl;
        public string _photoUrl;
        //private async void SendMessage(string attach = "")
        //{
            //string channel_id = string.Empty;
            //string channel_name = string.Empty;
            //int messageTo = 0;
            //if (!string.IsNullOrEmpty(Settings.photoUrl))
            //{
            //    _photoUrl = requestUrl + Settings.photoUrl;
            //}
            //else
            //{
            //    _photoUrl = "";
            //}

            //int channelTypeId = Settings.channelTypeId;

            //if (channelTypeId == 1)
            //{
            //    channel_name = Settings.channelTopName;
            //    channel_id = Settings.nodePath.Replace("chats", "").Replace("/", "");
            //    messageTo = Settings.firstId;
            //}
            //else if (channelTypeId == 2)
            //{
            //    channel_name = Settings.channelTopName;
            //    channel_id = "sid" + Settings.firstId;
            //    messageTo = Settings.firstId;
            //}
            //else if (channelTypeId == 3)
            //{
            //    channel_name = Settings.channelBtmName;
            //    channel_id = "sid" + Settings.firstId + "csid" + Settings.secondId;
            //    messageTo = Settings.secondId;
            //}
            //else if (channelTypeId == 4)
            //{
            //    channel_name = Settings.channelBtmName;
            //    channel_id = "sid" + Settings.firstId + "cbid" + Settings.secondId;
            //    messageTo = Settings.secondId;
            //}

            //var message = new ChatMessage()
            //{
            //    PushType = "chats",
            //    MessageFrom = Settings.profileId,
            //    MessageTo = messageTo,
            //    IsYourMessage = true,
            //    Text = NewMessageText,
            //    TextTime = DateTime.Now,
            //    AttachImg = attach,
            //    ChannelId = channel_id,
            //    ChannelName = channel_name,
            //    ChannelTypeId = Settings.channelTypeId,
            //    ChannelUrlPhoto = Settings.channelPhotoUrl,
            //    FirstId = Settings.firstId,
            //    FirstName = Settings.channelTopName,
            //    SecondId = Settings.secondId,
            //    SecondName = Settings.channelBtmName,
            //    NodePath = Settings.nodePath,
            //    FullName = Settings.fullName,
            //    UrlPhoto = _photoUrl
            //};

            //ChatMessages.Add(message);

            //MessagingCenter.Send<ChatViewModel>(this, "ScrollToEnd");
            //_chatService.SendMessage(message);

            //_firebaseDatabase.SetValue(nodePath + "/" + message.Id, message);

            //NewMessageText = "";
        //}

        #endregion


        #region Events

        private void ChatVM_NewMessageReceived(object sender, System.EventArgs e)
        {
            var body = e as BodyEventArgs;

            //if (UserCurent.Name != body.Message.UserName)
            //{
                // todo hide - because database has listner
                //ChatMessages.Add(body.Message);

                MessagingCenter.Send<ChatViewModel>(this, "ScrollToEnd");
            //}
        }

        #endregion

    }
}
