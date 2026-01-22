namespace Todo.Infrastructure.Persistence.Mappings;

public class UserPictureMapping : IEntityTypeConfiguration<UserPicture>
{
    public void Configure(EntityTypeBuilder<UserPicture> builder)
    {
        builder.
            ToTable("UserPicture")
            .HasKey(x => x.UserPictureID);

        builder
            .Property(x => x.Picture)
            .IsRequired(false);
    }
}
