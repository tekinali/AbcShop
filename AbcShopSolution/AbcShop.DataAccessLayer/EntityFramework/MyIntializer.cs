using AbcShop.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AbcShop.DataAccessLayer.EntityFramework
{
    public class MyIntializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            DateTime now = DateTime.Now;

            string[] turkeyCity ={"Adana","Adıyaman","Afyonkarahisar","Ağrı","Aksaray","Amasya","Ankara","Antalya","Ardahan","Artvin","Aydın","Balıkesir","Bartın","Batman","Bayburt","Bilecik","Bingöl","Bitlis",
            "Bolu","Burdur","Bursa","Çanakkale","Çankırı","Çorum","Denizli","Diyarbakır","Düzce","Edirne",
            "Elazığ","Erzincan","Erzurum","Eskişehir","Gaziantep","Giresun","Gümüşhane","Hakkâri","Hatay","Iğdır","Isparta","İçel","İstanbul","İzmir","Kahramanmaraş","Karabük",
            "Karaman","Kars","Kastamonu","Kayseri","Kırıkkale","Kırklareli","Kırşehir","Kilis","Kocaeli","Konya","Kütahya",
            "Malatya","Manisa","Mardin","Muğla","Muş","Nevşehir","Niğde","Ordu","Osmaniye","Rize","Sakarya","Samsun","Siirt","Sinop","Sivas","Şanlıurfa","Şırnak","Tekirdağ","Tokat","Trabzon","Tunceli","Uşak","Van","Yalova","Yozgat","Zonguldak","Diğer"
            };


            var roleStore = new RoleStore<ApplicationRole>(context);
            var roleManager = new RoleManager<ApplicationRole>(roleStore);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);


            // add City           
            for (int i = 0; i < turkeyCity.Length; i++)
            {
                City cty = new City()
                {

                    Name = turkeyCity[i],
                    CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"
                };
                context.Cities.Add(cty);

            }
            context.SaveChanges();
            List<City> db_cityList = context.Cities.ToList();

            // Add role
            if (!context.Roles.Any(i => i.Name == "Admin"))
            {             
                var role = new ApplicationRole() { Name = "Admin", Description = "Yönetici rolü." };
                roleManager.Create(role);
            }

            if (!context.Roles.Any(i => i.Name == "User"))
            {              
                var role = new ApplicationRole() { Name = "User", Description = "Kullanıcı rolü" }; ;
                roleManager.Create(role);
            }

            // Add admin
            ApplicationUser admin = new ApplicationUser()
            {
                Name="Yönetici",
                Surname="YÖNETİCİ",
                UserName="admin",
                IsDeleted=false,                   
                Email="admin@abcshop.com",
                CityId=37
            };
            userManager.Create(admin, "123456");
            userManager.AddToRole(admin.Id, "Admin");
            userManager.AddToRole(admin.Id, "User");

            ApplicationUser admin2 = new ApplicationUser()
            {
                Name = "Ali",
                Surname = "TEKİN",
                UserName = "alitekin",
                IsDeleted = false,        
                Email = "alitekin@abcshop.com",
                CityId = 39
            };
            userManager.Create(admin2, "123456");
            userManager.AddToRole(admin2.Id, "Admin");
            userManager.AddToRole(admin2.Id, "User");

            // Add user
            for (int i = 0; i < 20; i++)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(FakeData.NameData.GetFirstName()),
                    Surname = CultureInfo.CurrentCulture.TextInfo.ToUpper(FakeData.NameData.GetSurname()),
                    UserName = CultureInfo.CurrentCulture.TextInfo.ToLower($"user_{i}").Trim(),
                    IsDeleted = false,                            
                    Email = CultureInfo.CurrentUICulture.TextInfo.ToLower($"user_{i}@abcblog.com"),
                    CityId = db_cityList[FakeData.NumberData.GetNumber(1, db_cityList.Count()-1)].Id,
                };
                userManager.Create(user, "123456");               
                userManager.AddToRole(user.Id, "User");
            }
            context.SaveChanges();

            var allUsers = userManager.Users.ToList();
            var db_user = new List<ApplicationUser>();

            foreach (var item in allUsers)
            {
                if(userManager.IsInRole(item.Id,"User"))
                {
                    db_user.Add(item);
                }
            }

            // Add address
            for (int i = 0; i < db_user.Count; i++)
            {
                Address address = new Address()
                {
                    Title="Ev Adresi",
                    AddressLine=FakeData.PlaceData.GetAddress(),
                    MobilePhone=FakeData.PhoneNumberData.GetPhoneNumber(),
                    PostCode=FakeData.PlaceData.GetPostCode(),
                    CreatedOn=now,
                    ModifiedOn=now,
                    ModifiedUsername="system",
                    ApplicationUserId=db_user[i].Id,
                    CityId= db_user[i].CityId      
                };

                context.Addresses.Add(address);

                Address address2 = new Address()
                {
                    Title = "İş Adresi",
                    AddressLine = FakeData.PlaceData.GetAddress(),
                    MobilePhone = FakeData.PhoneNumberData.GetPhoneNumber(),
                    PostCode = FakeData.PlaceData.GetPostCode(),
                    CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system",
                    ApplicationUserId = db_user[i].Id,
                    CityId = db_user[i].CityId
                };

                context.Addresses.Add(address2);



            }

            context.SaveChanges();


            // Add order state
            OrderState os1 = new OrderState()
            {
                State="Onay Bekliyor",
                Description="Siparişiniz onay bekliyor.",
                Color = "info"
            };
            OrderState os2 = new OrderState()
            {
                State = "Sipariş Onaylandı",
                Description = "Sipariş onaylandı. Ürünler hazırlanacak.",
                Color= "primary"
            };
            OrderState os3 = new OrderState()
            {
                State = "Ürünler Hazırlanıyor",
                Description = "Ürünler hazırlanıyor. En kısa sürede kargoya verilecektir.",
                Color = "warning"
            };
            OrderState os4 = new OrderState()
            {
                State = "Kargoya Verildi",
                Description = "Siparişiniz kargoda. En kısa sürede size ulaşacaktır.",
                Color = "secondary"
            };
            OrderState os5 = new OrderState()
            {
                State = "Siapriş Tamamlandı",
                Description = "Sipariş teslim edildi.",
                Color = "success"
            };
            OrderState os6 = new OrderState()
            {
                State = "İptal",
                Description = "İptal edildi.",
                Color = "danger"
            };

            context.OrderStates.Add(os1);
            context.OrderStates.Add(os2);
            context.OrderStates.Add(os3);
            context.OrderStates.Add(os4);
            context.OrderStates.Add(os5);
            context.OrderStates.Add(os6);


            // Add category
            var categories = new List<Category>()
            {
                new Category(){ Name = "Cep Telefonu", Description = "Cep Telefonu ürünleri",   CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"},
                new Category(){ Name = "Bilgisayar", Description = "Bilgisayar ürünleri",   CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"},
                new Category(){ Name = "Elektrikli Ev Eşyaları", Description = "Elektrikli Ev Eşyaları ürünleri",   CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"},
                new Category(){ Name = "Beyaz Eşya", Description = "Beyaz Eşya ürünleri",   CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"},
                new Category(){ Name = "Kamera", Description = "Kamera ürünleri",   CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"},
                new Category(){ Name = "Televizyon", Description = "Televizyon ürünleri",   CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"},
                new Category(){ Name = "Müzik Sistemleri", Description = "Müzik Sistemleri ürünleri",   CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"},

            };

            foreach (var item in categories)
            {
                context.Categories.Add(item);
            }
            context.SaveChanges();

            List<Category> db_categoryList = context.Categories.ToList();

            var cepId = db_categoryList.Find(x => x.Name == "Cep Telefonu").Id;
            var pcId = db_categoryList.Find(x => x.Name == "Bilgisayar").Id;
            var eeeId = db_categoryList.Find(x => x.Name == "Elektrikli Ev Eşyaları").Id;
            var beyazId = db_categoryList.Find(x => x.Name == "Beyaz Eşya").Id;
            var kameraId = db_categoryList.Find(x => x.Name == "Kamera").Id;
            var tvId = db_categoryList.Find(x => x.Name == "Televizyon").Id;
            var mzkId = db_categoryList.Find(x => x.Name == "Müzik Sistemleri").Id;

            string[] cepBrand = { "Apple", "Apple", "Apple", "Asus", "Asus", "Vestel","Samsung", "Samsung", "Lg","Sony" };

            string[] pcBrand = { "Apple","Asus","Acer","Dell","Hp","Lenovo","Msi","Toshiba", "Apple", "Asus", "Acer", "Dell", "Hp", "Lenovo", "Msi", "Toshiba", "Diğer" };
            string[] p_pcBrand = { "Taşınabilir Bilgisayar", "Masaüstü bilgisayar", "All In One", "Taşınabilir Bilgisayar", "Masaüstü Bilgisayar", "All In One" };

            string[] eeeBrand = { "Arzum", "Philips","Sinbo","Tefal", "Arzum", "Philips", "Sinbo", "Arzum", "Philips", "Sinbo", "Tefal", "Arzum", "Philips" };
            string[] p_eeeBrand = { "Ütü","","Mutfak Robotu","Su Isıtıcısı","Kahve Makinesi", "Elektrikli Süpürge" };

            string[] beyazBrand = { "Arçelik","Beko","Bosh","Vestel","Siemens","Seg","Samsung", "Arçelik", "Beko", "Bosh", "Vestel", "Siemens", "Seg", "Samsung" };
            string[] p_beyazBrand = { "Buzdolabı", "Çamaşır Makinesi", "Bulaşık Makinesi", "Fırın", "Kurutma Makinesi"};

            string[] kameraBrand = { "Canon","Sony","Nikon", "Canon", "Sony", "Nikon" , "Canon", "Sony", "Nikon" };
            string[] p_kameraBrand = { "Dijital Fotoğraf Makinesi", "SLR Fotoğraf Makinesi", "Video Kamera" };

            string[] tvBrand = { "Sony","Vestel","Lg","Samsung", "Philips", "Sony", "Vestel", "Sony", "Vestel" };

            string[] mzkBrand = { "Sony","Jabra", "Philips", "Sennheiser", "Logitech", "Sony", "Jabra", "Philips", "Sennheiser", "Logitech" };
            string[] p_mzkBrand = { "Kulaklık","Müzik Seti","Pikap"};

            // Add product - Cep Telefonu
            for (int i = 0; i < FakeData.NumberData.GetNumber(20,40); i++)
            {
                string brand = cepBrand[FakeData.NumberData.GetNumber(0, cepBrand.Count() - 1)];
                Product product = new Product()
                {
                    Name = brand + " "+FakeData.TextData.GetAlphabetical(2)+" "+FakeData.NumberData.GetNumber(2016,2019)+" Cep Telefonu",
                    ProductCode= brand+FakeData.NumberData.GetNumber(100000, 9999999)+ (brand.Length > 3 ? brand.Substring(0, 3) : brand),
                    Description = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(3, 7))),
                    Features = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(5, 15))),
                    Title=FakeData.TextData.GetSentence(),
                    Price = FakeData.NumberData.GetNumber(11, 75) * 100,
                    Stock = FakeData.NumberData.GetNumber(1, 27) * 50,
                    MainImage = "defaultPhone.jpg",
                    IsHome = i % 3 == 0 ? true : false,
                    IsApproved = true,
                    CategoryId= cepId,
                    CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"
                };
                context.Products.Add(product);
            }
            context.SaveChanges();

            // Add product - Bilgisayar
            for (int i = 0; i < FakeData.NumberData.GetNumber(12, 29); i++)
            {
                string brand = pcBrand[FakeData.NumberData.GetNumber(0, pcBrand.Count() - 1)];
                string p_brand = p_pcBrand[FakeData.NumberData.GetNumber(0, p_pcBrand.Count() - 1)];
                Product product = new Product()
                {
                    Name = brand + " " + FakeData.TextData.GetAlphabetical(5) + " " + p_brand,
                    ProductCode = brand + FakeData.NumberData.GetNumber(100000, 9999999) + (brand.Length > 3 ? brand.Substring(0, 3) : brand),
                    Description = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(3, 10))),
                    Features = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(5, 15))),
                    Title = FakeData.TextData.GetSentence(),
                    Price = FakeData.NumberData.GetNumber(21, 91) * 100,
                    Stock = FakeData.NumberData.GetNumber(1, 27) * 3,
                    MainImage = "defaultPc.jpg",
                    IsHome = i % 4 == 0 ? true : false,
                    IsApproved = true,
                    CategoryId = pcId,
                    CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"
                };
                context.Products.Add(product);
            }
            context.SaveChanges();

            // Add product - Elektrikli Ev Eşyaları
            for (int i = 0; i < FakeData.NumberData.GetNumber(17, 39); i++)
            {
                string brand = eeeBrand[FakeData.NumberData.GetNumber(0, eeeBrand.Count() - 1)];
                string p_brand = p_eeeBrand[FakeData.NumberData.GetNumber(0, p_eeeBrand.Count() - 1)];
                Product product = new Product()
                {
                    Name = brand + " " + FakeData.TextData.GetAlphabetical(4) +FakeData.NumberData.GetNumber(100,999) +" " + p_brand,
                    ProductCode = brand + FakeData.NumberData.GetNumber(100000, 9999999) + (brand.Length > 3 ? brand.Substring(0, 3) : brand),
                    Description = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(3, 10))),
                    Features = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(5, 15))),
                    Title = FakeData.TextData.GetSentence(),
                    Price = FakeData.NumberData.GetNumber(9, 91) * 10,
                    Stock = FakeData.NumberData.GetNumber(1, 27) * 3,
                    MainImage = "defaultEEE.jpg",
                    IsHome = i % 5 == 0 ? true : false,
                    IsApproved = true,
                    CategoryId = eeeId,
                    CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"
                };
                context.Products.Add(product);
            }
            context.SaveChanges();

            // Add product - Beyaz Eşya
            for (int i = 0; i < FakeData.NumberData.GetNumber(7, 35); i++)
            {
                string brand = beyazBrand[FakeData.NumberData.GetNumber(0, beyazBrand.Count() - 1)];
                string p_brand = p_beyazBrand[FakeData.NumberData.GetNumber(0, p_beyazBrand.Count() - 1)];
                Product product = new Product()
                {
                    Name = brand + " " + FakeData.TextData.GetAlphabetical(5) + FakeData.NumberData.GetNumber(1000, 9999) + " " + p_brand,
                    ProductCode = brand + FakeData.NumberData.GetNumber(100000, 9999999) + (brand.Length > 3 ? brand.Substring(0, 3) : brand),
                    Description = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(3, 10))),
                    Features = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(5, 15))),
                    Title = FakeData.TextData.GetSentence(),
                    Price = FakeData.NumberData.GetNumber(21, 91) * 100,
                    Stock = FakeData.NumberData.GetNumber(1, 27) * 3,
                    MainImage = "defaultByz.jpg",
                    IsHome = i % 5 == 0 ? true : false,
                    IsApproved = true,
                    CategoryId = beyazId,
                    CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"
                };
                context.Products.Add(product);
            }
            context.SaveChanges();

            // Add product - Kamera
            for (int i = 0; i < FakeData.NumberData.GetNumber(5, 35); i++)
            {
                string brand = kameraBrand[FakeData.NumberData.GetNumber(0, kameraBrand.Count() - 1)];
                string p_brand = p_kameraBrand[FakeData.NumberData.GetNumber(0, p_kameraBrand.Count() - 1)];
                Product product = new Product()
                {
                    Name = brand + " " + FakeData.NumberData.GetNumber(10, 99)+ FakeData.TextData.GetAlphabetical(5) + " " + p_brand,
                    ProductCode = brand + FakeData.NumberData.GetNumber(100000, 9999999) + (brand.Length > 3 ? brand.Substring(0, 3) : brand),
                    Description = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(3, 10))),
                    Features = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(5, 15))),
                    Title = FakeData.TextData.GetSentence(),
                    Price = FakeData.NumberData.GetNumber(21, 41) * 100,
                    Stock = FakeData.NumberData.GetNumber(1, 27) * 3,
                    MainImage = "defaultCamera.jpg",
                    IsHome = i % 6 == 0 ? true : false,
                    IsApproved = true,
                    CategoryId = kameraId,
                    CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"
                };
                context.Products.Add(product);
            }
            context.SaveChanges();

            // Add product - Televizyon
            for (int i = 0; i < FakeData.NumberData.GetNumber(8, 28); i++)
            {
                string brand = tvBrand[FakeData.NumberData.GetNumber(0, tvBrand.Count() - 1)];
                Product product = new Product()
                {
                    Name = brand + " " + FakeData.TextData.GetAlphabetical(3) +FakeData.NumberData.GetNumber(32,55) +" inch " + FakeData.NumberData.GetNumber(2016, 2019)+" Televizyon",
                    ProductCode = brand + FakeData.NumberData.GetNumber(100000, 9999999) + (brand.Length > 3 ? brand.Substring(0, 3) : brand),
                    Description = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(3, 7))),
                    Features = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(5, 15))),
                    Title = FakeData.TextData.GetSentence(),
                    Price = FakeData.NumberData.GetNumber(11, 111) * 100,
                    Stock = FakeData.NumberData.GetNumber(1, 27) * 5,
                    MainImage = "defaultTv.jpg",
                    IsHome = i % 3 == 0 ? true : false,
                    IsApproved = true,
                    CategoryId = tvId,
                    CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"
                };
                context.Products.Add(product);
            }
            context.SaveChanges();

            // Add product - Müzik Sistemleri
            for (int i = 0; i < FakeData.NumberData.GetNumber(37, 135); i++)
            {
                string brand = mzkBrand[FakeData.NumberData.GetNumber(0, mzkBrand.Count() - 1)];
                string p_brand = p_mzkBrand[FakeData.NumberData.GetNumber(0, p_mzkBrand.Count() - 1)];
                Product product = new Product()
                {
                    Name = brand + " " + FakeData.NumberData.GetNumber(10, 99) + FakeData.TextData.GetAlphabetical(5) + " " + p_brand,
                    ProductCode = brand + FakeData.NumberData.GetNumber(100000, 9999999) + (brand.Length > 3 ? brand.Substring(0, 3) : brand),
                    Description = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(3, 10))),
                    Features = HttpUtility.HtmlEncode(FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(5, 15))),
                    Title = FakeData.TextData.GetSentence(),
                    Price = FakeData.NumberData.GetNumber(3,42) * 50,
                    Stock = FakeData.NumberData.GetNumber(1, 12) * 4,
                    MainImage = "defaultMusic.jpg",
                    IsHome = i % 5 == 0 ? true : false,
                    IsApproved = true,
                    CategoryId = mzkId,
                    CreatedOn = now,
                    ModifiedOn = now,
                    ModifiedUsername = "system"
                };
                context.Products.Add(product);
            }
            context.SaveChanges();


            // create contactmessage
            for (int i = 0; i < 40; i++)
            {
                ContactMessage cm = new ContactMessage()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    Subject = FakeData.TextData.GetSentence(),
                    Text = FakeData.TextData.GetSentences(2),
                    IsRead = i % 4 == 0 ? true : false,
                    DateTime = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMinutes(32), DateTime.Now.AddHours(50))                    

                };
                context.ContactMessages.Add(cm);
            }
            context.SaveChanges();

            base.Seed(context);
        }



    }
}
