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
        public IList<Rating> Ratings { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }
        
        //the singleton ingredient counter is used to create and edit several ingredients per recipe dynamically
        public class IngredientCounter
        {
            private static IngredientCounter instance;
            public int IngredientCount { get; set; }

            private IngredientCounter()
            {
                IngredientCount = 0;
            }

            public static IngredientCounter Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new IngredientCounter();
                    }
                    return instance;
                }
            }

        }
    }
}