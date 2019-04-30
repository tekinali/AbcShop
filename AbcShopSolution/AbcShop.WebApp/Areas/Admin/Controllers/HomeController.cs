using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbcShop.Entities;
using AbcShop.BusinessLayer;
using AbcShop.BusinessLayer.Result;
using AbcShop.WebApp.Areas.Admin.ViewModels.Home;
using Microsoft.AspNet.Identity;
using AbcShop.WebApp.Helpers;
using System.Globalization;
using AbcShop.WebApp.Filters;

namespace AbcShop.WebApp.Areas.Admin.Controllers
{
    [Exc]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        ProductManager _productManager;
        OrderManager _orderManager;
        UserManager _userManager;
        

        public HomeController()
        {
            _productManager = new ProductManager();
            _orderManager = new OrderManager();
            _userManager = new UserManager();
        }

        // GET: Admin/Home
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();

            model.Orders = _orderManager.ListQueryable().OrderByDescending(x=>x.OrderDate).Take(5).ToList();

            model.OrderCount = _orderManager.List().Count();
            model.ProductCount = _productManager.List().Count();
            model.UserCount = _userManager.ListUser().Count();
            model.OrderTotal = _orderManager.List().Select(x => x.Total).ToList().Sum();


            return View(model);
        }

        public ActionResult MyInfo()
        {
            var userId = User.Identity.GetUserId();
            var user = _userManager.Find(userId);

            MyInfoViewModel model = new MyInfoViewModel()
            {
                Username = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                City = user.City.Name
            };

            return View(model);
        }


        public ActionResult MyInfoEdit()
        {
            var userId = User.Identity.GetUserId();
            var user = _userManager.Find(userId);

            MyInfoEditViewModel model = new MyInfoEditViewModel()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                CityId = user.CityId
            };


            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name",model.CityId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyInfoEdit(MyInfoEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.Find(userId);

                user.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.Name).Trim();
                user.Surname = CultureInfo.CurrentCulture.TextInfo.ToUpper(model.Surname).Trim();
                user.Email = CultureInfo.CurrentCulture.TextInfo.ToLower(model.Email).Trim();
                user.CityId = model.CityId;

                BusinessLayerResult<ApplicationUser> res = _userManager.Update(user);

                if (res.Errors.Count() > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                else
                {
                    return RedirectToAction("MyInfo");
                }

            }


            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name",model.CityId);
            return View();
        }

        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();

            return RedirectToAction("Index", "Home",new { area=""});
         
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string newPassword)
        {
            if (!String.IsNullOrEmpty(newPassword))
            {
                var userId = User.Identity.GetUserId();
                bool res = _userManager.ChangePassword(userId, newPassword);
                if (res)
                {
                    // işlem başarılı
                    TempData["passwordRes"] = true;
                    return RedirectToAction("MyInfo", "Home",new { area="Admin"});
                }
            }
            TempData["passwordRes"] = false;

            return RedirectToAction("MyInfo", "Home");
        }


    }
}