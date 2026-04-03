using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Buradaki bağlantı dizesi sadece migration oluşturmak içindir.
            // Docker'daki PostgreSQL bilgilerini buraya yazıyoruz.
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=courier_db;Username=ozgur;Password=password123");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
