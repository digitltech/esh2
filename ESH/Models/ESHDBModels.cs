using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ESH.Models
{
    public class ESHDBModels
    {
        public class ApplicationUser : IdentityUser
        {
            public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
            {
                // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
                var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
                // Здесь добавьте утверждения пользователя
                return userIdentity;
            }
        }
        public class ESHDBContext : IdentityDbContext<ApplicationUser>
        {
            public ESHDBContext()
                : base("ESHConnect", throwIfV1Schema: false)
            {
            }

            public DbSet<Product>Products { get; set; }
            public DbSet<Img>Imgs { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<StockType>StockTypes { get; set; }
            public DbSet<Stock> Stocks { get; set; }
            public DbSet<PropertyType> PropertyTypes { get; set; }
            public DbSet<PropertyValue> PropertyValues { get; set; }
            public DbSet<Property>Properties { get; set; }
            public DbSet<Cart> Carts { get; set; }
            public DbSet<Costumer>Costumers { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderDetails> OrderDetails { get; set; }
            public DbSet<PaymentType> PaymentTypes { get; set; }
            public DbSet<StatusOrder> StatusOrders { get; set; }
            public DbSet<JoinProduct> JoinProducts { get; set; }
            public DbSet<SkladType> SkladTypes { get; set; }
            public DbSet<Sklad> Sklads { get; set; }
            public DbSet<Develirery> Develireries { get; set; }
            public DbSet<Manufacturer> Manufacturers { get; set; }
            public DbSet<ManufacturerSort> ManufacturerSorts { get; set; }
            public DbSet<PropertyName> PropertyNames { get; set; }
            public DbSet<PropertyAll> PropertyAlls { get; set; }
            public DbSet<Complect> Complects { get; set; }
            public DbSet<PriceType> PriceTypes { get; set; }
            public DbSet<City> Cities { get; set; }
            public DbSet<DeveliryPrice> DeveliryPrices { get; set; }
            public static ESHDBContext Create()
            {
                return new ESHDBContext();
            }
        }
    }
}