using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbcShop.Entities;
using AbcShop.BusinessLayer;
using AbcShop.BusinessLayer.Result;
using System.Net;
using AbcShop.WebApp.Filters;

namespace AbcShop.WebApp.Areas.Admin.Controllers
{
    [Exc]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {

        OrderManager _orderManager;
        OrderStateManager _stateManager;

        public OrderController()
        {
            _orderManager = new OrderManager();
            _stateManager = new OrderStateManager();
        }


        // GET: Admin/Order
        public ActionResult Index()
        {
            var orders = _orderManager.List();
            return View(orders);
        }

        public ActionResult Details(Guid? id)
        {
            var states = _stateManager.List();
        

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = _orderManager.Find(x => x.Id == id);
            if(order==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            ViewBag.StateId =  new SelectList(states, "Id", "State",order.OrderStateId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOrderState(Guid id,int OrderStateId)
        {
            var order = _orderManager.Find(x => x.Id == id);

            order.OrderStateId = OrderStateId;
            int res = _orderManager.Update(order);
            
                       
            return RedirectToAction("Index");
        }



    }
}