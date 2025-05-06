using System.ComponentModel.DataAnnotations;

namespace Quorom.ViewModels
{
    public class VerifyAuthenticatorVM
    {
        [Required]
        public string Code { get; set; }
        public string? ReturnURL { get; set; }
        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
    }
}
