namespace WebCookbook.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using Microsoft.AspNet.Identity;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<WebCookbook.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "WebCookbook.Models.ApplicationDbContext";
        }

        protected override void Seed(WebCookbook.Models.ApplicationDbContext context)
        {
            if(!context.Users.Any(u => u.UserName == "admin"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "admin", Email = "admin@admin.ch" };
                //IdentityRole role = new IdentityRole("admin");
                //IdentityUserRole userRole = new IdentityUserRole();
                //user.Roles.Add(userRole);
                //role.Users.Add(userRole);
                //context.Roles.Add(role);
                manager.Create(user, "password");
                
            }
            var IngredientsA = new List<Ingredient>();
            IngredientsA.Add(new Ingredient { AmountPerInitialServing = 150, Measurement = "mg", IngredientName = "Mehl" });
            var IngredientsB = new List<Ingredient>();
            IngredientsB.Add(new Ingredient { AmountPerInitialServing = 150, Measurement = "mg", IngredientName = "Mehl" });
            IngredientsB.Add(new Ingredient { AmountPerInitialServing = 4, Measurement = "Stück", IngredientName = "Eier" });
            context.Recipes.AddOrUpdate(
                r => r.Title,
                new Recipe { Title = "Kuchen", InitialServings = 1, Instructions="Vermische alles" ,Ingredients = IngredientsA, User = context.Users.FirstOrDefault(u => u.UserName=="user")},
                new Recipe { Title = "Crepes", InitialServings = 4, Instructions = "Vermische alles", Ingredients = IngredientsB, User = context.Users.FirstOrDefault(u => u.UserName == "user") }
                );
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
