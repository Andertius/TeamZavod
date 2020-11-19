// <copyright file="IntStringConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Memento.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// A binding converter.
    /// </summary>
    public class IntStringConverter : IValueConverter
    {
        /// <summary>
        /// Convert from int to string.
        /// </summary>
        /// <returns>A string converted from an int.</returns>
        /// <param name="value">The int value that should be converted to string.</param>
        /// <param name="targetType">The type to be converted to.</param>
        /// <param name="parameter">Additional parameter.</param>
        /// <param name="culture">The culture of the string.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is int number ? number.ToString() : string.Empty;
        }

        /// <summary>
        /// Convert from string to int.
        /// </summary>
        /// <returns>An int converted back from an string.</returns>
        /// <param name="value">The string value that should be converted back to int.</param>
        /// <param name="targetType">The type to be converted to.</param>
        /// <param name="parameter">Additional parameter.</param>
        /// <param name="culture">The culture of the string.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
