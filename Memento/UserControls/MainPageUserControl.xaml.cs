﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Memento.BLL;
using Memento.DAL;

namespace Memento.UserControls
{
    public partial class MainPageUserControl : UserControl
    {
        public MainPageUserControl()
        {
            InitializeComponent();

            DataContext = this;
            Decks = Repository.FetchAllDecks().ToList();
        }

        static private readonly DependencyProperty DecksProperty = DependencyProperty.Register(nameof(Decks), typeof(List<Deck>),
            typeof(MainPageUserControl), new PropertyMetadata(new List<Deck>()));

        public List<Deck> Decks
        {
            get => (List<Deck>)GetValue(DecksProperty);
            private set => SetValue(DecksProperty, value);
        }

        public event EventHandler<StartEditingEventArgs> StartEditingEvent;
        public event EventHandler OpenSettingsEvent;
        public event EventHandler OpenStatisticsEvent;

        private void Guide_Click(object sender, RoutedEventArgs e)
        {
            HelpMenu.Visibility = Visibility.Hidden;
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

        }

        private void StartEditing(object sender, RoutedEventArgs e)
        {
            var deck = PickDeckCombox.SelectedItem as Deck;
            StartEditingEvent?.Invoke(this, new StartEditingEventArgs(deck.Id));
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
