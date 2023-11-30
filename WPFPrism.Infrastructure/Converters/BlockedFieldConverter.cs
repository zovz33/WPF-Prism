using System.Globalization;
using System.Windows.Data;

namespace WPFPrism.Infrastructure.Converters
{
    public class BlockedFieldConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверяем, является ли EditNameHint равным "Id"
            if (value is string editNameHint && editNameHint == "Id")
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
