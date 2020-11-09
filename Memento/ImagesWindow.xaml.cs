using Memento.DAL;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memento
{
    public partial class ImagesWindow : Window
    {
        public ImagesWindow()
        {
            InitializeComponent();

            var cards = Repository.FetchAllCards();
            ImagesDictionary = new SortedDictionary<string, string>();

            foreach (var card in cards)
            {
                ImagesDictionary.Add(new FileInfo(card.ImagePath).Name, card.ImagePath);
            }

            foreach (var item in ImagesDictionary)
            {
                var stackPanel = new StackPanel() { Margin = new Thickness(0, 0, 10, 10) };

                var finalImage = new Button
                {
                    Width = 80,
                    Content = new Image { Source = new BitmapImage(new Uri($"pack://application:,,,/Memento;component/{item.Value}")) },
                    Margin = new Thickness(0, 0, 0, 2),
                };

                finalImage.Click += ChooseFile;

                var imageName = new TextBlock() { Text = item.Key, HorizontalAlignment = HorizontalAlignment.Center };
                stackPanel.Children.Add(finalImage);
                stackPanel.Children.Add(imageName);

                Images.Children.Add(stackPanel);
            }
        }

        public ImageSource ImageSource { get; private set; }
        public SortedDictionary<string, string> ImagesDictionary { get; }

        public void ChooseFile(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var image = button.Content as Image;

            ImageSource = image.Source;
            CurrentImage.Source = image.Source;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NewImage_Click(object sender, RoutedEventArgs e)
        {
            var openImageDialog = new OpenFileDialog
            {
                Filter = "Bitmap image files (*.png, *.bmp, *.jpg, *.exif, *.tiff)|*.png;*.bmp;*.jpg;*.exif;*.tiff"
            };

            if (openImageDialog.ShowDialog() == true)
            {
                //File.Copy(openImageDialog.FileName, @"");
            }
        }
    }
}
