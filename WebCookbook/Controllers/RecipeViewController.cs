using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebCookbook.Models;

namespace WebCookbook.Controllers
{
    public class RecipeViewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RecipeView
        public ActionResult Index()
        {
            return View(db.Recipes);
        }

        // GET: RecipeView/Create
        public ActionResult Create()
        {
            RecipeViewModel.IngredientCounter.Instance.IngredientCount = 0;
            return View(new RecipeViewModel());
        }

        // POST: RecipeView/Create
        [HttpPost]
        public ActionResult Create(RecipeViewModel completeRecipe, HttpPostedFileBase file)
        {
            try
            {
                foreach (Ingredient ingredient in completeRecipe.Ingredients)
                {
                    ingredient.Recipe = completeRecipe.Recipe;
                    completeRecipe.Recipe.Ingredients.Add(ingredient);

                    if (ModelState.IsValid)
                    {
                        db.Ingredients.Add(ingredient);
                    }
                }

                PictureUpload(completeRecipe, file);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                var exception = e;
                return View();
            }
        }

        // GET: RecipeView/Edit/5
        public ActionResult Edit(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            RecipeViewModel model = new RecipeViewModel() { Recipe = recipe, Ingredients = recipe.Ingredients.ToList() };
            return View(model);
        }

        // POST: RecipeView/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RecipeView/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RecipeView/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Recipe recipe = db.Recipes.Find(id);
                db.Recipes.Remove(recipe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);

            if (recipe == null)
            {
                return HttpNotFound();
            }

            RecipeViewModel model = new RecipeViewModel() { Recipe = recipe, Ingredients = recipe.Ingredients.ToList()};
            return View(model);
        }

        [HttpPost]
        public void PictureUpload(RecipeViewModel model, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string fileName = string.Format(Guid.NewGuid() + Path.GetExtension(file.FileName));
                string uploadDir = "/Images";
                var imagePath = Path.Combine(Server.MapPath(uploadDir), fileName);
                var imageUrl = Path.Combine(uploadDir, fileName);
                file.SaveAs(imagePath);
                model.Recipe.PictureUrl = imageUrl;
            }
        }

        public PartialViewResult AddIngredient(RecipeViewModel model)
        {
            RecipeViewModel.IngredientCounter.Instance.IngredientCount =
                RecipeViewModel.IngredientCounter.Instance.IngredientCount + 1;
            return PartialView("~/Views/Ingredients/CreatePartial.cshtml", model);
        }
    }
}
