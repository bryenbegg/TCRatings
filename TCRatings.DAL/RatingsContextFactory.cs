using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TCRatings.DAL
{
    public class RatingsContextFactory : IDesignTimeDbContextFactory<RatingsContext>
    {
        RatingsContext IDesignTimeDbContextFactory<RatingsContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RatingsContext>();
            optionsBuilder.UseSqlServer("Server=tcp:tcratingscomp.database.windows.net,1433;Database=coreDB;User ID=dbadmin;Password=Transport1!;Encrypt=true;Connection Timeout=30;"); 

            return new RatingsContext(optionsBuilder.Options);
        }
    }
}