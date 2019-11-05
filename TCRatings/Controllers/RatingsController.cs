using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using TCRatings.Client;

namespace TCRatings.Controllers
{
    public class RatingsController : Controller
    {
        private readonly IConfiguration _config;

        private RatingsClient clientRating;
        private RatingTypesClient clientRatingType;

        public System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

        public RatingsController(IConfiguration config)
        {
            _config = config;

            // Only if in production, replace the base URL of the clients that were auto-generated. 
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                

                clientRating = new RatingsClient(client);
                clientRatingType = new RatingTypesClient(client);


                clientRating.BaseUrl = _config.GetValue<string>("APIBaseURL");
                clientRatingType.BaseUrl = _config.GetValue<string>("APIBaseURL");

            }
            else
            {
                clientRating = new RatingsClient(client);
                clientRatingType = new RatingTypesClient(client);
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Only if in production, where Azure AD is present, add the fetching of the auth token and pass it along. 
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {

                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Request.Headers["X-MS-TOKEN-AAD-ACCESS-TOKEN"]);

                clientRating = new RatingsClient(client);
                clientRatingType = new RatingTypesClient(client);

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

                // force waiting for the callback in order to see the results when moving to the list view
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