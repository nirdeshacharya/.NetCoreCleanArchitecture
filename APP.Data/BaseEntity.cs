using System;
using System.ComponentModel.DataAnnotations;

namespace APP.Data
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastModifiedById { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
    }
}
