using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace APP.Data
{
    public partial class UserRole : IdentityUserRole<long>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
