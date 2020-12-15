// <copyright file="LearningUserControl.xaml.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Memento.BLL;
using Memento.BLL.AppHandlerEventArgs;

namespace Memento.UserControls
{
    /// <summary>
    /// Interaction logic for LearningUserControl.xaml.
    /// </summary>
    public partial class LearningUserControl : UserControl
    {
        private static readonly DependencyProperty LearningPageProperty = DependencyProperty.Register(
           nameof(AppHandler),
           typeof(AppHandler),
           typeof(LearningUserControl),
           new PropertyMetadata(new AppHandler()));

        private readonly List<FrameworkElement> hiddenElements = new List<FrameworkElement>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LearningUserControl"/> class.
        /// </summary>
        /// <param name="deckId">The id of the deck thats going to be shown when the page loads.</param>
        public LearningUserControl(int deckId)
        {
            DataContext = this;
            AppHandler = new AppHandler(deckId);
            AppHandler.Start(MainWindow.AppSettings.CardOrder);
            InitializeComponent();

            hiddenElements.Add(Trivial_Btn);
            hiddenElements.Add(Again_Btn);
            hiddenElements.Add(GotIt_Btn);
            hiddenElements.Add(Description);

            NextCardEvents += AppHandler.MoveCardIntoDeck;
            NextCardEvents += NextCard;

            if (!MainWindow.AppSettings.ShowImages)
            {
                CardImage.Visibility = Visibility.Hidden;
            }

            CardImage.Source = String.IsNullOrWhiteSpace(AppHandler.CurrentCard.ImagePath)
                     ? null
                     : new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), $"{AppHandler.CurrentCard.ImagePath}")));

            ImageScale.CenterX = CardImage.ActualWidth / 2;
            ImageScale.CenterY = CardImage.ActualHeight / 2;
        }

        /// <summary>
        /// Handler for some MoveCard events.
        /// </summary>
        public event EventHandler<AppHandlerMoveCardEventArgs> NextCardEvents;

        /// <summary>
        /// An event that handles the return to the main page.
        /// </summary>
        public event EventHandler MakeMainPageVisible;

        /// <summary>
        /// Gets or sets AppHandler instance.
        /// </summary>
        public AppHandler AppHandler
        {
            get => (AppHandler)GetValue(LearningPageProperty);
            set => SetValue(LearningPageProperty, value);
        }

        private void Stop_Btn_Click(object sender, RoutedEventArgs e)
        {
            MakeMainPageVisible?.Invoke(this, EventArgs.Empty);
            Logger.Log.Info($"Stop button pressed {DateTime.Now}");
        }

        private void Show_Btn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var element in hiddenElements)
            {
                element.Visibility = Visibility.Visible;
            }

            Show_Btn.Visibility = Visibility.Hidden;
            CardImage.Visibility = Visibility.Visible;
        }

        private void NextCard(object sender, AppHandlerMoveCardEventArgs e)
        {
            foreach (var element in hiddenElements)
            {
                element.Visibility = Visibility.Hidden;
            }

            Show_Btn.Visibility = Visibility.Visible;

            if (!MainWindow.AppSettings.ShowImages)
            {
                CardImage.Visibility = Visibility.Hidden;
            }

            CardImage.Source = String.IsNullOrWhiteSpace(AppHandler.CurrentCard.ImagePath)
                     ? null
                     : new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), $"{AppHandler.CurrentCard.ImagePath}")));

            ImageScale.CenterX = CardImage.ActualWidth / 2;
            ImageScale.CenterY = CardImage.ActualHeight / 2;
            Logger.Log.Info($"Opened image for {AppHandler.CurrentCard} card");
        }

        private void Trivial_Btn_Click(object sender, RoutedEventArgs e)
        {
            Logger.Log.Info($"Changed card from {AppHandler.CurrentCard} to {AppHandler.Deck[1]}");
            NextCardEvents?.Invoke(this, new AppHandlerMoveCardEventArgs(AppHandler.CurrentCard, RememberingLevels.Trivial));
            MainWindow.AppStatistics.AddCardLearned();
        }

        private void Again_Btn_Click(object sender, RoutedEventArgs e)
        {
            Logger.Log.Info($"Changed card from {AppHandler.CurrentCard} to {AppHandler.Deck[1]}");
            NextCardEvents?.Invoke(this, new AppHandlerMoveCardEventArgs(AppHandler.CurrentCard, RememberingLevels.Again));
        }

        private void GotIt_Btn_Click(object sender, RoutedEventArgs e)
        {
            Logger.Log.Info($"Changed card from {AppHandler.CurrentCard} to {AppHandler.Deck[1]}");
            NextCardEvents?.Invoke(this, new AppHandlerMoveCardEventArgs(AppHandler.CurrentCard, RememberingLevels.GotIt));
            MainWindow.AppStatistics.AddCardLearned();
        }
    }
}
