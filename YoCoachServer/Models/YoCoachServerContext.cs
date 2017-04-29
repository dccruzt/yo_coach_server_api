using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace YoCoachServer.Models
{
    public class ApplicationUserRole : IdentityUserRole<string> { }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //public virtual Student Student { get; set; }
        //public virtual Coach Coach { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] Picture { get; set; }
        public DateTimeOffset? Birthday { get; set; }
        [JsonIgnore]
        public virtual ICollection<Installation> Installations { get; set; }

        [JsonIgnore]
        public override string Id { get; set; }

        [JsonIgnore]
        public override string PasswordHash { get; set; }

        [JsonIgnore]
        public override string SecurityStamp { get; set; }

        [JsonIgnore]
        public override string PhoneNumber { get; set; }

        [JsonIgnore]
        public override bool PhoneNumberConfirmed { get; set; }

        [JsonIgnore]
        public override bool TwoFactorEnabled { get; set; }

        [JsonIgnore]
        public override DateTime? LockoutEndDateUtc { get; set; }

        [JsonIgnore]
        public override bool LockoutEnabled { get; set; }

        [JsonIgnore]
        public override int AccessFailedCount { get; set; }

        //[JsonIgnore]
        //public override ICollection<IdentityUserRole> Roles { get; }

        //[JsonIgnore]
        //public override ICollection<IdentityUserClaim> Claims { get; }

        [JsonIgnore]
        public override ICollection<IdentityUserLogin> Logins { get; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        public ApplicationRole()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
        }

        // Add any custom Role properties/code here
    }

    public class YoCoachServerContext : IdentityDbContext<ApplicationUser>
    {
        public YoCoachServerContext() : base("YoCoachServerDB", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Coach> Coach { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Gym> Gym { get; set; }
        public DbSet<Credit> Credit { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<StudentCoach> StudentCoach { get; set; }
        public DbSet<StudentDebit> ClientDebit { get; set; }
        public DbSet<Installation> Installation { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<Credit>().HasRequired(m => m.Gym).WithOptional(m => m.Credit);
            //modelBuilder.Entity<StudentDebit>().HasRequired(m => m.Schedule).WithOptional(m => m.StudentDebit);

            modelBuilder.Entity<ApplicationUser>().ToTable("User")
                .Ignore(c => c.PhoneNumber)
                .Ignore(c => c.PhoneNumberConfirmed)
                .Ignore(c => c.TwoFactorEnabled)
                .Ignore(c => c.LockoutEndDateUtc)
                .Ignore(c => c.LockoutEnabled)
                .Ignore(c => c.AccessFailedCount);

            modelBuilder.Entity<IdentityRole>().ToTable("Rol");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRol").HasKey(x => new { x.RoleId, x.UserId });

            //modelBuilder.Ignore<IdentityUserLogin>();
            //modelBuilder.Ignore<IdentityUserClaim>();
        }

        public static YoCoachServerContext Create()
        {
            return new YoCoachServerContext();
        }
    }
}