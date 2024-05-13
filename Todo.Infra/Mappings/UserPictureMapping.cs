using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Domain.Entities;

namespace Todo.Infra.Mappings;

public class UserPictureMapping : IEntityTypeConfiguration<UserPicture>
{
    public void Configure(EntityTypeBuilder<UserPicture> builder)
    {
        builder.
            ToTable<UserPicture>("UserPicture")
            .HasKey(x => x.UserPictureID);

        builder
            .Property(x => x.Picture)
            .IsRequired(false);
    }
}
