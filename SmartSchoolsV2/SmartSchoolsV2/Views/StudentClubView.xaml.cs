using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentClubView : ContentView
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public ObservableCollection<ClubRelationship> listClub { get; set; }
        public static Command LoadClubRelationship { get; set; }
        public int _school_id;
        public int _student_id;
        public int RowCount { get; set; }
        public static Label l = new Label();
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
        public StudentClubView()
        {
            InitializeComponent();
            this.BindingContext = this;

            _school_id = Settings.studentSchoolId;
            _student_id = Settings.studentId;
            LoadClubRelationship = new Command(async () => await StudentClubRelationship(_school_id, _student_id));

            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) => {
                var page = new SearchListPage2(AppResources.SelectClubText, "teacher", _school_id);
                page.DetailSet += this.OnDetailSet;
                Navigation.PushAsync(page);
            };
            btnAddClub.GestureRecognizers.Add(tapGestureRecognizerTxn);
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await StudentClubRelationship(_school_id, _student_id);
                });
            }
        }

        public async Task StudentClubRelationship(int school_id, int student_id)
        {

            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostStudentClubRelationship(school_id, student_id);
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
        public ClubRelationship student;
        async void OnClubSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as ClubRelationship;
            if (data == null) return;
            student = data;

            if (student.relationship_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                Settings.selectedSchoolId = data.school_id;
                Settings.selectedClubId = data.club_id;
                Settings.selectedClubName = data.club_name;
                Settings.createByStaffId = data.create_by_staff_id;
                Settings.studentClub = true;
                await Navigation.PushAsync(new ClubPage2(data.relationship_id,
                    Settings.studentProfileId,
                    data.club_id,
                    data.club_name,
                    data.school_id,
                    data.school_name,
                    Settings.studentFullName,
                    Settings.studentPhotoUrl,
                    data.create_by_staff_id));

                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}