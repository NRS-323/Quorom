using Microsoft.AspNetCore.Mvc;
using Quorom.Repositories;
using Quorom.ViewModels.MyActivities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Quorom.Controllers
{
    public class MyActivitiesController : Controller
    {
        private readonly IQuoromiteRepository _quoromiteRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IInitiativeRepository _initiativeRepository;

        public MyActivitiesController(
            IQuoromiteRepository quoromiteRepository,
            ITaskRepository taskRepository,
            IInitiativeRepository initiativeRepository)
        {
            _quoromiteRepository = quoromiteRepository;
            _taskRepository = taskRepository;
            _initiativeRepository = initiativeRepository;
        }

        public async Task<IActionResult> Index()
        {
            string? userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized();
            }

            var quoromite = await _quoromiteRepository.GetQuoromiteByEmailAsync(userEmail);
            if (quoromite == null)
            {
                return NotFound("User not found.");
            }

            var initiatives = await _initiativeRepository.GetInitiativesWithUserTasksAsync(quoromite.QuoromiteId);

            var model = new MyActivitiesViewModel
            {
                QuoromiteId = quoromite.QuoromiteId,
                FullName = quoromite.FullName,
                InitiativeActivities = initiatives
            };

            return View(model);
        }
    }
}
