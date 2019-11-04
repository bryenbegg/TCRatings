using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TCRatings.Models
{
    public class Rating : TCRatings.DAL.Models.Rating
    {
        public Rating() : base()
        {
            RatingTypeNames = new List<SelectListItem>();
        }

        [Display(Name = "Rating")]
        public string RatingTypeName { get; set; }

        public ICollection<SelectListItem> RatingTypeNames;

        public void FromClient(TCRatings.Client.Rating objIn, IEnumerable<TCRatings.Client.RatingType> lstTypes )
        {
            Id = objIn.Id;
            Comments = objIn.Comments;
            CreatedDate = objIn.CreatedDate.DateTime;
            RatingTypeId = objIn.RatingTypeId;
            RatingTypeName = lstTypes.Where(x => x.Id.Equals(objIn.RatingTypeId)).FirstOrDefault().Name;
        }
    }
}
