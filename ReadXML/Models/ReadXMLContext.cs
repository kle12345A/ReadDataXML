using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ReadXML.Models
{
    public partial class ReadXMLContext : DbContext
    {
        public ReadXMLContext()
        {
        }

        public ReadXMLContext(DbContextOptions<ReadXMLContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Xmldatum>  Xmldata { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                string ConnectionStr = config.GetConnectionString("MyContr");
                optionsBuilder.UseSqlServer(ConnectionStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Xmldatum>(entity =>
            {
                entity.ToTable("XMLData");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Author)
                    .HasMaxLength(255)
                    .HasColumnName("author");

                entity.Property(e => e.Genre)
                    .HasMaxLength(100)
                    .HasColumnName("genre");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.PublishDate)
                    .HasColumnType("datetime")
                    .HasColumnName("publish_date");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
