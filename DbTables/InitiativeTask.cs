namespace Quorom.DbTables
{
    public class InitiativeTask
    {
        public Guid InitiativeTaskId { get; set; }
        public Guid InitiativeId { get; set; }
        public Guid TaskId { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedOnDateTime { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedOnDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedOnDateTime { get; set; }

        public Initiative Initiative { get; set; } = null!;
        public Task Task { get; set; } = null!;
    }
}
