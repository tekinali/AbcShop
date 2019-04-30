using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbcShop.WebApp.ViewModels.Cart
{
    public class CheckoutViewModel
    {
        
        public Guid AddressId { get; set; }

        [Required(ErrorMessage = "Lütfen kart sahibinin ad soyad bilgisini giriniz")]
        public string ccName { get; set; }

        [Required(ErrorMessage = "Lütfen kart numarasını giriniz")]
        public string ccNumber { get; set; }

        [Required(ErrorMessage = "Lütfen kartın arkasındaki cvc numarasını giriniz.")]
        public string ccCvc { get; set; }

        public double Total { get; set; }


    }
}