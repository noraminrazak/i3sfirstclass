using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Popup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClassView2 : ContentView
	{
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int _school_id;
        public int _staff_id;
        public string _class_teacher_flag = "N";
        public int RowCount { get; set; }
        public static Label l = new Label();
        public static Command LoadStaffClassRelationship { get; set; }
        public static string _back;
        public static string Back
        {
            get { return _back; }
            set { _back = value; }
        }

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

        public async void OnDetailSet(object source, EventArgs e)
        {
            if (Back == "Y")
            {
            //    var page = new JoinClassPopupPage(Settings.classId, Settings.className, Settings.sessionCode);
            //    page.DetailSet += this.OnDetailSet;
            //    await PopupNavigation.Instance.PushAsync(page);
            //}
            //else if (Back == "X") 
            //{
            //    StaffJoinClass();
            }
        }

        public ClassView2 ()
		{
			InitializeComponent ();
            this.BindingContext = this;

            _school_id = Settings.schoolId;
            _staff_id = Settings.staffId;

            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) => {
                var page = new SearchListPage2(AppResources.SelectClassNameText, "join", _school_id);
                page.DetailSet += this.OnDetailSet;
                Navigation.PushAsync(page);
            };
            btnAddClass.GestureRecognizers.Add(tapGestureRecognizerTxn);

            LoadStaffClassRelationship = new Command(async () => await StaffClassRelationship());
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await StaffClassRelationship();
                });
            }
        }

        public async Task StaffClassRelationship()
        {
            //int school_id = Settings.schoolId;
            //int staff_id = Settings.staffId;
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostStaffClassRelationship(_school_id, _staff_id);
                    string jsonStr = await t;
                    StaffClassRelationshipProperty response = JsonConvert.DeserializeObject<StaffClassRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<StaffClassRelationship> list = new List<StaffClassRelationship>();
                        foreach (StaffClassRelationship sl in response.Data)
                        {
                            StaffClassRelationship post = new StaffClassRelationship();
                            post.relationship_id = sl.relationship_id;
                            post.class_id = sl.class_id;
                            post.class_name = sl.class_name;
                            post.school_name = sl.school_name;
                            post.school_type = sl.school_type;
                            post.session_code = sl.session_code;
                            post.class_teacher_flag = sl.class_teacher_flag;
                            post.total_student = sl.total_student + AppResources.SpaceStudentText;
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
                        lvClass2.Footer = l;
                        lvClass2.ItemsSource = list;
                    }
                    else
                    {
                        List<StaffClassRelationship> list = new List<StaffClassRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvClass2.Footer = l;
                        lvClass2.ItemsSource = list;
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

        ViewCell lastCell;
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
        public StaffClassRelationship classroom;
        async void OnClassSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as StaffClassRelationship;
            if (data == null) return;
            classroom = data;

            if (classroom.class_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                Settings.selectedSchoolId = _school_id;
                Settings.selectedSchoolName = classroom.school_name;
                Settings.selectedClassId = classroom.class_id;
                Settings.selectedClassName = classroom.class_name;
                await Navigation.PushAsync(new ClassPage2(classroom.relationship_id,_school_id, classroom.school_name, classroom.class_id, classroom.class_name, classroom.session_code));
                ((ListView)sender).SelectedItem = null;
            }
        }

        async void StaffJoinClass() 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    ShowLoadingPopup();

                    var t = srvc.PostStaffJoinClass(Settings.classId, Settings.staffId, _class_teacher_flag,  Settings.fullName);
                    string jsonStr = await t;
                    CrudProperty response = JsonConvert.DeserializeObject<CrudProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.DoneText, response.Message, "OK");

                        await StaffClassRelationship();
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