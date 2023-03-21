using SmartSchoolsV2.Class;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageTabbedPage : TabbedPage
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;

        public int _staff_type_id;
        public int _user_role_id;
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

        public MessageTabbedPage()
        {
            InitializeComponent();
            BindingContext = this;

            try
            {
                base.OnAppearing();
                GetUserRole();
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex.Message);
            }
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

        public async Task GetUserRole()
        {
            _user_role_id = Settings.userRoleId;

            MessageContactPage contactPage = new MessageContactPage() { IconImageSource = "users.png" };
            if (_user_role_id == 7) //merchant
            {
                contactPage.Title = AppResources.ContactText;
            }
            else if (_user_role_id == 8) //staff
            {
                _staff_type_id = Settings.staffTypeId;
                if (_staff_type_id == 1 || _staff_type_id == 2)
                {
                    contactPage.Title = AppResources.ContactText;
                }
                else
                {
                    contactPage.Title = AppResources.ContactText;
                }
            }
            else if (_user_role_id == 9) //parent
            {
                contactPage.Title = AppResources.ContactText;
            }

            MessageSchoolGroupPage schoolPage = new MessageSchoolGroupPage() { IconImageSource = "school.png", Title = AppResources.Schooltext };
            MessageClassGroupPage classPage = new MessageClassGroupPage() { IconImageSource = "class.png", Title = AppResources.ClassText };
            MessageClubGroupPage clubPage = new MessageClubGroupPage() { IconImageSource = "trophy.png", Title = AppResources.ClubText };

            if (_user_role_id == 7) //merchant
            {

                Children.Add(contactPage);
                Children.Add(clubPage);
            }
            else if (_user_role_id == 8) //staff
            {
                _staff_type_id = Settings.staffTypeId;
                if (_staff_type_id == 1 || _staff_type_id == 2)
                {
                    Children.Add(contactPage);
                    Children.Add(schoolPage);
                    Children.Add(clubPage);
                }
                else 
                {
                    Children.Add(contactPage);
                    Children.Add(schoolPage);
                    Children.Add(classPage);
                    Children.Add(clubPage);
                }
            }
            else if (_user_role_id == 9) //parent
            {
                Children.Add(contactPage);
                Children.Add(classPage);
                Children.Add(clubPage);
            }
        }
    }
}