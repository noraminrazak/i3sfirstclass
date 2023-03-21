using Plugin.StoreReview;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Resources;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpCenterPage1 : ContentPage
    {
        public int _user_role_id = Settings.userRoleId;
        public List<HelpCenter> helpCenter;
        public class HelpCenter
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class HelpCenterVM
        {
            public List<HelpCenter> HelpCenters { get; set; }
        }
        public HelpCenterPage1()
        {
            InitializeComponent();
            BindingContext = this;

            if (_user_role_id == 8)
            {
                helpCenter = new List<HelpCenter>
                {
                    new HelpCenter {Id = 1, Name = AppResources.ReportLostCardText},
                    new HelpCenter {Id = 2, Name = AppResources.CardReplacementText},
                    //new HelpCenter {Id = 3, Name = AppResources.ChangeCardPINText},
                    new HelpCenter {Id = 3, Name = AppResources.AccountStatus},
                    //new HelpCenter {Id = 4, Name = AppResources.ResubmitDocumentText},
                    new HelpCenter {Id = 5, Name = AppResources.LeaveReviewText},
                    new HelpCenter {Id = 6, Name = AppResources.ComplaintFormText},
                    new HelpCenter {Id = 7, Name = AppResources.FAQText},
                    new HelpCenter {Id = 8, Name = AppResources.DeleteMyAccountText}
                };
            }
            else if (_user_role_id == 9)
            {
                helpCenter = new List<HelpCenter>
                {
                    //new HelpCenter {Id = 3, Name = AppResources.ChangeCardPINText},
                    new HelpCenter {Id = 3, Name = AppResources.AccountStatus},
                    //new HelpCenter {Id = 4, Name = AppResources.ResubmitDocumentText},
                    new HelpCenter {Id = 5, Name = AppResources.LeaveReviewText},
                    new HelpCenter {Id = 6, Name = AppResources.ComplaintFormText},
                    new HelpCenter {Id = 7, Name = AppResources.FAQText},
                    new HelpCenter {Id = 8, Name = AppResources.DeleteMyAccountText}
                };
            }
            else if (_user_role_id == 7)
            {
                helpCenter = new List<HelpCenter>
                {
                    new HelpCenter {Id = 5, Name = AppResources.LeaveReviewText},
                    new HelpCenter {Id = 6, Name = AppResources.ComplaintFormText},
                    new HelpCenter {Id = 7, Name = AppResources.FAQText},
                    new HelpCenter {Id = 8, Name = AppResources.DeleteMyAccountText}
                };
            }

            lvHelpCenter.ItemsSource = helpCenter;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public HelpCenter helps;
        async void OnNameSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as HelpCenter;
            if (data == null) return;
            helps = data;

            if (helps.Id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                if (helps.Id == 1)
                {
                    await Navigation.PushAsync(new StaffCardLostReportPage());
                }
                else if (helps.Id == 2)
                {
                    await Navigation.PushAsync(new StaffCardReplacementPage());
                }
                else if (helps.Id == 5)
                {
                    await CrossStoreReview.Current.RequestReview(false);
                } 
                else if (helps.Id == 6) 
                {
                    //await Navigation.PushAsync(new FeedbackPage());
                    await Navigation.PushAsync(new SubmitTicketPage());
                }
                //else if (helps.Id == 4)
                //{
                //    await Navigation.PushAsync(new ResubmitKYCPage(10));
                //}
                //else if (helps.Id == 3)
                //{
                //    await Navigation.PushAsync(new VerifyAccountPage2(Settings.userName));
                //}
                else if (helps.Id == 3)
                {
                    await Navigation.PushAsync(new AccountStatusPage());
                }
                else if (helps.Id == 7)
                {
                    await Navigation.PushAsync(new FAQPage());
                }
                else if (helps.Id == 8)
                {
                    await Navigation.PushAsync(new DeleteAccountPage());
                }

                ((ListView)sender).SelectedItem = null;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            GC.Collect();
        }
    }
}