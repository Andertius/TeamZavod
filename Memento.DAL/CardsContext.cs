using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Memento.DAL
{
    class CardsContext : DbContext
    {
        public DbSet<CardModel> Cards { get; set; }
        public DbSet<TagToCardModel> Tags { get; set; }
        public DbSet<DeckToCardModel> DeckToCards { get; set; }
        public DbSet<DeckModel> Decks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite( connectionString: $"Data Source={Path.Combine(Directory.GetCurrentDirectory(), "DatabaseDAL.db")}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardModel>().ToTable("Card_Table");
            modelBuilder.Entity<DeckModel>().ToTable("Deck_Table");

            modelBuilder.Entity<TagToCardModel>()
                .ToTable("Tag_To_Card_Table")
                .HasKey(c => new { c.TagName, c.CardID });

            modelBuilder.Entity<DeckToCardModel>()
                .ToTable("Deck_To_Card_Table")
                .HasKey(c => new { c.CardID, c.DeckID });

            modelBuilder.Entity<DeckToCardModel>()
                .HasOne(p => p.Card)
                .WithMany(b => b.Decks)
                .HasForeignKey(b => b.CardID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeckToCardModel>()
                .HasOne(p => p.Deck)
                .WithMany(b => b.Cards)
                .HasForeignKey(b => b.DeckID)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<CardModel>()
            //    .HasMany(p => p.Tags)
            //    .WithOne();

            modelBuilder.Entity<TagToCardModel>()
                .HasOne(p => p.Card)
                .WithMany(b => b.Tags)
                .HasForeignKey(b => b.CardID)
                .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
