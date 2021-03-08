using InfiniteTiers.DevicesStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteTiers.DevicesStore.Data.DAL
{
    public static class DatabaseInitializer
    {
        public static void Initialize(ItgContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            #region Add Users
            var users = new User[]
            {
                new User{FullName="Nael Saleh" },
                new User{FullName="Deyaa Hamdan"},
                new User{FullName="Rayan Hamdan"},
            };
            foreach (User user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();
            #endregion

            #region Add Devices
            var devices = new Device[]
            {
                new Device{Name="Samsung charger",Description="Charge mobile", Manufacturer="Samsung", Model="1.2.5", SerialNumber="124579843246845684", PurchaseDate=DateTime.Parse("1-1-2015"),IsActive=true},
                new Device{Name="Mi charger",Description="Charge mobile", Manufacturer="Mi", Model="2.3.6", SerialNumber="224579843246845684", PurchaseDate=DateTime.Parse("2-1-2016"),IsActive=true},
                new Device{Name="Conditioner remote",Description="Conditioner", Manufacturer="Samsung", Model="3.4.7", SerialNumber="324579843246845684", PurchaseDate=DateTime.Parse("3-1-2017"),IsActive=true},
                new Device{Name="Conditioner remote",Description="Conditioner", Manufacturer="LG", Model="4.5.8", SerialNumber="424579843246845684", PurchaseDate=DateTime.Parse("4-1-2018"),IsActive=true},

            };
            foreach (Device device in devices)
            {
                context.Devices.Add(device);
            }
            context.SaveChanges();
            #endregion

            #region Add Categories
            var categories = new Category[]
            {
                new Category{Name="Charger"},
                new Category{Name="Remote Controller"}

            };
            foreach (Category category in categories)
            {
                context.Categories.Add(category);
            }
            context.SaveChanges();
            #endregion

            #region Add User Devices
            var userDevices = new UserDevice[]
           {
                new UserDevice{StartTime=DateTime.Parse("15-1-2020")},
                new UserDevice{StartTime=DateTime.Parse("15-1-2020")},
                new UserDevice{StartTime=DateTime.Parse("15-1-2020")},
                new UserDevice{StartTime=DateTime.Parse("15-1-2020")}

            };
            foreach (UserDevice userDevice in userDevices)
            {
                context.UserDevices.Add(userDevice);
            }
            context.SaveChanges(); 
            #endregion
        }
    }
}
