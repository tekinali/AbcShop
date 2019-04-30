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
    [Table("Addresses")]
    public class Address:EntityBase<Guid>
    {
        [DisplayName("Başlık"), Required]
        public string Title { get; set; }     
        
        [DisplayName("Adres"), Required]
        public string AddressLine { get; set; }

        [DisplayName("Posta Kodu"), Required]
        public string PostCode { get; set; }

        [DisplayName("Cep Telefonu"), Required]
        public string MobilePhone { get; set; }

        [DisplayName("Şehir")]
        public int CityId { get; set; }

        [DisplayName("Kullanıcı")]
        public string ApplicationUserId { get; set; }
        /**/

        public virtual City City { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }



    }
}
