using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCookbook.Models;

namespace WebCookbook.Authorization
{
    public class EditDeleteAuthorize : AuthorizeAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string UserName;
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if(!authorized)
            {
                return false;
            }

            var rd = httpContext.Request.RequestContext.RouteData;
            int id;
            Int32.TryParse((string)rd.Values["id"], out id);
            UserName = httpContext.User.Identity.Name;

            Recipe recipe = db.Recipes.Find(id);
            RecipeViewModel model = new RecipeViewModel() { Recipe = recipe, Ingredients = recipe.Ingredients.ToList() };

            rd.Values["model"] = model;
            authorized = checkForEditDeleteRights(model);
            return authorized;
        }

        private Boolean checkForEditDeleteRights(RecipeViewModel recipe)
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