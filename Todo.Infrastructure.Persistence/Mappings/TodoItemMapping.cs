namespace Todo.Infrastructure.Persistence.Mappings;

public class TodoItemMapping : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder
            .ToTable("TodoItem")
            .HasKey(x => x.TodoItemID);

        builder
            .Property(x => x.TodoItemID)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder
            .Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.IsDone)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(x => x.DeadLine)
            .IsRequired(false);

        builder
            .Property(x => x.CreatedIn)
            .IsRequired()
            .HasDefaultValueSql("getdate()");
    }
}
