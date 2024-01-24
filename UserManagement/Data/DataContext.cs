using Microsoft.EntityFrameworkCore;
using UserManagement.Models;


namespace UserManagement.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }

    }
}
