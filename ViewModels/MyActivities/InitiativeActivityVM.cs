using System;
using System.Collections.Generic;

namespace Quorom.ViewModels.MyActivities
{
    public class InitiativeActivityVM
    {
        public Guid InitiativeId { get; set; }
        public string Title { get; set; }
        public string Objective { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }

        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public List<TaskListViewModel> Tasks { get; set; } = new();
        public int ProgressPercentage => TotalTasks == 0 ? 0 : (int)Math.Round((double)CompletedTasks / TotalTasks * 100);


        public double CompletionPercentage
        {
            get
            {
                if (TotalTasks == 0) return 0;
                return (double)CompletedTasks / TotalTasks * 100.0;
            }
        }

        public List<TaskListViewModel> UserTasks { get; set; } = new();
    }
}
