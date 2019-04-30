using AbcShop.BusinessLayer.Abstract;
using AbcShop.BusinessLayer.Result;
using AbcShop.Entities;
using AbcShop.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcShop.BusinessLayer
{
    public class OrderManager : ManagerBase<Order>
    {
        private OrderStateManager _orderStateManager;
        private OrderLineManager _orderLineManager;

        public OrderManager()
        {
            _orderStateManager = new OrderStateManager();
            _orderLineManager = new OrderLineManager();
        }

        public new BusinessLayerResult<Order> Insert(Order data)
        {
            var stateId = _orderStateManager.Find(x => x.State == "Onay Bekliyor").Id;
            char key = 'C';


            BusinessLayerResult<Order> res = new BusinessLayerResult<Order>();

            var total = data.Total;
            if (total > 1000 && total < 4999)
            {
                key = 'D';
            }
            if (total > 4999)
            {
                key = 'F';
            }


            var order = new Order()
            {
                OrderNumber = CultureInfo.CurrentCulture.TextInfo.ToUpper(key + "0_" + FakeData.TextData.GetAlphabetical(3) + FakeData.NumberData.GetNumber(11111, 99999) + FakeData.TextData.GetAlphabetical(1)),
                OrderStateId = stateId,
                Total = data.Total,
                OrderDate=data.OrderDate,
                AddressLine=data.AddressLine,
                PostCode=data.PostCode,
                MobilePhone=data.MobilePhone,
                CityId=data.CityId,
                ApplicationUserId=data.ApplicationUserId,    
            };

            if (base.Insert(order) == 0)
            {
                res.AddError(ErrorMessageCode.OrderCouldNotInserted, "Başarısız! Sipariş oluşturulamdı.");
            }

            res.Result = Find(x => x.OrderNumber == order.OrderNumber);

            foreach (var item in data.OrderLines)
            {
                OrderLine ol = new OrderLine()
                {
                    OrderId=res.Result.Id,
                    ProductId=item.ProductId,
                    Price=item.Price,
                    Quantity=item.Quantity                    
                };
                int res2 = _orderLineManager.Insert(ol);
                if(res2==0)
                {
                    res.AddError(ErrorMessageCode.OrderCouldNotInserted, "Başarısız! Sipariş oluşturulamdı.");
                }
            }

            return res;
        }



    }
}
