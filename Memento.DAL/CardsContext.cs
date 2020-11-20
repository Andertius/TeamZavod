// <copyright file="CardsContext.cs" company="lnu.edu.ua">
// Copyright (c) lnu.edu.ua. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace Memento.DAL
{
    /// <summary>
    /// Core file of working with DB.
    /// </summary>
    internal class CardsContext : DbContext
    {
        /// <summary>
        /// Gets or sets Card model.
        /// </summary>
        public DbSet<CardModel> Cards { get; set; }

        /// <summary>
        /// Gets or sets Tag model.
        /// </summary>
        public DbSet<TagToCardModel> Tags { get; set; }

        /// <summary>
        /// Gets or sets Deck to Cards model.
        /// </summary>
        public DbSet<DeckToCardModel> DeckToCards { get; set; }

        /// <summary>
        /// Gets or sets Deck model.
        /// </summary>
        public DbSet<DeckModel> Decks { get; set; }

        /// <summary>
        /// Connect to databse on configuring.
        /// </summary>
        /// <param name="options">options for configuring.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(connectionString: $"Data Source={Path.Combine(Directory.GetCurrentDirectory(), "DatabaseDAL.db")}");
        }

        /// <summary>
        /// What to do if program calls a model .
        /// </summary>
        /// <param name="modelBuilder">Calls one of the models.</param>
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

            // modelBuilder.Entity<CardModel>()
            // .HasMany(p => p.Tags)
            // .WithOne();
            modelBuilder.Entity<TagToCardModel>()
                .HasOne(p => p.Card)
                .WithMany(b => b.Tags)
                .HasForeignKey(b => b.CardID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
