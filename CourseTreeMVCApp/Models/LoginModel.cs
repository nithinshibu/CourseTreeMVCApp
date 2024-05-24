using System.ComponentModel.DataAnnotations;

namespace CourseTreeMVCApp.Models
{
    public class LoginModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        //This property makes sure that the input property will be of type="password"
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string? LoginInValid { get; set; }

        public string? LoginFailedMessage { get; set; }

    }
}
