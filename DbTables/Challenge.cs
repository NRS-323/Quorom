namespace Quorom.DbTables
{
    public class Challenge
    {
        public Guid ChallengeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ChallengeTypeId { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedOnDateTime { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedOnDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedOnDateTime { get; set; }

        public ChallengeType ChallengeType { get; set; } = null!;
    }
}
