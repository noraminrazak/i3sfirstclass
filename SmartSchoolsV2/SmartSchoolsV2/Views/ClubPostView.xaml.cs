using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SmartSchoolsV2.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClubPostView : ContentView, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public static Command LoadSchoolClubPost { get; set; }
        private ObservableCollection<SchoolPost> listPost { get; set; }
        SchoolPost data = new SchoolPost();
        public int _user_role_id;
        public int _staff_id;
        public int _create_by_staff_id;
        public int _school_id;
        public int _club_id;
        public int RowCount { get; set; }
        public static Label l = new Label();
        private bool isBusy8;
        public bool IsBusy8
        {
            get => isBusy8;
            set
            {
                isBusy8 = value;
                OnPropertyChanged();
            }
        }

        private bool isSwipe;
        public bool IsSwipe
        {
            get
            {
                return isSwipe;
            }
            set
            {
                if (isSwipe != value)
                {
                    isSwipe = value;
                    OnPropertyChanged();
                }
            }
        }
        public ClubPostView ()
		{
			InitializeComponent ();
            this.BindingContext = this;

            _school_id = Settings.selectedSchoolId;
            _club_id = Settings.selectedClubId;
            _staff_id = Settings.staffId;
            _create_by_staff_id = Settings.createByStaffId;
            _user_role_id = Settings.userRoleId;
            if (_user_role_id == 8)
            {
                if (_staff_id == _create_by_staff_id)
                {
                    IsSwipe = true;
                    btnAddPost.IsVisible = true;
                }
                else {
                    IsSwipe = false;
                    btnAddPost.IsVisible = false;
                }
            }

            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) => {
                Navigation.PushAsync(new StaffPostPage("C",_school_id, 3, _club_id, data));
            };
            btnAddPost.GestureRecognizers.Add(tapGestureRecognizerTxn);

            LoadSchoolClubPost = new Command(async () => await SchoolClubPost(_school_id, _club_id));
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await SchoolClubPost(_school_id, _club_id);
                });
            }
        }

        public async Task SchoolClubPost(int school_id, int club_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy8 = true;
                    var t = srvc.PostSchoolClubPost(school_id, club_id);
                    string jsonStr = await t;
                    SchoolPostProperty response = JsonConvert.DeserializeObject<SchoolPostProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listPost = new ObservableCollection<SchoolPost>();
                        foreach (SchoolPost sl in response.Data)
                        {
                            SchoolPost post = new SchoolPost();
                            post.post_id = sl.post_id;
                            post.school_name = sl.school_name;
                            post.school_type = sl.school_type;
                            post.post_message = sl.post_message;
                            post.date_from = sl.date_from;
                            post.date_to = sl.date_to;
                            if (!string.IsNullOrEmpty(sl.post_photo_url))
                            {
                                post.post_photo_url = requestUrl + sl.post_photo_url;
                                post.post_photo_visible = true;
                            }
                            else
                            {
                                post.post_photo_visible = false;
                            }
                            post.create_by = sl.create_by;
                            DateTime _ceate_at = Convert.ToDateTime(sl.create_at);
                            post.create_at = _ceate_at.ToString("dd MMM yyyy");
                            if (!string.IsNullOrEmpty(sl.create_by_photo_url))
                            {
                                post.create_by_photo_url = requestUrl + sl.create_by_photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.image_visible = false;
                                post.initial_visible = true;
                            }
                            if (sl.create_by == Settings.fullName)
                            {
                                post.edit_visible = true;
                            }
                            else
                            {
                                post.edit_visible = false;
                            }
                            listPost.Add(post);
                        }
                        RowCount = listPost.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvPost.Footer = l;
                        lvPost.ItemsSource = listPost;
                    }
                    else
                    {
                        listPost = new ObservableCollection<SchoolPost>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvPost.Footer = l;
                        lvPost.ItemsSource = listPost;
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
                    IsBusy8 = false;
                }
                //HideLoadingPopup();
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void OnEditClicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            SchoolPost post = button.CommandParameter as SchoolPost;
            if (post.post_id > 0)
            {
                await Navigation.PushAsync(new StaffPostPage("U", Settings.schoolId, 3, _club_id, post));
            }
        }
    }
}