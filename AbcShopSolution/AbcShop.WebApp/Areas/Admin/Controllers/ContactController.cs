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
    public class ContactController : Controller
    {
        ContactManager _contactManager;

        public ContactController()
        {
            _contactManager = new ContactManager();
        }


        // GET: Admin/Contact
        public ActionResult Index()
        {
            var model = _contactManager.List();


            return View(model);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactMessage model = _contactManager.Find(x => x.Id == id);

            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _contactManager.Read((int)id);

            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var message = _contactManager.Find(x => x.Id == id);
            int res = _contactManager.Delete(message);

            return RedirectToAction("Index", "Contact");
        }

    }
}