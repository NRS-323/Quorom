using System;
using System.Collections.Generic;
using System.Drawing;
using Quorom.ViewModels.Dashboard;
using Quorom.ViewModels.MyActivities;

namespace Quorom.ViewModels.Dashboard
{
    public class InitiativeProgressViewModel
    {
        public Guid InitiativeId { get; set; }
        public string Title { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; }

        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks => TotalTasks - CompletedTasks;

        public double ProgressPercent => TotalTasks == 0 ? 0 : (double)CompletedTasks / TotalTasks * 100;
        public List<TaskListViewModel> UserTasks { get; set; } = new();
        public List<QuoromiteDashboardVM> InvolvedUsers { get; set; } = new();  // Add this line

    }



    public class DashboardViewModel
    {
        public List<QuoromiteDashboardVM> Quoromites { get; set; } = new();
        public List<InitiativeProgressViewModel> Initiatives { get; set; } = new();
        public string UserId { get; set; }
    }
}
