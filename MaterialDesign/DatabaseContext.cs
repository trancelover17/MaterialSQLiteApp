using Microsoft.EntityFrameworkCore;

namespace MaterialDesign
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Albums> Albums { get; set; }
        public DbSet<Artists> Artists { get; set; }
        public static string db_path { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(db_path);
    }
}
