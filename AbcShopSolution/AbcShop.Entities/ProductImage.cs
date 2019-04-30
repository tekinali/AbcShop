using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcShop.Entities
{
    [Table("ProductImages")]
    public class ProductImage : EntityBase<int>
    {
        [DisplayName("Resim Adı")]
        public string FileName { get; set; }

        [DisplayName("Ürün")]
        public Guid ProductId { get; set; }

        /**/

        public virtual Product Product { get; set; }

    }
}
