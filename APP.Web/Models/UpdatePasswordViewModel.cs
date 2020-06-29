using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Web.Models
{
    public class UpdatePasswordViewModel
    {
        [Required]
        public long UserId { get; set; }
        [Required(ErrorMessage = "Please enter a Password")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2}  characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public Boolean SendEmail { get; set; }
    }
}
