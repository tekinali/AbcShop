using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcShop.Entities
{
    [Table("Categories")]
    public class Category :EntityBase<int>
    {
        [DisplayName("Kategori Adı"), Required, StringLength(25)]
        public string Name { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }

        /**/
        public virtual List<Product> Products { get; set; }

        public Category()
        {
            Products = new List<Product>();
        }



    }
}
