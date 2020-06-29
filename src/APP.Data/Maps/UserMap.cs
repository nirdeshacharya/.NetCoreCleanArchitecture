using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace APP.Data.Maps
{
    class UserMap
    {
        public UserMap(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.UserName).IsRequired();
            entityBuilder.Property(t => t.FirstName).IsRequired().HasMaxLength(32);
            entityBuilder.Property(t => t.LastName).IsRequired().HasMaxLength(32);
            entityBuilder.Property(t => t.CreatedDate).HasDefaultValueSql("getdate()").IsRequired();
            entityBuilder.HasMany(u => u.UserRoles)
                         .WithOne(ur => ur.User)
                         .HasForeignKey(ur => ur.UserId)
                         .IsRequired();
        }
    }
}
