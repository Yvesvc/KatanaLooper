using System;
using System.Globalization;
using System.Windows.Data;

namespace KatanaLooper.Converters
{
    public class CanvasTopConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double? heightcontrol = values[0] as double?;
            double? heightChildControl = values[1] as double?;
            return (heightcontrol - heightChildControl) / 2;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}