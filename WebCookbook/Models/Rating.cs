using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCookbook.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public ApplicationUser User { get; set; }
        public bool Like { get; set; }
    }
}