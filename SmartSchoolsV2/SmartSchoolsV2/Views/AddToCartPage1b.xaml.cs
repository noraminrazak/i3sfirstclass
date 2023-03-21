using SmartSchoolsV2.Class;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.ComponentModel;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddToCartPage1b : ContentPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        CultureInfo ci = new CultureInfo(Settings.cultureInfo);
        public string requestUrl = Settings.requestUrl;
        public DateTime _pickup_date;
        public TimeSpan _pickup_time;
        public int _service_method_id;
        public int _merchant_id;
        public int _merchant_type_id;
        public string _delivery_location;
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
                    stackTiming.IsVisible = true;
                }

                btnNext1.IsVisible = true;
            }
        }
        public AddToCartPage1b()
        {
            InitializeComponent();
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            _service_method_id = 2;

            TimeSpan ts = new TimeSpan(0, 0, 1);
            if (DateTime.Now >= DateTime.Today.Add(ts))
            {
                //txtPickupDate.Text = DateTime.Now.AddDays(1).ToString("dddd, dd MMM yyyy", ci);
                //_pickup_date = DateTime.Today.AddDays(1);
                txtPickupDate.Text = DateTime.Now.ToString("dddd, dd MMM yyyy", ci);
                _pickup_date = DateTime.Today;
            }
            else
            {
                txtPickupDate.Text = DateTime.Now.ToString("dddd, dd MMM yyyy", ci);
                _pickup_date = DateTime.Today;
            }

        }

        async void StartCall(object sender, EventArgs args)
        {
            var page = new SearchListPage2(AppResources.SelectOperatorTypeText, "merchant", Settings.schoolId);
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

            if (e.NewDate.Date >= DateTime.Today.Date)
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
                //if (e.NewDate.Date < DateTime.Today.Date)
                //{
                    await DisplayAlert("", AppResources.BackdateNotAllowedText, null, "OK");
                //}
                //else
                //{
                    //if (DateTime.Now.TimeOfDay < TimeSpan.Parse("07:00"))
                    //{
                    //   txtPickupDate.Text = e.NewDate.ToString("dddd, dd MMM yyyy", ci);
                    //    _pickup_date = e.NewDate;
                    //}
                    //else
                    //{
                    //    await DisplayAlert("", AppResources.OrderMustBefore7Text, null, "OK");
                    //}
                //}

            }
        }

        void StartCall3(object sender, EventArgs args)
        {
            txtPickupTime.Unfocus();
            DateTime dt = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 40, 0);
            timePicker.Time = DateTime.Now.TimeOfDay + ts;
            Device.BeginInvokeOnMainThread(() => {
                timePicker.Focus();
            });
        }
        async void OnTimePickerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Time")
            {
                DateTime myTime = (DateTime)(_pickup_date + timePicker.Time);
                TimeSpan ts = new TimeSpan(6, 59, 0);

                //if (DateTime.Now.AddMinutes(35) >= myTime)
                //if (_pickup_date.Add(ts) >=  myTime)
                //{
                //    await DisplayAlert("", AppResources.OrderTimingAlreadyPastText, null, "OK");
                //}
                //else
                //{
                    //string hrs = (myTime - DateTime.Now).Hours.ToString();
                    //string mins = (myTime - DateTime.Now).Minutes.ToString();

                    DateTime time = DateTime.Today.Add(timePicker.Time);
                    txtPickupTime.Text = time.ToString("hh:mm tt"); // It will give "03:00 AM"
                    _pickup_time = timePicker.Time;
                //}
            }
        }

        void OnNextClicked(object sender, EventArgs args)
        {

            //if (!string.IsNullOrEmpty(txtCompanyName.Text))
            //{
                //if (!string.IsNullOrEmpty(txtPickupDate.Text))
                //{
                    if (!string.IsNullOrEmpty(txtPickupTime.Text))
                    {
                        GetCartPage2();
                    }
                    else
                    {
                        SnackB.Message = AppResources.OrderTimingRequiredText;
                        SnackB.IsOpen = !SnackB.IsOpen;
                    }
                //}
                //else
                //{
                //    SnackB.Message = AppResources.DeliveryDateRequiredText;
                //    SnackB.IsOpen = !SnackB.IsOpen;
                //}
            //}
            //else
            //{
            //    SnackB.Message = AppResources.OperatorTypeRequiredText;
            //    SnackB.IsOpen = !SnackB.IsOpen;
            //}
        }

        public async void GetCartPage2() 
        {
            //if (!string.IsNullOrEmpty(txtLocation.Text))
            //{
            //    _delivery_location = txtLocation.Text.Trim();
            //}
            //else {
            //    _delivery_location = string.Empty;
            //}

            await Navigation.PushAsync(new AddToCartPage2(Settings.schoolId, Settings.schoolName, string.Empty, Settings.profileId, Settings.fullName, _pickup_date, _pickup_time, _service_method_id, _delivery_location, _merchant_id));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
    }
}