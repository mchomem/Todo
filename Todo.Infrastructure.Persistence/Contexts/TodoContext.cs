namespace Todo.Infrastructure.Persistence.Contexts;

public class TodoContext : DbContext
{
    #region Properties

    public DbSet<User> Users { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<UserPicture> UserPictures { get; set; }

    #endregion

    #region Constructors

    public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

    #endregion

    #region Events

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    #endregion
}
