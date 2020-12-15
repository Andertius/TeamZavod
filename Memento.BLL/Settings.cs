// <copyright file="Settings.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Xml.Serialization;

namespace Memento.BLL
{
    /// <summary>
    /// The main logic for settings.
    /// </summary>
    [Serializable]
    public class Settings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            HoursPerDay = 3.5;
            CardsPerDay = 25;
            Theme = Theme.Light;
            CardOrder = CardOrder.Random;
            ShowImages = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        /// <param name="hours">Hours per day.</param>
        /// <param name="cards">Cards per day.</param>
        /// <param name="theme">The theme.</param>
        /// <param name="order">Cards order.</param>
        /// <param name="showImages">Show images.</param>
        public Settings(double hours, int cards, Theme theme, CardOrder order, bool showImages)
        {
            HoursPerDay = hours;
            CardsPerDay = cards;
            Theme = theme;
            CardOrder = order;
            ShowImages = showImages;
        }

        /// <summary>
        /// Gets or sets hoursPerDay.
        /// </summary>
        [XmlElement("HoursPerDay")]
        public double HoursPerDay { get; set; }

        /// <summary>
        /// Gets or sets cardsPerDay.
        /// </summary>
        [XmlElement("CardsPerDay")]
        public int CardsPerDay { get; set; }

        /// <summary>
        /// Gets or sets the theme.
        /// </summary>
        [XmlElement("Theme")]
        public Theme Theme { get; set; }

        /// <summary>
        /// Gets or sets cardOrder.
        /// </summary>
        [XmlElement("CardOrder")]
        public CardOrder CardOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether showImages.
        /// </summary>
        [XmlElement("ShowImages")]
        public bool ShowImages { get; set; }

        /// <summary>
        /// Checks CardsPerDay for validity.
        /// </summary>
        /// <returns>true if CardsPerDay is valid.</returns>
        public bool CheckCardsPerDay()
        {
            return CardsPerDay >= 0 && CardsPerDay <= 1000;
        }

        /// <summary>
        /// Checks HoursPerDay for validity.
        /// </summary>
        /// <returns>true if HoursPerDay is valid.</returns>
        public bool CheckHoursPerDay()
        {
            return HoursPerDay >= 0 && HoursPerDay <= 24;
        }

        /// <summary>
        /// Writes settings to xml file.
        /// </summary>
        /// <param name="filePath">file path to write to.</param>
        public void WriteToXMLFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !filePath.EndsWith(".xml"))
            {
                throw new ArgumentException("Invalid file name");
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            StreamWriter writer = new StreamWriter(filePath);
            serializer.Serialize(writer, this);

            writer.Close();
            Logger.Log.Info($"Application settings successfully saved to file {filePath}");
        }

        /// <summary>
        /// Reads settings from xml file.
        /// </summary>
        /// <param name="filePath">File path to read settings from.</param>
        public void ReadFromXMLFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !filePath.EndsWith(".xml"))
            {
                throw new ArgumentException("Invalid file name");
            }

            if (!File.Exists(filePath))
            {
                throw new DirectoryNotFoundException("No such file");
            }

            XmlSerializer deserializer = new XmlSerializer(typeof(Settings));
            StreamReader reader = new StreamReader(filePath);

            Settings settings = (Settings)deserializer.Deserialize(reader);
            reader.Close();

            this.HoursPerDay = settings.HoursPerDay;
            this.CardsPerDay = settings.CardsPerDay;
            this.Theme = settings.Theme;
            this.CardOrder = settings.CardOrder;
            this.ShowImages = settings.ShowImages;

            Logger.Log.Info($"Application settings was successfully read from file {filePath}");
        }
    }
}
