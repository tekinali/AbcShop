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
    [Table("ContactMessages")]
    public class ContactMessage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Konu"), StringLength(250)]
        public string Subject { get; set; }

        [DisplayName("Ad"), Required, StringLength(30)]
        public string Name { get; set; }

        [DisplayName("Soyad"), Required, StringLength(30)]
        public string Surname { get; set; }

        [DisplayName("Mesaj"), Required, StringLength(250)]
        public string Text { get; set; }

        [DisplayName("E-posta"), Required, DataType(DataType.EmailAddress), StringLength(70)]
        public string Email { get; set; }

        public bool IsRead { get; set; }
      
        public DateTime DateTime { get; set; }


    }
}
