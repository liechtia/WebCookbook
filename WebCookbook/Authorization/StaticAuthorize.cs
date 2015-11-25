using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebCookbook.Models;

namespace WebCookbook.Authorization
{
    public static class StaticAuthorize
    {
        public static Boolean isAuthorized(int id, IPrincipal User)
        {
            if(!User.Identity.IsAuthenticated)
            {
                return false;
            }
            ApplicationDbContext db = new ApplicationDbContext();
            Recipe recipe = db.Recipes.Find(id);
            RecipeViewModel rmodel = new RecipeViewModel() { Recipe = recipe, Ingredients = recipe.Ingredients.ToList() };

            var authorized = checkForEditDeleteRights(rmodel, db, User.Identity.Name);
            return authorized;
        }

        private static Boolean checkForEditDeleteRights(RecipeViewModel recipe,ApplicationDbContext db, string UserName)
        {
            ApplicationUser appUser = db.Users.FirstOrDefault(x => x.UserName == UserName); // get current user
            var adminRole = db.Roles.FirstOrDefault(x => x.Name == "admin");
            bool isAdmin = false;
            if (adminRole != null)
            {
                var usersWithAdminRole = adminRole.Users.FirstOrDefault(x => x.UserId == appUser.Id);
                if (usersWithAdminRole != null)
                    isAdmin = true;
            }
            if (appUser.Equals(recipe.Recipe.User) || isAdmin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}