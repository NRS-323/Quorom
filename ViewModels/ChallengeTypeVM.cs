using System.ComponentModel.DataAnnotations;

namespace Quorom.ViewModels
{
    public class ChallengeTypeVM
    {
        public class AddChallengeTypeVM
        {
            [Required]
            public string Title { get; set; }
            public string? Description { get; set; }
        }
        public class UpdateChallengeTypeVM
        {
            public Guid ChallengeTypeId { get; set; }
            [Required]
            public string Title { get; set; }
            public string? Description { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
