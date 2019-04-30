using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcShop.Entities.ValueObjects
{
    public class RegisterViewModel
    {

        [DisplayName("Adınız"), Required(ErrorMessage = "{0} alanı boş geçilemez"),
           StringLength(30, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Name { get; set; }

        [DisplayName("Soyadınız"), Required(ErrorMessage = "{0} alanı boş geçilemez"),
           StringLength(30, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adınız"), Required(ErrorMessage = "{0} alanı boş geçilemez"),
            StringLength(20, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Username { get; set; }

        [DisplayName("E-posta adresiniz"), Required, StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("E-posta adresiniz (tekrar)"), Required, StringLength(50),
            Compare(nameof(Email), ErrorMessage = "{0} ile {1} uyuşmuyor.")]
        [EmailAddress]
        public string ReEmail { get; set; }

        [DisplayName("Şifreniz"), Required(ErrorMessage = "{0} alanı boş geçilemez"), DataType(DataType.Password),
            StringLength(16, MinimumLength = 6, ErrorMessage = "{0} min. {2} - max. {1} karakter olmalı.")]
        public string Password { get; set; }

        [DisplayName("Şifreniz (Tekrar)"), Required(ErrorMessage = "{0} alanı boş geçilemez"), DataType(DataType.Password),
            StringLength(16, MinimumLength = 6, ErrorMessage = "{0} min. {2} - max. {1} karakter olmalı."), Compare(nameof(Password), ErrorMessage = "{0} ile {1} uyuşmuyor.")]
        public string RePassword { get; set; }

        [DisplayName("Şehir"), Required]
        public int CityId { get; set; }


    }
}
