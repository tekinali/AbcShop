using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AbcShop.Entities;

namespace AbcShop.WebApp.Areas.Admin.ViewModels.Home
{
    public class IndexViewModel
    {
        public int ProductCount { get; set; }
        public int UserCount { get; set; }
        public int OrderCount { get; set; }
        public double OrderTotal { get; set; }
        public List<Order> Orders { get; set; }

        public IndexViewModel()
        {
            Orders = new List<Order>();
        }
        
    }
}