using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCookbook.Models
{
    public class RecipesViewModel
    {
        public Recipe Recipe { get; set; }
        public IList<Ingredient> Ingredients { get; set; } 
    }
}