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
        public DbSet<DeckToCardModel> DeckToCards { get; set; }
        public DbSet<DeckModel> Decks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite( connectionString: "Data Source=DatabaseDAL.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardModel>().ToTable("Card_Table");
            modelBuilder.Entity<DeckModel>().ToTable("Deck_Table");

            modelBuilder.Entity<TagtoCardModel>()
                .ToTable("Tag_To_Card_Table")
                .HasKey(c => new { c.TagName, c.CardID });

            modelBuilder.Entity<DeckToCardModel>()
                .ToTable("Deck_To_Card_Table")
                .HasKey(c => new { c.CardID, c.DeckID });

            modelBuilder.Entity<DeckToCardModel>()
                .HasOne(p => p.Card)
                .WithMany(b => b.Decks)
                .HasForeignKey(b => b.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeckToCardModel>()
                .HasOne(p => p.Card)
                .WithMany(b => b.Decks)
                .HasForeignKey(b => b.DeckId)
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
