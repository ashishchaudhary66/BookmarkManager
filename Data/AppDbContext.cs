using Microsoft.EntityFrameworkCore;
using BookmarkManager.Models.Entities;

namespace BookmarkManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bookmark> Bookmarks => Set<Bookmark>();

        public DbSet<Tag> Tags => Set<Tag>();

        public DbSet<BookmarkTag> BookmarkTags => Set<BookmarkTag>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
