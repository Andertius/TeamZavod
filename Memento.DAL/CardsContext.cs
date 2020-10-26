using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Memento.DAL
{
    class CardsContext : DbContext
    {
        public DbSet<CardModel> Cards { get; set; }
        public DbSet<TagtoCardModel> Tags { get; set; }
        //public DbSet<DeckToCard> DeckToCards { get; set; }
        //public DbSet<Deck> Decks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite( connectionString:"Data Source=DatabaseDAL.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardModel>().ToTable("\"Card_Table\"");
            // modelBuilder.Entity<Deck>().ToTable("Deck_Table");

            modelBuilder.Entity<TagtoCardModel>()
                .ToTable("\"Tag_To_Card_Table\"")
                .HasKey(c => new { c.TagName, c.CardID });

            modelBuilder.Entity<DeckToCard>()
                .ToTable("\"Deck_To_Card_Table\"")
                .HasKey(c => new { c.CardID, c.DeckID });

            modelBuilder.Entity<DeckToCard>()
                .HasOne(p => p.Card)
                .WithMany(b => b.Decks)
                .HasForeignKey(b => b.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<CardModel>()
            //    .HasMany(p => p.Tags)
            //    .WithOne();

            modelBuilder.Entity<TagtoCardModel>()
                .HasOne(p => p.Card)
                .WithMany(b => b.Tags)
                .HasForeignKey(b => b.CardID)
                .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
