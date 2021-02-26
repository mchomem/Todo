using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Models.Entities;

namespace Todo.Core.Models.DataBase.Mappings
{
    // TODO revisar
    // como configurar nomes para constraints
    // relacionamento entre a entidade User via Fluente API.

    public class TodoItemMapping : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> modelBuilder)
        {
            modelBuilder
                .ToTable("TodoItem")
                .HasKey(x => x.TodoItemID);

            modelBuilder
                .Property(x => x.TodoItemID)
                .IsRequired()
                .ValueGeneratedOnAdd();

            modelBuilder
                .Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder
                .Property(x => x.IsDone)
                .IsRequired()
                .HasDefaultValue(false);

            modelBuilder
                .Property(x => x.DeadLine)
                .IsRequired(false);

            modelBuilder
                .Property(x => x.CreatedIn)
                .IsRequired()
                .HasDefaultValueSql("getdate()");
        }
    }
}
