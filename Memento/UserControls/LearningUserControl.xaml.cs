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

        private List<FrameworkElement> hiddenElements = new List<FrameworkElement>();

        private MainWindow mainWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="LearningUserControl"/> class.
        /// </summary>
        /// <param name="deckId">The id of the deck thats going to be shown when the page loads.</param>
        /// <param name="window">Reference to main window which calls user control.</param>
        public LearningUserControl(int deckId, MainWindow window)
        {
            DataContext = this;
            AppHandler = new AppHandler(deckId);
            mainWindow = window;
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

            if (MainWindow.AppSettings.Theme == Theme.Dark)
            {
                LearningGrid.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#2c303a");
            }
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
<<<<<<< HEAD
=======

            MainWindow.AppStatistics.AddCardLearned(this, EventArgs.Empty);
>>>>>>> 7840c459ec671ca4a844a3f9cebc8997f31aab98
        }

        private void Trivial_Btn_Click(object sender, RoutedEventArgs e)
        {
            NextCardEvents?.Invoke(this, new AppHandlerMoveCardEventArgs(AppHandler.CurrentCard, RememberingLevels.Trivial));
        }

        private void Again_Btn_Click(object sender, RoutedEventArgs e)
        {
            NextCardEvents?.Invoke(this, new AppHandlerMoveCardEventArgs(AppHandler.CurrentCard, RememberingLevels.Again));
            mainWindow.AppStatistics.AddCardLearned();
        }

        private void GotIt_Btn_Click(object sender, RoutedEventArgs e)
        {
            NextCardEvents?.Invoke(this, new AppHandlerMoveCardEventArgs(AppHandler.CurrentCard, RememberingLevels.GotIt));
            mainWindow.AppStatistics.AddCardLearned();
        }
    }
}