using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace APP.Data
{
    public class User : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? LastLoginTime { get; set; }

 
        public virtual UserDepartment UserDepartment { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
