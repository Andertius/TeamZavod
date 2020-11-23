using System.Windows;
using System.Windows.Controls;
using Memento.BLL;
using System;
using System.Collections.Generic;
using Memento.BLL.AppHandlerEventArgs;
using System.Windows.Media.Imaging;

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
           new PropertyMetadata(new AppHandler(0)));

        private List<FrameworkElement> hiddenElements = new List<FrameworkElement>();

        public LearningUserControl(int deckId)
        {
            DataContext = this;
            AppHandler = new AppHandler(deckId);
            AppHandler.Start(CardOrder.Random, true);
            InitializeComponent();

            hiddenElements.Add(Trivial_Btn);
            hiddenElements.Add(Again_Btn);
            hiddenElements.Add(GotIt_Btn);
            hiddenElements.Add(Description);

            //bool showimg = true;
            //if (!showimg)
            //{
            //    CardImage.Visibility = Visibility.Hidden;
            //}
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
            //CardImage.Visibility = Visibility.Visible;
        }

        private void NextCard()
        {
            foreach (var element in hiddenElements)
            {
                element.Visibility = Visibility.Hidden;
            }
            Show_Btn.Visibility = Visibility.Visible;

            Word.Text = AppHandler.Deck[0].Word;
            Transcription.Text = AppHandler.Deck[0].Transcription;
            Description.Text = AppHandler.Deck[0].Description;
            //CardImage.Source = new BitmapImage(new Uri(@$"..\\{AppHandler.Deck[0].ImagePath}"));
            //CardImage.Visibility = Visibility.Visible;
        }

        private void Trivial_Btn_Click(object sender, RoutedEventArgs e)
        {
            AppHandler.MoveCardIntoDeck(this, new AppHandlerMoveCardEventArgs(AppHandler.Deck[0], RememberingLevels.Trivial));
            NextCard();
        }

        private void Again_Btn_Click(object sender, RoutedEventArgs e)
        {
            AppHandler.MoveCardIntoDeck(this, new AppHandlerMoveCardEventArgs(AppHandler.Deck[0], RememberingLevels.Again));
            NextCard();
        }

        private void GotIt_Btn_Click(object sender, RoutedEventArgs e)
        {
            AppHandler.MoveCardIntoDeck(this, new AppHandlerMoveCardEventArgs(AppHandler.Deck[0], RememberingLevels.GotIt));
            NextCard();
        }
    }
}