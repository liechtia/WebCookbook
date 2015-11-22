﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebCookbook.Authorization;
using WebCookbook.Models;

namespace WebCookbook.Controllers
{
    public class RecipeViewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private RecipeViewModel GetRecipeViewModelByRecipeId(int? id)
        {
            Recipe recipe = db.Recipes.Find(id);
            RecipeViewModel model = new RecipeViewModel() { Recipe = recipe, Ingredients = recipe.Ingredients.ToList() };
            return model;
        }

        // GET: RecipeView
        public ActionResult Index()
        {
            return View(db.Recipes);
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
            RecipeViewModel.IngredientCounter.Instance.IngredientCount = 0;
            return View(model);
        }

        // POST: RecipeView/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(RecipeViewModel model, HttpPostedFileBase file)
        {
            try
            {
                IList<Ingredient> ingredients = model.Ingredients;
                Recipe recipe = model.Recipe;

                var recipeViewModelByRecipeId = GetRecipeViewModelByRecipeId(model.Recipe.RecipeId);

                //RecipeViewModel model = GetRecipeViewModelByRecipeId(id);

                //if (ModelState.IsValid)
                //{
                //    db.Entry(model.Recipe).State = EntityState.Modified;

                //    foreach (Ingredient ingredient in model.Ingredients)
                //    {
                //        db.Entry(ingredient).State = EntityState.Modified;
                //    }

                //    PictureUpload(model, file);
                RecipeViewModel.IngredientCounter.Instance.IngredientCount = 0;
                //    db.SaveChanges();
                //    return RedirectToAction("Index");
                //}

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                var exception = e;
                return View(model);
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
            RecipeViewModel.IngredientCounter.Instance.IngredientCount++;
            return PartialView("~/Views/Ingredients/CreatePartial.cshtml", model);
        }

        public PartialViewResult AddIngredientEdit(RecipeViewModel model)
        {
            RecipeViewModel.IngredientCounter.Instance.IngredientCount++;
            var ingredients = model.Ingredients;
            return PartialView("~/Views/Ingredients/CreatePartial.cshtml", model);
        }

    }
}
