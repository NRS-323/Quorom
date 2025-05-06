using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quorom.DbTables
{
    public class QuoromUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Position { get; set; }
        public DateTime CreatedOnDateTime { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime UpdatedOnDateTime { get; set; }
        public string UpdatedByUserId { get; set; }
        [NotMapped]
        public string RoleId { get; set; }
        [NotMapped]
        public string Role {  get; set; }
        [NotMapped]
        public string UserClaim { get; set; }
    }
}
