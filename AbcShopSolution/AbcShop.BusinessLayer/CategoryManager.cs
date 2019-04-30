using AbcShop.DataAccessLayer.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbcShop.Entities;
using AbcShop.Entities.Messages;
using AbcShop.BusinessLayer.Result;
using AbcShop.BusinessLayer.Abstract;
using System.Globalization;

namespace AbcShop.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        public CategoryManager()
        {

        }

        public new BusinessLayerResult<Category> Insert(Category data)
        {
            data.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Name).TrimStart().TrimEnd();
            if (!String.IsNullOrEmpty(data.Description))
            {
                data.Description = data.Description.TrimStart().TrimEnd();
            }
            Category cat = Find(x => x.Name == data.Name);

            BusinessLayerResult<Category> res = new BusinessLayerResult<Category>();

            if (cat != null) //böyle kategori var
            {
                res.AddError(ErrorMessageCode.CategoryAlreadyExists, "Başarısız! Aynı ada sahip Kategori mevcut!");
            }
            else
            {
                DateTime now = DateTime.Now;
                int dbResult = base.Insert(new Category()
                {
                    Name = data.Name,
                    Description = data.Description
                });

                if (dbResult > 0)
                {
                    // kayıt başarılı

                }
                else
                {
                    // kayıt başarısız
                    res.AddError(ErrorMessageCode.CategoryCouldNotInserted, "Başarısız! Kategori eklenmedi");
                }

            }
            return res;
        }

        public new BusinessLayerResult<Category> Update(Category data)
        {
            data.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Name).TrimStart().TrimEnd();
            if (!String.IsNullOrEmpty(data.Description))
            {
                data.Description = data.Description.TrimStart().TrimEnd();
            }

            Category db_cat = Find(x => x.Name == data.Name);
            BusinessLayerResult<Category> res = new BusinessLayerResult<Category>();

            res.Result = data;

            if (db_cat != null && db_cat.Id != data.Id)
            {
                res.AddError(ErrorMessageCode.CategoryAlreadyExists, "Kategori adı kayıtlı.");

                return res;
            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Name = data.Name;
            res.Result.Description = data.Description;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.CategoryCouldNotUpdated, "Kategori güncellenemedi.");
            }

            return res;

        }

        public new BusinessLayerResult<Category> Delete(Category data)
        {
            BusinessLayerResult<Category> res = new BusinessLayerResult<Category>();
            res.Result = Find(x => x.Id == data.Id);

            var products = res.Result.Products;
            if(products.Count>0)
            {
                res.AddError(ErrorMessageCode.CategoryCouldNotRemove, "Kategori silinemedi. Kategorinin içersinde ürün bulunmaktadır. İşleme devam etmek için kategorinin içersininde ürün bulunmaması gerekmektedir.");
                return res;
            }

            if(base.Delete(res.Result)==0)
            {
                res.AddError(ErrorMessageCode.CategoryCouldNotRemove, "Kategori silinemedi.");
            }            

            return res;
        }


    }
}
