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
    [Table("Cities")]
    public class City :EntityBase<int>
    {
        [DisplayName("Yaşadığınız Şehir"), Required, StringLength(50)]
        public string Name { get; set; }


        /**/
        public virtual List<Address> Addresses { get; set; }
        public virtual List<ApplicationUser> ApplicationUsers { get; set; }
        public virtual List<Order> Orders { get; set; }

        public City()
        {
            Addresses = new List<Address>();
            ApplicationUsers = new List<ApplicationUser>();
            Orders = new List<Order>();
        }



    }


}
