using Microsoft.AspNetCore.Mvc.Rendering;
using Quorom.DbTables;

namespace Quorom.ViewModels
{
    public class InitiativeVM
    {
        public class AddInitiativeVM
        {
            public string Title { get; set; }
            public string? Description { get; set; }
            public string Objective { get; set; }
            public Guid InitiativeTypeId { get; set; }
            public string Owner { get; set; }
            public string[] QuoromiteEmails { get; set; } = Array.Empty<string>();

            public IEnumerable<SelectListItem>? InitiativeTypes { get; set; }
            public IEnumerable<SelectListItem>? Quoromites { get; set; }

        }
        public class GetInitiativesVM
        {
            public Guid InitiativeId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string InitiativeType { get; set; }
            public string Objective { get; set; }
            public string Status { get; set; }
            public string Owner { get; set; }
            public bool IsArchived { get; set; }
            public string AddedBy { get; set; }
            public DateTime AddedOn { get; set; }
            public ICollection<Quoromite> Quoromites { get; set; }
        }
        public class UpdateInitiativeVM
        {
            public Guid InitiativeId { get; set; }
            public string Title { get; set; }
            public string? Description { get; set; }
            public Guid InitiativeTypeId { get; set; }
            public string Owner { get; set; }
            public string Objective { get; set; }
            public string Status { get; set; }
            public string SubStatus { get; set; }
            public bool IsArchived { get; set; }
            public string[] QuoromiteEmails { get; set; } = Array.Empty<string>();

            public IEnumerable<SelectListItem>? InitiativeTypes { get; set; }
            public IEnumerable<SelectListItem>? Quoromites { get; set; }
        }
    }
}
