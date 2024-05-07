using Microsoft.EntityFrameworkCore;
using Todo.Data.Mappings;
using Todo.Models;

namespace Todo.Data
{
    public class TodoDataContext : DbContext
    {
        public TodoDataContext(DbContextOptions<TodoDataContext> options)
            : base(options) { }
        
        public DbSet<TodoModel> Todos { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TodoMap());
        }

    }
}
