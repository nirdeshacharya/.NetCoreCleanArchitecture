using System;
using System.Collections.Generic;
using System.Text;

namespace APP.Data
{
    public class UserDepartment : BaseEntity
    {
        public long UserId { get; set; }

        public virtual User User { get; set; }
    }
}
