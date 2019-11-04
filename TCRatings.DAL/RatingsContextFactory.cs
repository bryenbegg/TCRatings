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
            optionsBuilder.UseSqlite("Data Source=blog.db");

            return new RatingsContext(optionsBuilder.Options);
        }
    }
}