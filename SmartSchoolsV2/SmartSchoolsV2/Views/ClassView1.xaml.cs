using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using System;
using System.Collections.Generic;
using SmartSchoolsV2.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClassView1 : ContentView
	{
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public static Command LoadSchoolInfo { get; set; }
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
        public ClassView1 ()
		{
			InitializeComponent ();
            this.BindingContext = this;

            LoadSchoolInfo = new Command(async () => await SchoolInfo());
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await SchoolInfo();
                });
            }
        }

        public async Task SchoolInfo()
        {
            int school_id = Settings.schoolId;
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostSchoolInfo(school_id);
                    string jsonStr = await t;
                    SchoolInfoProperty response = JsonConvert.DeserializeObject<SchoolInfoProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        List<SchoolInfo> list = new List<SchoolInfo>();
                        foreach (SchoolInfo sl in response.Data)
                        {
                            SchoolInfo post = new SchoolInfo();
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            post.school_code = sl.school_code;
                            post.school_type_id = sl.school_type_id;
                            post.school_type = sl.school_type;
                            post.city = sl.city;
                            post.state_name = sl.state_name;
                            post.total_staff = sl.total_staff + AppResources.SpaceStaffText;
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
                        lvClass1.Footer = l;
                        lvClass1.ItemsSource = list;
                    }
                    else
                    {
                        List<SchoolInfo> list = new List<SchoolInfo>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvClass1.Footer = l;
                        lvClass1.ItemsSource = list;
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
        public SchoolInfo school;
        async void OnClassSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as SchoolInfo;
            if (data == null) return;
            school = data;

            if (school.school_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                Settings.schoolId = school.school_id;
                Settings.schoolName = school.school_name;
                Settings.stateName = school.state_name;
                Settings.cityName = school.city;
                Settings.schoolCode = school.school_code;
                await Navigation.PushAsync(new ClassPage1(school.school_id, school.school_name,school.state_name,school.city,school.school_code));
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}