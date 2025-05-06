using System;
using System.Collections.Generic;

namespace Quorom.ViewModels.MyActivities
{
    public class MyActivitiesViewModel
    {
        public Guid QuoromiteId { get; set; }
        public string FullName { get; set; }

        public List<InitiativeActivityVM> InitiativeActivities { get; set; } = new();

        // Legacy support if needed
        public List<InitiativeProgressVM> Initiatives { get; set; } = new();
        public List<MyTaskVM> MyTasks { get; set; } = new();
    }

    public class InitiativeProgressVM
    {
        public Guid InitiativeId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Objective { get; set; }

        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
    }

    public class MyTaskVM
    {
        public Guid TaskId { get; set; }
        public Guid InitiativeId { get; set; }

        public string TaskTitle { get; set; }
        public string TaskType { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
