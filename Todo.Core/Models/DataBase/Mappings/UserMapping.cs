using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Models.Entities;

namespace Todo.Core.Models.DataBase.Mappings
{
    // TODO revisar
    // como configurar nomes para constraints
    // relacionamento entre a entidade TodoItem via Fluente API.

    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder
                .ToTable<User>("User")
                .HasKey(x => x.UserID);

            modelBuilder
                .Property(x => x.UserID)
                .IsRequired()
                .ValueGeneratedOnAdd();

            modelBuilder
                .Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder
                .Property(x => x.Login)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder
                .HasIndex(x => x.Login)
                .IsUnique();

            modelBuilder
                .Property(x => x.Password)
                .HasMaxLength(300)
                .IsRequired();

            modelBuilder
                .Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
