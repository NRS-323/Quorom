namespace Quorom.ViewModels
{
    public class AuditLogVM
    {
        public class GetAuditLogs
        {
            public Guid AuditLogId { get; set; }
            public string Listener { get; set; }
            public string Activity { get; set; }
            public DateTime LogDateTime { get; set; }
            public string Position { get; set; }
        }
    }
}
