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
    public class ProductController : Controller
    {
        private ProductManager _productManager;
        private ProductImageManager _productImageManager;

        public ProductController()
        {
            _productManager = new ProductManager();
            _productImageManager = new ProductImageManager();
        }

        // GET: Admin/Product
        public ActionResult Index()
        {
            var model = CacheHelper.GetHomeProductsFromCache();
            return View(model);
        }

        public ActionResult Details(Guid? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var product = _productManager.Find(x => x.Id == id);

            if (product == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            product.Features = HttpUtility.HtmlDecode(product.Features);
            product.Description = HttpUtility.HtmlDecode(product.Description);

            return View(product);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var product = _productManager.Find(x => x.Id == id);

            if (product == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            product.Features = HttpUtility.HtmlDecode(product.Features);
            product.Description = HttpUtility.HtmlDecode(product.Description);

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Name", product.CategoryId);

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Product model)
        {
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            ModelState.Remove("CreatedOn");
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Product> res = _productManager.Update(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                else
                {
                    CacheHelper.RemoveGetCategoriesFromCache();
                    CacheHelper.RemoveGetHomeProductsFromCache();
                    CacheHelper.RemoveGetProductsFromCache();

                    return RedirectToAction("Index", "Product");
                }
            }


            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Name", model.CategoryId);
            return View(model);
        }


        public ActionResult EditImage(Guid? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var product = _productManager.Find(x => x.Id == id);

            if (product == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImage(Guid? id, HttpPostedFileBase Image)
        {
            var productId = id;

            if(ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 2178369)
                {
                    ModelState.AddModelError("Hata", "Dosya boyutu çok fazla!");            
                    return RedirectToAction("EditImage", "Product", new { @id = productId });                }

                if (Image != null && (
                  Image.ContentType == "image/jpeg" ||
                  Image.ContentType == "image/jpg" ||
                  Image.ContentType == "image/png"))
                {
                    ProductImage model = new ProductImage();
                    model.ProductId = (Guid)id;

                    string filename = $"{FakeData.TextData.GetNumeric(12)}.{Image.ContentType.Split('/')[1]}";

                    Image.SaveAs(Server.MapPath($"~/Upload/Images/Product/{filename}"));
                    model.FileName = filename;

                    int res = _productImageManager.Insert(model);

                    CacheHelper.RemoveGetHomeProductsFromCache();
                    CacheHelper.RemoveGetProductsFromCache();
                }

            }
            return RedirectToAction("EditImage", "Product", new { @id= productId});
        }

        public ActionResult RemoveImage(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductImage image = _productImageManager.Find(x => x.Id == id);
            var productId = image.ProductId;

            if(image==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if(image.FileName!= "default.jpg")
            {
                if(System.IO.File.Exists(Server.MapPath("~/Upload/Images/Product/" +image.FileName)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Upload/Images/Product/" + image.FileName));
                }
            }
            int res = _productImageManager.Delete(image);
            CacheHelper.RemoveGetHomeProductsFromCache();
            CacheHelper.RemoveGetProductsFromCache();

            return RedirectToAction("EditImage", "Product", new { @id = productId });
        }

        public ActionResult RemoveMainImage(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = _productManager.Find(x => x.Id == id);
            var productId = id;

            if(product==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (product.MainImage != "default.jpg")
            {
                if (System.IO.File.Exists(Server.MapPath("~/Upload/Images/Product/" + product.MainImage)))
                {
                    System.IO.File.Delete(Server.MapPath("~/Upload/Images/Product/" + product.MainImage));
                }
            }

            product.MainImage = "default.jpg";

            BusinessLayerResult<Product> res = _productManager.Update(product);
            CacheHelper.RemoveGetHomeProductsFromCache();
            CacheHelper.RemoveGetProductsFromCache();

            return RedirectToAction("EditImage", "Product", new { @id = productId });
        }

        public ActionResult MakeMainImage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductImage image = _productImageManager.Find(x => x.Id == id);
            var productId = image.ProductId;

            if (image == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = _productManager.Find(x => x.Id == image.ProductId);

            product.MainImage = image.FileName;
            BusinessLayerResult<Product> res = _productManager.Update(product);


            int res2 = _productImageManager.Delete(image);
            CacheHelper.RemoveGetHomeProductsFromCache();
            CacheHelper.RemoveGetProductsFromCache();

            return RedirectToAction("EditImage", "Product", new { @id = product.Id });
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Product model, HttpPostedFileBase MainImage)
        {
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            ModelState.Remove("CreatedOn");
            if (ModelState.IsValid)
            {
                model.MainImage = "default.jpg";
                
                if (MainImage != null && MainImage.ContentLength > 2178369)
                {
                    ModelState.AddModelError("Hata", "Dosya boyutu çok fazla!");
                    ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Name", model.CategoryId);
                    return View(model);
                }

                if (MainImage != null && (
                  MainImage.ContentType == "image/jpeg" ||
                  MainImage.ContentType == "image/jpg" ||
                  MainImage.ContentType == "image/png"))
                {
                    string filename = $"{FakeData.TextData.GetNumeric(12)}.{MainImage.ContentType.Split('/')[1]}";

                    MainImage.SaveAs(Server.MapPath($"~/Upload/Images/Product/{filename}"));
                    model.MainImage = filename;
                }


                BusinessLayerResult<Product> res = _productManager.Insert(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                else
                {
                    CacheHelper.RemoveGetCategoriesFromCache();
                    CacheHelper.RemoveGetHomeProductsFromCache();
                    CacheHelper.RemoveGetProductsFromCache();

                    return RedirectToAction("Index", "Product");
                }

            }


            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Name", model.CategoryId);
            return View();
        }


    }
}