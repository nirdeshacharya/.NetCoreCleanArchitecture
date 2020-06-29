using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP.Data.Maps
{
    public class UserRoleMap
    {
        public UserRoleMap(EntityTypeBuilder<UserRole> entityBuilder)
        {
            entityBuilder.HasKey(x => new { x.UserId, x.RoleId});
            entityBuilder.HasIndex(x => x.RoleId);
            entityBuilder.ToTable("UserRoles");
        }
    }
}
