using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartSchoolsV2.Converters
{
    public class BoolToColorMessage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                if ((bool)value)
                    return "#C5FFB8";

            return "#FFFFFF";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
