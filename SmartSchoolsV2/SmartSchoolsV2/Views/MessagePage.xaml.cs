using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagePage : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public string _channel_id;
        public int _channel_type_id;
        public int _receiver_id;
        public string _channel_name;
        public int _pid = Settings.profileId;
        public object viewModel;
        public MessagePage(int channel_type_id, int sender_id, int receiver_id, string channel_name,string channel_photo_url = "", string channel_id = "")
        {
            _channel_type_id = channel_type_id;
            _channel_id = channel_id;
            Settings.nodePath = "chats/" + channel_id;
            if (channel_type_id == 1)
            {

                if (sender_id == _pid)
                {
                    _pid = sender_id;
                    _receiver_id = receiver_id;
                }
                else {
                    _pid = receiver_id;
                    _receiver_id = sender_id;
                }

                if (_pid > _receiver_id)
                {
                    _channel_name = channel_name + "_" + Settings.fullName;
                }
                else 
                {
                    _channel_name = Settings.fullName + "_" + channel_name;
                }
            }
            else {
                
                _channel_name = channel_name;
                _receiver_id = receiver_id;
            }

            viewModel = ViewModelLocator.Instance.Resolve(typeof(ChatViewModel));
            this.BindingContext = viewModel;

            InitializeComponent();

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                AddMessage();
            };
            btnSend.GestureRecognizers.Add(tapGestureRecognizer);

            if (!string.IsNullOrEmpty(channel_photo_url) && !string.IsNullOrWhiteSpace(channel_photo_url))
            {
                userInitial.IsVisible = false;
                userImg.IsVisible = true;
                userImg.Source = channel_photo_url;
            }
            else
            {
                userInitial.IsVisible = true;
            }

            if (channel_type_id == 1)
            {
                lblTopTitle.Text = channel_name;
                lblBottomTitle.Text = string.Empty;
                lblBottomTitle.IsVisible = false;
            }
            else if (channel_type_id == 2) 
            {
                lblTopTitle.Margin = new Thickness(0,5,0,0);
                lblTopTitle.Text = channel_name.Split("_")[0];
                lblBottomTitle.Text = channel_name.Split("_")[1];
                lblBottomTitle.IsVisible = true;
            }
            else if (channel_type_id == 3)
            {
                lblTopTitle.Margin = new Thickness(0, 5, 0, 0);
                lblTopTitle.Text = channel_name.Split("_")[0];
                lblBottomTitle.Text = channel_name.Split("_")[1];
                lblBottomTitle.IsVisible = true;
            }
            else if (channel_type_id == 4)
            {
                lblTopTitle.Margin = new Thickness(0, 5, 0, 0);
                lblTopTitle.Text = channel_name.Split("_")[0];
                lblBottomTitle.Text = channel_name.Split("_")[1];
                lblBottomTitle.IsVisible = true;
            }

            Subscribe();

#if !DEBUG
            if (Device.OS == TargetPlatform.iOS)
            {
                EntryChat.Focused += EntryChat_Focused;
                EntryChat.Unfocused += EntryChat_Completed;
            }
#endif
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateUserStatus(2); //online
        }

        private async void AddMessage()
        {
            TimeSpan start = new TimeSpan(07, 0, 0);
            TimeSpan end = new TimeSpan(19, 0, 0);
            TimeSpan now = DateTime.Now.TimeOfDay;

            if (now >= start && now <= end)
            {
                IsBusy = true;

                if (!String.IsNullOrEmpty(EntryChat.Text))
                {
                    SendMessage();
                }

                IsBusy = false;
            }
            else
            {

                await App.Current.MainPage.DisplayAlert(AppResources.SorryText, AppResources.CannotSendMessageNowText, "OK");
            }
        }

        async void SendMessage() 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostChatSend(_channel_id, _channel_type_id, _channel_name , _pid, _receiver_id, 1, EntryChat.Text, 
                        string.Empty, string.Empty, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        //do onthing
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() => {
                            SnackB.Message = response.Message;
                            SnackB.IsOpen = !SnackB.IsOpen;
                        });

                    }
                }
                catch (Exception)
                {
                    Device.BeginInvokeOnMainThread(() => {
                        SnackB.Message = AppResources.SomethingWrongText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    });

                }
                finally
                {
                    MessagingCenter.Send<ChatViewModel>((ChatViewModel)viewModel, "ScrollToEnd");
                    EntryChat.Text = "";
                }
            }
            else
            {
                Device.BeginInvokeOnMainThread(() => {
                    SnackB.Message = AppResources.CheckInternetText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                });
            }
        }

        public async void UpdateUserStatus(int status_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostChatUserStatus(_channel_id, Settings.profileId, status_id, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        //do onthing
                    }
                    else
                    {
                        //SnackB.Message = response.Message;
                        //SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {

                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            UpdateUserStatus(1); //offline
            GC.Collect();
        }

        private void EntryChat_Completed(object sender, System.EventArgs e)
        {
            //InputGrid.TranslateTo(0, 0, 250, Easing.Linear);
        }

        private void EntryChat_Focused(object sender, FocusEventArgs e)
        {
            //InputGrid.TranslateTo(0, -300, 250, Easing.Linear);
            //btnAttach.IsVisible = false;

        }
        private void Subscribe()
        {
            MessagingCenter.Subscribe<ChatViewModel>(this, "ScrollToEnd", (sender) =>
            {
                ScrollToEnd();
            });

        }
        private void ScrollToEnd()
        {
            var v = ChatList.ItemsSource.Cast<object>().LastOrDefault();
            ChatList.ScrollTo(v, ScrollToPosition.End, true);
        }
    }
}