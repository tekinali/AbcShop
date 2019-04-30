using AbcShop.BusinessLayer.Abstract;
using AbcShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbcShop.BusinessLayer.Result;
using System.Globalization;
using AbcShop.Entities.Messages;
using System.Web;
using Microsoft.Security.Application;

namespace AbcShop.BusinessLayer
{
    public class ProductManager : ManagerBase<Product>
    {
        public ProductManager()
        {

        }

        public new BusinessLayerResult<Product> Insert(Product data)
        {
            data.Name= CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Name).TrimStart().TrimEnd();
            data.ProductCode= CultureInfo.CurrentCulture.TextInfo.ToUpper(data.ProductCode).TrimStart().TrimEnd();

            var prorducts= ListQueryable().Where(x => x.Name == data.Name || x.ProductCode == data.ProductCode);

            BusinessLayerResult<Product> res = new BusinessLayerResult<Product>();

            if(prorducts.Count() > 0)
            {
                foreach (var item in prorducts)
                {
                    if(item.Name==data.Name)
                    {
                        res.AddError(ErrorMessageCode.ProductAlreadyExists, "Ürün Adı kayıtlı.");
                    }

                    if (item.Name == data.Name)
                    {
                        res.AddError(ErrorMessageCode.ProductAlreadyExists, "Ürün Kodu kayıtlı.");
                    }

                }
                return res;
            }

            data.Features = Sanitizer.GetSafeHtmlFragment(HttpUtility.HtmlEncode(data.Features));
            data.Description = Sanitizer.GetSafeHtmlFragment(HttpUtility.HtmlEncode(data.Description));

            if(base.Insert(data)==0)
            {
                res.AddError(ErrorMessageCode.ProductCouldNotInserted, "Ürün eklenemdi..");
            }

            return res;
        }

        public new BusinessLayerResult<Product> Update(Product data)
        {
            data.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Name).TrimStart().TrimEnd();
            data.ProductCode = CultureInfo.CurrentCulture.TextInfo.ToUpper(data.ProductCode).TrimStart().TrimEnd();

            var products = ListQueryable().Where(x => x.Name == data.Name || x.ProductCode == data.ProductCode);

            BusinessLayerResult<Product> res = new BusinessLayerResult<Product>();

            if(products.Count() > 0)
            {
                foreach (var item in products)
                {
                    if(item.Id!=data.Id)
                    {
                        if (item.Name == data.Name)
                        {
                            res.AddError(ErrorMessageCode.ProductAlreadyExists, "Ürün Adı kayıtlı.");
                        }

                        if (item.Name == data.Name)
                        {
                            res.AddError(ErrorMessageCode.ProductAlreadyExists, "Ürün Kodu kayıtlı.");
                        }
                    }
                }
                return res;
            }

            data.Features = Sanitizer.GetSafeHtmlFragment(HttpUtility.HtmlEncode(data.Features));
            data.Description = Sanitizer.GetSafeHtmlFragment(HttpUtility.HtmlEncode(data.Description));

            if(base.Update(data)==0)
            {
                res.AddError(ErrorMessageCode.ProductCouldNotUpdated, "Ürün güncellenemedi.");
            }


            return res;
        }


    }
}
