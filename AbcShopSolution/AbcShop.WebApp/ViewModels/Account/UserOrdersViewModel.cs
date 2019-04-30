using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AbcShop.WebApp.ViewModels.Account
{
    public class UserOrdersViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Sipariş Numarası")]
        public string OrderNumber { get; set; }

        [DisplayName("Toplam Fiyat")]
        public double Total { get; set; }

        [DisplayName("Sipariş Tarihi")]
        public DateTime OrderDate { get; set; }

        [DisplayName("Durum")]
        public string OrderState { get; set; }

    }
}