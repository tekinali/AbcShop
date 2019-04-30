using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AbcShop.WebApp.ViewModels.Account
{
    public class UserProfileEditViewModel
    {
        [DisplayName("Ad")]
        public string Name { get; set; }

        [DisplayName("Soyad")]
        public string Surname { get; set; }    

        [DisplayName("Eposta")]
        public string Email { get; set; }

        [DisplayName("Şehir")]
        public int CityId { get; set; }
    }
}