namespace Quorom.DbTables
{
    public class Quoromite
    {
        //A member of the system or externl to recieve correspondence or access the system
        public Guid QuoromiteId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreatedOnDateTime { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdatedOnDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedOnDateTime { get; set; }

        public ICollection<Task> Tasks { get; set; }
    }
}
