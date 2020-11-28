// <copyright file="MainPageUserControl.xaml.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Memento.DAL;

namespace Memento.UserControls
{
    /// <summary>
    /// Interaction logic for MainPageUserControl.xaml.
    /// </summary>
    public partial class MainPageUserControl : UserControl
    {
        private static readonly DependencyProperty DecksProperty = DependencyProperty.Register(
            nameof(Decks),
            typeof(List<Deck>),
            typeof(MainPageUserControl),
            new PropertyMetadata(new List<Deck>()));

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageUserControl"/> class.
        /// </summary>
        public MainPageUserControl()
        {
            InitializeComponent();

            DataContext = this;
            Decks = Repository.FetchAllDecks().ToList();
        }

        /// <summary>
        /// An event that handles the start of editing (opening deck editor page).
        /// </summary>
        public event EventHandler<StartEditingEventArgs> StartEditingEvent;

        /// <summary>
        /// An event that handles the opening of settings page page.
        /// </summary>
        public event EventHandler OpenSettingsEvent;

        /// <summary>
        /// An event that handles the opening of statistics page.
        /// </summary>
        public event EventHandler OpenStatisticsEvent;

        /// <summary>
        /// An event that handles the opening of learning process.
        /// </summary>
        public event EventHandler<StartLearningEventArgs> OpenLearningEvent;

        /// <summary>
        /// Gets or sets decks that the user works with.
        /// </summary>
        public List<Deck> Decks
        {
            get => (List<Deck>)GetValue(DecksProperty);
            set => SetValue(DecksProperty, value);
        }

        private void Guide_Click(object sender, RoutedEventArgs e)
        {
            HelpMenu.Visibility = Visibility.Hidden;
            string target = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
            Process.Start("cmd", $"/c start {target}");
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            HelpMenu.Visibility = Visibility.Hidden;
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void SupportUs_Click(object sender, RoutedEventArgs e)
        {
            HelpMenu.Visibility = Visibility.Hidden;
            string target = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
            Process.Start("cmd", $"/c start {target}");
        }

        private void ExitProgram(object sender, ExecutedRoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void Help(object sender, ExecutedRoutedEventArgs e)
        {
            if (HelpMenu.Visibility == Visibility.Visible)
            {
                HelpMenu.Visibility = Visibility.Hidden;
            }
            else
            {
                HelpMenu.Visibility = Visibility.Visible;
            }
        }

        private void StartLearning(object sender, RoutedEventArgs e)
        {
            if (PickDeckCombox.SelectedItem is Deck deck)
            {
                OpenLearningEvent?.Invoke(this, new StartLearningEventArgs(deck.Id));
            }
            else
            {
                MessageBox.Show("Pick up deck first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void StartEditing(object sender, RoutedEventArgs e)
        {
            if (PickDeckCombox.SelectedItem is Deck deck)
            {
                StartEditingEvent?.Invoke(this, new StartEditingEventArgs(deck.Id));
            }
            else
            {
                StartEditingEvent?.Invoke(this, new StartEditingEventArgs());
            }
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            OpenSettingsEvent?.Invoke(this, EventArgs.Empty);
        }

        private void OpenStatistics(object sender, RoutedEventArgs e)
        {
            OpenStatisticsEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
