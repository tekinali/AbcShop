using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcShop.WebApp.Models.Notification
{
    public class OkViewModel : NotifiyViewModelBase<string>
    {
        public OkViewModel()
        {
            Title = "İşlem Başarılı";
        }
    }
}