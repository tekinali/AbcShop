using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbcShop.WebApp.ViewModels.Account
{
    public class CreateAddressViewModel
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
    }
}