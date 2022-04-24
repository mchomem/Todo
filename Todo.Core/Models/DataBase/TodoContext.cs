using Microsoft.EntityFrameworkCore;
using Todo.Core.Models.DataBase.Mappings;
using Todo.Core.Models.Entities;

namespace Todo.Core.Models.DataBase
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
