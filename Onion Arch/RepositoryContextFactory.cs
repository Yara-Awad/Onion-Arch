using Microsoft.EntityFrameworkCore;
using Repository;

namespace Onion_Arch
{
    public class RepositoryContextFactory
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
             .Build();

            var builder = new DbContextOptionsBuilder<RepositoryContext>()

           .UseSqlServer(configuration.GetConnectionString("sqlConnection"));

            return new RepositoryContext(builder.Options);
        }
    }
}
