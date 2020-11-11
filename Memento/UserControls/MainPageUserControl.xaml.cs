using System;
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
    /// <summary>
    /// Interaction logic for MainPageUserControl.xaml
    /// </summary>
    public partial class MainPageUserControl : UserControl
    {
        public MainPageUserControl()
        {
            InitializeComponent();

            DataContext = this;
            Decks = Repository.FetchAllDecks().ToList();
            IsInEditor = false;
            IsInLearningProcess = false;
        }

        static private readonly DependencyProperty DecksProperty = DependencyProperty.Register(nameof(Decks), typeof(List<Deck>),
            typeof(MainPageUserControl), new PropertyMetadata(new List<Deck>()));

        public List<Deck> Decks
        {
            get => (List<Deck>)GetValue(DecksProperty);
            private set => SetValue(DecksProperty, value);
        }

        public AppHandler LearningProcess { get; private set; }
        public DeckEditor DeckEditor { get; set; }
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
            Window.GetWindow(this).Title = "Memento - Deck Editor";
        }

        public void GoToMainPageFromDeckEditor(object sender, EventArgs e)
        {
            DeckEditorPage.MakeMainPageVisible -= GoToMainPageFromDeckEditor;
            DeckEditorPage.BecameEdited -= ChangeMainTitle;
            Content = MainPageContent;
            Window.GetWindow(this).Title = "Memento";
        }

        public void ChangeMainTitle(object sender, CardEditedEventArgs e)
        {
            Window.GetWindow(this).Title = e.Title;
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
            Window.GetWindow(this).Title = "Memento - Settings";
        }

        public void GoToMainPageFromSettings(object sender, EventArgs e)
        {
            SettingsPage.MakeMainPageVisible -= GoToMainPageFromSettings;
            Content = MainPageContent;
            Window.GetWindow(this).Title = "Memento";
        }

        public void Guide_Click(object sender, RoutedEventArgs e)
        {
            HelpMenu.Visibility = Visibility.Hidden;
        }

        public void About_Click(object sender, RoutedEventArgs e)
        {
            HelpMenu.Visibility = Visibility.Hidden;
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        public void SupportUs_Click(object sender, RoutedEventArgs e)
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
    }
}
