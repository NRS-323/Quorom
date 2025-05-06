namespace Quorom.DbTables
{
    public class NotificationGroupQuoromite
    {
        public Guid NotificationGroupQuoromiteId { get; set; }
        public Guid NotificationGroupId { get; set; }
        public Guid QuoromiteId { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedOnDateTime { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedOnDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedOnDateTime { get; set; }

        public Quoromite Quoromite { get; set; } = null!;
        public NotificationGroup NotificationGroup { get; set; } = null!;
    }
}
