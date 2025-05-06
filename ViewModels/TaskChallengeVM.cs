using Microsoft.AspNetCore.Mvc.Rendering;
using Quorom.DbTables;
using System.ComponentModel.DataAnnotations;

namespace Quorom.ViewModels
{
    public class TaskChallengeVM
    {
        public class AddTaskChallengeVM
        {
            public string? TaskTitle { get; set; }
            [Required]
            public string Title { get; set; }
            [Required]
            public Guid ChallengeTypeId { get; set; }
            [Required]
            public string Description { get; set; }
            public IEnumerable<SelectListItem>? ChallengeTypes { get; set; }
        }

        public class GetTaskChallengeVM
        {
            public Guid TaskChallengeId { get; set; }
            public Guid ChallengeId { get; set; }
            public Guid TaskId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string ChallengeType { get; set; }
            public string CreatedByUserId { get; set; }
            public DateTime CreatedOnDateTime { get; set; }
            public DateTime UpdatedOnDateTime { get; set; }
        }
        public class GetTaskChallengeListVM
        {
            public Guid TaskId { get; set; } //Tasks
            public string TaskTitle { get; set; }
            public List<GetTaskChallengeVM>? TaskChallenges { get; set; }
        }

        public class UpdateTaskChallengeVM
        {
            public Guid TaskChallengeId { get; set; }
            public Guid ChallengeId { get; set; }
            public Guid TaskId { get; set; }
            public string? TaskTitle { get; set; }
            [Required]
            public string Title { get; set; }
            [Required]
            public Guid ChallengeTypeId { get; set; }
            [Required]
            public string Description { get; set; }
            public IEnumerable<SelectListItem>? ChallengeTypes { get; set; }
        }
    }
}
