using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchListPage1 : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public string _page;
        public string _title;
        public string _option;
        public int _state_id;
        public List<State> listState;
        public List<City> listCity;
        public List<Country> listCountry;
        public List<Occupation> listOccupation;
        public List<CardType> listCardType;
        public List<UserRace> listUserRace;
        public List<StaffShift> listShift;
        public List<ReasonForAbsent> listReason;
        public List<ProblemType> listFeedback;

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
        public bool IsBusy1
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }

        public SearchListPage1(string title, string option, string page, int state_id = 0)
        {
            InitializeComponent();
            BindingContext = this;
            _title = title;
            _option = option;
            _page = page;
            _state_id = state_id;
            searchBar.Unfocus();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblTitleView.Text = _title;

            if (_option == "state")
            {
                LookupState();
            }
            else if (_option == "city")
            {
                LookupCity(_state_id);
            }
            else if (_option == "country" || _option == "nation")
            {
                LookupCountry();
            }
            else if (_option == "user-race") 
            {
                LookupUserRace();
            }
            else if (_option == "reason-for-absent")
            {
                LookupReasonForAbsent();
            }
            else if (_option == "card-type")
            {
                LookupCardType();
            }
            else if (_option == "shift") 
            {
                LookupStaffShift();
            }
            else if (_option == "problem-type")
            {
                LookupProblemType();
            }
            else if (_option == "occupation")
            {
                LookupOccupation();
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        async void LookupStaffShift()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy1 = true;
                    var t = srvc.PostStaffShift(Settings.schoolId);
                    string jsonStr = await t;
                    StaffShiftProperty response = JsonConvert.DeserializeObject<StaffShiftProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listShift = new List<StaffShift>();
                        foreach (StaffShift r in response.Data)
                        {
                            StaffShift prop = new StaffShift();
                            prop.shift_id = r.shift_id;
                            prop.shift_code = r.shift_code;
                            prop.start_time = r.start_time;
                            prop.end_time = r.end_time;
                            prop.search_id = r.shift_id;
                            prop.search_name = r.shift_code;
                            listShift.Add(prop);
                        }
                        RowCount = listShift.Count;
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
                        lvSearch.ItemsSource = listShift;
                    }
                    else
                    {
                        List<StaffShift> list = new List<StaffShift>();
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
                    IsBusy1 = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void LookupState() 
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy1 = true;
                    var t = srvc.PostLookupState();
                    string jsonStr = await t;
                    StateProperty response = JsonConvert.DeserializeObject<StateProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listState = new List<State>();
                        foreach (State r in response.Data)
                        {
                            State prop = new State();
                            prop.state_id = r.state_id;
                            prop.state_name = r.state_name;
                            prop.search_id = r.state_id;
                            prop.search_name = r.state_name;
                            listState.Add(prop);
                        }
                        RowCount = listState.Count;
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
                        lvSearch.ItemsSource = listState;
                    }
                    else
                    {
                        List<State> list = new List<State>();
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
                    IsBusy1 = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void LookupCity(int state_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy1 = true;
                    var t = srvc.PostLookupCity(state_id);
                    string jsonStr = await t;
                    CityProperty response = JsonConvert.DeserializeObject<CityProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listCity = new List<City>();
                        foreach (City r in response.Data)
                        {
                            City prop = new City();
                            prop.city_id = r.city_id;
                            prop.city_name = r.city_name;
                            prop.search_id = r.city_id;
                            prop.search_name = r.city_name;
                            listCity.Add(prop);
                        }
                        RowCount = listCity.Count;
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
                        lvSearch.ItemsSource = listCity;
                    }
                    else
                    {
                        List<City> list = new List<City>();
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
                    IsBusy1 = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void LookupCountry()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostLookupCountry();
                    string jsonStr = await t;
                    CountryProperty response = JsonConvert.DeserializeObject<CountryProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listCountry = new List<Country>();
                        foreach (Country r in response.Data)
                        {
                            Country prop = new Country();
                            prop.country_id = r.country_id;
                            prop.country_code = r.country_code;
                            prop.search_id = r.country_id;
                            if (_page == "create-password")
                            {
                                prop.country_name = r.country_name + " (" + r.country_code + ")";
                                prop.search_name = r.country_name + " (" + r.country_code + ")" ;
                            }
                            else 
                            {
                                prop.country_name = r.country_name;
                                prop.search_name = r.country_name;
                            }

                            listCountry.Add(prop);
                        }
                        RowCount = listCountry.Count;
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
                        lvSearch.ItemsSource = listCountry;
                    }
                    else
                    {
                        List<Country> list = new List<Country>();
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
                    IsBusy = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void LookupUserRace()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostLookupUserRace();
                    string jsonStr = await t;
                    UserRaceProperty response = JsonConvert.DeserializeObject<UserRaceProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listUserRace = new List<UserRace>();
                        foreach (UserRace r in response.Data)
                        {
                            UserRace prop = new UserRace();
                            prop.user_race_id = r.user_race_id;
                            prop.user_race = r.user_race;
                            prop.search_id = r.user_race_id;
                            prop.search_name = r.user_race;
                            listUserRace.Add(prop);
                        }
                        RowCount = listUserRace.Count;
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
                        lvSearch.ItemsSource = listUserRace;
                    }
                    else
                    {
                        List<UserRace> list = new List<UserRace>();
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
                    IsBusy = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void LookupCardType()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostLookupCardType();
                    string jsonStr = await t;
                    CardTypeProperty response = JsonConvert.DeserializeObject<CardTypeProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listCardType = new List<CardType>();
                        foreach (CardType r in response.Data)
                        {
                            CardType prop = new CardType();
                            prop.card_type_id = r.card_type_id;
                            prop.card_type = r.card_type;
                            prop.search_id = r.card_type_id;
                            prop.search_name = r.card_type;
                            listCardType.Add(prop);
                        }
                        RowCount = listCardType.Count;
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
                        lvSearch.ItemsSource = listCardType;
                    }
                    else
                    {
                        List<CardType> list = new List<CardType>();
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
                    IsBusy = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void LookupReasonForAbsent()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostLookupReasonForAbsent();
                    string jsonStr = await t;
                    ReasonForAbsentProperty response = JsonConvert.DeserializeObject<ReasonForAbsentProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listReason = new List<ReasonForAbsent>();
                        foreach (ReasonForAbsent r in response.Data)
                        {
                            ReasonForAbsent prop = new ReasonForAbsent();
                            prop.reason_id = r.reason_id;
                            if (Settings.cultureInfo == "en-US") 
                            {
                                prop.reason_for_absent = r.reason_for_absent;
                            } 
                            else if (Settings.cultureInfo == "ms-MY") 
                            {
                                prop.reason_for_absent = r.reason_for_absent_bm;
                            }

                            prop.search_id = r.reason_id;
                            if (Settings.cultureInfo == "en-US")
                            {
                                prop.search_name = r.reason_for_absent;
                            }
                            else if (Settings.cultureInfo == "ms-MY")
                            {
                                prop.search_name = r.reason_for_absent_bm;
                            }

                            listReason.Add(prop);
                        }
                        RowCount = listReason.Count;
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
                        lvSearch.ItemsSource = listReason;
                    }
                    else
                    {
                        List<ReasonForAbsent> list = new List<ReasonForAbsent>();
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
                    IsBusy = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void LookupOccupation()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostLookupOccupation();
                    string jsonStr = await t;
                    OccupationProperty response = JsonConvert.DeserializeObject<OccupationProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listOccupation = new List<Occupation>();
                        foreach (Occupation r in response.Data)
                        {
                            Occupation prop = new Occupation();
                            prop.occupation_id = r.occupation_id;
                            prop.occupation = r.occupation;
                            prop.search_id = r.occupation_id;
                            prop.search_name = r.occupation;
                            listOccupation.Add(prop);
                        }
                        RowCount = listOccupation.Count;
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
                        lvSearch.ItemsSource = listOccupation;
                    }
                    else
                    {
                        List<Occupation> list = new List<Occupation>();
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
                    IsBusy = false;
                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }

        async void LookupProblemType()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    var t = srvc.PostLookupProblemType();
                    string jsonStr = await t;
                    ProblemTypeProperty response = JsonConvert.DeserializeObject<ProblemTypeProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listFeedback = new List<ProblemType>();
                        foreach (ProblemType r in response.Data)
                        {
                            ProblemType prop = new ProblemType();
                            prop.problem_type_id = r.problem_type_id;
                            if (Settings.cultureInfo == "en-US")
                            {
                                prop.problem_type = r.problem_type;
                            }
                            else if (Settings.cultureInfo == "ms-MY")
                            {
                                prop.problem_type = r.problem_type_bm;
                            }

                            prop.problem_type_bm = r.problem_type_bm;
                            prop.search_id = r.problem_type_id;
                            if (Settings.cultureInfo == "en-US")
                            {
                                prop.search_name = r.problem_type;
                            }
                            else if (Settings.cultureInfo == "ms-MY")
                            {
                                prop.search_name = r.problem_type_bm;
                            }

                            listFeedback.Add(prop);
                        }
                        RowCount = listFeedback.Count;
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
                        lvSearch.ItemsSource = listFeedback;
                    }
                    else
                    {
                        List<ProblemType> list = new List<ProblemType>();
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
                    IsBusy = false;
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
            if (_option == "state")
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    lvSearch.ItemsSource = listState;
                }

                else
                {
                    lvSearch.ItemsSource = listState.Where(x => x.search_name.ToLower().Contains(e.NewTextValue));
                    RowCount = listState.Where(x => x.search_name.ToLower().Contains(e.NewTextValue)).Count();
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
            else if (_option == "city")
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    lvSearch.ItemsSource = listCity;
                }
                else
                {
                    lvSearch.ItemsSource = listCity.Where(x => x.search_name.ToLower().Contains(e.NewTextValue));
                    RowCount = listCity.Where(x => x.search_name.ToLower().Contains(e.NewTextValue)).Count();
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
            else if (_option == "country" || _option == "nation") 
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    lvSearch.ItemsSource = listCountry;
                }

                else
                {
                    lvSearch.ItemsSource = listCountry.Where(x => x.search_name.ToLower().Contains(e.NewTextValue));
                    RowCount = listCountry.Where(x => x.search_name.ToLower().Contains(e.NewTextValue)).Count();
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
            else if (_option == "user-race")
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    lvSearch.ItemsSource = listUserRace;
                }

                else
                {
                    lvSearch.ItemsSource = listUserRace.Where(x => x.search_name.ToLower().Contains(e.NewTextValue));
                    RowCount = listUserRace.Where(x => x.search_name.ToLower().Contains(e.NewTextValue)).Count();
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
            else if (_option == "reason-for-absent")
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    lvSearch.ItemsSource = listReason;
                }

                else
                {
                    lvSearch.ItemsSource = listReason.Where(x => x.search_name.ToLower().Contains(e.NewTextValue));
                    RowCount = listReason.Where(x => x.search_name.ToLower().Contains(e.NewTextValue)).Count();
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
            else if (_option == "card-type")
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    lvSearch.ItemsSource = listCardType;
                }

                else
                {
                    lvSearch.ItemsSource = listCardType.Where(x => x.search_name.ToLower().Contains(e.NewTextValue));
                    RowCount = listCardType.Where(x => x.search_name.ToLower().Contains(e.NewTextValue)).Count();
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
            else if (_option == "shift")
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    lvSearch.ItemsSource = listShift;
                }

                else
                {
                    lvSearch.ItemsSource = listShift.Where(x => x.search_name.ToLower().Contains(e.NewTextValue));
                    RowCount = listShift.Where(x => x.search_name.ToLower().Contains(e.NewTextValue)).Count();
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
            else if (_option == "problem-type")
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    lvSearch.ItemsSource = listShift;
                }

                else
                {
                    lvSearch.ItemsSource = listFeedback.Where(x => x.search_name.ToLower().Contains(e.NewTextValue));
                    RowCount = listFeedback.Where(x => x.search_name.ToLower().Contains(e.NewTextValue)).Count();
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
            else if (_option == "occupation")
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    lvSearch.ItemsSource = listOccupation;
                }

                else
                {
                    lvSearch.ItemsSource = listOccupation.Where(x => x.search_name.ToLower().Contains(e.NewTextValue));
                    RowCount = listOccupation.Where(x => x.search_name.ToLower().Contains(e.NewTextValue)).Count();
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

        public StaffShift listShiftSelect;
        public State listStateSelect;
        public City listCitySelect;
        public Country listCountrySelect;
        public UserRace listUserRaceSelect;
        public ReasonForAbsent listReasonSelect;
        public CardType listCardTypeSelect;
        public ProblemType listFeedbackSelect;
        public Occupation listOccupationSelect;
        async void OnSearchSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (_option == "state")
            {
                var data = e.SelectedItem as State;
                if (data == null) return;
                listStateSelect = data;

                if (listStateSelect.search_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.stateId = listStateSelect.state_id;
                    Settings.stateName = listStateSelect.state_name;

                    ((ListView)sender).SelectedItem = null;

                    await Navigation.PopAsync();
                    if (_page == "add-student")
                    {
                        AddStudentPage1.Back = "Y";
                    }
                    else if (_page == "profile")
                    {
                        EditProfilePage.Back = "Y";
                        EditProfilePage.Option = _option;
                    }
                    else if (_page == "register")
                    {
                        RegisterPage.Back = "Y";
                        RegisterPage.Option = _option;
                    }
                    else if (_page == "kyc")
                    {
                        NewKYCPage.Back = "Y";
                        NewKYCPage.Option = _option;
                    }

                    OnDetailSet();
                }
            }
            else if (_option == "city")
            {
                var data = e.SelectedItem as City;
                if (data == null) return;
                listCitySelect = data;

                if (listCitySelect.search_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.cityId = listCitySelect.city_id;
                    Settings.cityName = listCitySelect.city_name;

                    ((ListView)sender).SelectedItem = null;

                    await Navigation.PopAsync();

                    if (_page == "register")
                    {
                        RegisterPage.Back = "Y";
                        RegisterPage.Option = _option;
                    }
                    else if (_page == "profile")
                    {
                        EditProfilePage.Back = "Y";
                        EditProfilePage.Option = _option;
                    }
                    else if (_page == "kyc")
                    {
                        NewKYCPage.Back = "Y";
                        NewKYCPage.Option = _option;
                    }

                    OnDetailSet();
                }
            }
            else if (_option == "country" || _option == "nation") 
            {
                var data = e.SelectedItem as Country;
                if (data == null) return;
                listCountrySelect = data;

                if (listCountrySelect.search_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.countryId = listCountrySelect.country_id;
                    Settings.countryName = listCountrySelect.country_name;

                    ((ListView)sender).SelectedItem = null;

                    await Navigation.PopAsync();

                    if (_page == "add-student")
                    {
                        AddStudentPage1.Back = "Y";
                    }
                    else if (_page == "profile")
                    {
                        EditProfilePage.Back = "Y";
                        EditProfilePage.Option = _option;
                    }
                    else if (_page == "create-password") 
                    {
                        Settings.countryCode = listCountrySelect.country_code;
                        CreatePasswordPage.Back = "Y";
                    }
                    else if (_page == "register")
                    {
                        RegisterPage.Back = "Y";
                        RegisterPage.Option = _option;
                    }
                    else if (_page == "registeracc")
                    {
                        RegisterAccountPage.Back = "Y";
                        RegisterAccountPage.Option = _option;
                    }
                    else if (_page == "kyc")
                    {
                        NewKYCPage.Back = "Y";
                        NewKYCPage.Option = _option;
                    }

                    OnDetailSet();
                }
            }
            else if (_option == "user-race")
            {
                var data = e.SelectedItem as UserRace;
                if (data == null) return;
                listUserRaceSelect = data;

                if (listUserRaceSelect.search_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.userRaceId = listUserRaceSelect.user_race_id;
                    Settings.userRace = listUserRaceSelect.user_race;

                    ((ListView)sender).SelectedItem = null;

                    await Navigation.PopAsync();

                    if (_page == "add-student")
                    {
                        AddStudentPage1.Back = "Y";
                    }
                    else if (_page == "profile")
                    {
                        EditProfilePage.Back = "Y";
                        EditProfilePage.Option = _option;
                    }

                    OnDetailSet();
                }
            }
            else if (_option == "reason-for-absent")
            {
                var data = e.SelectedItem as ReasonForAbsent;
                if (data == null) return;
                listReasonSelect = data;

                if (listReasonSelect.search_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.reasonId = listReasonSelect.reason_id;

                    ((ListView)sender).SelectedItem = null;

                    await Navigation.PopAsync();

                    if (_page == "class-attendance")
                    {
                        ClassAttendancePage.Back = "Y";
                    }
                    else if (_page == "school-attendance")
                    {
                        SchoolAttendancePage.Back = "Y";
                    }
                    else if (_page == "club-attendance")
                    {
                        ClubAttendancePage.Back = "Y";
                    }

                    OnDetailSet();
                }
            }
            else if (_option == "card-type")
            {
                var data = e.SelectedItem as CardType;
                if (data == null) return;
                listCardTypeSelect = data;

                if (listCardTypeSelect.search_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.cardTypeId = listCardTypeSelect.card_type_id;
                    Settings.cardType = listCardTypeSelect.card_type;

                    ((ListView)sender).SelectedItem = null;

                    await Navigation.PopAsync();

                    if (_page == "add-student")
                    {
                        AddStudentPage1.Back = "Y";
                    }
                    else if (_page == "profile")
                    {
                        EditProfilePage.Back = "Y";
                        EditProfilePage.Option = _option;
                    }
                    else if (_page == "register")
                    {
                        RegisterPage.Back = "Y";
                        RegisterPage.Option = _option;
                    }

                    OnDetailSet();
                }
            }
            else if (_option == "shift")
            {
                var data = e.SelectedItem as StaffShift;
                if (data == null) return;
                listShiftSelect = data;

                if (listShiftSelect.search_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.shiftId = listShiftSelect.shift_id;
                    Settings.shiftCode = listShiftSelect.shift_code;

                    ((ListView)sender).SelectedItem = null;

                    await Navigation.PopAsync();

                    if (_page == "attendance")
                    {
                        SchoolAttendanceView.Back = "Y";
                    }
                    else if (_page == "profile") 
                    {
                        StaffProfilePage.Back = "Y";
                    }

                    OnDetailSet();
                }
            }
            else if (_option == "occupation")
            {
                var data = e.SelectedItem as Occupation;
                if (data == null) return;
                listOccupationSelect = data;

                if (listOccupationSelect.search_id > 0)
                {
                    if (((ListView)sender).SelectedItem == null)
                        return;
                    //Do stuff here with the SelectedItem ...
                    Settings.occupationId = listOccupationSelect.occupation_id;
                    Settings.occupationName = listOccupationSelect.occupation;

                    ((ListView)sender).SelectedItem = null;

                    await Navigation.PopAsync();

                    if (_page == "register")
                    {
                        RegisterPage.Back = "Y";
                        RegisterPage.Option = _option;
                    }
                    else if (_page == "profile")
                    {
                        EditProfilePage.Back = "Y";
                        EditProfilePage.Option = _option;
                    }
                    else if (_page == "kyc")
                    {
                        NewKYCPage.Back = "Y";
                        NewKYCPage.Option = _option;
                    }

                    OnDetailSet();
                }
            }
        }

    }
}