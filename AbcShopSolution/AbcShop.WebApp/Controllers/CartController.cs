using AbcShop.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbcShop.BusinessLayer;
using AbcShop.WebApp.ViewModels.Cart;
using Microsoft.AspNet.Identity;
using AbcShop.Entities;
using AbcShop.BusinessLayer.Result;
using AbcShop.WebApp.Models.Notification;
using AbcShop.WebApp.Filters;

namespace AbcShop.WebApp.Controllers
{
    [Exc]
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private ProductManager _productManager;
        private AddressManager _addressManager;
        private OrderManager _orderManager;

        public CartController()
        {
            _productManager = new ProductManager();
            _addressManager = new AddressManager();
            _orderManager = new OrderManager();
        }

        // GET: Cart 
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(GetCart());
        }

        [AllowAnonymous]
        public Cart GetCart()
        {
            var cart = (Cart)Session["Cart"];

            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }

            return cart;
        }

        [AllowAnonymous]
        public PartialViewResult CartSymbol()
        {
            return PartialView("_PartialCartSymbol", GetCart());
        }

        [AllowAnonymous]
        public ActionResult AddToCart(Guid Id)
        {
            var product = _productManager.Find(x => x.Id == Id);

            if (product != null)
            {
                GetCart().AddProduct(product, 1);
            }

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult ReduceToProductCount(Guid Id)
        {
            var product = _productManager.Find(x => x.Id == Id);

            if (product != null)
            {
                GetCart().ReduceToProductCount(product);
            }

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult RemoveFromCart(Guid Id)
        {
            var product = _productManager.Find(x => x.Id == Id);

            if (product != null)
            {
                GetCart().DeleteProduct(product);
            }

            return RedirectToAction("Index");
        }


        [AllowAnonymous]
        public ActionResult ClearCart()
        {
            var cart = GetCart();
            cart.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Checkout()
        {
            var cart = GetCart();

            if (cart.CartLines.Count == 0)
            {
                ModelState.AddModelError("UrunYokError", "Sepetinizde ürün bulunmamaktadır.");
                return RedirectToAction("Index");
            }

            CheckoutViewModel model = new CheckoutViewModel();
            model.Total = cart.Total();

            var userId = User.Identity.GetUserId();

            var addressList = _addressManager.ListQueryable().Where(x => x.ApplicationUserId == userId).ToList();

            if (addressList.Count == 0)
            {
                ViewBag.NullAddress = true;
                return View(model);

            }

            ViewBag.AddressId = new SelectList(addressList, "Id", "Title");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var address = _addressManager.Find(x => x.Id == model.AddressId);
                var userId = User.Identity.GetUserId();
                var cart = GetCart();

                var order = new Order()
                {
                    AddressLine = address.AddressLine,
                    PostCode = address.PostCode,
                    CityId = address.CityId,
                    MobilePhone = address.MobilePhone,
                    OrderDate = DateTime.Now,
                    ApplicationUserId = userId,
                    Total = cart.Total(),
                    OrderLines = new List<OrderLine>()
                };
                foreach (var item in cart.CartLines)
                {
                    var oL = new OrderLine()
                    {
                        Quantity = item.Quantity,
                        Price=item.Product.Price*item.Quantity,
                        ProductId=item.Product.Id 
                    };
                    order.OrderLines.Add(oL);
                }

                BusinessLayerResult<Order> res = _orderManager.Insert(order);

                if(res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                else
                {

                    cart.Clear();

                    OkViewModel notifyObj = new OkViewModel()
                    {
                        Title = "Ödeme Başarılı.",
                        RedirectingUrl = Url.Action("UserOrders", "Account")
                    };

                    notifyObj.Items.Add("Siparişiniz alındı.");
                    notifyObj.Items.Add("Ödemenin onaylanması sonrasında siparişleriniz en kısa sürede hazırlanıp kargoya verilecektir.");

                    return View("Ok", notifyObj);
                  
                }

            }

            var uuserId = User.Identity.GetUserId();
            var addressList = _addressManager.ListQueryable().Where(x => x.ApplicationUserId == uuserId).ToList();
            ViewBag.AddressId = new SelectList(addressList, "Id", "Title");

            return View(model);
        }

    }
}