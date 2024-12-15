using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Context
{
    public class ProductDbcontext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ProductUpdateLog> ProductUpdateLogs { get; set; }


        public ProductDbcontext(DbContextOptions<ProductDbcontext> options) : base(options)
        {
        }
    }

    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
