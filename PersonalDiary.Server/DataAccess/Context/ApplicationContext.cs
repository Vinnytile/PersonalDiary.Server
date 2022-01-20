using Microsoft.EntityFrameworkCore;
using SharedData.Models;

namespace DataAccess.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Note> Notes { get; set; }
    }
}
