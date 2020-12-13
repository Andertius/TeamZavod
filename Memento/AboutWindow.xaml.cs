// <copyright file="AboutWindow.xaml.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System.Windows;
using System.Windows.Media;

using Memento.BLL;

namespace Memento
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml.
    /// </summary>
    public partial class AboutWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutWindow"/> class.
        /// </summary>
        public AboutWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
