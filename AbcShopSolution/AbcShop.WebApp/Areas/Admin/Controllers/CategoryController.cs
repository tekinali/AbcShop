using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbcShop.Entities;
using AbcShop.BusinessLayer;
using AbcShop.BusinessLayer.Result;
using AbcShop.WebApp.Helpers;
using System.Net;
using AbcShop.WebApp.Filters;

namespace AbcShop.WebApp.Areas.Admin.Controllers
{
    [Exc]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        CategoryManager _categoryManager;

        public CategoryController()
        {
            _categoryManager = new CategoryManager();
        }

        // GET: Admin/Category
        public ActionResult Index()
        {
            var model = CacheHelper.GetCategoriesFromCache();
            return View(model);          
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = CacheHelper.GetCategoriesFromCache().Find(x => x.Id == id);

            if(category==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(category);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category model)
        {
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            ModelState.Remove("CreatedOn");
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Category> res = _categoryManager.Insert(model);
                if (res.Errors.Count > 0)
                {
                    // başarısız
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                else
                {
                    // başarılı          
                    CacheHelper.RemoveGetCategoriesFromCache();
                    CacheHelper.RemoveGetHomeProductsFromCache();
                    CacheHelper.RemoveGetProductsFromCache();

                    return RedirectToAction("Index", "Category");
                }
            }


            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = CacheHelper.GetCategoriesFromCache().Find(x => x.Id == id);

            if (category == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            ModelState.Remove("CreatedOn");
            if (ModelState.IsValid)
            {          
                BusinessLayerResult<Category> res = _categoryManager.Update(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                else
                {
                    CacheHelper.RemoveGetCategoriesFromCache();
                    CacheHelper.RemoveGetHomeProductsFromCache();
                    CacheHelper.RemoveGetProductsFromCache();

                    return RedirectToAction("Details", "Category", new { @id = model.Id });
                }
            }


            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryManager.Find(x => x.Id == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = _categoryManager.Find(x => x.Id == id);
            BusinessLayerResult<Category> res = _categoryManager.Delete(category);

            if (res.Errors.Count > 0)
            {
                res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                return View(category);
            }
            else
            {
                CacheHelper.RemoveGetCategoriesFromCache();
                CacheHelper.RemoveGetHomeProductsFromCache();
                CacheHelper.RemoveGetProductsFromCache();

                return RedirectToAction("Index", "Category");
            }
        }



    }
}