using System.ComponentModel.DataAnnotations;

namespace Quorom.ViewModels
{
    public class NotificationGroupQuoromiteVM
    {
        public class AddNotificationGroupQuoromiteVM
        {
            public string? NotificationGroupName { get; set; }
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public string Description { get; set; }
            [Required]
            public string Email { get; set; }
        }
        public class GetNotificationGroupQuoromiteVM
        {
            public Guid NotificationGroupQuoromiteId { get; set; }
            public Guid NotificationGroupId { get; set; }
            public Guid QuoromiteId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Description { get; set; }
            public bool IsActive { get; set; }
        }
        public class GetNotificationGroupQuoromiteListVM
        {
            public Guid NotificationGroupId { get; set; }
            public string NotificationGroupName { get; set; }
            public List<GetNotificationGroupQuoromiteVM>? GetNotificationGroupQuoromites { get; set; }
        }
        public class UpdateNotificationGroupQuoromiteVM
        {
            public string? NotificationGroupName { get; set; }
            public Guid NotificationGroupQuoromiteId { get; set; }
            public Guid NotificationGroupId { get; set; }
            public Guid QuoromiteId { get; set; }
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public string Description { get; set; }
            [Required]
            public string Email { get; set; }
            public bool IsActive { get; set; }
        }

        public class AddMultipleNotificationGroupQuoromitesVM
        {
            public Guid NotificationGroupId { get; set; }
            public string NotificationGroupName { get; set; }
            public List<SelectableQuoromite> Quoromites { get; set; }
        }

        public class SelectableQuoromite
        {
            public Guid QuoromiteId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public bool IsSelected { get; set; }
        }
    }
}
