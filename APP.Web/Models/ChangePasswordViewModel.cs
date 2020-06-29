using System.ComponentModel.DataAnnotations;

namespace APP.Web.Models {
    public class ChangePasswordViewModel {
        [Required(ErrorMessage = "Please enter Existing Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Please enter a Password")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2}  characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}