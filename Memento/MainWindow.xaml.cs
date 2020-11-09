using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Memento.UserControls;
using Memento.BLL;
using Memento.DAL;

namespace Memento
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Decks = Repository.FetchAllDecks().ToList();
            IsInEditor = false;
            IsInLearningProcess = false;
        }

        public AppHandler LearningProcess { get; private set; }
        public DeckEditor DeckEditor { get; set; }
        public List<Deck> Decks { get; private set; }

        public DeckEditorUserControl DeckEditorPage { get; set; }

        public bool IsInEditor { get; private set; }
        public bool IsInLearningProcess { get; private set; }

        public void StartLearning(object sender, RoutedEventArgs e)
        {
            LearningProcess = new AppHandler((int)((Button)sender).Tag);
            LearningProcess.Start();
        }

        public void StartEditing(object sender, RoutedEventArgs e)
        {
            //unsubscribe all events for all the other pages

            //Button Tag is the deck id
            Content = DeckEditorPage = new DeckEditorUserControl(Int32.Parse((string)((Button)sender).Tag))
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            DeckEditorPage.MakeMainPageVisible += GoToMainPage;
            Title = "Memento - Deck Editor";
        }

        public void GoToMainPage(object sender, EventArgs e)
        {
            DeckEditorPage.MakeMainPageVisible -= GoToMainPage;
            Content = A;
        }
    }
}
