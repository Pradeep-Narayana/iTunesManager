using iTunesManager.Models;
using System.Data.Entity;

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