using Microsoft.AspNetCore.Mvc.Rendering;
using Quorom.DbTables;
using System.ComponentModel.DataAnnotations;

namespace Quorom.ViewModels
{
    public class InitiativeTaskVM
    {
        public class AddInitiativeTaskVM
        {
            public string? InitiativeTitle { get; set; }
            [Required]
            public string Title { get; set; }
            [Required]
            public Guid TaskTypeId { get; set; }
            [Required]
            public string Description { get; set; }
            public DateTime PlannedStartDateTime { get; set; }
            public DateTime PlannedStopDateTime { get; set; }
            [Required]
            public string[] QuoromiteEmails { get; set; } = Array.Empty<string>();
           
            public IEnumerable<SelectListItem>? Quoromites { get; set; }
            public IEnumerable<SelectListItem>? TaskTypes { get; set; }
        }

        public class GetInitiativeTaskVM
        {
            public Guid InitiativeTaskId { get; set; }
            public Guid TaskId { get; set; }
            public Guid InitiativeId { get; set; }
            public string Title { get; set; }
            public DateTime PlannedStartDateTime { get; set; }
            public DateTime PlannedStopDateTime { get; set; }
            public DateTime ActualStartDateTime { get; set; }
            public DateTime ActualStopDateTime { get; set; }
            public DateTime CreatedOnDateTime { get; set; }
            public DateTime UpdatedOnDateTime { get; set; }
            public string TaskType { get; set; }
            public string Status { get; set; }
            public int ChallengeCount { get; set; }
            public ICollection<Quoromite> Quoromites { get; set; }
        }
        public class GetInitiativeTaskListVM
        {
            public Guid InitiativeId { get; set; } //Initiatives
            public string InitiativeTitle { get; set; }
            public List<GetInitiativeTaskVM>? InitiativeTasks { get; set; }
        }
        public class UpdateActualStartTimeTaskVM
        {
            public Guid InitiativeTaskId { get; set; }
            public Guid TaskId { get; set; }
            public Guid InitiativeId { get; set; }
            public string? InitiativeTitle { get; set; }
            public string TaskTitle { get; set; }
            public string TaskType { get; set; }
            public string Description { get; set; }
            public DateTime PlannedStartDateTime { get; set; }
            public DateTime PlannedStopDateTime { get; set; }
            [Required]
            public DateTime ActualStartDateTime { get; set; }
            public IEnumerable<SelectListItem>? TaskTypes { get; set; }
        }

        public class UpdateInitiativeTaskVM : IValidatableObject
        {
            public Guid InitiativeTaskId { get; set; }
            public Guid TaskId { get; set; }
            public Guid InitiativeId { get; set; }
            public string? InitiativeTitle { get; set; }
            [Required]
            public string Title { get; set; }
            [Required]
            public Guid TaskTypeId { get; set; }
            [Required]
            public string Description { get; set; }
            public DateTime PlannedStartDateTime { get; set; }
            public DateTime PlannedStopDateTime { get; set; }
            public DateTime ActualStartDateTime { get; set; }
            public DateTime ActualStopDateTime { get; set; }
            [Required]
            public string[] QuoromiteEmails { get; set; } = Array.Empty<string>();
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (QuoromiteEmails.Count() == 0)
                {
                    yield return new ValidationResult("Please select the Task Issue or Issues", new[] { "IssueIds" });
                }
            }
            public IEnumerable<SelectListItem>? Quoromites { get; set; }
            public IEnumerable<SelectListItem>? TaskTypes { get; set; }
        }
    }
}
