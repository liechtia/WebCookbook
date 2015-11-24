using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCookbook.Models
{
    public class Ingredient
    { 
        [Key]
        public int IngredientId { get; set; }
        public string Measurement { get; set; }
        public int AmountPerInitialServing { get; set; }
        public string IngredientName { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}