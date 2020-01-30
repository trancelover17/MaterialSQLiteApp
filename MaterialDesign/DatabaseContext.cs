using Microsoft.EntityFrameworkCore;

namespace MaterialDesign
{
    public class DatabaseContext : DbContext
    {
        private static string db_path;

        public DbSet<Albums> Albums { get; set; }
        public DbSet<Artists> Artists { get; set; }

        public static string Db_path { set 
            { 
                if (!string.IsNullOrEmpty(value))
                db_path = $@"Data Source={value}"; 
            } 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(db_path);
    }
}
