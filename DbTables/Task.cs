namespace Quorom.DbTables
{
    public class Task
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public Guid TaskTypeId { get; set; }
        public string Description { get; set; }
        public DateTime PlannedStartDateTime { get; set; }
        public DateTime PlannedStopDateTime { get; set; }
        public DateTime ActualStartDateTime { get; set; }
        public DateTime ActualStopDateTime { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public bool IsCompleted { get; set; }
        public string? CompletedByUserId { get; set; }
        public DateTime? CompletedOnDateTime { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedOnDateTime { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedOnDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedOnDateTime { get; set; }

        public TaskType TaskType { get; set; } = null!;
        public ICollection<Quoromite> Quoromites { get; set; }
    }
}
