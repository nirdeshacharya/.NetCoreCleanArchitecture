using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Web.Models
{
    public class UserAddViewModel
    {
        [Required(ErrorMessage = "Please enter a First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter a Last Name")]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter a Password")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Phone]
        public string Phone { get; set; }
        [Required(ErrorMessage="Please select a Role")]
        public string RoleName { get; set; }

        [Required]
        public Boolean TwoFactorAuthentication { get; set; }
        public IList<SelectListItem> Role { get; set; } 

        
    }
}
