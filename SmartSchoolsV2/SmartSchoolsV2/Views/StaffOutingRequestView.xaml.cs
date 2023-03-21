using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Models;
using System;
using SmartSchoolsV2.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Popup;
using Rg.Plugins.Popup.Extensions;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffOutingRequestView : ContentView
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<StaffOutingMonth> listOuting;
        public ObservableCollection<OutingGroup> Outings { get; private set; } = new ObservableCollection<OutingGroup>();
        public class OutingGroup : ObservableCollection<StaffOutingMonth>
        {
            public string outing_month { get; private set; }

            public OutingGroup(string outingMonth, ObservableCollection<StaffOutingMonth> outings) : base(outings)
            {
                outing_month = outingMonth;
            }
        }
        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public static Command LoadStaffOutingRequest { get; set; }
        public StaffOutingRequestView()
        {
            InitializeComponent();
            BindingContext = this;

            LoadStaffOutingRequest = new Command(async () => await StaffOutingRequest(2, Settings.schoolId));

            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) => {
                StudentOuting value = new StudentOuting();
                Navigation.PushAsync(new StaffOutingRequestTabbedPage());
            };
            btnHistory.GestureRecognizers.Add(tapGestureRecognizerTxn);
        }

        public async Task StaffOutingRequest(int outing_status_id, int school_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;

                    var t = srvc.PostStaffOutingRequestMonthGroup(outing_status_id, school_id);
                    string jsonStr = await t;
                    StudentOutingGroupProperty response = JsonConvert.DeserializeObject<StudentOutingGroupProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        if (response.Data.Count > 0)
                        {
                            Outings.Clear();

                            foreach (StudentOutingGroup sl in response.Data)
                            {
                                await StaffOutingRequestMonth(outing_status_id, school_id, sl.outing_month);

                                Outings.Add(new OutingGroup(sl.outing_month.ToString("MMMM yyyy"), listOuting));
                            }
                        }
                        else 
                        {
                            Outings.Clear();
                            listOuting.Clear();
                        }

                    }
                    else
                    {

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

        public async Task StaffOutingRequestMonth(int outing_status_id, int school_id, DateTime outing_month)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostStaffOutingRequestMonth(outing_status_id, school_id, outing_month);
                    string jsonStr = await t;
                    StaffOutingMonthProperty response = JsonConvert.DeserializeObject<StaffOutingMonthProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        int total_daily_outing = 0;
                        int total_overnight = 0;
                        listOuting = new ObservableCollection<StaffOutingMonth>();
                        foreach (StaffOutingMonth sl in response.Data)
                        {
                            StaffOutingMonth post = new StaffOutingMonth();
                            post.school_id = sl.school_id;
                            post.outing_type_id = sl.outing_type_id;
                            post.outing_type = sl.outing_type;
                            post.outing_status_id = sl.outing_status_id;
                            post.outing_status = sl.outing_status;
                            post.outing_month = sl.outing_month;
                            if (sl.outing_type_id == 1) {
                                total_daily_outing++;
                                post.outing_application = total_daily_outing.ToString() + " Applications";
                            }
                            else if (sl.outing_type_id == 2)
                            {
                                total_overnight++;
                                post.outing_application = total_overnight.ToString() + " Applications";
                            }

                            listOuting.Add(post);
                        }
                    }
                    else
                    {

                    }
                }
                catch (Exception)
                {
                    //SnackB.Message = AppResources.SomethingWrongText;
                    //SnackB.IsOpen = !SnackB.IsOpen;
                }
                finally
                {

                }
            }
            else
            {
                //SnackB.Message = AppResources.CheckInternetText;
                //SnackB.IsOpen = !SnackB.IsOpen;
            }
        }


        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var previous = e.PreviousSelection.FirstOrDefault() as ShoppingCart;
            var current = e.CurrentSelection.FirstOrDefault() as StaffOutingMonth;

            if (current.school_id > 0)
            {
                await Navigation.PushAsync(new StaffOutingRequestListPage(current.outing_status, current.outing_type_id, current.outing_type, current.outing_month));
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