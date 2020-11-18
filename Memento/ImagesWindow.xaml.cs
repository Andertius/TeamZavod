// <copyright file="ImagesWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Memento
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Microsoft.Win32;

    public partial class ImagesWindow : Window
    {
        private string path;

        public ImagesWindow(ImageSource imgSource)
        {
            this.InitializeComponent();

            this.ImageSource = imgSource;
            this.CurrentImage.Source = imgSource;

            var images = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "images"));
            this.ImagesDictionary = new SortedDictionary<string, string>();

            foreach (var image in images)
            {
                this.ImagesDictionary.Add(Path.GetFileName(image), image);
            }

            this.RenderImages(this, EventArgs.Empty);

            var dp = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));
            dp.AddValueChanged(this.SearchTextBox, this.RenderImages);
        }

        public ImageSource ImageSource { get; private set; }

        public string SelectedPath { get; private set; }

        public SortedDictionary<string, string> ImagesDictionary { get; }

        public void ChooseFile(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var image = button.Content as Image;

            this.path = (string)button.Tag;
            this.CurrentImage.Source = image.Source;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.ImageSource = this.CurrentImage.Source;
            this.SelectedPath = this.path;
            this.Close();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            this.ImageSource = null;
            this.SelectedPath = string.Empty;
            this.Close();
        }

        private void NewImage_Click(object sender, RoutedEventArgs e)
        {
            var openImageDialog = new OpenFileDialog
            {
                Filter = "Bitmap image files (*.png, *.bmp, *.jpg, *.exif, *.tiff)|*.png;*.bmp;*.jpg;*.exif;*.tiff",
            };

            if (openImageDialog.ShowDialog() == true)
            {
                string copyPath = Path.Combine(Directory.GetCurrentDirectory(), "images", Path.GetFileName(openImageDialog.FileName));

                try
                {
                    File.Copy(openImageDialog.FileName, copyPath);
                }
                catch (IOException exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                this.ImagesDictionary.Add(Path.GetFileName(openImageDialog.FileName), Path.Combine(Directory.GetCurrentDirectory(), "images", Path.GetFileName(openImageDialog.FileName)));

                this.RenderImages(this, EventArgs.Empty);
            }
        }

        private void RenderImages(object sender, EventArgs e)
        {
            this.Images.Children.Clear();

            foreach (var item in this.ImagesDictionary)
            {
                if (item.Key.ToLower().Contains(this.SearchTextBox.Text.ToLower().Trim()))
                {
                    var stackPanel = new StackPanel() { Margin = new Thickness(0, 0, 10, 10), MaxWidth = 100 };
                    string path = Path.Combine("images", item.Value);

                    var finalImage = new Button
                    {
                        Width = 80,
                        Content = new Image { Source = new BitmapImage(new Uri(path)) },
                        Margin = new Thickness(0, 0, 0, 2),
                        Tag = Path.Combine("images", item.Key),
                    };

                    finalImage.Click += this.ChooseFile;

                    var imageName = new TextBlock() { Text = item.Key, HorizontalAlignment = HorizontalAlignment.Center, TextWrapping = TextWrapping.Wrap };
                    stackPanel.Children.Add(finalImage);
                    stackPanel.Children.Add(imageName);

                    this.Images.Children.Add(stackPanel);
                }
            }
        }
    }
}
