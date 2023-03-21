using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using System;
using System.Collections.ObjectModel;
using SmartSchoolsV2.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Popup;
using Rg.Plugins.Popup.Extensions;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClubMemberView : ContentView
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<ClubMember> listMember { get; set; }
        public static Command LoadClubMember { get; set; }
        public int _user_role_id;
        public int _staff_id;
        public int _create_by_staff_id;
        public int _school_id;
        public string _school_name;
        public int _club_id;
        public string _club_name;
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
        public ClubMemberView()
        {
            InitializeComponent();
            this.BindingContext = this;

            _school_id = Settings.selectedSchoolId;
            _school_name = Settings.selectedSchoolName;
            _club_id = Settings.selectedClubId;
            _club_name = Settings.selectedClubName;
            _staff_id = Settings.staffId;
            _create_by_staff_id = Settings.createByStaffId;
            _user_role_id = Settings.userRoleId;
            if (_user_role_id == 8)
            {
                if (_staff_id == _create_by_staff_id)
                {
                    btnAddMember.IsVisible = true;
                }
                else
                {
                    btnAddMember.IsVisible = false;
                }
            }
            else {
                btnAddMember.IsVisible = false;
            }
            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new StaffAddClubMemberPage(_school_id,_school_name, _club_id, _club_name));
            };
            btnAddMember.GestureRecognizers.Add(tapGestureRecognizerTxn);

            LoadClubMember = new Command(async () => await ClubMember(_school_id, _club_id));
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await ClubMember(_school_id,_club_id);
                });
            }
        }
        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.LightGoldenrodYellow;
                lastCell = viewCell;
            }
        }

        async void Delete_Invoked(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            ClubMember product = item.BindingContext as ClubMember;

            bool answer = await App.Current.MainPage.DisplayAlert(AppResources.ConfirmDeleteText, AppResources.DoYouReallyWantToRemoveText + product.full_name + "?", AppResources.YesText, AppResources.CancelText);
            if (answer)
            {
                await RemoveClubMember(product.relationship_id, _club_id, product.profile_id, Settings.fullName);
            }
        }

        public async Task RemoveClubMember(int relationship_id, int club_id,int profile_id, string update_by)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffRemoveClubMember(relationship_id, club_id, profile_id, update_by);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await ClubMember(_school_id, _club_id);
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.SorryText, response.Message, "OK");
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    HideLoadingPopup();
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        public async Task ClubMember(int school_id, int club_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvMember.ItemsSource = null;
                    var t = srvc.PostSchoolClubMember(school_id,club_id);
                    string jsonStr = await t;
                    ClubMemberProperty response = JsonConvert.DeserializeObject<ClubMemberProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listMember = new ObservableCollection<ClubMember>();
                        foreach (ClubMember sl in response.Data)
                        {
                            ClubMember post = new ClubMember();
                            post.relationship_id = sl.relationship_id;
                            post.profile_id = sl.profile_id;
                            post.full_name = sl.full_name;
                            post.status_code = sl.status_code;
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

                            post.user_role_id = sl.user_role_id;
                            if (sl.user_role_id == 7) 
                            {
                                post.user_role = AppResources.MerchantText;
                            } 
                            else if (sl.user_role_id == 8) 
                            {
                                post.user_role = AppResources.StaffText;
                            } 
                            else if (sl.user_role_id == 9) 
                            {
                                post.user_role = AppResources.ParentText;
                            } 
                            else if (sl.user_role_id == 10) 
                            {
                                post.user_role = AppResources.StudentText;
                            }

                            listMember.Add(post);
                        }
                        RowCount = listMember.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvMember.Footer = l;
                        lvMember.ItemsSource = listMember;
                    }
                    else
                    {
                        listMember = new ObservableCollection<ClubMember>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvMember.Footer = l;
                        lvMember.ItemsSource = listMember;
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