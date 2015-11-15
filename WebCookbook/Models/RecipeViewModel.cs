using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCookbook.Models
{
    public class RecipeViewModel
    {
        public Recipe Recipe { get; set; }
        public IList<Ingredient> Ingredients { get; set; } 
    }
}