using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quorom.DbTables;
using Quorom.Repositories;
using static Quorom.ViewModels.NotificationGroupVM;

namespace Quorom.Controllers
{
    [Authorize]
    public class NotificationGroupController : Controller
    {
        private readonly INotificationGroupRepository _notificationGroupStore;
        private readonly IBannerRepository _bannerList;
        private readonly UserManager<QuoromUser> _userManager;
        private readonly IAuditLogRepository _auditLog;
        public NotificationGroupController
            (
                INotificationGroupRepository notificationGroupStore,
                IBannerRepository bannerList,
                UserManager<QuoromUser> userManager,
                IAuditLogRepository auditLog
            )
        {
            _notificationGroupStore = notificationGroupStore;
            _bannerList = bannerList;
            _userManager = userManager;
            _auditLog = auditLog;
        }
        [HttpGet]
        public IActionResult AddNotificationGroup()
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Contributer}")]
        public async Task<IActionResult> AddNotificationGroup(AddNotificationGroupVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                TempData[MyConstants.Messages.Error] = "Pay attention to the data you are entering!";
                return View(model);
            }
            var user = (await _userManager.GetUserAsync(User)).Email;
            var thisNotificationGroupRecord = new NotificationGroup()
            {
                Name = model.Name,
                Description = model.Description,
                IsActive = true,
                CreatedOnDateTime = DateTime.Now,
                CreatedByUserId = user,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
                IsDeleted = false,
                DeletedByUserId = null,
                DeletedOnDateTime = null,
            };
            var newNotificationGroupRecord = await _notificationGroupStore.AddAsync(thisNotificationGroupRecord);

            var notificationGroupLog = new AuditLog()
            {
                UserId = user,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Type = MyConstants.ProcessType.AddRecord,
                Table = MyConstants.DbTables.NotificationGroup,
                AssociatedId = newNotificationGroupRecord.NotificationGroupId,
                CreatedOnDateTime = DateTime.Now
            };
            await _auditLog.AddLogAsync(notificationGroupLog);
            TempData[MyConstants.Messages.Success] = "The Recipient was Created Successfully!";
            return RedirectToAction("GetNotificationGroups", "NotificationGroup");
        }
        [HttpGet]
        public async Task<IActionResult> GetNotificationGroups()
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var records = await _notificationGroupStore.ReadAllActiveAsync();
            return View(records);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateNotificationGroup(Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var record = await _notificationGroupStore.ReadAsync(id);
            if (record != null)
            {
                var thisRecord = new UpdateNotificationGroupVM
                {
                    NotificationGroupId = record.NotificationGroupId,
                    Name = record.Name,
                    Description = record.Description,
                    IsActive = record.IsActive,
                };

                var user = (await _userManager.GetUserAsync(User)).Email;
                var notificationGroupLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.ReadRecord,
                    Table = MyConstants.DbTables.NotificationGroup,
                    AssociatedId = record.NotificationGroupId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _auditLog.AddLogAsync(notificationGroupLog);
                return View(thisRecord);
            }
            return View(null);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Modifier}")]
        public async Task<IActionResult> UpdateNotificationGroup(UpdateNotificationGroupVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                TempData[MyConstants.Messages.Error] = "RECORD NOT SAVED! Please ensure you are entering VALID information";
                return RedirectToAction("GetNotificationGroups", "NotificationGroup", new { id = model.NotificationGroupId });
            }
            var user = (await _userManager.GetUserAsync(User)).Email;
            var thisNotificationGroupRecord = new NotificationGroup
            {
                NotificationGroupId = model.NotificationGroupId,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
            };
            var updatedNotificationGroupRecord = await _notificationGroupStore.UpdateAsync(thisNotificationGroupRecord);

            if (updatedNotificationGroupRecord != null)
            {
                var notificationGroupLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.UpdateRecord,
                    Table = MyConstants.DbTables.NotificationGroup,
                    AssociatedId = updatedNotificationGroupRecord.NotificationGroupId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _auditLog.AddLogAsync(notificationGroupLog);
                TempData[MyConstants.Messages.Success] = "The Notification Group Updated Successfully!";
                return RedirectToAction("GetNotificationGroups", "NotificationGroup");
            }
            else
            {
                TempData[MyConstants.Messages.Error] = "There was an error";
                return RedirectToAction("UpdateNotificationGroup", new { id = model.NotificationGroupId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Deleter}")]
        public async Task<IActionResult> DeleteSoft(Guid id)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var record = await _notificationGroupStore.ReadAsync(id);
            var deletedRecord = await _notificationGroupStore.DeleteSoftAsync(record.NotificationGroupId, user);
            if (deletedRecord != null)
            {
                var log1 = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecordSoft,
                    Table = MyConstants.DbTables.NotificationGroup,
                    AssociatedId = record.NotificationGroupId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _auditLog.AddLogAsync(log1);

                TempData[MyConstants.Messages.Success] = "The NotificationGroup was Deleted Successfully!";
                return RedirectToAction("GetNotificationGroups", "NotificationGroup");
            }
            return RedirectToAction("UpdateNotificationGroup", "NotificationGroup", new { id = record.NotificationGroupId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.SuperUser}")]
        public async Task<IActionResult> Delete(UpdateNotificationGroupVM model)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var deletedNotificationGroupGuid = await _notificationGroupStore.DeleteAsync(model.NotificationGroupId);

            if (deletedNotificationGroupGuid != null)
            {
                var notificationGroupLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecord,
                    Table = MyConstants.DbTables.NotificationGroup,
                    AssociatedId = model.NotificationGroupId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _auditLog.AddLogAsync(notificationGroupLog);

                TempData[MyConstants.Messages.Success] = "The NotificationGroup was Deleted Successfully!";
                return RedirectToAction("GetNotificationGroups", "NotificationGroup");
            }
            return RedirectToAction("UpdateNotificationGroup", "NotificationGroup", new { id = model.NotificationGroupId });
        }
    }
}
