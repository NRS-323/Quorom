using System;
using System.Collections.Generic;
using Quorom.DbTables;
using DbTask = Quorom.DbTables.Task;


namespace Quorom.ViewModels.MyActivities
{
    public class MyInitiativeViewModel
    {
        public Guid InitiativeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Owner { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public List<DbTask> AllTasks { get; set; } = new();
        public List<DbTask> MyTasks { get; set; } = new();

        public int TotalTasks => AllTasks.Count;
        public int CompletedTasks => AllTasks.Count(t => t.IsCompleted);
        public double CompletionPercentage => TotalTasks == 0 ? 0 : Math.Round((double)CompletedTasks / TotalTasks * 100, 2);
        public string? Objective { get; set; }

    }
}
