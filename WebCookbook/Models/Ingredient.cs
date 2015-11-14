using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCookbook.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Measurement { get; set; }
        public int AmountPerInitialServing { get; set; }
        public string IngredientName { get; set; }
        public int RecId { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}