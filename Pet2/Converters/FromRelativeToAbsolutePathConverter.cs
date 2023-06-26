using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace Pet2.Converters
{
    class FromRelativeToAbsolutePathConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            return value == null
                ? null
                : $@"{AppDomain.CurrentDomain.BaseDirectory}\{(string)value}";
        }

        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
