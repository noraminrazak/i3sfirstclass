using Newtonsoft.Json;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Services;
using SmartSchoolsV2.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SmartSchoolsV2.Resources;

namespace SmartSchoolsV2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WalletView : ContentView, INotifyPropertyChanged
    {
        Connection conn = new Connection();
        ServiceWrapper srvc = new ServiceWrapper();
        public string requestUrl = Settings.requestUrl;
        public int RowCount { get; set; }
        public static Label l = new Label();
        public static Command LoadParentStudentRelationship { get; set; }
        ViewCell lastCell;
        bool isBusy;
        private ObservableCollection<ParentStudentRelationship> listWallet { get; set; }
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        public WalletView ()
		{
			InitializeComponent ();
            this.BindingContext = this;
            LoadParentStudentRelationship = new Command(async () => await ParentStudentRelationship());
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await ParentStudentRelationship();
                });
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
                        listWallet = new ObservableCollection<ParentStudentRelationship>();
                        foreach (ParentStudentRelationship sl in response.Data)
                        {
                            ParentStudentRelationship post = new ParentStudentRelationship();
                            post.student_id = sl.student_id;
                            post.profile_id = sl.profile_id;
                            post.student_number = sl.student_number;
                            post.full_name = sl.full_name;
                            post.school_id = sl.school_id;
                            post.school_name = sl.school_name;
                            post.school_type_id = sl.school_type_id;
                            post.school_type = sl.school_type;
                            post.class_id = sl.class_id;
                            post.class_name = sl.class_name;
                            if (!string.IsNullOrEmpty(sl.photo_url))
                            {
                                post.photo_url = requestUrl + sl.photo_url;
                                post.image_visible = true;
                                post.initial_visible = false;
                            }
                            else
                            {
                                post.image_visible = false;
                                post.initial_visible = true;
                            }
                            if (sl.card_id > 0)
                            {
                                post.card_id = sl.card_id;
                                post.card_number = sl.card_number;
                                post.card_status_id = sl.card_status_id;
                                post.card_status = sl.card_status;
                            }
                            if (sl.wallet_id > 0)
                            {
                                post.wallet_id = sl.wallet_id;
                                post.wallet_number = sl.wallet_number;
                                post.account_balance = "RM " + sl.account_balance;
                                post.dot_visible = true;
                            }
                            else
                            {
                                post.wallet_number = "N/A";
                                post.account_balance = "RM 0.00";
                                post.dot_visible = false;
                            }
                            listWallet.Add(post);
                        }
                        RowCount = listWallet.Count;
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
                        lvStudent.ItemsSource = listWallet;
                    }
                    else
                    {
                        listWallet = new ObservableCollection<ParentStudentRelationship>();
                        l.HorizontalTextAlignment = TextAlignment.Center;
                        l.Text = AppResources.NoRecordFoundText;
                        lvStudent.Footer = l;
                        lvStudent.ItemsSource = listWallet;
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
        public ParentStudentRelationship wallet;
        async void OnWalletSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var data = e.SelectedItem as ParentStudentRelationship;
            if (data == null) return;
            wallet = data;

            if (wallet.student_id > 0)
            {
                if (((ListView)sender).SelectedItem == null)
                    return;
                //Do stuff here with the SelectedItem ...
                Settings.studentWalletId = data.wallet_id;
                Settings.studentWalletNumber = data.wallet_number;
                Settings.studentCardId = data.card_id;
                Settings.studentCardNumber = data.card_number;
                Settings.studentCardStatusId = data.card_status_id;
                Settings.studentCardStatus = data.card_status;
                Settings.studentId = data.student_id;

                await Navigation.PushAsync(new WalletPage(data.student_id,
                    data.profile_id,
                    data.full_name,
                    data.wallet_number,
                    data.account_balance,
                    data.photo_url,
                    data.school_id,
                    data.school_name,
                    data.school_type_id,
                    data.class_id,
                    data.class_name));

                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}