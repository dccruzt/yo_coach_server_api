using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace YoCoachServer.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual Client Client { get; set; }
        public virtual Coach Coach { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] Picture { get; set; }
        public int? Age { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class YoCoachServerContext : IdentityDbContext<ApplicationUser>
    {
        public YoCoachServerContext() : base("YoCoachServerDB", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Coach> Coach { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Gym> Gym { get; set; }
        public DbSet<Credit> Credit { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<ClientCoach> ClientCoach { get; set; }
        public DbSet<ClientDebit> ClientDebit { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Credit>().HasRequired(m => m.Gym).WithOptional(m => m.Credit);
            modelBuilder.Entity<ClientDebit>().HasRequired(m => m.Schedule).WithOptional(m => m.ClientDebit);
        }

        public static YoCoachServerContext Create()
        {
            return new YoCoachServerContext();
        }
    }
}