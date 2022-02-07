using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Gwenael.Domain;

namespace Gwenael.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<GwenaelDbContext>
    {
        public GwenaelDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<GwenaelDbContext>()
                .UseSqlServer(
                    @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Gwenael;Integrated Security=True;",
                    optionsBuilder =>
                        optionsBuilder.MigrationsAssembly(typeof(DesignTimeDbContextFactory).Assembly.FullName))
                .Options;

            return new GwenaelDbContext(options);
        }
    }
}