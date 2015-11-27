using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebCookbook.Authorization;
using WebCookbook.Models;
using static System.String;

namespace WebCookbook.Controllers
{
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class RecipeViewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private RecipeViewModel GetRecipeViewModelByRecipeId(int? id)
        {
            db = new ApplicationDbContext();
            Recipe recipe = db.Recipes.FirstOrDefault(r => r.RecipeId == id);
            IQueryable<Ingredient> ingredients = db.Ingredients.Where(i => i.Recipe.RecipeId == id);

            RecipeViewModel model = new RecipeViewModel() { Recipe = recipe, Ingredients = ingredients.ToList() };
            return model;
        }

        // GET: RecipeView
        public ActionResult Index()
        {
            List<RecipeViewModel> modelList = new List<RecipeViewModel>();
            foreach (Recipe recipe in db.Recipes)
            {
                RecipeViewModel model = GetRecipeViewModelByRecipeId(recipe.RecipeId);
                modelList.Add(model);
            }
            return View(modelList);
        }

        // GET: RecipeView/Create
        [Authorize]
        public ActionResult Create()
        {
            RecipeViewModel.IngredientCounter.Instance.IngredientCount = 0;
            return View(new RecipeViewModel());
        }

        // POST: RecipeView/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
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
                var appUser = db.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                completeRecipe.Recipe.User = appUser;
                appUser.Recipes.Add(completeRecipe.Recipe);
                PictureUpload(completeRecipe, file);
                RecipeViewModel.IngredientCounter.Instance.IngredientCount = 0;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RecipeView/Edit/5
        [EditDeleteAuthorize]
        public ActionResult Edit(RecipeViewModel model)
        {
            RecipeViewModel recipeViewModel = GetRecipeViewModelByRecipeId(model.Recipe.RecipeId);
            RecipeViewModel.IngredientCounter.Instance.IngredientCount = -1;
            return View(recipeViewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecipeViewModel recipeViewModel, HttpPostedFileBase file, bool deleteImage)
        {
            try
            {
                IList<Ingredient> ingredients = recipeViewModel.Ingredients;

                RecipeViewModel recipeViewModelByRecipeId = GetRecipeViewModelByRecipeId(recipeViewModel.Recipe.RecipeId);

                if (ModelState.IsValid)
                {
                    recipeViewModelByRecipeId.Recipe.Title = recipeViewModel.Recipe.Title;
                    recipeViewModelByRecipeId.Recipe.Instructions = recipeViewModel.Recipe.Instructions;
                    recipeViewModelByRecipeId.Recipe.InitialServings = recipeViewModel.Recipe.InitialServings;

                    IList<Ingredient> toDelete = recipeViewModelByRecipeId.Recipe.Ingredients.ToList();

                    foreach (Ingredient ingredient in toDelete)
                    {
                        db.Ingredients.Remove(ingredient);
                    }
                    db.SaveChanges();
                    foreach (Ingredient ingredient in ingredients)
                    {
                        ingredient.Recipe = recipeViewModelByRecipeId.Recipe;
                        recipeViewModelByRecipeId.Recipe.Ingredients.Add(ingredient);
                    }

                    db.Entry(recipeViewModelByRecipeId.Recipe).State = EntityState.Modified;

                    db.SaveChanges();
                    RecipeViewModel.IngredientCounter.Instance.IngredientCount = 0;

                    if (file != null && !deleteImage)
                    {
                        string url = PictureUpload(recipeViewModelByRecipeId, file);
                        recipeViewModel.Recipe.PictureUrl = url;
                        db.SaveChanges();
                        RedirectToAction("Index");
                    }
                    if (deleteImage)
                    {
                        recipeViewModel.Recipe.PictureUrl = null;
                        recipeViewModelByRecipeId.Recipe.PictureUrl = null;
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index", recipeViewModelByRecipeId);
            }
            catch (Exception e)
            {
                string msg = e.Message;
                return View();
            }
        }
       
        // GET: RecipeView/Delete/5
        [EditDeleteAuthorize]
        public ActionResult Delete(RecipeViewModel model)
        {
            return View(model);
        }

        // POST: RecipeView/Delete/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
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
                return View(GetRecipeViewModelByRecipeId(id));
            }
        }

        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            return View(GetRecipeViewModelByRecipeId(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string PictureUpload(RecipeViewModel model, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string fileName = Format(Guid.NewGuid() + Path.GetExtension(file.FileName));
                string uploadDir = "/Images";
                var imagePath = Path.Combine(Server.MapPath(uploadDir), fileName);
                var imageUrl = Path.Combine(uploadDir, fileName);
                file.SaveAs(imagePath);
                model.Recipe.PictureUrl = imageUrl;

                return imageUrl;
            }

            return Empty;
        }

        public PartialViewResult AddIngredient(RecipeViewModel model)
        {
            RecipeViewModel.IngredientCounter.Instance.IngredientCount++;
            return PartialView("~/Views/Ingredients/CreatePartial.cshtml", model);
        }

        public PartialViewResult AddIngredientEdit(int? recipeId)
        {
            RecipeViewModel model = GetRecipeViewModelByRecipeId(recipeId);
            model.Ingredients.Add(new Ingredient() {Recipe = model.Recipe});
            RecipeViewModel.IngredientCounter.Instance.IngredientCount++;
            return PartialView("~/Views/Ingredients/CreatePartial.cshtml", model);
        }
    }
}
