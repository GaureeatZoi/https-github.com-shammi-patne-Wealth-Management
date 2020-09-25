using System.ComponentModel.DataAnnotations;
using ZOI.BAL.ViewModels;

namespace ZOI.APP.Models
{
    public class RegisterViewModel : BaseViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password", ErrorMessage ="Password and Confirm password must match")]
        public string ConfirmPassword { get; set; }
    }
}
