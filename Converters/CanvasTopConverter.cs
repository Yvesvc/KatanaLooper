using System;
using System.Globalization;
using System.Windows.Data;

namespace KatanaLooper.Converters
{
    public class CanvasTopConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double? heightOfCanvas = values[0] as double?;
            double? heightOfChild = values[1] as double?;
            return (heightOfCanvas - heightOfChild) / 2;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}