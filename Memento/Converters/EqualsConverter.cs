// <copyright file="EqualsConverter.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows.Data;

namespace Memento.Converters
{
    /// <summary>
    /// A binding converter.
    /// </summary>
    public class EqualsConverter : IValueConverter
    {
        /// <summary>
        /// Convert to bool by comparing the value to the parameter.
        /// </summary>
        /// <returns>A bool.</returns>
        /// <param name="value">An enum that the parameter will be compared to.</param>
        /// <param name="targetType">The type to be converted to.</param>
        /// <param name="parameter">An enum that the value will be compared to.</param>
        /// <param name="culture">The culture, i guess.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(parameter);
        }

        /// <summary>
        /// Convert to an appropriate enum depending on the parameter.
        /// </summary>
        /// <returns>An appropriate enum.</returns>
        /// <param name="value">A bool, that should be true to return an enum.</param>
        /// <param name="targetType">The type to be converted to.</param>
        /// <param name="parameter">An enum that will be returned if conditions are met.</param>
        /// <param name="culture">The culture, i guess.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
        }
    }
}
