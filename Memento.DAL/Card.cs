using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Memento.DAL
{
    public class Card : IEquatable<Card>, INotifyPropertyChanged
    {
        private int id;
        private string word;
        private string description;
        private string transcription;
        private Difficulty difficulty;
        private string imagePath;

        public Card()
        {
            Id = -1;
            Word = "";
            Description = "";
            Transcription = "";
            Difficulty = Difficulty.None;
            ImagePath = "";
            Tags = new ObservableCollection<string>();
        }

        public Card(string word, string description, string transcription = "", string imagePath = "", Difficulty difficulty = Difficulty.None)
        {
            Id = -1;
            Word = word;
            Description = description;
            Transcription = transcription;
            Difficulty = difficulty;
            ImagePath = imagePath;
            Tags = new ObservableCollection<string>();
        }

        public Card(Card card)
        {
            Id = card.Id;
            Word = card.Word;
            Description = card.Description;
            Transcription = card.Transcription;
            Difficulty = card.Difficulty;
            ImagePath = card.ImagePath;
            Tags = new ObservableCollection<string>(card.Tags.OrderBy(x => x));
        }

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public string Word
        {
            get => word;
            set
            {
                word = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        public string Transcription
        {
            get => transcription;
            set
            {
                transcription = value;
                OnPropertyChanged();
            }
        }

        public Difficulty Difficulty
        {
            get => difficulty;
            set
            {
                difficulty = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => imagePath;
            set
            {
                imagePath = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Tags { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return $"{Word} [{Transcription}] - {Description}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Word, Description);
        }

        public override bool Equals(object obj)
        {
            return obj is Card card && Equals(card);
        }

        public bool Equals(Card card)
        {
            return Id == card.Id || (Word == card.Word &&
                                     Description == card.Description &&
                                     Transcription == card.Transcription &&
                                     Difficulty == card.Difficulty &&
                                     ImagePath == card.ImagePath);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
