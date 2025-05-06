namespace Quorom.DbTables
{
    public class Initiative
    {
        public Guid InitiativeId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid InitiativeTypeId { get; set; }
        public string Owner {  get; set; }
        public string Objective { get; set; }
        public string Status { get; set; }
        public bool IsArchived { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedOnDateTime { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedOnDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedOnDateTime { get; set; }

        public InitiativeType InitiativeType { get; set; } = null!;
    }
}
