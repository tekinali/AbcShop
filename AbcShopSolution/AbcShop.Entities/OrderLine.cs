using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcShop.Entities
{
    [Table("OrderLines")]
    public class OrderLine : EntityBase<Guid>
    {
        [DisplayName("Adet")]
        public int Quantity { get; set; }

        [DisplayName("Birim Fiyat")]
        public double Price { get; set; }

        [DisplayName("Ürün")]
        public Guid ProductId { get; set; }

        [DisplayName("Sipariş")]
        public Guid OrderId { get; set; }

        /**/

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
