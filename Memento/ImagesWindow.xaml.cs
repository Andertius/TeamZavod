// <copyright file="ImagesWindow.xaml.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Microsoft.Win32;

namespace Memento
{
    /// <summary>
    /// Interaction logic for ImagesWindow.xaml.
    /// </summary>
    public partial class ImagesWindow : Window
    {
        private string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagesWindow"/> class.
        /// </summary>
        /// <param name="imgSource">The currently selected image.</param>
        public ImagesWindow(ImageSource imgSource)
        {
            InitializeComponent();

            if (MainWindow.AppSettings.Theme == BLL.Theme.Dark)
            {
                ImagesWindowGrid.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#2c303a");
            }

            ImageSource = imgSource;
            CurrentImage.Source = imgSource;
            path = String.Empty;

            var images = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "images"));
            ImagesDictionary = new SortedDictionary<string, string>();

            foreach (var image in images)
            {
                ImagesDictionary.Add(Path.GetFileName(image), image);
            }

            RenderImages(this, EventArgs.Empty);

            var dp = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));
            dp.AddValueChanged(SearchTextBox, RenderImages);
        }

        /// <summary>
        /// Gets the image source.
        /// </summary>
        public ImageSource ImageSource { get; private set; }

        /// <summary>
        /// Gets the path of the selected image.
        /// </summary>
        public string SelectedPath { get; private set; }

        /// <summary>
        /// Gets all the images.
        /// </summary>
        public SortedDictionary<string, string> ImagesDictionary { get; }

        /// <summary>
        /// Change the selected image.
        /// </summary>
        /// <param name="sender">The object that invoked the method.</param>
        /// <param name="e">No parameters.</param>
        public void ChooseFile(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var image = button.Content as Image;

            path = (string)button.Tag;
            CurrentImage.Source = image.Source;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ImageSource = CurrentImage.Source;
            SelectedPath = path;
            Close();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            ImageSource = null;
            SelectedPath = String.Empty;
            Close();
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

                ImagesDictionary.Add(Path.GetFileName(openImageDialog.FileName), Path.Combine(Directory.GetCurrentDirectory(), "images", Path.GetFileName(openImageDialog.FileName)));

                RenderImages(this, EventArgs.Empty);
            }
        }

        private void RenderImages(object sender, EventArgs e)
        {
            Images.Children.Clear();

            foreach (var item in ImagesDictionary)
            {
                if (item.Key.ToLower().Contains(SearchTextBox.Text.ToLower().Trim()))
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

                    finalImage.Click += ChooseFile;

                    var imageName = new TextBlock() { Text = item.Key, HorizontalAlignment = HorizontalAlignment.Center, TextWrapping = TextWrapping.Wrap };
                    stackPanel.Children.Add(finalImage);
                    stackPanel.Children.Add(imageName);

                    Images.Children.Add(stackPanel);
                }
            }
        }
    }
}
