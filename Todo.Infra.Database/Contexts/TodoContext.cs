namespace Todo.Infra.Database.Contexts;

public class TodoContext : DbContext
{
    #region Properties

    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserPicture> UserPictures { get; set; }

    #endregion

    #region Constructors

    public TodoContext() : base() { }

    #endregion

    #region Events

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer(AppSettings.StringConnection);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    #endregion
}
