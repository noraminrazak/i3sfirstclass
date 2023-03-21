using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageContactPage : ContentPage
    {
        IFirebaseSubscribe subscribe = DependencyService.Get<IFirebaseSubscribe>();
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _profileId = Settings.profileId;
        public int RowCount { get; set; }
        public bool SetHasNavigationBar { get; private set; }

        public static Label l = new Label();
        public int _staff_type_id;
        public int _user_role_id;
        public List<Contact> listContact;
        public MessageContactPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            searchBar.Text = string.Empty;
            _user_role_id = Settings.userRoleId;

            if (_user_role_id == 7) //merchant
            {
                MerchantSchoolRelationship();
            }
            else if (_user_role_id == 8) //staff
            {
                _staff_type_id = Settings.staffTypeId;
                if (_staff_type_id == 1 || _staff_type_id == 2)
                {
                    ContactSchoolStaffMerchant();
                }
                else
                {
                    StaffClassRelationship();
                }
            }
            else if (_user_role_id == 9) //parent
            {
                ContactClassTeacher();
            }
        }

        async void ContactClassTeacher()
        {

            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvContact.ItemsSource = null;
                    var t = srvc.PostContactClassTeacher(Settings.parentId);
                    string jsonStr = await t;
                    ContactProperty response = JsonConvert.DeserializeObject<ContactProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listContact = new List<Contact>();
                        foreach (Contact sl in response.Data)
                        {
                            Contact post = new Contact();
                            post.profile_id = sl.profile_id;
                            post.full_name = sl.full_name;
                            if (Settings.cultureInfo == "en-US")
                            {
                                post.user_role = sl.user_role;
                            }
                            else {
                                post.user_role = sl.user_role_bm;
                            }
                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url = requestUrl + sl.photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.image_visible = false;
                                post.initial_visible = true;
                            }

                            listContact.Add(post);

                            //string path = string.Empty;
                            //if (_profileId > sl.profile_id)
                            //{
                            //    path = "pid" + sl.profile_id + "pid" + _profileId;
                            //}
                            //else {
                            //    path = "pid" + _profileId + "pid" + sl.profile_id;
                            //}
                            //subscribe.SubscribeToTopic(path);
                        }
                        RowCount = listContact.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvContact.Footer = l;
                        lvContact.ItemsSource = listContact;
                    }
                    else
                    {
                        List<Contact> list = new List<Contact>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvContact.Footer = l;
                        lvContact.ItemsSource = list;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
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
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //thats all you need to make a search  
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                lvContact.ItemsSource = listContact;
            }
            else
            {
                lvContact.ItemsSource = listContact.Where(x => x.full_name.ToLower().StartsWith(e.NewTextValue));
                RowCount = listContact.Where(x => x.full_name.ToLower().StartsWith(e.NewTextValue)).Count();
                l.HorizontalTextAlignment = TextAlignment.Center;
                if (RowCount > 0)
                {
                    l.Text = RowCount + AppResources.RecordText;
                }
                else
                {
                    l.Text = AppResources.NoRecordFoundText;
                }
                lvContact.Footer = l;
            }
        }
        public async void StaffClassRelationship()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostStaffClassRelationship(Settings.schoolId, Settings.staffId);
                    string jsonStr = await t;
                    StaffClassRelationshipProperty response = JsonConvert.DeserializeObject<StaffClassRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        int[] array = new int[response.Data.Count];
                        int i = 0;
                        foreach (StaffClassRelationship r in response.Data)
                        {
                            array[i++] = r.class_id;
                        }

                        await ContactStudentParentStaffMerchant(array);
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
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async void MerchantSchoolRelationship()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostMerchantSchoolRelationship(Settings.merchantId);
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

                        await ContactSchoolStaff(array);
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
            }
            else
            {
                SnackB.Message = AppResources.CheckInternetText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async Task ContactStudentParentStaffMerchant(int[] class_id)
        {

            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvContact.ItemsSource = null;
                    var t = srvc.PostContactStudentParentStaffMerchant(Settings.schoolId, class_id);
                    string jsonStr = await t;
                    ContactProperty response = JsonConvert.DeserializeObject<ContactProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listContact = new List<Contact>();
                        foreach (Contact sl in response.Data)
                        {
                            Contact post = new Contact();
                            post.profile_id = sl.profile_id;
                            post.full_name = sl.full_name;
                            if (Settings.cultureInfo == "en-US")
                            {
                                post.user_role = sl.user_role;
                            }
                            else
                            {
                                post.user_role = sl.user_role_bm;
                            }
                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url = requestUrl + sl.photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.image_visible = false;
                                post.initial_visible = true;
                            }

                            listContact.Add(post);

                            //string path = string.Empty;
                            //if (_profileId > sl.profile_id)
                            //{
                            //    path = "pid" + sl.profile_id + "pid" + _profileId;
                            //}
                            //else
                            //{
                            //    path = "pid" + _profileId + "pid" + sl.profile_id;
                            //}
                            //subscribe.SubscribeToTopic(path);
                        }
                        RowCount = listContact.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvContact.Footer = l;
                        lvContact.ItemsSource = listContact;
                    }
                    else
                    {
                        List<Contact> list = new List<Contact>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvContact.Footer = l;
                        lvContact.ItemsSource = list;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
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

        async void ContactSchoolStaffMerchant()
        {

            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvContact.ItemsSource = null;
                    var t = srvc.PostContactSchoolStaffMerchant(Settings.schoolId, Settings.profileId);
                    string jsonStr = await t;
                    ContactProperty response = JsonConvert.DeserializeObject<ContactProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listContact = new List<Contact>();
                        foreach (Contact sl in response.Data)
                        {
                            Contact post = new Contact();
                            post.profile_id = sl.profile_id;
                            post.full_name = sl.full_name;
                            if (Settings.cultureInfo == "en-US")
                            {
                                post.user_role = sl.user_role;
                            }
                            else
                            {
                                post.user_role = sl.user_role_bm;
                            }
                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url = requestUrl + sl.photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.image_visible = false;
                                post.initial_visible = true;
                            }

                            listContact.Add(post);

                            //string path = string.Empty;
                            //if (_profileId > sl.profile_id)
                            //{
                            //    path = "pid" + sl.profile_id + "pid" + _profileId;
                            //}
                            //else
                            //{
                            //    path = "pid" + _profileId + "pid" + sl.profile_id;
                            //}
                            //subscribe.SubscribeToTopic(path);
                        }
                        RowCount = listContact.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvContact.Footer = l;
                        lvContact.ItemsSource = listContact;
                    }
                    else
                    {
                        List<Contact> list = new List<Contact>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvContact.Footer = l;
                        lvContact.ItemsSource = list;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
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

        async Task ContactSchoolStaff(int[] school_id)
        {

            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvContact.ItemsSource = null;
                    var t = srvc.PostContactSchoolStaff(school_id);
                    string jsonStr = await t;
                    ContactProperty response = JsonConvert.DeserializeObject<ContactProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listContact = new List<Contact>();
                        foreach (Contact sl in response.Data)
                        {
                            Contact post = new Contact();
                            post.profile_id = sl.profile_id;
                            post.full_name = sl.full_name;
                            if (Settings.cultureInfo == "en-US")
                            {
                                post.user_role = sl.user_role;
                            }
                            else
                            {
                                post.user_role = sl.user_role_bm;
                            }
                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url = requestUrl + sl.photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.image_visible = false;
                                post.initial_visible = true;
                            }

                            listContact.Add(post);

                            //string path = string.Empty;
                            //if (_profileId > sl.profile_id)
                            //{
                            //    path = "pid" + sl.profile_id + "pid" + _profileId;
                            //}
                            //else
                            //{
                            //    path = "pid" + _profileId + "pid" + sl.profile_id;
                            //}

                            //subscribe.SubscribeToTopic(path);

                        }
                        RowCount = listContact.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvContact.Footer = l;
                        lvContact.ItemsSource = listContact;
                    }
                    else
                    {
                        List<Contact> list = new List<Contact>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvContact.Footer = l;
                        lvContact.ItemsSource = list;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
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
        public Contact contact = new Contact();
        async void OnContactSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as Contact;
            if (data == null) return;
            contact = data;

            if (contact.profile_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                int myPid = Settings.profileId;
                int receiverId = data.profile_id;
                string channelId = string.Empty;
                if (myPid > receiverId)
                {
                    channelId = "pid" + receiverId + "_pid" + myPid;
                }
                else 
                {
                    channelId = "pid" + myPid + "_pid" + receiverId;
                }

                await Navigation.PushAsync(new MessagePage(1,Settings.profileId,data.profile_id,data.full_name,data.photo_url, channelId));
                ((ListView)sender).SelectedItem = null;
            }
        }
        //protected override bool OnBackButtonPressed()
        //{
        //    return true;
        //}
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
    }
}