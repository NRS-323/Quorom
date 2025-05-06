using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quorom.DbTables;
using Quorom.Repositories;
using static Quorom.ViewModels.NotificationGroupQuoromiteVM;

namespace Quorom.Controllers
{

    [Authorize]
    public class NotificationGroupQuoromiteController : Controller
    {
        private readonly INotificationGroupQuoromiteRepository _notificationGroupQuoromiteStore;
        private readonly IQuoromiteRepository _recipientStore;
        private readonly INotificationGroupRepository _notificationGroupList;
        private readonly IBannerRepository _bannerList;
        private readonly UserManager<QuoromUser> _userManager;
        private readonly IAuditLogRepository _auditLog;
        public NotificationGroupQuoromiteController
            (
                INotificationGroupQuoromiteRepository notificationGroupQuoromiteStore,
                IQuoromiteRepository recipientStore,
                INotificationGroupRepository notificationGroupList,
                IBannerRepository bannerList,
                UserManager<QuoromUser> userManager,
                IAuditLogRepository auditLog
            )
        {
            _notificationGroupQuoromiteStore = notificationGroupQuoromiteStore;
            _recipientStore = recipientStore;
            _notificationGroupList = notificationGroupList;
            _bannerList = bannerList;
            _userManager = userManager;
            _auditLog = auditLog;
        }
        [HttpGet]
        public async Task<IActionResult> AddNotificationGroupQuoromite(Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var model = new AddNotificationGroupQuoromiteVM { };
            model.NotificationGroupName = (await _notificationGroupList.ReadAsync(id)).Name;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Contributer}")]
        public async Task<IActionResult> AddNotificationGroupQuoromite(AddNotificationGroupQuoromiteVM model, Guid id)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                model.NotificationGroupName = (await _notificationGroupList.ReadAsync(id)).Name;
                TempData[MyConstants.Messages.Error] = "Pay attention to the data you are entering!";
                return View(model);
            }
            var user = (await _userManager.GetUserAsync(User)).Email;
            var thisQuoromiteRecord = new Quoromite()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = model.FirstName + " " + model.LastName,
                Description = model.Description,
                Email = model.Email,
                IsActive = true,
                CreatedOnDateTime = DateTime.Now,
                CreatedByUserId = user,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
                IsDeleted = false,
                DeletedByUserId = null,
                DeletedOnDateTime = null,
            };
            var newQuoromiteRecord = await _recipientStore.AddAsync(thisQuoromiteRecord);

            var thisNotificationGroupQuoromiteRecord = new NotificationGroupQuoromite()
            {
                NotificationGroupId = id,
                QuoromiteId = newQuoromiteRecord.QuoromiteId,
                CreatedOnDateTime = DateTime.Now,
                CreatedByUserId = user,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
                IsDeleted = false,
                DeletedByUserId = null,
                DeletedOnDateTime = null,
            };
            var newNotificationGroupQuoromite = await _notificationGroupQuoromiteStore.AddAsync(thisNotificationGroupQuoromiteRecord);

            var recipientLog = new AuditLog()
            {
                UserId = user,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Type = MyConstants.ProcessType.AddRecord,
                Table = MyConstants.DbTables.Quoromite,
                AssociatedId = newNotificationGroupQuoromite.QuoromiteId,
                CreatedOnDateTime = DateTime.Now
            };
            await _auditLog.AddLogAsync(recipientLog);

            var notificationGroupQuoromiteLog = new AuditLog()
            {
                UserId = user,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Type = MyConstants.ProcessType.AddRecord,
                Table = MyConstants.DbTables.NotificationGroupQuoromite,
                AssociatedId = newNotificationGroupQuoromite.NotificationGroupQuoromiteId,
                CreatedOnDateTime = DateTime.Now
            };
            await _auditLog.AddLogAsync(notificationGroupQuoromiteLog);
            TempData[MyConstants.Messages.Success] = $"The {newNotificationGroupQuoromite.NotificationGroupQuoromiteId} created successfully!";
            return RedirectToAction("GetNotificationGroupQuoromite", "NotificationGroupQuoromite", new { id = newNotificationGroupQuoromite.NotificationGroupId });
        }
        [HttpGet]
        public async Task<IActionResult> GetNotificationGroupQuoromite(Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var records = await _notificationGroupQuoromiteStore.ReadMergeAsync(id);
            return View(records);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateNotificationGroupQuoromite(Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var record = await _notificationGroupQuoromiteStore.ReadAsync(id);
            if (record != null)
            {
                var record2 = await _recipientStore.ReadAsync(record.QuoromiteId);
                if (record2 != null)
                {
                    var thisRecord = new UpdateNotificationGroupQuoromiteVM
                    {
                        NotificationGroupQuoromiteId = record.NotificationGroupQuoromiteId,
                        QuoromiteId = record.QuoromiteId,
                        NotificationGroupId = record.NotificationGroupId,
                        FirstName = record2.FirstName,
                        LastName = record2.LastName,
                        Description = record2.Description,
                        Email = record2.Email,
                        IsActive = true,
                    };
                    thisRecord.NotificationGroupName = (await _notificationGroupList.ReadAsync(record.NotificationGroupId)).Name;
                    var user = (await _userManager.GetUserAsync(User)).Email;
                    var recipientLog = new AuditLog()
                    {
                        UserId = user,
                        IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Type = MyConstants.ProcessType.ReadRecord,
                        Table = MyConstants.DbTables.Quoromite,
                        AssociatedId = record.QuoromiteId,
                        CreatedOnDateTime = DateTime.Now
                    };
                    await _auditLog.AddLogAsync(recipientLog);

                    var notificationGroupQuoromiteLog = new AuditLog()
                    {
                        UserId = user,
                        IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Type = MyConstants.ProcessType.ReadRecord,
                        Table = MyConstants.DbTables.NotificationGroupQuoromite,
                        AssociatedId = record.NotificationGroupQuoromiteId,
                        CreatedOnDateTime = DateTime.Now
                    };
                    await _auditLog.AddLogAsync(notificationGroupQuoromiteLog);
                    return View(thisRecord);
                }
            }
            return View(null);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Modifier}")]
        public async Task<IActionResult> UpdateNotificationGroupQuoromite(UpdateNotificationGroupQuoromiteVM model)
        {
            if (!ModelState.IsValid)
            {
                model.NotificationGroupName = (await _notificationGroupList.ReadAsync(model.NotificationGroupId)).Name;
                TempData[MyConstants.Messages.Error] = "RECORD NOT SAVED! Please ensure you are entering VALID information";
                return RedirectToAction("GetNotificationGroupQuoromite", "NotificationGroupQuoromite", new { id = model.NotificationGroupId });
            }
            var user = (await _userManager.GetUserAsync(User)).Email;
            var thisQuoromiteRecord = new Quoromite
            {
                QuoromiteId = model.QuoromiteId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = model.FirstName + " " + model.LastName,
                Description = model.Description,
                Email = model.Email,
                IsActive = model.IsActive,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
            };
            var updatedQuoromiteRecord = await _recipientStore.UpdateAsync(thisQuoromiteRecord);

            var thisNotificationGroupQuoromiteRecord = new NotificationGroupQuoromite
            {
                NotificationGroupQuoromiteId = model.NotificationGroupQuoromiteId,
                NotificationGroupId = model.NotificationGroupId,
                QuoromiteId = model.QuoromiteId,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
            };
            var updatedNotificationGroupQuoromiteRecord = await _notificationGroupQuoromiteStore.UpdateAsync(thisNotificationGroupQuoromiteRecord);

            if (updatedNotificationGroupQuoromiteRecord != null && updatedQuoromiteRecord != null)
            {
                var recipientLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.UpdateRecord,
                    Table = MyConstants.DbTables.Quoromite,
                    AssociatedId = updatedQuoromiteRecord.QuoromiteId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _auditLog.AddLogAsync(recipientLog);

                var notificationGroupQuoromiteLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.UpdateRecord,
                    Table = MyConstants.DbTables.NotificationGroupQuoromite,
                    AssociatedId = updatedNotificationGroupQuoromiteRecord.NotificationGroupQuoromiteId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _auditLog.AddLogAsync(notificationGroupQuoromiteLog);
                TempData[MyConstants.Messages.Success] = "The NotificationGroup Quoromite Updated Successfully!";
                return RedirectToAction("GetNotificationGroupQuoromite", "NotificationGroupQuoromite", new { id = updatedNotificationGroupQuoromiteRecord.NotificationGroupId });
            }
            else
            {
                TempData[MyConstants.Messages.Error] = "You did not create this Record! Thus you CANNOT edit it!";
                return RedirectToAction("UpdateNotificationGroupQuoromite", new { id = model.NotificationGroupQuoromiteId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Deleter}")]
        public async Task<IActionResult> DeleteSoft(Guid id)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var record = await _notificationGroupQuoromiteStore.ReadAsync(id);
            var deletedRecord = await _notificationGroupQuoromiteStore.DeleteSoftAsync(record.NotificationGroupQuoromiteId, user);
            var deletedRecord2 = await _recipientStore.DeleteSoftAsync(record.QuoromiteId, user);
            if (deletedRecord != null && deletedRecord2 != null)
            {
                var log1 = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecordSoft,
                    Table = MyConstants.DbTables.NotificationGroupQuoromite,
                    AssociatedId = record.NotificationGroupQuoromiteId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _auditLog.AddLogAsync(log1);
                var log2 = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecordSoft,
                    Table = MyConstants.DbTables.Quoromite,
                    AssociatedId = record.QuoromiteId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _auditLog.AddLogAsync(log2);
                TempData[MyConstants.Messages.Success] = "The NotificationGroup Quoromite was Deleted Successfully!";
                return RedirectToAction("GetNotificationGroupQuoromites", "NotificationGroupQuoromite", new { id = record.NotificationGroupId });
            }
            return RedirectToAction("UpdateNotificationGroupQuoromite", "NotificationGroupQuoromite", new { id = record.NotificationGroupQuoromiteId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.SuperUser}")]
        public async Task<IActionResult> Delete(UpdateNotificationGroupQuoromiteVM model)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var deletedNotificationGroupQuoromiteGuid = await _notificationGroupQuoromiteStore.DeleteAsync(model.NotificationGroupQuoromiteId);
            var deletedQuoromiteGuid = await _recipientStore.DeleteAsync(model.QuoromiteId);

            if (deletedNotificationGroupQuoromiteGuid != null && deletedQuoromiteGuid != null)
            {
                var notificationGroupQuoromiteLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecord,
                    Table = MyConstants.DbTables.NotificationGroupQuoromite,
                    AssociatedId = model.NotificationGroupQuoromiteId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _auditLog.AddLogAsync(notificationGroupQuoromiteLog);

                var recipientLog = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecord,
                    Table = MyConstants.DbTables.Quoromite,
                    AssociatedId = model.QuoromiteId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _auditLog.AddLogAsync(recipientLog);

                TempData[MyConstants.Messages.Success] = "The NotificationGroup Quoromites was Deleted Successfully!";
                return RedirectToAction("GetNotificationGroupQuoromite", "NotificationGroupQuoromite");
            }
            return RedirectToAction("UpdateNotificationGroupQuoromite", "NotificationGroupQuoromite", new { id = model.NotificationGroupQuoromiteId });
        }

        [HttpGet]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Contributer}")]
        public async Task<IActionResult> AddMultipleQuoromites(Guid notificationGroupId)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();

            var notificationGroup = await _notificationGroupList.ReadAsync(notificationGroupId);
            if (notificationGroup == null)
            {
                return NotFound();
            }

            // Get existing members
            var existingLinks = await _notificationGroupQuoromiteStore.ReadAllAsync(notificationGroupId);
            var existingQuoromiteIds = existingLinks.Select(x => x.QuoromiteId).ToList();

            // Get available Quoromites
            var allQuoromites = await _recipientStore.ReadAllActiveAsync();
            var availableQuoromites = allQuoromites
                .Where(q => !existingQuoromiteIds.Contains(q.QuoromiteId) && q.IsActive && !q.IsDeleted)
                .Select(q => new SelectableQuoromite
                {
                    QuoromiteId = q.QuoromiteId,
                    FullName = q.FullName,
                    Email = q.Email,
                    IsSelected = false
                })
                .ToList();

            var model = new AddMultipleNotificationGroupQuoromitesVM
            {
                NotificationGroupId = notificationGroupId,
                NotificationGroupName = notificationGroup.Name,
                Quoromites = availableQuoromites
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Contributer}")]
        public async Task<IActionResult> AddMultipleQuoromites(AddMultipleNotificationGroupQuoromitesVM model, Guid notificationGroupId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                TempData[MyConstants.Messages.Error] = "Pay attention to the data you are entering!";
                return View(model);
            }


            var user = (await _userManager.GetUserAsync(User)).Email;
            var now = DateTime.Now;
            var addedCount = 0;

            foreach (var quoromite in model.Quoromites.Where(q => q.IsSelected))
            {
                // Check if link already exists
                var existing = await _notificationGroupQuoromiteStore.GetByGroupAndQuoromiteAsync(
                    model.NotificationGroupId,
                    quoromite.QuoromiteId
                );

                if (existing == null)
                {
                    var newLink = new NotificationGroupQuoromite()
                    {
                        NotificationGroupId = model.NotificationGroupId,
                        QuoromiteId = quoromite.QuoromiteId,
                        CreatedByUserId = user,
                        CreatedOnDateTime = now,
                        UpdatedByUserId = user,
                        UpdatedOnDateTime = now,
                        IsDeleted = false,
                        DeletedByUserId = null,
                        DeletedOnDateTime = null,
                    };

                    await _notificationGroupQuoromiteStore.AddAsync(newLink);
                    addedCount++;
                    // Audit log
                    var log = new AuditLog
                    {
                        UserId = user,
                        IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Type = MyConstants.ProcessType.AddRecord,
                        Table = MyConstants.DbTables.NotificationGroupQuoromite,
                        AssociatedId = newLink.NotificationGroupQuoromiteId,
                        CreatedOnDateTime = now
                    };
                    await _auditLog.AddLogAsync(log);
                }
            }

            TempData[MyConstants.Messages.Success] = $"Successfully added {addedCount} members to the group!";
            return RedirectToAction("GetNotificationGroupQuoromite", new { id = model.NotificationGroupId });
        }
    }
}
