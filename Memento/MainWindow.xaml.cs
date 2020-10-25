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

using Memento.BLL;
using Memento.DAL;

namespace Memento
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Decks = Repository.FetchAllDecks();
            IsInEditor = false;
            IsInLearningProcess = false;
        }

        public AppHandler LearningProcess { get; private set; }
        public DeckEditor DeckEditor { get; set; }
        public List<Deck> Decks { get; private set; }

        public bool IsInEditor { get; private set; }
        public bool IsInLearningProcess { get; private set; }

        //Deck editor events
        public event EventHandler<DeckEditorAddCardEventArgs> CardAdded;
        public event EventHandler<DeckEditorRemoveCardEventArgs> CardRemoved;
        public event EventHandler<DeckEditorDeckEventArgs> DeckChanged;
        public event EventHandler<DeckEditorDeckEventArgs> ChangesSaved;
        public event EventHandler<ExitDeckEditorEventArgs> EditorExited;

        public void StartLearning(object sender, RoutedEventArgs e)
        {
            LearningProcess = new AppHandler((int)((Button)sender).Tag);
            LearningProcess.Start();
        }

        public void StartEditing(object sender, RoutedEventArgs e)
        {
            DeckEditor = new DeckEditor((int)((Button)sender).Tag);

            CardAdded += DeckEditor.AddCard;
            CardRemoved += DeckEditor.RemoveCard;
            DeckChanged += DeckEditor.ChangeDeck;
            ChangesSaved += DeckEditor.SaveChanges;
            EditorExited += DeckEditor.ExitEditor;
        }
    }
}
