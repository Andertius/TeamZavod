using System.Windows;
using System.Windows.Controls;
using Memento.BLL;
using System;
using System.Collections.Generic;
using Memento.BLL.AppHandlerEventArgs;
using System.Windows.Media.Imaging;
using System.IO;

namespace Memento.UserControls
{
    /// <summary>
    /// Interaction logic for LearningUserContro.xaml
    /// </summary>
    public partial class LearningUserControl : UserControl
    {
        private static readonly DependencyProperty LearningPageProperty = DependencyProperty.Register(
           nameof(AppHandler),
           typeof(AppHandler),
           typeof(LearningUserControl),
           new PropertyMetadata(new AppHandler()));

        private List<FrameworkElement> hiddenElements = new List<FrameworkElement>();

        public event EventHandler<AppHandlerMoveCardEventArgs> NextCardEvents;

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
        }

        /// <summary>
        /// An event that handles the return to the main page.
        /// </summary>
        public event EventHandler MakeMainPageVisible;

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

        }

        private void Trivial_Btn_Click(object sender, RoutedEventArgs e)
        {
            NextCardEvents?.Invoke(this, new AppHandlerMoveCardEventArgs(AppHandler.CurrentCard, RememberingLevels.Trivial));
        }

        private void Again_Btn_Click(object sender, RoutedEventArgs e)
        {
            NextCardEvents?.Invoke(this, new AppHandlerMoveCardEventArgs(AppHandler.CurrentCard, RememberingLevels.Again));
        }

        private void GotIt_Btn_Click(object sender, RoutedEventArgs e)
        {
            NextCardEvents?.Invoke(this, new AppHandlerMoveCardEventArgs(AppHandler.CurrentCard, RememberingLevels.GotIt));
        }
    }
}