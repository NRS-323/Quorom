using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quorom.DbTables;
using Quorom.Repositories;
using static Quorom.ViewModels.TaskTypeVM;

namespace Quorom.Controllers
{
    [Authorize]
    public class TaskTypeController : Controller
    {
        private readonly ITaskTypeRepository _taskTypeStore;
        private readonly IBannerRepository _bannerList;
        private readonly UserManager<QuoromUser> _userManager;
        private readonly IAuditLogRepository _log;
        public TaskTypeController(
            ITaskTypeRepository taskTypeStore,
            IBannerRepository bannerList,
            UserManager<QuoromUser> userManager,
            IAuditLogRepository log
            )
        {
            _taskTypeStore = taskTypeStore;
            _bannerList = bannerList;
            _userManager = userManager;
            _log = log;

        }
        [HttpGet]
        public IActionResult AddTaskType()
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            return View();
        }
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Contributer},{MyConstants.QuoromRoleNames.Modifier}," +
           $"{MyConstants.QuoromRoleNames.Deleter},{MyConstants.QuoromRoleNames.Administrator},{MyConstants.QuoromRoleNames.SuperUser}")]
        [HttpPost]
        public async Task<IActionResult> AddTaskType(AddTaskTypeVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                return View();
            }
            var user = await _userManager.GetUserAsync(User);
            var record = new TaskType
            {
                Title = model.Title,
                IsActive = true,
                Description = model.Description,
                CreatedOnDateTime = DateTime.Now,
                CreatedByUserId = user.Email,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user.Email,
                DeletedByUserId = null,
                DeletedOnDateTime = null,
                IsDeleted = false,
            };
            var Id = await _taskTypeStore.AddAsync(record);
            var log = new AuditLog()
            {
                UserId = user.Email,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Type = MyConstants.ProcessType.AddRecord,
                Table = MyConstants.DbTables.TaskType,
                AssociatedId = Id.TaskTypeId,
                CreatedOnDateTime = DateTime.Now
            };
            await _log.AddLogAsync(log);
            TempData[MyConstants.Messages.Success] = $"Task Type {model.Title} added!";
            return RedirectToAction("GetTaskTypes", "TaskType");
        }
        [HttpGet]
        public async Task<IActionResult> GetTaskTypes()
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var records = await _taskTypeStore.ReadAllAsync();
            return View(records);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateTaskType(Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var user = (await _userManager.GetUserAsync(User)).Email;
            if (!ModelState.IsValid)
            {
                return View();
            }
            var record = await _taskTypeStore.ReadAsync(id);
            if (record != null)
            {
                var model = new UpdateTaskTypeVM
                {
                    TaskTypeId = record.TaskTypeId,
                    Title = record.Title,
                    Description = record.Description,
                    IsActive = record.IsActive,
                };
                var log = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.ReadRecord,
                    Table = MyConstants.DbTables.TaskType,
                    AssociatedId = id,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);
                return View(model);
            }
            return View(null);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Modifier},{MyConstants.QuoromRoleNames.Administrator},{MyConstants.QuoromRoleNames.SuperUser}")]
        public async Task<IActionResult> UpdateTaskType(UpdateTaskTypeVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                return View();
            }
            var user = await _userManager.GetUserAsync(User);
            var record = new TaskType
            {
                TaskTypeId = model.TaskTypeId,
                Title = model.Title,
                Description = model.Description,
                IsActive = model.IsActive,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user.Email,
            };
            var updatedRecord = await _taskTypeStore.UpdateAsync(record);
            if (updatedRecord != null)
            {
                var log = new AuditLog()
                {
                    UserId = user.Email,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.UpdateRecord,
                    Table = MyConstants.DbTables.TaskType,
                    AssociatedId = updatedRecord.TaskTypeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);
                TempData[MyConstants.Messages.Success] = $"The Task Type {model.Title} Updated!";
                return RedirectToAction("GetTaskTypes", "TaskType");
            }
            else
            {
                TempData[MyConstants.Messages.Error] = "Something went wrong!";
                return RedirectToAction("UpdateTaskType", new { id = model.TaskTypeId });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Deleter},{MyConstants.QuoromRoleNames.Administrator},{MyConstants.QuoromRoleNames.SuperUser}")]
        public async Task<IActionResult> DeleteSoft(UpdateTaskTypeVM model)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var deletedRecord = await _taskTypeStore.DeleteSoftAsync(model.TaskTypeId, user);
            if (deletedRecord != null)
            {
                var log = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecordSoft,
                    Table = MyConstants.DbTables.TaskType,
                    AssociatedId = model.TaskTypeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);
                TempData[MyConstants.Messages.Success] = $"The Task Type {model.Title} Deleted!";
                return RedirectToAction("GetTaskTypes");
            }
            return RedirectToAction("UpdateTaskType", new { id = model.TaskTypeId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.SuperUser}")]
        public async Task<IActionResult> Delete(UpdateTaskTypeVM deleteRecord)
        {
            var user = await _userManager.GetUserAsync(User);
            var deletedRecord = await _taskTypeStore.DeleteAsync(deleteRecord.TaskTypeId);
            if (deletedRecord != null)
            {
                var log = new AuditLog()
                {
                    UserId = user.Email,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecord,
                    Table = MyConstants.DbTables.TaskType,
                    AssociatedId = deleteRecord.TaskTypeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);
                TempData[MyConstants.Messages.Success] = $"The Task Type {deleteRecord.Title} Deleted!";
                return RedirectToAction("GetTaskTypes");
            }
            return RedirectToAction("UpdateTaskType", new { id = deleteRecord.TaskTypeId });
        }
    }
}
