using System.ComponentModel.DataAnnotations;

namespace WPFIdentity.Models
{
    public class Credentials
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
