using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using TCRatings.Client;

namespace TCRatings.Controllers
{
    public class RatingsController : Controller
    {
        private readonly IConfiguration _config;

        private readonly RatingsClient clientRating = new RatingsClient(new System.Net.Http.HttpClient());
        private readonly RatingTypesClient clientRatingType = new RatingTypesClient(new System.Net.Http.HttpClient());

        public RatingsController(IConfiguration config)
        {
            _config = config;

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                clientRating.BaseUrl = _config.GetValue<string>("APIBaseURL");
                clientRatingType.BaseUrl = _config.GetValue<string>("APIBaseURL");
            }
        }

        // GET: Ratings
        public ActionResult Index()
        {
            var allRatingsRaw = clientRating.GetRatingAllAsync().Result;
            var lstAllRatings = new List<Models.Rating>();

            var allRatingTypesRaw = clientRatingType.GetRatingTypeAllAsync().Result;

            foreach (var rating in allRatingsRaw)
            {
                var curRating = new Models.Rating();
                curRating.FromClient(rating, allRatingTypesRaw);
                lstAllRatings.Add(curRating);
            }

            
            return View(lstAllRatings);
        }

        // GET: Ratings/Details/5
        public ActionResult Details(int id)
        {
            var curRatingRaw = clientRating.GetRatingAsync(id).Result;

            var allRatingTypesRaw = clientRatingType.GetRatingTypeAllAsync().Result;

            var curRating = new Models.Rating();
            curRating.FromClient(curRatingRaw, allRatingTypesRaw);

            return View(curRating);
        }

        // GET: Ratings/Create
        public ActionResult Create()
        {
            var newRating = new Models.Rating();
            var allRatingTypesRaw = clientRatingType.GetRatingTypeAllAsync().Result;

            foreach (var curType in allRatingTypesRaw)
            {
                newRating.RatingTypeNames.Add(new SelectListItem() { Value = curType.Id.ToString(), Text = curType.Name });
            }
            

            return View(newRating);
        }

        // POST: Ratings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {

                Client.Rating newRating = new Client.Rating();
                newRating.Comments = collection.Where(x => x.Key.Equals("Comments")).Select(x => x.Value).FirstOrDefault();
                newRating.RatingTypeId = int.Parse(collection.Where(x => x.Key.Equals("RatingTypeId")).Select(x => x.Value).FirstOrDefault());
                newRating.CreatedDate = DateTime.Now;

                _ = clientRating.PostRatingAsync(newRating).GetAwaiter().GetResult();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

    }
}