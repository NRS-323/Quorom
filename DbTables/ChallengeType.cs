namespace Quorom.DbTables
{
    public class ChallengeType
    {
        public Guid ChallengeTypeId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedOnDateTime { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedOnDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedOnDateTime { get; set; }

        public ICollection<Challenge> Challenges { get; } = new List<Challenge>();
    }
}
