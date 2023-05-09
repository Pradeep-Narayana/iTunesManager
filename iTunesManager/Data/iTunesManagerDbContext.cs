using iTunesManager.Models;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace iTunesManager.Data
{
    public class iTunesManagerDbContext : DbContext
    {
        public DbSet<ClickCountModel> ClickCountModels { get; set; }
        public DbSet<RecentAccessModel> RecentAccessModels { get; set; }

        public iTunesManagerDbContext() : base("name=iTunesManagerDbContext")
        {
        }
    }
}