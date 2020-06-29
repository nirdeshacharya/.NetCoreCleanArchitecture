using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP.Data.Maps
{
    public class UserLoginDetailMap
    {
        public UserLoginDetailMap(EntityTypeBuilder<UserLoginDetail> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.ToTable("UserLoginDetails");
        }
    }
}
