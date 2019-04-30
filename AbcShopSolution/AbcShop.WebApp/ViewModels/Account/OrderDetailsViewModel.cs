using AbcShop.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbcShop.WebApp.ViewModels.Account
{
    public class OrderDetailsViewModel
    {
        [DisplayName("Sipariş Numarası")]
        public string OrderNumber { get; set; }

        [DisplayName("Sipariş Durumu")]
        public string OrderState { get; set; }

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

        [DisplayName("Şehir"), Required]
        public string City { get; set; }

        public List<OrderLine> OrderLines { get; set; }

        public OrderDetailsViewModel()
        {
            OrderLines = new List<OrderLine>();
        }


    }
}