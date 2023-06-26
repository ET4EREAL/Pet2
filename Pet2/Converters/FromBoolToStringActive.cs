using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace Pet2.Converters
{
    class FromBoolToStringActive : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            /*
            string text;
            if ((bool)value) text = "Актуален";
            else text = "Не актуален";
            return text;*/
            return ((bool)value) ? "Оплачено" : "Не оплачено";
        }

        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
