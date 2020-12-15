// <copyright file="DoubleStringConverter.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows.Data;

namespace Memento
{
    /// <summary>
    /// Converts from double to String.
    /// </summary>
    public class DoubleStringConverter : IValueConverter
    {
        /// <summary>
        /// Convert from double to string.
        /// </summary>
        /// <param name="value">value of passing object.</param>
        /// <param name="targetType">check what type it is.</param>
        /// <param name="parameter">parameter.</param>
        /// <param name="culture">culture.</param>
        /// <returns>string representation of double.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is double number ? Math.Round(number, 2).ToString(culture) : String.Empty;
        }

        /// <summary>
        /// Convert from string to double.
        /// </summary>
        /// <param name="value">value of passing object.</param>
        /// <param name="targetType">check what type it is.</param>
        /// <param name="parameter">parameter.</param>
        /// <param name="culture">culture.</param>
        /// <returns>string representation of double.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
