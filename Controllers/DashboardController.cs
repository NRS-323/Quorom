using Microsoft.AspNetCore.Mvc;
using Quorom.Repositories;
using Quorom.ViewModels.Dashboard;
using Quorom.ViewModels.MyActivities;
using System.Linq;
using System.Threading.Tasks;

namespace Quorom.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IQuoromiteRepository _quoromiteList;
        private readonly ITaskRepository _taskStore;
        private readonly IInitiativeTaskRepository _initiativeTaskStore;

        public DashboardController(IQuoromiteRepository quoromiteList, ITaskRepository taskStore, IInitiativeTaskRepository initiativeTaskStore)
        {
            _quoromiteList = quoromiteList;
            _taskStore = taskStore;
            _initiativeTaskStore = initiativeTaskStore;
        }

        public async Task<IActionResult> Index()
        {
            // Fetching active Quoromites and their task stats
            var quoromites = await _quoromiteList.ReadAllActiveAsync();
            var quoromiteStats = await _taskStore.GetQuoromiteTaskStatsAsync(quoromites.ToList());

            // Fetching initiative progress stats
            var initiativeStats = await _taskStore.GetInitiativeProgressStatsAsync();

            // Fetching the currently logged-in user's ID
            var userId = User.Identity.Name; // Or use another property like User.FindFirst(ClaimTypes.NameIdentifier) if it's available

            // Preparing the model for the dashboard view
            var model = new DashboardViewModel
            {
                Quoromites = quoromiteStats,
                Initiatives = initiativeStats,
                UserId = userId,
            };

            // Returning the model to the view
            return View("~/Views/Dashboard/Index.cshtml", model);
        }

        // Optional: If you want to add more actions (e.g., filtering, sorting), you can add them here.
    }
}
