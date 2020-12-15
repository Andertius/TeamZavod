// <copyright file="ProgressToAngleConverter.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Memento
{
    /// <summary>
    /// Converts progress in progress bar to angle.
    /// </summary>
    public class ProgressToAngleConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts progress to angle.
        /// </summary>
        /// <param name="values">progress value.</param>
        /// <param name="targetType">target type.</param>
        /// <param name="parameter">parameter.</param>
        /// <param name="culture">culture.</param>
        /// <returns>progress angle.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double progress = (double)values[0];
            ProgressBar bar = values[1] as ProgressBar;

            return 359.999 * (progress / (bar.Maximum - bar.Minimum));
        }

        /// <summary>
        /// Convert from angle to progress.
        /// </summary>
        /// <param name="value">angle.</param>
        /// <param name="targetTypes">target type.</param>
        /// <param name="parameter">parameter.</param>
        /// <param name="culture">culture.</param>
        /// <returns>object[].</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
