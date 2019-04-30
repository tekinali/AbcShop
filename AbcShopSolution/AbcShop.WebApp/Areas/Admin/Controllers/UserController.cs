using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbcShop.Entities;
using AbcShop.BusinessLayer;
using System.Net;
using AbcShop.WebApp.Filters;

namespace AbcShop.WebApp.Areas.Admin.Controllers
{
    [Exc]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        UserManager _userManager;

        public UserController()
        {
            _userManager = new UserManager();
        }

        // GET: Admin/User
        public ActionResult Index()
        {
            var model = _userManager.ListUser();
            return View(model);
        }

        public ActionResult Details(string id)
        {
            if(String.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = _userManager.Find(id);

            if(user==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(user);
        }

        public ActionResult UpdateStatus(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = _userManager.Find(userId);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var res = _userManager.ChangeStatus(userId);

            return RedirectToAction("Details", "User",new { id= userId });
        }


    }
}