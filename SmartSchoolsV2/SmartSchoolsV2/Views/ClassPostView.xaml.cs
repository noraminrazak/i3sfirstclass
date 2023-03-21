using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClassPostView : ContentView, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public static Command LoadSchoolClassPost { get; set; }
        SchoolPost data = new SchoolPost();
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _user_role_id;
        public int _school_id;
        public int _class_id;
        public int _staff_type_id;
        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
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
        public ClassPostView ()
		{
			InitializeComponent ();
            BindingContext = this;

            _user_role_id = Settings.userRoleId;
            if (_user_role_id == 8)
            {
                _school_id = Settings.selectedSchoolId;
                _class_id = Settings.selectedClassId;
                btnAddPost.IsVisible = true;
                IsSwipe = true;
            }
            else if (_user_role_id == 9)
            {
                _school_id = Settings.studentSchoolId;
                _class_id = Settings.studentClassId;
                btnAddPost.IsVisible = false;
                IsSwipe = false;
            }

            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) => {
                Navigation.PushAsync(new StaffPostPage("C",_school_id, 2, _class_id, data ));
            };
            btnAddPost.GestureRecognizers.Add(tapGestureRecognizerTxn);

            LoadSchoolClassPost = new Command(async () => await SchoolClassPost(_school_id, _class_id));
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await SchoolClassPost(_school_id, _class_id);
                });
            }
        }

        public async Task SchoolClassPost(int school_id, int class_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvPost.ItemsSource = null;
                    var t = srvc.PostSchoolClassPost(school_id, class_id);
                    string jsonStr = await t;
                    SchoolPostProperty response = JsonConvert.DeserializeObject<SchoolPostProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<SchoolPost> list = new List<SchoolPost>();
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
                        lvPost.Footer = l;
                        lvPost.ItemsSource = list;
                    }
                    else
                    {
                        List<SchoolPost> list = new List<SchoolPost>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvPost.Footer = l;
                        lvPost.ItemsSource = list;
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

        async void OnEditClicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            SchoolPost post = button.CommandParameter as SchoolPost;
            if (post.post_id > 0)
            {
                await Navigation.PushAsync(new StaffPostPage("U", Settings.schoolId, 2, _class_id, post));
            }
        }
    }
}