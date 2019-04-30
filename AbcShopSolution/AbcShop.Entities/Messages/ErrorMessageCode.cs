using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcShop.Entities.Messages
{
    public enum ErrorMessageCode
    {
        UsernameAlreadyExists = 101,
        EmailAlreadyExists = 102,
        UserIsNotActive = 103,
        UsernameOrPassWrong = 104,
        CheckYourEmail = 105,
        UserAlreadyActive = 106,
        ActivateIdDoesNotExists = 107,
        UserNotFound = 108,
        ProfileCouldNotUpdated = 109,
        UserCouldNotRemove = 110,
        UserCouldNotFind = 111,
        UserCouldNotInserted = 112,
        UserCouldNotUpdated = 113,
        UserIsDelete = 114,

        ProductAlreadyExists = 301,
        ProductNotFound = 302,
        ProductCouldNotUpdated = 303,
        ProductCouldNotInserted = 304,
        ProductCouldNotRemove = 305,
        ProductIsDelete = 436,

        CategoryAlreadyExists = 401,
        CategoryNotFound = 402,
        CategoryCouldNotUpdated = 403,
        CategoryCouldNotInserted = 404,
        CategoryCouldNotRemove = 405,
        CategoryIsDelete = 406,

        OrderAlreadyExists = 501,
        OrderNotFound = 502,
        OrderCouldNotUpdated = 503,
        OrderCouldNotInserted = 504,
        OrderCouldNotRemove = 505,
        OrderIsDelete = 506,

        AddressAlreadyExists = 701,
        AddressNotFound = 702,
        AddressCouldNotUpdated = 703,
        AddressCouldNotInserted = 704,
        AddressCouldNotRemove = 705,
        AddressIsDelete = 706
    }
}
