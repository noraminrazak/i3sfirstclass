using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SmartSchoolsV2.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchListPage3 : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public int _user_role_id;
        public int _school_id;
        public int _merchant_type_id;
        public string _search_name;
        public string _title;
        public string _option;
        public List<CardAssignment> listAssignment;
        public List<SchoolMerchant> listMerchant;
        public delegate void SetDetailEventHandler(object source, EventArgs args);
        public event SetDetailEventHandler DetailSet;
        protected virtual void OnDetailSet()
        {
            if (DetailSet != null)
            {
                DetailSet(this, EventArgs.Empty);
            }
        }
        bool isBusy;
        public bool IsBusy3
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public SearchListPage3(string title, string option, int param1, int param2, string param3)
        {
            InitializeComponent();
            BindingContext = this;
            _title = title;
            _option = option;
            if (_option == "report" || _option == "replacement")
            {
                _user_role_id = param1;
                _school_id = param2;
                _search_name = param3;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblTitleView.Text = _title;

            if (_option == "report" || _option == "replacement")
            {
                CardAssignment(_user_role_id, _school_id, _search_name);
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
        async void CardAssignment(int user_role_id, int school_id, string search_name)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy3 = true;
                    var t = srvc.PostCardSearchAssignment(user_role_id, school_id, search_name);
                    string jsonStr = await t;
                    CardAssignmentProperty response = JsonConvert.DeserializeObject<CardAssignmentProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listAssignment = new List<CardAssignment>();
                        foreach (CardAssignment r in response.Data)
                        {
                            CardAssignment prop = new CardAssignment();
                            if (!string.IsNullOrEmpty(r.photo_url))
                            {
                                prop.photo_url = requestUrl + r.photo_url;
                                prop.image_visible = true;
                                prop.initial_visible = false;
                            }
                            else
                            {
                                prop.image_visible = false;
                                prop.initial_visible = true;
                            }
                            prop.profile_id = r.profile_id;
                            prop.full_name = r.full_name;
                            prop.card_id = r.card_id;
                            prop.card_number = r.card_number;
                            prop.card_status_id = r.card_status_id;
                            prop.card_status = r.card_status;
                            prop.search_name = r.full_name;
                            prop.search_name2 = r.school_name;
                            prop.search_name3 = r.class_name;
                            listAssignment.Add(prop);
                        }
                        RowCount = listAssignment.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvSearch.Footer = l;
                        lvSearch.ItemsSource = listAssignment;
                    }
                    else
                    {
                        List<CardAssignment> list = new List<CardAssignment>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvSearch.Footer = l;
                        lvSearch.ItemsSource = list;
                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {
                    IsBusy3 = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            //thats all you need to make a search  
            if (_option == "report" || _option == "replacement")
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    lvSearch.ItemsSource = listAssignment;
                }
                else
                {
                    lvSearch.ItemsSource = listAssignment.Where(x => x.search_name.ToLower().Contains(e.NewTextValue));
                    RowCount = listAssignment.Where(x => x.search_name.ToLower().Contains(e.NewTextValue)).Count();
                    l.HorizontalTextAlignment = TextAlignment.Center;
                    if (RowCount > 0)
                    {
                        l.Text = RowCount + AppResources.RecordText;
                    }
                    else
                    {
                        l.Text = AppResources.NoRecordFoundText;
                    }
                    lvSearch.Footer = l;
                }
            }
        }
        public CardAssignment listData;
        async void OnSearchSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (_option == "report" || _option == "replacement")
            {
                var data = e.SelectedItem as CardAssignment;
                if (data == null) return;
                listData = data;

                if (listData.profile_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.assignProfileId = listData.profile_id;

                    ((ListView)sender).SelectedItem = null;

                    await Navigation.PopAsync();
                    if (_option == "report")
                    {
                        StaffCardLostReportPage.Back = "Y";
                    }
                    else if (_option == "replacement")
                    {
                        StaffCardReplacementPage.Back = "Y";
                    }

                    OnDetailSet();
                }
            }
        }
    }
}