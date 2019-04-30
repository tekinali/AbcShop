using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbcShop.Entities;
using AbcShop.Entities.ValueObjects;
using AbcShop.WebApp.Helpers;
using AbcShop.BusinessLayer;
using AbcShop.BusinessLayer.Result;
using AbcShop.WebApp.Models.Notification;
using AbcShop.WebApp.ViewModels.Account;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Globalization;
using AbcShop.WebApp.Filters;

namespace AbcShop.WebApp.Controllers
{
    [Exc]
    [Authorize(Roles = "User")]
    public class AccountController : Controller
    {
        private UserManager _userManager;
        private AddressManager _addressManager;
        private OrderManager _orderManager;
        private ResetPasswordManager _resetPasswordManager;

        public AccountController()
        {
            _userManager = new UserManager();
            _addressManager = new AddressManager();
            _orderManager = new OrderManager();
            _resetPasswordManager = new ResetPasswordManager();
        }

        
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            if(Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<ApplicationUser> res = _userManager.Login(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(model);
                }

                if (!String.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }


        [AllowAnonymous]
        public ActionResult LoginAdmin(string ReturnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LoginAdmin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<ApplicationUser> res = _userManager.LoginAdmin(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(model);
                }

                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            return View(model);
        }


        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<ApplicationUser> res = _userManager.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.CityId);
                    return View(model);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = Url.Action("Login", "Account")
                };

                notifyObj.Items.Add("Giriş sayfasına yönlendiriliyorsunuz.");                

                return View("Ok", notifyObj);
            }

            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.CityId);
            return View(model);
        }

        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserProfile()
        {
            var userId = User.Identity.GetUserId();

            var user = _userManager.Find(userId);

            UserProfileViewModel model = new UserProfileViewModel()
            {
                Username = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                City = user.City.Name
            };

            return View(model);
        }

        public ActionResult UserProfileEdit()
        {
            var userId = User.Identity.GetUserId();
            var user = _userManager.Find(userId);

            UserProfileEditViewModel model = new UserProfileEditViewModel()
            {              
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                CityId = user.CityId
            };


            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.CityId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfileEdit(UserProfileEditViewModel model)
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

                if(res.Errors.Count()>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                else
                {
                    return RedirectToAction("UserProfile");
                }

            }
            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.CityId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string newPassword)
        {
            if(!String.IsNullOrEmpty(newPassword))
            {
                var userId = User.Identity.GetUserId();   
                bool res = _userManager.ChangePassword(userId,newPassword);
                if(res)
                {
                    // işlem başarılı
                    TempData["passwordRes"] = true;
                    return RedirectToAction("UserProfile", "Account");
                }  
            }
            TempData["passwordRes"] = false;

            return RedirectToAction("UserProfile", "Account");
        }

        public ActionResult DeleteMyAccount()
        {
            var userId = User.Identity.GetUserId();
            bool res = _userManager.DeleteAccount(userId);

            if(res)
            {
                var authManager = HttpContext.GetOwinContext().Authentication;
                authManager.SignOut();

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "İşem Başarılı",
                    RedirectingUrl = Url.Action("Index", "Home")
                };
                notifyObj.Items.Add("Hesabınız silindi.");

                return View("Ok", notifyObj);
            }
            else
            {
                TempData["deleteRes"] = false;

                return RedirectToAction("UserProfile", "Account");
            }       
        }

        public ActionResult UserAddress()
        {
            var userId = User.Identity.GetUserId();
            List<Address> addresses = new List<Address>();

            addresses = _addressManager.ListQueryable().Where(x => x.ApplicationUserId == userId).ToList();

            return View(addresses);
        }

        public ActionResult UserOrders()
        {
            var userId = User.Identity.GetUserId();

            var orderList = _orderManager.ListQueryable().Where(x => x.ApplicationUserId == userId).ToList();

            var model = orderList.Select(x => new UserOrdersViewModel()
            {
                OrderDate = x.OrderDate,
                OrderNumber = x.OrderNumber,
                OrderState = x.OrderState.State,
                Total = x.Total,
                Id=x.Id
            }).OrderByDescending(y => y.OrderDate).ToList(); 

            return View(model);
        }

        public ActionResult OrderDetails(Guid? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            

            var order = _orderManager.Find(x => x.Id == Id && x.ApplicationUserId == userId);

            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cityName = CacheHelper.GetCitiesFromCache().Find(x => x.Id == order.CityId).Name;
            OrderDetailsViewModel model = new OrderDetailsViewModel()
            {
                OrderNumber=order.OrderNumber,
                OrderState=order.OrderState.State,
                Total=order.Total,
                OrderDate=order.OrderDate,
                AddressLine=order.AddressLine,
                PostCode=order.PostCode,
                MobilePhone=order.MobilePhone,
                City= cityName,
                OrderLines=order.OrderLines
            };


            return View(model);
        }


        public ActionResult CreateAddress()
        {

            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAddress(CreateAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                Address address = new Address()
                {
                    Title = model.Title,
                    AddressLine = model.AddressLine,
                    MobilePhone = model.MobilePhone,
                    PostCode = model.PostCode,
                    CityId = model.CityId,
                    ApplicationUserId = User.Identity.GetUserId()
                };
                BusinessLayerResult<Address> res = _addressManager.Insert(address);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                else
                {
                    return RedirectToAction("UserAddress", "Account");
                }

            }
            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name");
            return View(model);
        }


        public ActionResult DeleteAddress(Guid id)
        {
            var userId = User.Identity.GetUserId();
            var address = _addressManager.Find(x => x.Id == id && x.ApplicationUserId == userId);

            if (address == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int res = _addressManager.Delete(address);

            return RedirectToAction("UserAddress", "Account");
        }


        [AllowAnonymous]
        public ActionResult Recoverpw()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Recoverpw(string email)
        {
            if(String.IsNullOrEmpty(email))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BusinessLayerResult<ApplicationUser> res = _userManager.ForgatPassword(email);
            if (res.Errors.Count > 0)
            {
                res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                return View(email);
            }

            OkViewModel notifyObj = new OkViewModel()
            {
                Title = "İşlem Başarılı",
                RedirectingUrl = Url.Action("Login", "Account")
            };

            notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz link'e tıklayarak şifrenizi değiştirebilirsiniz.");


            return View("Ok", notifyObj);
        }
        

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmed(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ResetPassword res = _resetPasswordManager.Find(x => x.ResetGuid == id);
            if(res==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Session.Remove("ResId");
            Session["ResId"] = res.Id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmed(string newPassword)
        {
            Guid resId = Guid.Parse(Session["ResId"].ToString());

            if (resId == null || String.IsNullOrEmpty(newPassword))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ResetPassword resPass = _resetPasswordManager.Find(x => x.Id == resId);
            var userId = resPass.ApplicationUserId;

            bool res = _userManager.ChangePassword(userId, newPassword);
            if(res)
            {
                resPass.IsUsed = true;
                int res2 = _resetPasswordManager.Update(resPass);

                // işlem başarılı
                Session.Remove("ResId");
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "İşlem Başarılı",
                    RedirectingUrl = Url.Action("Login", "Account")
                };
                notifyObj.Items.Add("Şifreniz değiştirildi. Giriş sayfasına yönlendirliyorsunuz.");

                return View("Ok", notifyObj);
            }
            else
            {
                ModelState.AddModelError("", "İşlem başarısız");
            }
            
            return View();
        }


    }
}