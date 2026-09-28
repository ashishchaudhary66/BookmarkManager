using BookmarkManager.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookmarkManager.Configurations;

public class BookmarkTagConfiguration : IEntityTypeConfiguration<BookmarkTag>
{
    public void Configure(EntityTypeBuilder<BookmarkTag> builder)
    {
        builder.ToTable("bookmark_tags");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.BookmarkId)
            .HasColumnName("bookmark_id")
            .IsRequired();

        builder.Property(x => x.TagId)
            .HasColumnName("tag_id")
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

        // Bookmark -> BookmarkTag
        builder.HasOne(x => x.Bookmark)
            .WithMany(x => x.BookmarkTags)
            .HasForeignKey(x => x.BookmarkId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_bookmark_tags_bookmarks");

        // Tag -> BookmarkTag
        builder.HasOne(x => x.Tag)
            .WithMany(x => x.BookmarkTags)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_bookmark_tags_tags");

        // Prevent duplicate active bookmark/tag relationships.
        builder.HasIndex(x => new
            {
                x.BookmarkId,
                x.TagId
            })
            .IsUnique()
            .HasDatabaseName("UX_bookmark_tags_active")
            .HasFilter("is_deleted = 0");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
