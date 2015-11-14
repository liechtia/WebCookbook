using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCookbook.Models
{
    public class Recipe
    {
        public Recipe()
        {
            Ingredients = new List<Ingredient>();
        }

        public int RecipeId { get; set; }
        public string Title { get; set; }
        public int InitialServings { get; set; }
        public string Instructions { get; set; }
        public ApplicationUser User { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
}