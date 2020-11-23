using System.Windows;
using System.Windows.Controls;
using Memento.BLL;

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

        public LearningUserControl(int deckId)
        {
            DataContext = this;
            AppHandler = new AppHandler(deckId);
            System.Console.WriteLine(AppHandler.Deck[0].ImagePath.ToString());
            InitializeComponent();
            System.Console.WriteLine(AppHandler.Deck[0].ImagePath);
        }

        public AppHandler AppHandler
        {
            get => (AppHandler)GetValue(LearningPageProperty);
            set => SetValue(LearningPageProperty, value);
        }
    }
}
