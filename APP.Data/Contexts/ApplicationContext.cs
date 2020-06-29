using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using APP.Data.Maps;

namespace APP.Data.Contexts
{
    public class ApplicationContext : IdentityDbContext<User,Role, long, IdentityUserClaim<long>,UserRole, IdentityUserLogin<long>,IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

         
            new UserMap(modelBuilder.Entity<User>().ToTable("Users"));
           
           
            new UserLoginDetailMap(modelBuilder.Entity<UserLoginDetail>());

            new UserRoleMap(modelBuilder.Entity<UserRole>());

            modelBuilder.Entity<IdentityRoleClaim<long>>(i => i.ToTable("RoleClaims"));
            modelBuilder.Entity<IdentityUserClaim<long>>(i => i.ToTable("UserClaims"));
            modelBuilder.Entity<IdentityUserLogin<long>>(i => i.ToTable("UserLogins"));
            modelBuilder.Entity<IdentityUserToken<long>>(i => i.ToTable("UserTokens"));
            modelBuilder.Entity<Role>(i => i.ToTable("Roles"));
        }
    }
}
