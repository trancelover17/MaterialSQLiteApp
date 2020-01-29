using Microsoft.EntityFrameworkCore;

namespace MaterialDesign
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Albums> Albums { get; set; }
        public DbSet<Artists> Artists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=C:\Users\vyacheslav.v.sorokin\Documents\!Разработки мои\mydb.db");
    }
}
