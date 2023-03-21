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
    public partial class StudentOutingRequestView : ContentView
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<StudentOuting> listOuting;
        public ObservableCollection<OutingGroup> Outings { get; private set; } = new ObservableCollection<OutingGroup>();
        public class OutingGroup : ObservableCollection<StudentOuting>
        {
            public string outing_month { get; private set; }

            public OutingGroup(string outingMonth, ObservableCollection<StudentOuting> outings) : base(outings)
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
        public static Command LoadStudentOutingRequest { get; set; }
        public StudentOutingRequestView()
        {
            InitializeComponent();
            BindingContext = this;

            LoadStudentOutingRequest = new Command(async () => await StudentOutingRequest(Settings.studentId, Settings.studentSchoolId));

            var tapGestureRecognizerTxn = new TapGestureRecognizer();
            tapGestureRecognizerTxn.Tapped += (s, e) => {
                StudentOuting value = new StudentOuting();
                Navigation.PushAsync(new StudentOutingRequestFormPage(Settings.studentProfileId, "new", value));
            };
            btnNewOuting.GestureRecognizers.Add(tapGestureRecognizerTxn);
        }

        public async Task StudentOutingRequest(int student_id, int school_id)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;

                    var t = srvc.PostStudentOutingRequestMonthGroup(student_id, school_id);
                    string jsonStr = await t;
                    StudentOutingGroupProperty response = JsonConvert.DeserializeObject<StudentOutingGroupProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        if (response.Data.Count > 0)
                        {
                            Outings.Clear();

                            foreach (StudentOutingGroup sl in response.Data)
                            {
                                await PostStudentOutingRequestMonth(student_id, school_id, sl.outing_month);

                                Outings.Add(new OutingGroup(sl.outing_month.ToString("MMMM yyyy"), listOuting));
                            }
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

        public async Task PostStudentOutingRequestMonth(int student_id, int school_id, DateTime outing_month)
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    var t = srvc.PostStudentOutingRequestMonth(student_id, school_id, outing_month);
                    string jsonStr = await t;
                    StudentOutingProperty response = JsonConvert.DeserializeObject<StudentOutingProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listOuting = new ObservableCollection<StudentOuting>();
                        foreach (StudentOuting sl in response.Data)
                        {
                            StudentOuting post = new StudentOuting();
                            post.outing_id = sl.outing_id;
                            post.student_id = sl.student_id;
                            post.full_name = sl.full_name;
                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url = requestUrl + sl.photo_url;
                            }
                            else
                            {
                                post.photo_url = "";
                            }
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            post.outing_type_id = sl.outing_type_id;
                            post.outing_type = sl.outing_type;
                            post.check_out_date = sl.check_out_date;
                            post.check_in_date = sl.check_in_date;
                            post.outing_reason = sl.outing_reason;
                            post.outing_status_id = sl.outing_status_id;
                            post.outing_status = sl.outing_status;
                            post.check_out_at = sl.check_out_at;
                            post.check_in_at = sl.check_in_at;
                            post.request_by_id = sl.request_by_id;
                            post.request_by = sl.request_by;
                            post.request_by_user_role_id = sl.request_by_user_role_id;
                            post.request_by_user_role = sl.request_by_user_role;
                            post.approve_by_id = sl.approve_by_id;
                            post.approve_by = sl.approve_by;
                            post.approve_at = sl.approve_at;
                            post.approve_comment = sl.approve_comment;
                            post.outing_date = "Date: " + sl.check_out_date.ToString("dd/MM/yyyy") + " - " + sl.check_in_date.ToString("dd/MM/yyyy");
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
            var current = e.CurrentSelection.FirstOrDefault() as StudentOuting;

            if (current.outing_id > 0)
            {
                if (current.outing_status_id == 1)
                {
                    await Navigation.PushAsync(new StudentOutingRequestFormPage(Settings.studentProfileId, "edit", current));
                }
                else 
                {
                    await Navigation.PushAsync(new StudentOutingRequestViewPage(Settings.studentProfileId, current));
                }
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