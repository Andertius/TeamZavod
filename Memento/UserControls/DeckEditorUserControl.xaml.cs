using System;
using System.Collections.Generic;
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

using Memento.DAL;
using Memento.BLL;

namespace Memento.UserControls
{
    public partial class DeckEditorUserControl : UserControl
    {
        private readonly DependencyProperty DeckEditorProperty = DependencyProperty.Register(nameof(DeckEditor), typeof(DeckEditor), typeof(DeckEditorUserControl),
            new PropertyMetadata(new DeckEditor()));

        private readonly DependencyProperty CurrentCardProperty = DependencyProperty.Register(nameof(CurrentCard), typeof(Card), typeof(DeckEditorUserControl),
            new PropertyMetadata(new Card()));

        public DeckEditorUserControl(int deckId)
        {
            DataContext = this;
            InitializeComponent();

            DeckEditor = new DeckEditor(deckId);

            CardAdded += DeckEditor.AddCard;
            CardRemoved += DeckEditor.RemoveCard;
            DeckChanged += DeckEditor.ChangeDeck;
            ChangesSaved += DeckEditor.SaveChanges;
            EditorExited += DeckEditor.ExitEditor;
        }

        public DeckEditor DeckEditor
        {
            get => (DeckEditor)GetValue(DeckEditorProperty);
            set => SetValue(DeckEditorProperty, value);
        }

        public Card CurrentCard
        {
            get => (Card)GetValue(CurrentCardProperty);
            set => SetValue(CurrentCardProperty, value);
        }

        //Deck editor events
        public event EventHandler<DeckEditorAddCardEventArgs> CardAdded;
        public event EventHandler<DeckEditorRemoveCardEventArgs> CardRemoved;
        public event EventHandler<DeckEditorDeckEventArgs> DeckChanged;
        public event EventHandler<DeckEditorDeckEventArgs> ChangesSaved;
        public event EventHandler<ExitDeckEditorEventArgs> EditorExited;

        public event EventHandler MakeMainPageVisible;

        public void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            MakeMainPageVisible?.Invoke(this, EventArgs.Empty);
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
