using System.Windows.Input;
using Xamarin.Forms;

namespace SmartSchoolsV2.Customs
{
    public class ExtendedEditor : Editor
    {
        public static readonly BindableProperty CompleteCommandProperty =
           BindableProperty.Create("CompleteCommand", typeof(ICommand),
               typeof(ExtendedEditor), default(ICommand),
               BindingMode.OneWay, null);

        public ICommand CompleteCommand
        {
            get { return (ICommand)GetValue(CompleteCommandProperty); }
            set { SetValue(CompleteCommandProperty, value); }
        }

        public static BindableProperty PlaceholderProperty
          = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(ExtendedEditor));

        public static BindableProperty PlaceholderColorProperty
           = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(ExtendedEditor), Color.LightGray);

        public static BindableProperty BorderColorProperty
            = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ExtendedEditor), Color.LightGray);

        public static BindableProperty HasRoundedCornerProperty
        = BindableProperty.Create(nameof(HasRoundedCorner), typeof(bool), typeof(ExtendedEditor), false);

        public static BindableProperty IsExpandableProperty
        = BindableProperty.Create(nameof(IsExpandable), typeof(bool), typeof(ExtendedEditor), false);

        public bool IsExpandable
        {
            get { return (bool)GetValue(IsExpandableProperty); }
            set { SetValue(IsExpandableProperty, value); }
        }
        public bool HasRoundedCorner
        {
            get { return (bool)GetValue(HasRoundedCornerProperty); }
            set { SetValue(HasRoundedCornerProperty, value); }
        }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public ExtendedEditor()
        {
            TextChanged += OnTextChanged;
        }

        ~ExtendedEditor()
        {
            TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsExpandable) InvalidateMeasure();
        }
    }
}
