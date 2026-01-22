namespace Todo.Infrastructure.Persistence.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("User")
            .HasKey(x => x.UserID);

        builder
            .Property(x => x.UserID)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Login)
            .HasMaxLength(20)
            .IsRequired();

        builder
            .HasIndex(x => x.Login)
            .IsUnique();

        builder
            .Property(x => x.Password)
            .HasMaxLength(300)
            .IsRequired();

        builder
            .Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder
            .HasOne(u => u.Picture)
            .WithOne(up => up.User)
            .HasForeignKey<UserPicture>(u => u.PictureFromUserID)
            .HasConstraintName("FK_User_UserPicture_PictureFromUserID")
            .OnDelete(DeleteBehavior.NoAction);
    }
}
