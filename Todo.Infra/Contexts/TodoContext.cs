using Microsoft.EntityFrameworkCore;
using Todo.Infra.Mappings;
using Todo.Domain.Entities;
using Todo.Domain.Shareds;

namespace Todo.Infra.Contexts
{
    public class TodoContext : DbContext
    {
        #region Properties

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPicture> UserPictures { get; set; }

        #endregion

        #region Constructors

        public TodoContext() : base() {}

        #endregion

        #region Events

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(AppSettings.StringConnection);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new UserPictureMapping());
            modelBuilder.ApplyConfiguration(new TodoItemMapping());
        }

        #endregion
    }
}
