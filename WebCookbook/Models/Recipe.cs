using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [DataType(DataType.MultilineText)]
        public string Instructions { get; set; }

        public string PictureUrl { get; set; }

        public ApplicationUser User { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}