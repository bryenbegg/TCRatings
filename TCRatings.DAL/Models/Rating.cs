using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TCRatings.DAL.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Rating")]
        public int RatingTypeId { get; set; }


        [Display(Name = "Comments")]
        [MaxLength(250)]
        public string Comments { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        public Rating()
        {
            CreatedDate = DateTime.Now;
        }

    }
}
