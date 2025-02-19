using Microsoft.EntityFrameworkCore;

namespace L01_2021RR650.Models
{
    public class BlogDBContext : DbContext
    {
        public BlogDBContext(DbContextOptions<BlogDBContext> options) : base(options) 
        {
        }

        public DbSet<roles> roles { get; set; }
        public DbSet<usuarios> usuarios { get; set; }
        public DbSet<calificaciones> calificaciones { get; set; }
        public DbSet<comentarios> comentarios { get; set; }
    }
}
