using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcShop.Entities;
using AbcShop.BusinessLayer;
using System.Web.Helpers;

namespace AbcShop.WebApp.Helpers
{
    public class CacheHelper
    {

        public static List<City> GetCitiesFromCache()
        {
            var result = WebCache.Get("cities-cache");

            if (result == null)
            {
                CityManager _cityManager = new CityManager();
                result = _cityManager.ListQueryable().OrderBy(x => x.Name).ToList();

                WebCache.Set("cities-cache", result, 20, true);
            }

            return result;
        }

        public static List<Category> GetCategoriesFromCache()
        {
            var result = WebCache.Get("categories-cache");

            if (result == null)
            {
                CategoryManager _categoryManager = new CategoryManager();
                result = _categoryManager.ListQueryable().OrderBy(x => x.Name).ToList();

                WebCache.Set("categories-cache", result, 20, true);
            }

            return result;
        }

        public static List<Product> GetProductsFromCache()
        {
            var result = WebCache.Get("products-cache");

            if (result == null)
            {
                ProductManager _productManager = new ProductManager();
                result = _productManager.ListQueryable().OrderBy(y => y.Name).ToList();

                WebCache.Set("products-cache", result, 20, true);
            }

            return result;
        }

        public static List<Product> GetHomeProductsFromCache()
        {
            var result = WebCache.Get("homeproducts-cache");

            if (result == null)
            {
                ProductManager _productManager = new ProductManager();
                result = _productManager.ListQueryable().Where(x => x.IsApproved == true && x.IsHome==true).OrderBy(y => y.Name).ToList();

                WebCache.Set("homeproducts-cache", result, 20, true);
            }

            return result;
        }

        //////////////////////////////////////////////////////////////

        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }

        public static void RemoveGetCitiesFromCache()
        {
            Remove("cities-cache");
        }

        public static void RemoveGetCategoriesFromCache()
        {
            Remove("categories-cache");
        }

        public static void RemoveGetProductsFromCache()
        {
            Remove("products-cache");
        }
        public static void RemoveGetHomeProductsFromCache()
        {
            Remove("homeproducts-cache");
        }

    }
}