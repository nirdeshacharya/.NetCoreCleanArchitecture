using System;
using System.Collections.Generic;
using System.Text;

namespace APP.Data
{
    public class UserLoginDetail : BaseEntity
    {
        public long UserId { get; set; }
        public DateTime LoginTime { get; set; }

        public virtual User User { get; set; }
    }
}
