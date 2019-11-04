using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TCRatings.DAL;
using TCRatings.DAL.Models;

namespace TCRatings.API.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var ctx = new RatingsContext(serviceProvider.GetRequiredService<DbContextOptions<RatingsContext>>()))
            {
                //Check if the Types are setup

                if (ctx.RatingType.Any())
                {
                    return; // DB has already been seeded
                }

                var lstRatingTypes = new List<RatingType>();
                lstRatingTypes.Add(new RatingType
                {
                    //Id = 1,
                    Name = "Excellent"
                });

                lstRatingTypes.Add(new RatingType
                {
                    //Id = 2,
                    Name = "Moderate"
                });

                lstRatingTypes.Add(new RatingType
                {
                    //Id = 3,
                    Name = "Needs Improvement"
                });

                ctx.RatingType.AddRange(lstRatingTypes);

                ctx.Rating.AddRange(
                    new Rating
                    {
                        //Id = 1,
                        Comments = "This was made on seeding of the database.",
                        CreatedDate = DateTime.Now,
                        RatingTypeId = 1
                    },
                    new Rating
                    {
                        Comments = "This was made as well on the seeding of the database.",
                        CreatedDate = DateTime.Now,
                        RatingTypeId = 3
                    }
                    );
                ctx.SaveChanges();
            }
        }
    }
}
