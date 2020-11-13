using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Memento
{
    public partial class NewDeckWindow : Window
    {
        public NewDeckWindow()
        {
            InitializeComponent();
        }

        public string DeckName { get; set; } = "";
        public string TagName { get; set; } = "";

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeckNameTextBox.Text.Trim() == String.Empty)
            {
                ColorAnimation ca = new ColorAnimation(Colors.Black, Colors.Red, new Duration(TimeSpan.FromMilliseconds(500)));
                DeckNameTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                DeckNameTextBlock.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ca);

                await Task.Delay(500);

                ColorAnimation ca1 = new ColorAnimation(Colors.Red, Colors.Black, new Duration(TimeSpan.FromMilliseconds(500)));
                DeckNameTextBlock.Foreground = new SolidColorBrush(Colors.Red);
                DeckNameTextBlock.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, ca1);

                return;
            }

            DeckName = DeckNameTextBox.Text;
            TagName = TagNameTextBox.Text.Trim();

            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
