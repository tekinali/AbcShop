using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AbcShop.Entities
{
    [Table("Products")]
    public class Product:EntityBase<Guid>
    {  
        [DisplayName("Ürün Adı"), Required]
        public string Name { get; set; }

        [DisplayName("Ürün Kodu"),Required]
        public string ProductCode { get; set; }

        [DisplayName("Ürün Başlık"), StringLength(80)]     
        public string Title { get; set; }

        [DisplayName("Ürün Açıklama"),StringLength(5000)]
        [AllowHtml]
        public string Description { get; set; }

        [DisplayName("Ürün Özellikleri"), StringLength(5000)]
        [AllowHtml]
        public string Features { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }

        public string MainImage { get; set; }

        public bool IsHome { get; set; }

        public bool IsApproved { get; set; }

        public int CategoryId { get; set; }

        /**/

        public virtual Category Category { get; set; }

        public virtual List<OrderLine> OrderLines { get; set; }
        public virtual List<ProductImage> ProductImages { get; set; }

        public Product()
        {
            OrderLines = new List<OrderLine>();
            ProductImages = new List<ProductImage>();
        }



    }
}
