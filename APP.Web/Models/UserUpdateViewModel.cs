using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Web.Models
{
    public class UserUpdateViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Phone { get; set; }
        [Required]
        public string RoleName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public long? DepartmentId { get; set; }

        public IList<SelectListItem> Role { get; set; }
        public IList<SelectListItem> Department { get; set; }
    }
}
