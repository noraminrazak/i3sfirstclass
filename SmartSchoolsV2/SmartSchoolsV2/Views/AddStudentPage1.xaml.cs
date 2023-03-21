using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using System;
using System.Collections.Generic;
using SmartSchoolsV2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStudentPage1 : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();

        public List<School> list = new List<School>();
        public School school = new School();
        public int prevStateId;
        public int newStateId;

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
                txtState.Text = Settings.stateName;
                txtSearchSchool.Text = "";
                lvSchool.IsVisible = false;
                lvSchool.ItemsSource = list;
            }
        }
        public AddStudentPage1()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        async void StartCall(object sender, EventArgs args)
        {
            var page = new SearchListPage1(AppResources.SelectStateText, "state", "add-student");
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        async void OnSearchClicked(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtState.Text))
            {
                if (!string.IsNullOrEmpty(txtSearchSchool.Text))
                {
                    if (txtSearchSchool.Text.Length > 2)
                    {
                        if (conn.IsConnected() == true)
                        {
                            try
                            {
                                lvSchool.IsVisible = true;
                                IsBusy = true;
                                var t = srvc.PostSearchSchool(Settings.stateId, txtSearchSchool.Text);
                                string jsonStr = await t;
                                SchoolProperty response = JsonConvert.DeserializeObject<SchoolProperty>(jsonStr);
                                if (response.Success == true)
                                {
                                    list = new List<School>();
                                    foreach (School r in response.Data)
                                    {
                                        School prop = new School();
                                        prop.school_id = r.school_id;
                                        prop.school_name = r.school_name;
                                        prop.school_code = r.school_code;
                                        prop.city = r.city;
                                        list.Add(prop);
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
                                    lvSchool.Footer = l;
                                    lvSchool.ItemsSource = list;
                                }
                                else
                                {
                                    List<School> list = new List<School>();
                                    l.HorizontalTextAlignment = TextAlignment.Center;
                                    l.Text = AppResources.NoRecordFoundText;
                                    lvSchool.Footer = l;
                                    lvSchool.ItemsSource = list;
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
                            //SnackB.Message = AppResources.CheckInternetText;
                            //SnackB.IsOpen = !SnackB.IsOpen;
                        }
                    }
                    else 
                    {
                        SnackB.Message = AppResources.PleaseEnterMoreCharText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                    
                }
                else
                {
                    SnackB.Message = AppResources.SchoolNameRequiredText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                }
            }
            else {
                SnackB.Message = AppResources.StateRequiredText;
                SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void OnSchoolSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as School;
            if (data == null) return;
            school = data;

            if (school.school_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                await Navigation.PushAsync(new AddStudentPage2(school.school_id, school.school_name));

                ((ListView)sender).SelectedItem = null;

            }
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}