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
        public SettingsUserControl SettingsPage { get; set; }

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

            DeckEditorPage.MakeMainPageVisible += GoToMainPageFromDeckEditor;
            DeckEditorPage.BecameEdited += ChangeMainTitle;
            Title = "Memento - Deck Editor";
        }

        public void GoToMainPageFromDeckEditor(object sender, EventArgs e)
        {
            DeckEditorPage.MakeMainPageVisible -= GoToMainPageFromDeckEditor;
            DeckEditorPage.BecameEdited -= ChangeMainTitle;
            Content = MainPageContent;
            Title = "Memento";
        }

        public void OpenSettings(object sender, RoutedEventArgs e)
        {
            Content = SettingsPage = new SettingsUserControl()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            SettingsPage.MakeMainPageVisible += GoToMainPageFromSetings;
            Title = "Memento - Settings";
        }

        public void GoToMainPageFromSetings(object sender, EventArgs e)
        {
            SettingsPage.MakeMainPageVisible -= GoToMainPageFromSetings;
            Content = MainPageContent;
            Title = "Memento";
        }

        public void Help_Click(object sender, RoutedEventArgs e)
        {

        }

        public void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ChangeMainTitle(object sender, CardEditedEventArgs e)
        {
            Title = e.Title;
        }
    }
}
