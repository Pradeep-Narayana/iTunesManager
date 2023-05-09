using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using iTunesManager.Models;
using Microsoft.EntityFrameworkCore;

namespace iTunesManager.Data
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<ClickCountModel> ClickCountModel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("FileName=iTunesManagerDb.db", option => {
                option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClickCountModel>().ToTable("ClickCountsTable", "test");
            modelBuilder.Entity<ClickCountModel>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.HasIndex(i => i.TrackName).IsUnique();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}