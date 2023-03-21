using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;
using System.Globalization;
using Xamarin.Essentials;
using System.Windows.Input;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddToCartPage1 : ContentPage, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
        public string requestUrl = Settings.requestUrl;
        public DateTime? MyDate { get; set; }
        public int RowCount { get; set; }
        public static Label l = new Label();
        public ObservableCollection<ParentStudentRelationship> listStudent { get; set; }

        public DateTime _pickup_date;
        public int _merchant_id;
        public int _merchant_type_id;
        public int _school_id;
        public string _school_name;
        public string _class_name;
        public string _full_name;
        public int _profile_id;
        public static string _option;
        public static string Option
        {
            get { return _option; }
            set { _option = value; }
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
                if (Option == "merchant")
                {
                    _merchant_id = Settings.selectMerchantId;
                    txtCompanyName.Text = Settings.selectMerchantType;
                    _merchant_type_id = Settings.selectMerchantTypeId;
                }

                if (_merchant_type_id != 4) 
                {
                    stackDeliveryDate.IsVisible = true;
                }

                btnNext1.IsVisible = true;
            }
        }
        public AddToCartPage1()
        {
            InitializeComponent();
            BindingContext = this;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ParentStudentRelationship();

            TimeSpan ts = new TimeSpan(7, 0, 0);
            if (DateTime.Now >= DateTime.Today.Add(ts))
            {
                txtPickupDate.Text = DateTime.Now.AddDays(1).ToString("dddd, dd MMM yyyy", ci);
                _pickup_date = DateTime.Today.AddDays(1);
                //txtPickupDate.Text = DateTime.Now.ToString("dddd, dd MMM yyyy", ci);
                //_pickup_date = DateTime.Today;
            }
            else
            {
                txtPickupDate.Text = DateTime.Now.ToString("dddd, dd MMM yyyy", ci);
                _pickup_date = DateTime.Today;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }

        async void StartCall(object sender, EventArgs args)
        {
            var page = new SearchListPage2(AppResources.SelectOperatorTypeText, "merchant", _school_id);
            page.DetailSet += this.OnDetailSet;
            await Navigation.PushAsync(page);
        }

        void StartCall2(object sender, EventArgs args)
        {
            txtPickupDate.Unfocus();
            Device.BeginInvokeOnMainThread(() =>
            {
                datePicker.Focus();
            });
        }

        private async void OnDeliveryDateSelected(object sender, DateChangedEventArgs e)
        {
            DayOfWeek day = e.NewDate.DayOfWeek;

            if (e.NewDate.Date > DateTime.Today.Date)
            {
                //if ((day == DayOfWeek.Saturday) || (day == DayOfWeek.Sunday))
                //{
                //    bool answer = await DisplayAlert("", AppResources.OrderForWeekendText, AppResources.YesText, AppResources.CancelText);
                //    if (answer == true)
                //    {
                //        txtPickupDate.Text = e.NewDate.ToString("dddd, dd MMM yyyy", ci);
                //        _pickup_date = e.NewDate;
                //    }
                //}
                //else
                //{
                txtPickupDate.Text = e.NewDate.ToString("dddd, dd MMM yyyy", ci);
                _pickup_date = e.NewDate;
                //}
            }
            else
            {
                if (e.NewDate.Date < DateTime.Today.Date)
                {
                    await DisplayAlert("", AppResources.BackdateNotAllowedText, null, "OK");
                }
                else
                {
                    if (DateTime.Now.TimeOfDay < TimeSpan.Parse("07:00"))
                    {
                        txtPickupDate.Text = e.NewDate.ToString("dddd, dd MMM yyyy", ci);
                        _pickup_date = e.NewDate;
                    }
                    else
                    {
                        await DisplayAlert("", AppResources.OrderMustBefore7Text, null, "OK");
                    }
                }
            }
        }

        public async Task ParentStudentRelationship()
        {
            if (conn.IsConnected() == true)
            {
                try
                {
                    IsBusy = true;
                    lvStudent.ItemsSource = null;
                    var t = srvc.PostParentStudentRelationship(Settings.parentId);
                    string jsonStr = await t;
                    ParentStudentRelationshipProperty response = JsonConvert.DeserializeObject<ParentStudentRelationshipProperty>(jsonStr);
                    if (response.Success == true)
                    {
                        listStudent = new ObservableCollection<ParentStudentRelationship>();
                        foreach (ParentStudentRelationship sl in response.Data)
                        {
                            ParentStudentRelationship post = new ParentStudentRelationship();
                            post.student_id = sl.student_id;
                            post.profile_id = sl.profile_id;
                            post.student_number = sl.student_number;
                            post.full_name = sl.full_name;
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url_student = sl.photo_url;
                                post.photo_url = requestUrl + sl.photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.image_visible = false;
                                post.initial_visible = true;
                            }
                            if (sl.class_id > 0)
                            {
                                post.class_id = sl.class_id;
                                post.class_name = sl.class_name;
                                post.dot_visible = true;
                            }
                            else
                            {
                                post.dot_visible = false;
                            }
                            post.is_selected = false;
                            post.school_type_id = sl.school_type_id;
                            post.school_type = sl.school_type;
                            listStudent.Add(post);
                        }
                        RowCount = listStudent.Count;
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        if (RowCount > 0)
                        {
                            l.Text = RowCount + AppResources.RecordText;
                        }
                        else
                        {
                            l.Text = AppResources.NoRecordFoundText;
                        }
                        lvStudent.Footer = l;
                        lvStudent.ItemsSource = listStudent;
                    }
                    else
                    {
                        listStudent = new ObservableCollection<ParentStudentRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvStudent.Footer = l;
                        lvStudent.ItemsSource = listStudent;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error : " + ex.Message);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        SnackB.Message = AppResources.SomethingWrongText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    });
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SnackB.Message = AppResources.CheckInternetText;
                    SnackB.IsOpen = !SnackB.IsOpen;
                });
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

        public ICommand RemoveCommand => new Command(async (item) => await ExecuteRemoveCommand(item));
        private async Task ExecuteRemoveCommand(object item)
        {
            lblSelectStudent.Text = AppResources.SelectStudentText;
            lvStudent.IsVisible = true;

            await ParentStudentRelationship();

            gridProfile.IsVisible = false;
            stackMerchant.IsVisible = false;
            txtCompanyName.Text = string.Empty;
            _school_id = 0;
            _merchant_id = 0;
            _school_name = string.Empty;
            _class_name = string.Empty;
            _profile_id = 0;
            _full_name = string.Empty;

            stackDeliveryDate.IsVisible = false;

            TimeSpan ts = new TimeSpan(7, 0, 0);
            if (DateTime.Now >= DateTime.Today.Add(ts))
            {
                txtPickupDate.Text = DateTime.Now.AddDays(1).ToString("dddd, dd MMM yyyy", ci);
                _pickup_date = DateTime.Today.AddDays(1);
            }
            else
            {
                txtPickupDate.Text = DateTime.Now.ToString("dddd, dd MMM yyyy", ci);
                _pickup_date = DateTime.Today;
            }

            btnNext1.IsVisible = false;
        }

        void OnStudentSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as ParentStudentRelationship;
            if (data == null) return;

            if (data.student_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;

                lblSelectStudent.Text = AppResources.StudentText;
                lvStudent.IsVisible = false;

                gridProfile.IsVisible = true;
                stackMerchant.IsVisible = true;
                _school_id = data.school_id;
                _school_name = data.school_name;
                _class_name = data.class_name;
                _profile_id = data.profile_id;
                _full_name = data.full_name;

                lblFullName.Text = data.full_name;
                lblSchoolName.Text = data.school_name;
                lblClassName.Text = data.class_name;

                if (!string.IsNullOrEmpty(data.photo_url))
                {
                    userInitial.IsVisible = false;
                    userImage.IsVisible = true;
                    userImage.Source = requestUrl + data.photo_url;
                }
                else
                {
                    userInitial.IsVisible = true;
                }
                //if (!string.IsNullOrWhiteSpace(txtPickupDate.Text))
                //{
                //    await Navigation.PushAsync(new AddToCartPage2(data.school_id,data.school_name, data.class_name, data.profile_id, data.full_name, _pickup_date, DateTime.MinValue.TimeOfDay, 1, string.Empty));
                //}
                //else {
                //    SnackB.Message = AppResources.DeliveryDateRequiredText;
                //    SnackB.IsOpen = !SnackB.IsOpen;
                //}

                ((ListView)sender).SelectedItem = null;
            }
        }

        async void OnNextClicked(object sender, EventArgs args)
        {
            if (_merchant_type_id != 4)
            {
                //if (!string.IsNullOrWhiteSpace(txtPickupDate.Text))
                //{
                    await Navigation.PushAsync(new AddToCartPage2(_school_id, _school_name, _class_name, _profile_id, _full_name, _pickup_date, DateTime.MinValue.TimeOfDay, 1, string.Empty, _merchant_id));
                //}
                //else
                //{
                //    SnackB.Message = AppResources.DeliveryDateRequiredText;
                //    SnackB.IsOpen = !SnackB.IsOpen;
                //}
            }
            else 
            {
                await Navigation.PushAsync(new AddToCartPage2(_school_id, _school_name, _class_name, _profile_id, _full_name, _pickup_date, DateTime.MinValue.TimeOfDay, 1, string.Empty, _merchant_id));
            }
        }
    }
}