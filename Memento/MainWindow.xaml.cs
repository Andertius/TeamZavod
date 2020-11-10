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
using System.Diagnostics;

namespace Memento
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            Decks = Repository.FetchAllDecks().ToList();
            IsInEditor = false;
            IsInLearningProcess = false;
            //PickDeckCombox.ItemsSource = Decks;
        }

        private readonly DependencyProperty DecksProperty = DependencyProperty.Register(nameof(Decks), typeof(List<Deck>), typeof(MainWindow),
            new PropertyMetadata(new List<Deck>()));

        public List<Deck> Decks
        {
            get => (List<Deck>)GetValue(DecksProperty);
            private set => SetValue(DecksProperty, value);
        }

        public AppHandler LearningProcess { get; private set; }
        public DeckEditor DeckEditor { get; set; }
        //public List<Deck> Decks { get; private set; }
        public Settings AppSettings { get; set; }

        public DeckEditorUserControl DeckEditorPage { get; set; }
        public SettingsUserControl SettingsPage { get; set; }

        public bool IsInEditor { get; private set; }
        public bool IsInLearningProcess { get; private set; }

        public void StartLearning(object sender, RoutedEventArgs e)
        {
            LearningProcess = new AppHandler((int)((Button)sender).Tag);
            LearningProcess.Start(SettingsPage.AppSettings.CardsOrder, SettingsPage.AppSettings.ShowImages);
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
            if (AppSettings is null)
            {
                AppSettings = new Settings();
            }
            Content = SettingsPage = new SettingsUserControl(AppSettings)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            SettingsPage.MakeMainPageVisible += GoToMainPageFromSettings;
            Title = "Memento - Settings";
        }

        public void GoToMainPageFromSettings(object sender, EventArgs e)
        {
            SettingsPage.MakeMainPageVisible -= GoToMainPageFromSettings;
            Content = MainPageContent;
            Title = "Memento";
        }

        public void Help_Click(object sender, RoutedEventArgs e)
        {
            if (HelpPanel.Visibility == Visibility.Visible)
            {
                HelpPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                HelpPanel.Visibility = Visibility.Visible;
            }
        }

        public void Guide_Click(object sender, RoutedEventArgs e)
        {
            HelpPanel.Visibility = Visibility.Hidden;
        }

        public void About_Click(object sender, RoutedEventArgs e)
        {
            HelpPanel.Visibility = Visibility.Hidden;
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        public void SupportUs_Click(object sender, RoutedEventArgs e)
        {
            HelpPanel.Visibility = Visibility.Hidden;
        }

        private void ExitProgram(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        public void ChangeMainTitle(object sender, CardEditedEventArgs e)
        {
            Title = e.Title;
        }

        //private void StartLearning(object sender, ExecutedRoutedEventArgs e)
        //{

        //}
    }
}
