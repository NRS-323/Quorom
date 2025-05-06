using System.ComponentModel.DataAnnotations;

namespace Quorom.ViewModels
{
    public class InitiativeTypeVM
    {
        public class AddInitiativeTypeVM
        {
            [Required]
            public string Title { get; set; }
            public string? Description { get; set; }
        }
        public class UpdateInitiativeTypeVM
        {
            public Guid InitiativeTypeId { get; set; }
            [Required]
            public string Title { get; set; }
            public string? Description { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
