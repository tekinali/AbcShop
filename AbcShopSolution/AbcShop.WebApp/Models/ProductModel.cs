using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbcShop.WebApp.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }

        [DisplayName("Ürün Adı"), Required]
        public string Name { get; set; }

        [DisplayName("Ürün Kodu"), Required]
        public string ProductCode { get; set; }

        [DisplayName("Ürün Bşlık")]
        public string Title { get; set; }        

        public double Price { get; set; }

        public int Stock { get; set; }

        public string MainImage { get; set; }

        public int CategoryId { get; set; }

    }
}