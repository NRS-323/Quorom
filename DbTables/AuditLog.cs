namespace Quorom.DbTables
{
    public class AuditLog
    {
        public Guid AuditLogId { get; set; }
        public string UserId { get; set; }
        public string? IPAddress { get; set; }
        public Guid AssociatedId { get; set; }
        public string Type { get; set; }
        public string Table { get; set; }
        public DateTime CreatedOnDateTime { get; set; }
    }
}
