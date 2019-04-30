using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcShop.Entities
{
    [Table("ResetPasswords")]
    public class ResetPassword : EntityBase<Guid>
    {
        public bool IsUsed { get; set; }

        [Required, ScaffoldColumn(false)]
        public Guid ResetGuid { get; set; }

        public string ApplicationUserId { get; set; }
        
        /**/

        public virtual ApplicationUser ApplicationUser { get; set; }


    }
}
