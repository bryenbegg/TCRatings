using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace TCRatings.DAL
{
    public class RatingsContext : DbContext 
    {

        public RatingsContext(DbContextOptions<RatingsContext> options) : base(options)
        {
        }

        public DbSet<TCRatings.DAL.Models.Rating> Rating { get; set; }
        public DbSet<TCRatings.DAL.Models.RatingType> RatingType { get; set; }
    }
}

