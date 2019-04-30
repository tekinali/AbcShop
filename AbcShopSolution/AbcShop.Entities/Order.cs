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
    [Table("Orders")]
    public class Order:EntityBase<Guid>
    {
        [DisplayName("Sipariş Numarası")]
        public string OrderNumber { get; set; }

        [DisplayName("Toplam Fiyat")]
        public double Total { get; set; }

        [DisplayName("Sipariş Tarihi")]
        public DateTime OrderDate { get; set; }       

        [DisplayName("Başlık"), Required]
        public string AddressLine { get; set; }

        [DisplayName("Posta Kodu"), Required]
        public string PostCode { get; set; }

        [DisplayName("Cep Telefonu"), Required]
        public string MobilePhone { get; set; }

        [DisplayName("Durum")]
        public int OrderStateId { get; set; }

        [DisplayName("Şehir")]
        public int CityId { get; set; }


        [DisplayName("Kullanıcı")]
        public string ApplicationUserId { get; set; }

        /**/

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual City City { get; set; }

        public virtual OrderState OrderState { get; set; }

        public virtual List<OrderLine> OrderLines { get; set; }


        public Order()
        {
            OrderLines = new List<OrderLine>();
        }

    }
}
