using System.ComponentModel.DataAnnotations;

namespace Quorom.ViewModels
{
    public class NotificationGroupVM
    {
        public class AddNotificationGroupVM
        {
            [Required]
            public string Name { get; set; }
            [Required]
            public string Description { get; set; }
        }
        public class UpdateNotificationGroupVM
        {
            public Guid NotificationGroupId { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string Description { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
