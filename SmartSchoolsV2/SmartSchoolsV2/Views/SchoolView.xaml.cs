using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Popup;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.ViewModels;
using System.Runtime.CompilerServices;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SchoolView : ContentView, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<SchoolPost> listPost { get; set; }
        SchoolPost data = new SchoolPost();
        public static Command LoadSchoolPost { get; set; }
        public int _merchant_id;
        public int _user_role_id;
        public int _parent_id;
        public int _staff_type_id;

        private bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
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
                    OnPropertyChanged("IsSwipe");
                }
            }
        }

        public SchoolView()
        {
            InitializeComponent();
            this.BindingContext = this;

            var tapGestureRecognizerAdd = new TapGestureRecognizer();
            tapGestureRecognizerAdd.Tapped += async (s, e) => {
                await Navigation.PushAsync(new StaffPostPage("C",Settings.schoolId, 1, 0, data));
            };
            btnAddPost.GestureRecognizers.Add(tapGestureRecognizerAdd);

            LoadSchoolPost = new Command(async () => await GetUserRole());
        }
        public async Task GetUserRole()
        {
            _user_role_id = Settings.userRoleId;

            if (_user_role_id == 9)
            {
                await ParentSchoolRelationship();
            }
            else if (_user_role_id == 8)
            {
                await StaffSchoolRelationship();
                _staff_type_id = Settings.staffTypeId;

                if (_staff_type_id == 1 || _staff_type_id == 6)
                {
                    btnAddPost.IsVisible = true;
                }
                else
                {
                    IsSwipe = false;
                    btnAddPost.IsVisible = false;
                }
            }
            else if (_user_role_id == 7)
            {
                await MerchantSchoolRelationship();
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (_user_role_id == 9)
                    {
                        await ParentSchoolRelationship();
                    }
                    else if (_user_role_id == 8)
                    {
                        await StaffSchoolRelationship();
                        int staff_type_id = Settings.staffTypeId;

                        if (staff_type_id == 1)
                        {
                            btnAddPost.IsVisible = true;
                        }
                        else
                        {
                            btnAddPost.IsVisible = false;
                        }
                    }
                    else if (_user_role_id == 7)
                    {
                        await MerchantSchoolRelationship();
                    }
                });
            }
        }

        public async Task StaffSchoolRelationship() 
        {
            int[] _school_id = new int[1];
            _school_id[0] = Settings.schoolId;
            await SchoolPost(_school_id);
        }

        public async Task ParentSchoolRelationship()
        {
            _parent_id = Settings.parentId;

            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostParentSchoolRelationship(_parent_id);
                    string jsonStr = await t;
                    ParentSchoolRelationshipProperty response = JsonConvert.DeserializeObject<ParentSchoolRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        int[] array = new int[response.Data.Count];
                        int i = 0;
                        foreach (ParentSchoolRelationship r in response.Data)
                        {
                            array[i++] = r.school_id;
                        }

                        await SchoolPost(array);
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
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task MerchantSchoolRelationship()
        {
            _merchant_id = Settings.merchantId;

            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostMerchantSchoolRelationship(_merchant_id);
                    string jsonStr = await t;
                    ParentSchoolRelationshipProperty response = JsonConvert.DeserializeObject<ParentSchoolRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        int[] array = new int[response.Data.Count];
                        int i = 0;
                        foreach (ParentSchoolRelationship r in response.Data)
                        {
                            array[i++] = r.school_id;
                        }

                        await SchoolPost(array);
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
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task SchoolPost(int[] school_id)
        {
            if (school_id.Length > 0) 
            {
                if (conn.IsConnected() == true)
                {
                    try
                    {
                        lvPost.ItemsSource = null;
                        IsBusy = true;

                        var t = srvc.PostSchoolPost(school_id);
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
                                else {
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
            else
            {
                listPost = new ObservableCollection<SchoolPost>();
                l.HorizontalTextAlignment = TextAlignment.Center;
                l.Text = AppResources.NoRecordFoundText;
                lvPost.Footer = l;
                lvPost.ItemsSource = listPost;
            }
        }

        async void OnEditClicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            SchoolPost post = button.CommandParameter as SchoolPost;
            if (post.post_id > 0) {
                await Navigation.PushAsync(new StaffPostPage("U", Settings.schoolId, 1, 0, post));
            }
        }
        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }

        static public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);

            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;
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