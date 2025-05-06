using System;
using System.Collections.Generic;

namespace Quorom.ViewModels.Dashboard
{
    public class QuoromiteDashboardVM
    {
        public Guid QuoromiteId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks => TotalTasks - CompletedTasks;

        public List<string> InitiativeTitles { get; set; } = new();
    }



}
