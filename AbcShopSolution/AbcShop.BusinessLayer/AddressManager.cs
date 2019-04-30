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
    public class AddressManager : ManagerBase<Address>
    {
        public new BusinessLayerResult<Address> Insert(Address data)
        {
            data.Title= CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Title).TrimStart().TrimEnd();

            Address address = Find(x => x.Title == data.Title && x.ApplicationUserId == data.ApplicationUserId);

            BusinessLayerResult<Address> res = new BusinessLayerResult<Address>();

            if(address!=null)
            {
                // kullanıcının aynı başlığa sahip adresi var
                res.AddError(ErrorMessageCode.AddressAlreadyExists, "Başarısız! Aynı başlığa sahip adres kullanıcıda mevcut!");
                return res;
            }

            data.AddressLine = data.AddressLine.Trim();
            data.PostCode = data.PostCode.Trim();
            data.MobilePhone = data.MobilePhone.Trim();

            int dbRes = base.Insert(data);

            if (dbRes > 0)
            {
                // kayıt başarılı
            }
            else
            {
                // kayıt başarısız
                res.AddError(ErrorMessageCode.AddressCouldNotInserted, "Başarısız! Adres eklenmedi");
            }


            return res;
        }


    }
}
