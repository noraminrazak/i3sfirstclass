using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClubView : ContentView, INotifyPropertyChanged
	{
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public ObservableCollection<ClubRelationship> listClub { get; set; }
        public static Command LoadClubRelationship { get; set; }
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _school_id;
        public int _staff_id;
        public int _parent_id;
        public int _merchant_id;
        public int _user_role_id;
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

            }
        }
        public ClubView ()
		{
			InitializeComponent ();
            this.BindingContext = this;
            LoadClubRelationship = new Command(async () => await GetUserRole());

            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) => {
                _user_role_id = Settings.userRoleId;

                if (_user_role_id == 8)
                {
                    DisplayActionSheetStaff();
                }
                else if (_user_role_id == 7 || _user_role_id == 9)
                {
                    //var page = new SearchListPage2(AppResources.SelectClubText, "headmaster", _school_id);
                    //page.DetailSet += this.OnDetailSet;
                    //Navigation.PushAsync(page);
                }
            };
            btnAddClub.GestureRecognizers.Add(tapGestureRecognizerTxn);
        }


        public async Task GetUserRole()
        {
            _user_role_id = Settings.userRoleId;

            if (_user_role_id == 9)
            {
                btnStack.IsVisible = false;
                await ParentClubRelationship();
            }
            else if (_user_role_id == 8)
            {
                await StaffClubRelationship();
            }
            else if (_user_role_id == 7)
            {
                btnStack.IsVisible = false;
                await MerchantClubRelationship();
            }
        }

        [Obsolete]
        async void DisplayActionSheetStaff() 
        {
            var action = await App.Current.MainPage.DisplayActionSheet(AppResources.PleaseSelectText, AppResources.CancelText, null, AppResources.CreateNewClubText, AppResources.JoinExistingText);
            //switch (action)
            //{
            //case "Create new club":
            if (action == "Create new club" || action == "Cipta kelab baru") 
            {
                var result = await App.Current.MainPage.DisplayPromptAsync(AppResources.CreateNewClubText, AppResources.PleaseEnterNewClubNameText, "OK", AppResources.CancelText, null, 50, Keyboard.Create(KeyboardFlags.CapitalizeWord));
                if (!string.IsNullOrWhiteSpace(result))
                {
                    _school_id = Settings.schoolId;
                    _staff_id = Settings.staffId;
                    await StaffCreateClub(result, _school_id, _staff_id, Settings.fullName);
                }
                else
                {
                    if (result == "")
                    {
                        SnackB.Message = AppResources.NewClubNameRequired;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                }
                //break;
            }
            else if (action == "Join existing club" || action == "Sertai kelab sedia ada") {
                //case "Join existing club":
                var page = new SearchListPage2(AppResources.SelectClubText, "all", _school_id);
                page.DetailSet += this.OnDetailSet;
                _ = Navigation.PushAsync(page);
                //break;
            }
            //default:
            //break;
            //}
        }

        async Task StaffCreateClub(string club_name, int school_id, int staff_id, string create_by)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffCreateClub(club_name, school_id, staff_id, create_by);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.DoneText, response.Message, "OK");
                        await GetUserRole();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.SorryText, response.Message, "OK");
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

        public async Task StaffClubRelationship()
        {
            _school_id = Settings.schoolId;
            _staff_id = Settings.staffId;
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostStaffClubRelationship(_school_id, _staff_id);
                    string jsonStr = await t;
                    ClubRelationshipProperty response = JsonConvert.DeserializeObject<ClubRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listClub = new ObservableCollection<ClubRelationship>();
                        foreach (ClubRelationship sl in response.Data)
                        {
                            ClubRelationship post = new ClubRelationship();
                            post.relationship_id = sl.relationship_id;
                            post.club_id = sl.club_id;
                            post.club_name = sl.club_name;
                            post.school_id = sl.school_id;
                            post.create_by_staff_id = sl.create_by_staff_id;
                            post.school_name = sl.school_name;
                            post.school_type = sl.school_type;
                            post.total_member = sl.total_member + AppResources.MemberText;
                            listClub.Add(post);
                        }
                        RowCount = listClub.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvClub.Footer = l;
                        lvClub.ItemsSource = listClub;
                    }
                    else
                    {
                        listClub = new ObservableCollection<ClubRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvClub.Footer = l;
                        lvClub.ItemsSource = listClub;
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

        public async Task MerchantClubRelationship()
        {
            _merchant_id = Settings.merchantId;

            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostMerchantClubRelationship(_merchant_id);
                    string jsonStr = await t;
                    ClubRelationshipProperty response = JsonConvert.DeserializeObject<ClubRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listClub = new ObservableCollection<ClubRelationship>();
                        foreach (ClubRelationship sl in response.Data)
                        {
                            ClubRelationship post = new ClubRelationship();
                            post.relationship_id = sl.relationship_id;
                            post.club_id = sl.club_id;
                            post.club_name = sl.club_name;
                            post.school_id = sl.school_id;
                            post.create_by_staff_id = sl.create_by_staff_id;
                            post.school_name = sl.school_name;
                            post.school_type = sl.school_type;
                            post.total_member = sl.total_member + AppResources.MemberText;
                            listClub.Add(post);
                        }
                        RowCount = listClub.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvClub.Footer = l;
                        lvClub.ItemsSource = listClub;
                    }
                    else
                    {
                        listClub = new ObservableCollection<ClubRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvClub.Footer = l;
                        lvClub.ItemsSource = listClub;
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

        public async Task ParentClubRelationship()
        {
            _parent_id = Settings.parentId;

            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostParentClubRelationship(_parent_id);
                    string jsonStr = await t;
                    ClubRelationshipProperty response = JsonConvert.DeserializeObject<ClubRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listClub = new ObservableCollection<ClubRelationship>();
                        foreach (ClubRelationship sl in response.Data)
                        {
                            ClubRelationship post = new ClubRelationship();
                            post.relationship_id = sl.relationship_id;
                            post.club_id = sl.club_id;
                            post.club_name = sl.club_name;
                            post.school_id = sl.school_id;
                            post.create_by_staff_id = sl.create_by_staff_id;
                            post.school_name = sl.school_name;
                            post.school_type = sl.school_type;
                            post.total_member = sl.total_member + AppResources.MemberText;
                            listClub.Add(post);
                        }
                        RowCount = listClub.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvClub.Footer = l;
                        lvClub.ItemsSource = listClub;
                    }
                    else
                    {
                        listClub = new ObservableCollection<ClubRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvClub.Footer = l;
                        lvClub.ItemsSource = listClub;
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

        public ClubRelationship value;
        async void OnClubSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as ClubRelationship;
            if (data == null) return;
            value = data;

            if (value.relationship_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                Settings.selectedSchoolId = data.school_id;
                Settings.selectedSchoolName = data.school_name;
                Settings.selectedClubId = data.club_id;
                Settings.selectedClubName = data.club_name;
                Settings.createByStaffId = data.create_by_staff_id;
                Settings.studentClub = false;

                if (_user_role_id == 8 && data.create_by_staff_id == _staff_id)
                {
                    await Navigation.PushAsync(new ClubPage(data.relationship_id,
                        Settings.profileId,
                        data.club_id,
                        data.club_name,
                        data.school_id,
                        data.school_name,
                        Settings.fullName,
                        Settings.photoUrl,
                        data.create_by_staff_id));
                }
                else 
                {
                    await Navigation.PushAsync(new ClubPage2(data.relationship_id,
                        Settings.profileId,
                        data.club_id,
                        data.club_name,
                        data.school_id,
                        data.school_name,
                        Settings.fullName,
                        Settings.photoUrl,
                        data.create_by_staff_id));
                }

                ((ListView)sender).SelectedItem = null;
            }
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