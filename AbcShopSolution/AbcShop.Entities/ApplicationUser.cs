using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcShop.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }        

        [DisplayName("Delete")]
        public bool IsDeleted { get; set; }

        public int CityId { get; set; }

        public virtual City City { get; set; }

        /**/

        public virtual List<Address> Addresses { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual List<ResetPassword> ResetPasswords { get; set; }

        public ApplicationUser()
        {
            Addresses = new List<Address>();
            Orders = new List<Order>();
            ResetPasswords = new List<ResetPassword>();
        }


    }
}
