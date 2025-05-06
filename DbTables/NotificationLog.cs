namespace Quorom.DbTables
{
    public class NotificationLog
    {
        public Guid NotificationLogId { get; set; }
        public string NotificationType { get; set; }
        public Guid? NotificationGroupId { get; set; }
        public string? RecipientEmail { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedOnDateTime { get; set; }
    }
}
