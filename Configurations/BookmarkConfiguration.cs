using BookmarkManager.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookmarkManager.Configurations;

public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
{
    public void Configure(EntityTypeBuilder<Bookmark> builder)
    {
        builder.ToTable("bookmarks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Title)
            .HasColumnName("title")
            .HasMaxLength(500);

        builder.Property(x => x.Url)
            .HasColumnName("url")
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(x => x.NormalizedUrl)
            .HasColumnName("normalized_url")
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("SYSUTCDATETIME()")
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();

        // Only active bookmarks must have unique normalized URLs.
        builder.HasIndex(x => x.NormalizedUrl)
            .IsUnique()
            .HasDatabaseName("UX_bookmarks_normalized_url_active")
            .HasFilter("is_deleted = 0");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
