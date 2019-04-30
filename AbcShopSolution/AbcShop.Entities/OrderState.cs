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
    [Table("OrderStates")]
    public class OrderState
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Durum"), Required, StringLength(30)]
        public string State { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }
      
        public string Color { get; set; }

        /**/

        public virtual List<Order> Orders { get; set; }       


        public OrderState()
        {
            Orders = new List<Order>();
        }

    }
}
