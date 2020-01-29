using Microsoft.EntityFrameworkCore;

namespace MaterialDesign
{
    public class DatabaseContext : DbContext
    {
        private static string _db_path;
        public DbSet<Albums> Albums { get; set; }
        public DbSet<Artists> Artists { get; set; }
        public static string db_path { set { _db_path = $@"Data Source={value}"; } }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(_db_path);
    }
}
