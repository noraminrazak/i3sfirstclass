using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SmartSchoolsV2.Class;
using SmartSchoolsV2.Resources;
using SmartSchoolsV2.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartSchoolsV2.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorPickerPopupPage : PopupPage
    {
        public delegate void SetDetailEventHandler(object source, EventArgs args);
        public event SetDetailEventHandler DetailSet;
        public string _header = string.Empty;
        ViewCell lastCell;
        protected virtual void OnDetailSet()
        {
            if (DetailSet != null)
            {
                DetailSet(this, EventArgs.Empty);
            }
        }

        public class ColorDict
        {
            public string color_name { get; set; }
            public Color color { get; set; }
        }
        public IList<ColorDict> colorDict { get; private set; }

        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.Transparent;
                lastCell = viewCell;
            }
        }

        public ColorPickerPopupPage(string header)
        {
            InitializeComponent();

            Settings.textColor = "";
            Settings.bgColor = "";

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                OnCloseButtonTapped(s, e);
            };
            //CloseImage.GestureRecognizers.Add(tapGestureRecognizer);

            listView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
            {
                var item = (ColorDict)e.SelectedItem;
                //DisplayAlert("ItemSelected", item.merchant_id.ToString(), "OK");
                if (header == "Text")
                {
                    Settings.textColor = item.color.ToHex();
                    _header = AppResources.ColorPickerTitleTextText;
                }
                else
                {
                    Settings.bgColor = item.color.ToHex();
                    _header = AppResources.ColorPickerTitleBgText;
                }

                CloseAllPopup();
                ProductDetailPage.Back = "Y";
                OnDetailSet();
            };

            lblHeader.Text = _header;
            colorDict = new List<ColorDict>();
            colorDict.Add(new ColorDict { color_name = AppResources.BlackText, color = Color.FromHex("#FF000000") });
            colorDict.Add(new ColorDict { color_name = AppResources.WhiteText, color = Color.FromHex("#FFFFFFFF") });
            colorDict.Add(new ColorDict { color_name = AppResources.RedText, color = Color.FromHex("#FFFF0000") });
            colorDict.Add(new ColorDict { color_name = AppResources.GreenText, color = Color.FromHex("#FF008800") });
            colorDict.Add(new ColorDict { color_name = AppResources.YellowText, color = Color.FromHex("#FFFFFF00") });
            colorDict.Add(new ColorDict { color_name = AppResources.BlueText, color = Color.FromHex("#FF0000FF") });
            colorDict.Add(new ColorDict { color_name = AppResources.BrownText, color = Color.FromHex("#FFA52A2A") });
            colorDict.Add(new ColorDict { color_name = AppResources.OrangeText, color = Color.FromHex("#FFFF8C00") });
            colorDict.Add(new ColorDict { color_name = AppResources.PinkText, color = Color.FromHex("#FFFF69B4") });
            colorDict.Add(new ColorDict { color_name = AppResources.PurpleText, color = Color.FromHex("#FF800080") });
            colorDict.Add(new ColorDict { color_name = AppResources.GreenText, color = Color.FromHex("#FFA9A9A9") });
            listView.ItemsSource = colorDict;

        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }

        protected override bool OnBackgroundClicked()
        {
            CloseAllPopup();
            return false;
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}