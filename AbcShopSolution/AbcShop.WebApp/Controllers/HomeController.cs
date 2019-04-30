using AbcShop.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbcShop.Entities;
using AbcShop.WebApp.Models;
using PagedList;
using System.Net;
using System.Globalization;
using AbcShop.BusinessLayer;
using AbcShop.WebApp.Filters;

namespace AbcShop.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        ContactManager _contactManager;

        public HomeController()
        {
            _contactManager = new ContactManager();
        }


        // GET: Home
        public ActionResult Index(int Page = 1)
        {
            var products = CacheHelper.GetHomeProductsFromCache()
                .Select(x => new ProductModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title.Length > 50 ? x.Title.Substring(0, 47) + "..." : x.Title,
                    Price = x.Price,
                    ProductCode = x.ProductCode,
                    MainImage = x.MainImage
                });

            return View(products.ToList().ToPagedList(Page, 8));
        }

        public ActionResult ProductList(int Page = 1, int? id = -1)
        {
            if (id == null || id == -1)
            {
                var products = CacheHelper.GetProductsFromCache()
                .Select(x => new ProductModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title.Length > 50 ? x.Title.Substring(0, 47) + "..." : x.Title,
                    Price = x.Price,
                    ProductCode = x.ProductCode,
                    MainImage = x.MainImage,
                    CategoryId = x.CategoryId
                });

                return View(products.ToList().ToPagedList(Page, 8));
            }
            else
            {
                var category = CacheHelper.GetCategoriesFromCache().Find(x => x.Id == id);
                if (category == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var products = CacheHelper.GetProductsFromCache().Where(c=>c.CategoryId==id) 
                    .Select(x => new ProductModel()
                      {
                          Id = x.Id,
                          Name = x.Name,
                          Title = x.Title.Length > 50 ? x.Title.Substring(0, 47) + "..." : x.Title,
                          Price = x.Price,
                          ProductCode = x.ProductCode,
                          MainImage = x.MainImage,
                          CategoryId = x.CategoryId
                      });
                ViewBag.CategoryId = id;              

                return View(products.ToList().ToPagedList(Page, 8));
            }


        }

        [ChildActionOnly]
        public PartialViewResult GetCategories()
        {
            var categories = CacheHelper.GetCategoriesFromCache();

            return PartialView("_PartialGetCategories", categories);

        }

        public ActionResult ProductDetails(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = CacheHelper.GetProductsFromCache().Find(x => x.Id == id);

            if(product==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product.Features = HttpUtility.HtmlDecode(product.Features);
            product.Description = HttpUtility.HtmlDecode(product.Description);

            return View(product);
        }


        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactMessage model)
        {
            if (ModelState.IsValid)
            {
                ContactMessage message = new ContactMessage()
                {
                    Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.Name).Trim(),
                    Surname = CultureInfo.CurrentCulture.TextInfo.ToUpper(model.Surname).Trim(),
                    Email = CultureInfo.CurrentUICulture.TextInfo.ToLower(model.Email).Trim().Replace(" ", string.Empty),
                    Subject = model.Subject.Trim(),
                    Text = model.Text.Trim(),
                    IsRead = false,
                    DateTime = DateTime.Now
                };
                int db_Res = _contactManager.Insert(message);
                if (db_Res > 0)
                {
                    ViewBag.Result = true;
                }
                else
                {
                    ViewBag.Result = false;
                }

            }
            return View(model);
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }


    }
}