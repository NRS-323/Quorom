using System;

namespace Quorom.ViewModels.MyActivities
{
    public class TaskListViewModel
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
