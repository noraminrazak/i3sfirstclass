using Firebase.Database;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MessageView : ContentView
	{
        FirebaseClient fcm = new FirebaseClient("https://i-3s-512d2.firebaseio.com/");
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<ChatHistory> listChannel { get; set; }
        public static Command LoadChatChannel { get; set; }
        ViewCell lastCell;
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
        public MessageView ()
		{
			InitializeComponent ();
            this.BindingContext = this;

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                Navigation.PushAsync(new MessageTabbedPage());
            };
            btnMessage.GestureRecognizers.Add(tapGestureRecognizer);

            LoginByPassword();

            LoadChatChannel = new Command(async () => await LoadChatHistory());
        }

        public async Task LoadChatHistory()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvChannel.ItemsSource = null;
                    var t = srvc.PostChatHistory(Settings.profileId);
                    string jsonStr = await t;
                    ChatHistoryProperty response = JsonConvert.DeserializeObject<ChatHistoryProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listChannel = new ObservableCollection<ChatHistory>();
                        foreach (ChatHistory sl in response.Data)
                        {
                            ChatHistory post = new ChatHistory();
                            post.channel_id = sl.channel_id;
                            post.channel_type_id = sl.channel_type_id;
                            post.channel_type = sl.channel_type;
                            post.channel_name_full = sl.channel_name;
                            if (sl.channel_type_id == 1)
                            {
                                if (sl.profile_id == sl.sender_id)
                                {
                                    post.channel_name = sl.receiver_name;
                                    if (!string.IsNullOrEmpty(sl.receiver_photo_url))
                                    {
                                        post.channel_photo_url = requestUrl + sl.receiver_photo_url;
                                        post.image_visible = true;
                                        post.initial_visible = false;
                                    }
                                    else
                                    {
                                        post.image_visible = false;
                                        post.initial_visible = true;
                                    }
                                }
                                else 
                                {
                                    post.channel_name = sl.sender_name;
                                    if (!string.IsNullOrEmpty(sl.sender_photo_url))
                                    {
                                        post.channel_photo_url = requestUrl + sl.sender_photo_url;
                                        post.image_visible = true;
                                        post.initial_visible = false;
                                    }
                                    else
                                    {
                                        post.image_visible = false;
                                        post.initial_visible = true;
                                    }
                                }
                            }
                            else 
                            {
                                post.channel_name = sl.channel_name.Split("_")[0];
                                post.image_visible = false;
                                post.initial_visible = true;
                            }
                            post.sender_id = sl.sender_id;
                            post.sender_name = sl.sender_name;
                            post.sender_photo_url = sl.sender_photo_url;
                            post.receiver_id = sl.receiver_id;
                            post.receiver_name = sl.receiver_name;
                            post.receiver_photo_url = sl.receiver_photo_url;
                            if (sl.sent_at.Date == DateTime.Today)
                            {
                                post.time_message = sl.sent_at.ToString("hh:mm tt");
                            }
                            else if (sl.sent_at.Date == DateTime.Today.AddDays(-1))
                            {
                                post.time_message = AppResources.YesterdayText;
                            }
                            else 
                            {
                                post.time_message = sl.sent_at.ToString("dd/MM/yyyy");
                            }
                            post.last_message = sl.last_message;
                            post.unread_count = sl.unread_count;
                            if (sl.unread_count > 0)
                            {
                                post.count_visible = true;
                            }
                            else 
                            {
                                post.count_visible = false;
                            }
                            listChannel.Add(post);
                        }
                        RowCount = listChannel.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvChannel.Footer = l;
                        lvChannel.ItemsSource = listChannel;
                    }
                    else
                    {
                        listChannel = new ObservableCollection<ChatHistory>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvChannel.Footer = l;
                        lvChannel.ItemsSource = listChannel;
                    }
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
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public IFirebaseAuth _firebaseAuth = DependencyService.Get<IFirebaseAuth>();
        private async void LoginByPassword()
        {
            try
            {
                //ShowLoadingPopup();

                var result = await _firebaseAuth.LoginAsync("software@emerging.com.my", "software@123");
                //ResultText = result;
                //if (result != "")
                    //await Navigation.PushAsync(new MessageTabbedPage());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("---> " + ex.Message);
                //ResultText = ex.Message;
            }
            finally
            {
                //HideLoadingPopup();
            }
        }

        private async void GetUser()
        {
            var user = _firebaseAuth.GetUser();
            if (!String.IsNullOrEmpty(user.Name))
            {
                await Navigation.PushAsync(new MessageTabbedPage());
            }
        }

        public ChatHistory channel = new ChatHistory();
        async void OnChannelSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as ChatHistory;
            if (data == null) return;
            channel = data;

            if (!string.IsNullOrEmpty(channel.channel_id))
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                if (data.channel_type_id == 1)
                {
                    await Navigation.PushAsync(new MessagePage(data.channel_type_id, data.sender_id, data.receiver_id, data.channel_name, data.channel_photo_url, data.channel_id));
                }
                else {
                    await Navigation.PushAsync(new MessagePage(data.channel_type_id, data.sender_id, data.receiver_id, data.channel_name_full, data.channel_photo_url, data.channel_id));
                }
                ((ListView)sender).SelectedItem = null;

            }
        }
        async void Delete_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            ChatHistory data = item.BindingContext as ChatHistory;

            //bool answer = await App.Current.MainPage.DisplayAlert(data.channel_name, AppResources.DoYouReallyWantToJoinChannelText, AppResources.YesText, AppResources.CancelText);
            //if (answer == true)
            //{
                if (conn.IsConnected() == true)
                {
                    try
                    {
                        var t = srvc.PostChatLeaveChannel(data.channel_id, Settings.profileId, Settings.fullName);
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
                        await LoadChatHistory();
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() => {
                        SnackB.Message = AppResources.CheckInternetText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    });
                }
            //}
        }

        //LoadingPopupPage loadingPage = new LoadingPopupPage();
        //async void ShowLoadingPopup()
        //{
        //    await Navigation.PushPopupAsync(loadingPage);
        //}
        //async void HideLoadingPopup()
        //{
        //    await Task.Delay(500);
        //    await Navigation.RemovePopupPageAsync(loadingPage);
        //}
    }
}