using System.ComponentModel.DataAnnotations;

namespace Quorom.ViewModels
{
    public class ChangePasswordVM
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password must have a minimum length of 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and your confirmation password do not match!")]
        public string ConfirmNewPassword { get; set; }
    }
}
