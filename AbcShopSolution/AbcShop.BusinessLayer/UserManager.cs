using AbcShop.DataAccessLayer.Abstract;
using AbcShop.DataAccessLayer.EntityFramework;
using AbcShop.Entities;
using AbcShop.Entities.ValueObjects;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.AspNet.Identity.Owin;
using AbcShop.BusinessLayer.Result;
using System.Globalization;
using AbcShop.Entities.Messages;
using AbcShop.BusinessLayer.Helpers;

namespace AbcShop.BusinessLayer
{
    public class UserManager 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly ResetPasswordManager _resetPasswordManager;

        private DatabaseContext db;

        public UserManager()
        {

            _resetPasswordManager = new ResetPasswordManager();


            db = new DatabaseContext();

            var roleStore = new RoleStore<ApplicationRole>(db);
            _roleManager = new RoleManager<ApplicationRole>(roleStore);

            var userStore = new UserStore<ApplicationUser>(db);
            _userManager = new UserManager<ApplicationUser>(userStore);

            _userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager)
            {
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = true
            };
        }

        public BusinessLayerResult<ApplicationUser> RegisterUser(RegisterViewModel data)
        {
            BusinessLayerResult<ApplicationUser> res = new BusinessLayerResult<ApplicationUser>();

            var user = new ApplicationUser();

            user.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Name).TrimStart().TrimEnd();
            user.Surname = CultureInfo.CurrentCulture.TextInfo.ToUpper(data.Surname).TrimStart().TrimEnd();
            user.UserName = CultureInfo.CurrentCulture.TextInfo.ToLower(data.Username).Trim().Replace(" ", string.Empty);
            user.Email= CultureInfo.CurrentCulture.TextInfo.ToLower(data.Email).Trim().Replace(" ", string.Empty);      
            user.IsDeleted = false;       
            user.CityId = data.CityId;

            var result = _userManager.Create(user, data.Password);

            if(result.Succeeded)
            {
                _userManager.AddToRole(user.Id, "User");

                res.Result = _userManager.Users.FirstOrDefault(x => x.UserName == user.UserName);
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, item);
                }
            }


            return res;
        }

        public BusinessLayerResult<ApplicationUser> Login(LoginViewModel data)
        {
            BusinessLayerResult<ApplicationUser> res = new BusinessLayerResult<ApplicationUser>();

            var user = _userManager.Find(data.Username, data.Password);

            if(user==null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Adı ve/veya Şifre yanlış");
                return res;
            }

            if(user.IsDeleted==true)
            {
                res.AddError(ErrorMessageCode.UserIsDelete, "Kullanıcı yakın zamanda silinmiş.");
                return res;
            }
                   

            if(_userManager.IsInRole(user.Id,"User"))
            {
                var authManager = HttpContext.Current.GetOwinContext().Authentication;

                var identityclaims = _userManager.CreateIdentity(user, "ApplicationCookie");
                var authProperties = new AuthenticationProperties();
                authProperties.IsPersistent = data.RememberMe;
                authManager.SignOut();
                authManager.SignIn(authProperties, identityclaims);

                res.Result = user;
            }
            else
            {
                res.AddError(ErrorMessageCode.UserIsDelete, "Kullanıcının sistemde rolü yok.");
            }
            

            return res;
        }

        public BusinessLayerResult<ApplicationUser> LoginAdmin(LoginViewModel data)
        {
            BusinessLayerResult<ApplicationUser> res = new BusinessLayerResult<ApplicationUser>();

            var user = _userManager.Find(data.Username, data.Password);

            if (user == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Adı ve/veya Şifre yanlış");
                return res;
            }

            if (user.IsDeleted == true)
            {
                res.AddError(ErrorMessageCode.UserIsDelete, "Kullanıcı yakın zamanda silinmiş.");
                return res;
            }


            if (_userManager.IsInRole(user.Id, "Admin"))
            {
                var authManager = HttpContext.Current.GetOwinContext().Authentication;

                var identityclaims = _userManager.CreateIdentity(user, "ApplicationCookie");
                var authProperties = new AuthenticationProperties();
                authProperties.IsPersistent = data.RememberMe;
                authManager.SignOut();
                authManager.SignIn(authProperties, identityclaims);

                res.Result = user;
            }
            else
            {
                res.AddError(ErrorMessageCode.UserIsDelete, "Kullanıcının sistemde rolü yok.");
            }


            return res;
        }


        public ApplicationUser Find(string id)
        {
            return _userManager.FindById(id);           
        }     

        public List<ApplicationUser> ListUser()
        {
            var allUsers = _userManager.Users.ToList();
            var db_user = new List<ApplicationUser>();

            foreach (var item in allUsers)
            {
                if (_userManager.IsInRole(item.Id, "User"))
                {
                    if(_userManager.IsInRole(item.Id, "Admin")==false)
                    {
                        db_user.Add(item);
                    }                
                }
            }

            return db_user;
        }

        public BusinessLayerResult<ApplicationUser> Update (ApplicationUser data)
        {
            var result = _userManager.Update(data);
            BusinessLayerResult<ApplicationUser> res = new BusinessLayerResult<ApplicationUser>();

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotUpdated, item);
                }
                res.AddError(ErrorMessageCode.UserCouldNotUpdated,"Kullanıcı güncellenemedi.");
            }

            return res;          
        }

        public bool ChangePassword(string userId,string newPassword)
        {
            var user = _userManager.FindById(userId);
            if(user!=null)
            {
                _userManager.RemovePassword(user.Id);
                _userManager.AddPassword(user.Id, newPassword);

                return true;
            }

            
            return false;
        }

        public bool DeleteAccount(string userId)
        {
            var user = _userManager.FindById(userId);
            if (user != null)
            {
                user.IsDeleted = true;
                var result = _userManager.Update(user);

                if (result.Succeeded)
                {
                    return true;
                }              
            }

            return false;
        }

        public bool ChangeStatus(string userId)
        {
            var user = _userManager.FindById(userId);
            if (user != null)
            {
                user.IsDeleted = !user.IsDeleted;
                var result = _userManager.Update(user);

                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        public BusinessLayerResult<ApplicationUser> ForgatPassword(string email)
        {
            email= CultureInfo.CurrentCulture.TextInfo.ToLower(email).Trim().Replace(" ", string.Empty);
            BusinessLayerResult<ApplicationUser> res = new BusinessLayerResult<ApplicationUser>();

            res.Result = _userManager.FindByEmail(email);

            if(res.Result==null)
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kayıtlı e-posta bulunamadı.");
                return res;
            }

            ResetPassword db_resetPassword = _resetPasswordManager.Find(x => x.ApplicationUserId == res.Result.Id && x.IsUsed == false);
            if(db_resetPassword!=null)
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Lütfen eposta adresinizi kontrol ediniz. Böyle bir istekte daha önceden bulunulmuş.");
                return res;
            }

            ResetPassword rp = new ResetPassword()
            {
                IsUsed=false,
                ApplicationUserId=res.Result.Id,
                ResetGuid=Guid.NewGuid()
            };

            int dbResult = _resetPasswordManager.Insert(rp);

            if (dbResult > 0)
            {
                string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                string resetUrl = $"{siteUri}/Account/ResetPasswordConfirmed/{rp.ResetGuid}";
                string body = $"Merhaba {res.Result.UserName}; <br>Şifrenizi değiştirme için <a href='{resetUrl}' target='_blank'>tıklayınız.</a>";

                MailHelper.SendMail(body, res.Result.Email, "Şifre değiştirme");
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Hata Oluştu.");
            }


            return res;
        }


    }
}
