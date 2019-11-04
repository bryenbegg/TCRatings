using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TCRatings.DAL;
using TCRatings.DAL.Models;

namespace TCRatings.API.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    [ApiController]
    public class RatingTypesController : ControllerBase
    {
        private readonly RatingsContext _context;

        public RatingTypesController(RatingsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all Rating Types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RatingType>>> GetRatingType()
        {
            return await _context.RatingType.ToListAsync();
        }

        /// <summary>
        /// Gets a specific Rating Type
        /// </summary>
        /// <param name="id">ID for the Rating Type</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RatingType>> GetRatingType(int id)
        {
            var ratingType = await _context.RatingType.FindAsync(id);

            if (ratingType == null)
            {
                return NotFound();
            }

            return ratingType;
        } 

        private bool RatingTypeExists(int id)
        {
            return _context.RatingType.Any(e => e.Id == id);
        }
    }
}
