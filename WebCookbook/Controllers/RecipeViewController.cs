using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCookbook.Controllers
{
    public class RecipeViewController : Controller
    {
        // GET: RecipeView
        public ActionResult Index()
        {
            return View("~/Views/Recipes/Index.cshtml");
        }

        //// GET: RecipeView/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: RecipeView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RecipeView/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index", "RecipeView");
            }
            catch
            {
                return View();
            }
        }

        //// GET: RecipeView/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: RecipeView/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: RecipeView/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: RecipeView/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        public ActionResult AddIngredient()
        {
            return PartialView("~/Views/Ingredients/CreatePartial.cshtml");
        }
    }
}
