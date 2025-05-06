using System.ComponentModel.DataAnnotations;

namespace Quorom.ViewModels
{
    public class TaskTypeVM
    {
        public class AddTaskTypeVM
        {
            [Required]
            public string Title { get; set; }
            public string? Description { get; set; }
        }
        public class UpdateTaskTypeVM
        {
            public Guid TaskTypeId { get; set; }
            [Required]
            public string Title { get; set; }
            public string? Description { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
