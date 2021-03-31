using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Todo.Core.Models.Entities;

namespace Todo.Core.Models.DataBase.Mappings
{
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
}
