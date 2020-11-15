using System;
using System.IO;
using ConsoleTables;
using Microsoft.Data.Sqlite;
using Memento.DAL;
using System.Linq;

namespace DatabaseHandler
{
    class Program
    {
        static void FillDatabase()
        {
            SqliteConnection connection = new SqliteConnection("Data Source=Database.db");
            connection.Open();

            string[] decks = File.ReadAllLines("Deck_Table_Data.txt");
            for (int i = 0; i < decks.Length; i++)
            {
                SqliteCommand com = connection.CreateCommand();
                string[] deck = decks[i].Split();
                com.CommandText = $"INSERT INTO Deck_Table(deck_name, tag_name, number_of_cards) " +
                    $"VALUES(@deck_name, @tag_name, @number_of_cards)";
                com.Parameters.AddWithValue("@deck_name", deck[0]);
                com.Parameters.AddWithValue("@tag_name", deck[1]);
                com.Parameters.AddWithValue("@number_of_cards", Int32.Parse(deck[2]));
                com.Prepare();
                com.ExecuteNonQuery();
            }

            string[] cards = File.ReadAllLines("Card_Table_Data.txt");
            for (int i = 0; i < cards.Length; i++)
            {
                SqliteCommand com = connection.CreateCommand();
                string[] card = cards[i].Split(", ");
                com.CommandText = $"INSERT INTO Card_Table(word, transcription, description, difficulty_level, image_path) " +
                    $"VALUES(@word, @transcription, @description, @difficulty_level, @image_path)";
                com.Parameters.AddWithValue("@word", card[0]);
                com.Parameters.AddWithValue("@transcription", card[1]);
                com.Parameters.AddWithValue("@description", card[2]);
                com.Parameters.AddWithValue("@difficulty_level", card[3]);
                com.Parameters.AddWithValue("@image_path", card[4]);
                com.Prepare();
                com.ExecuteNonQuery();
            }

            string[] deckToCard = File.ReadAllLines("Deck_To_Card_Table_Data.txt");
            for (int i = 0; i < deckToCard.Length; i++)
            {
                SqliteCommand com = connection.CreateCommand();
                string[] line = deckToCard[i].Split();
                com.CommandText = $"INSERT INTO Deck_To_Card_Table(deck_id, card_id) VALUES(@deck_id, @card_id)";
                com.Parameters.AddWithValue("@deck_id", Int32.Parse(line[0]));
                com.Parameters.AddWithValue("@card_id", Int32.Parse(line[1]));
                com.Prepare();
                com.ExecuteNonQuery();
            }

            string[] tagToCard = File.ReadAllLines("Tag_To_Card_Table_Data.txt");
            for (int i = 0; i < tagToCard.Length; i++)
            {
                SqliteCommand com = connection.CreateCommand();
                string[] line = tagToCard[i].Split();
                com.CommandText = $"INSERT INTO Tag_To_Card_Table(card_id, tag_name) VALUES(@card_id, @tag_name)";
                com.Parameters.AddWithValue("@card_id", Int32.Parse(line[0]));
                com.Parameters.AddWithValue("@tag_name", line[1]);
                com.Prepare();
                com.ExecuteNonQuery();
            }

            connection.Close();
        }

        static void PrintDatabaseData()
        {
            SqliteConnection connection = new SqliteConnection("Data Source=Database.db");
            connection.Open();

            SqliteCommand com = connection.CreateCommand();
            com.CommandText = "SELECT deck_id, deck_name, tag_name, number_of_cards FROM Deck_Table";
            ConsoleTable table = new ConsoleTable("deck_id", "deck_name", "tag_name", "number_of_cards");
            Console.WriteLine("Deck_Table\n");

            SqliteDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                table.AddRow(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
            }
            reader.Close();
            table.Write();

            com.CommandText = "SELECT card_id, word, transcription, description, difficulty_level, image_path FROM Card_Table";
            table = new ConsoleTable("card_id", "word", "transcription", "description", "difficulty_level", "image_path");
            Console.WriteLine("\nCard_Table\n");

            reader = com.ExecuteReader();
            while (reader.Read())
            {
                table.AddRow(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),
                    reader.GetString(5));
            }
            reader.Close();
            table.Write();

            com.CommandText = "SELECT deck_id, card_id FROM Deck_To_Card_Table";
            table = new ConsoleTable("deck_id", "card_id");
            Console.WriteLine("\nDeck_To_Card_Table\n");

            reader = com.ExecuteReader();
            while (reader.Read())
            {
                table.AddRow(reader.GetInt32(0), reader.GetInt32(1));
            }
            reader.Close();
            table.Write();

            com.CommandText = "SELECT card_id, tag_name FROM Tag_To_Card_Table";
            table = new ConsoleTable("card_id", "tag_name");
            Console.WriteLine("\nTag_To_Card_Table\n");

            reader = com.ExecuteReader();
            while (reader.Read())
            {
                table.AddRow(reader.GetInt32(0), reader.GetString(1));
            }
            reader.Close();
            table.Write();

            connection.Close();
        }

        static void Main()
        {
            //Repository.RemoveCard(3);

        }
    }
}
