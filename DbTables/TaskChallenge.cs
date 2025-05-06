namespace Quorom.DbTables
{
    public class TaskChallenge
    {
        public Guid TaskChallengeId { get; set; }
        public Guid TaskId { get; set; }
        public Guid ChallengeId { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedOnDateTime { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedOnDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedOnDateTime { get; set; }

        public Task Task { get; set; } = null!;
        public Challenge Challenge { get; set; } = null!;
    }
}
