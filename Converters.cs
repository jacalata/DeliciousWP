using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data; //converter

namespace ToDelicious
{

    /// <summary>
    /// A generic boolean to object converter from 
    /// http://geekswithblogs.net/codingbloke/archive/2010/05/28/a-generic-boolean-value-converter.aspx
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BoolToValueConverter<T> : IValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return FalseValue;
            else
                return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? value.Equals(TrueValue) : false;
        }
    }

    /// <summary>
    /// specific instance of the generic bool converter
    /// used to make the 'selected' image visible on an event listing only if starred=true
    /// needs an equivalent resource in app.xaml
    /// </summary>
    public class BoolToVisibilityConverter : BoolToValueConverter<Visibility> { }

    // should I have one for 'enabling' things, or just make them invisible?
}
