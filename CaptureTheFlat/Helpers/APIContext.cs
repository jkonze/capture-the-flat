using CaptureTheFlat.Models;
using Microsoft.EntityFrameworkCore;

namespace CaptureTheFlat.Helpers
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
    }
}