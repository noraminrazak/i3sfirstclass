using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationPage : ContentPage
	{
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<UserNotify> listNotify { get; set; }
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
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
        public NotificationPage ()
		{
            InitializeComponent();
            BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await UserNotify();
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await UserNotify();
                });
            }
        }
        public async Task UserNotify()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvNotify.ItemsSource = null;
                    var t = srvc.PostUserNotify(Settings.profileId);
                    string jsonStr = await t;
                    UserNotifyProperty response = JsonConvert.DeserializeObject<UserNotifyProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listNotify = new ObservableCollection<UserNotify>();
                        if (response.Data.Count > 0)
                        {
                            listNotify = new ObservableCollection<UserNotify>();
                            foreach (UserNotify sl in response.Data)
                            {
                                UserNotify post = new UserNotify();
                                post.notify_id = sl.notify_id;
                                post.notify_subject = sl.notify_subject;
                                post.notify_message_full = sl.notify_message;
                                if (sl.notify_message.Length > 90)
                                {
                                    post.notify_message = sl.notify_message.Substring(0,89) + "...";
                                }
                                else {
                                    post.notify_message = sl.notify_message;
                                }

                                post.notify_photo_url = sl.notify_photo_url;
                                post.notify_link_text = sl.notify_link_text;
                                post.notify_link = sl.notify_link;
                                post.notify_link_text = sl.notify_link_text;
                                post.notify_link_param = sl.notify_link_param;
                                post.read_flag = sl.read_flag;
                                if (sl.read_flag == "N") 
                                {
                                    post.message_color = "#000000";
                                    post.subject_color = "#0080ff";
                                }
                                else {
                                    post.message_color = "#979797";
                                    post.subject_color = "#000000";
                                }
                                post.create_at = Convert.ToDateTime(sl.create_at).ToString("dd MMM yyyy", ci);
                                listNotify.Add(post);
                            }
                            RowCount = listNotify.Count;
                            l.HorizontalTextAlignment = TextAlignment.Center;
                            if (RowCount > 0)
                            {
                                l.Text = RowCount + AppResources.RecordText;
                            }
                            else
                            {
                                l.Text = AppResources.NoRecordFoundText;
                            }
                            lvNotify.Footer = l;
                            lvNotify.ItemsSource = listNotify;
                        }
                        else
                        {
                            listNotify = new ObservableCollection<UserNotify>();
                            l.HorizontalTextAlignment = TextAlignment.Center;
                            l.Text = AppResources.NoRecordFoundText;
                            lvNotify.Footer = l;
                            lvNotify.ItemsSource = listNotify;
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
                    IsBusy = false;
                }
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public UserNotify uno;
        async void OnNotifySelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as UserNotify;
            if (data == null) return;
            uno = data;

            if (uno.notify_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...

                await Navigation.PushAsync(new NotificationPage2(uno));
                ((ListView)sender).SelectedItem = null;
            }
        }

        async void Delete_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            UserNotify data = item.BindingContext as UserNotify;

            //bool answer = await App.Current.MainPage.DisplayAlert(AppResources.ConfirmDeleteText, AppResources.DoYouReallyWantToDeleteText + product.category_name + "?", AppResources.YesText, AppResources.CancelText);
            //if (answer)
            //{
                await DeleteNotify(data.notify_id);
            //}
        }

        public async Task DeleteNotify(int notify_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostUserDeleteNotify(notify_id, Settings.profileId, Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await UserNotify();
                    }
                    else
                    {
                        SnackB.Message = AppResources.SomethingWrongText;
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
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
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