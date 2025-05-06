using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quorom.DbTables;
using Quorom.Repositories;
using static Quorom.ViewModels.InitiativeVM;

namespace Quorom.Controllers
{
    [Authorize]
    public class InitiativeController : Controller
    {
        private readonly IInitiativeRepository _initiativeStore;
        private readonly IInitiativeTypeRepository _initiativeTypeList;
        private readonly IBannerRepository _bannerList;
        private readonly UserManager<QuoromUser> _userManager;
        private readonly IAuditLogRepository _log;
        public InitiativeController
            (
            IInitiativeTypeRepository initiativeTypeList,
            IInitiativeRepository initiativeStore,
            IBannerRepository bannerList,
            UserManager<QuoromUser> userManager,
            IAuditLogRepository log
            )
        {
            _initiativeTypeList = initiativeTypeList;
            _initiativeStore = initiativeStore;
            _bannerList = bannerList;
            _userManager = userManager;
            _log = log;
        }
        [HttpGet]
        public async Task<IActionResult> AddInitiative()
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var model = new AddInitiativeVM
            {
                InitiativeTypes = (await _initiativeTypeList.ReadAllActiveAsync()).Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.InitiativeTypeId.ToString()
                }),
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Contributer}")]
        public async Task<IActionResult> AddInitiative(AddInitiativeVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                model.InitiativeTypes = (await _initiativeTypeList.ReadAllActiveAsync()).Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.InitiativeTypeId.ToString()
                });
                TempData[MyConstants.Messages.Error] = "Please review the data you are entering!";
                return View(model);
            }
            var user = (await _userManager.GetUserAsync(User)).Email;
            var record = new Initiative
            {
                Title = model.Title,
                InitiativeTypeId = model.InitiativeTypeId,
                Description = model.Description,
                Objective = model.Objective,
                Owner = model.Owner,
                Status = MyConstants.Statuses.Opened,
                IsArchived = false,
                CreatedOnDateTime = DateTime.Now,
                UpdatedOnDateTime = DateTime.Now,
                CreatedByUserId = user,
                UpdatedByUserId = user,
            };
            var Id = await _initiativeStore.AddAsync(record);
            var log = new AuditLog()
            {
                UserId = user,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Type = MyConstants.ProcessType.AddRecord,
                Table = MyConstants.DbTables.Initiative,
                AssociatedId = Id.InitiativeId,
                CreatedOnDateTime = DateTime.Now
            };
            await _log.AddLogAsync(log);
            TempData[MyConstants.Messages.Success] = $"Initiative {model.Title} added!";
            return RedirectToAction("UpdateInitiative", new { id = Id.InitiativeId });
        }
        [HttpGet]
        public async Task<IActionResult> GetInitiatives()
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var initiatives = await _initiativeStore.GetInitiativesAsync();
            return View(initiatives);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateInitiative(Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var user = (await _userManager.GetUserAsync(User)).Email;
            var record = await _initiativeStore.ReadAsync(id);
            if (record != null)
            {
                var updateRecordRequest = new UpdateInitiativeVM
                {
                    InitiativeId = record.InitiativeId,
                    Title = record.Title,
                    InitiativeTypeId = record.InitiativeTypeId,
                    Description = record.Description,
                    Objective = record.Objective,
                    Owner = record.Owner,
                    Status = record.Status,
                    IsArchived = record.IsArchived,
                    InitiativeTypes = (await _initiativeTypeList.ReadAllActiveAsync()).Select(x => new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.InitiativeTypeId.ToString()
                    }),
                };
                var log = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.ReadRecord,
                    Table = MyConstants.DbTables.Initiative,
                    AssociatedId = id,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);
                return View(updateRecordRequest);
            }
            return View(null);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Modifier}")]
        public async Task<IActionResult> UpdateInitiative(UpdateInitiativeVM model)
        {
            if (!ModelState.IsValid)
            {
                model.InitiativeTypes = (await _initiativeTypeList.ReadAllActiveAsync()).Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.InitiativeTypeId.ToString()
                });
                TempData[MyConstants.Messages.Error] = "RECORD NOT SAVED! Please ensure you are entering VALID information";
                return RedirectToAction("UpdateInitiative", "Initiative", new { id = model.InitiativeId });
            }
            var user = (await _userManager.GetUserAsync(User)).Email;
            var record = new Initiative
            {
                InitiativeId = model.InitiativeId,
                Title = model.Title,
                InitiativeTypeId = model.InitiativeTypeId,
                Description = model.Description,
                Owner = model.Owner,
                Objective = model.Objective,
                Status = model.Status,
                IsArchived = model.IsArchived,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user,
            };
            var updatedRecord = await _initiativeStore.UpdateAsync(record);
            if (updatedRecord != null)
            {
                var log = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.UpdateRecord,
                    Table = MyConstants.DbTables.Initiative,
                    AssociatedId = updatedRecord.InitiativeId,
                    CreatedOnDateTime = DateTime.Now
                };
                TempData[MyConstants.Messages.Success] = $"Initiative {model.Title} updated!";
                return RedirectToAction("UpdateInitiative", "Initiative", new { id = model.InitiativeId });
            }
            else
            {
                TempData[MyConstants.Messages.Error] = "Error!";
                return RedirectToAction("UpdateInitiative", new { id = model.InitiativeId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Deleter}")]
        public async Task<IActionResult> DeleteSoft(Guid id)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var record = await _initiativeStore.ReadAsync(id);
            if (record != null)
            {
                var deletedRecord = await _initiativeStore.DeleteSoftAsync(record.InitiativeId, user);
                if (deletedRecord != null)
                {
                    var log1 = new AuditLog()
                    {
                        UserId = user,
                        IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Type = MyConstants.ProcessType.DeleteRecordSoft,
                        Table = MyConstants.DbTables.Initiative,
                        AssociatedId = record.InitiativeId,
                        CreatedOnDateTime = DateTime.Now
                    };
                    await _log.AddLogAsync(log1);

                    TempData[MyConstants.Messages.Success] = $"Initiative {record.Title} Deleted!";
                    return RedirectToAction("GetInitiatives", "Initiative");
                }
                return RedirectToAction("UpdateInitiative", "Initiative", new { id = record.InitiativeId });
            }
            return RedirectToAction("GetInitiatives", "Initiative");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.SuperUser}")]
        public async Task<IActionResult> Delete(UpdateInitiativeVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            var guid = model.InitiativeId;
            var deletedRecord = await _initiativeStore.DeleteAsync(guid);
            if (deletedRecord != null)
            {
                var log = new AuditLog()
                {
                    UserId = user.Email,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecord,
                    Table = MyConstants.DbTables.Initiative,
                    AssociatedId = guid,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);
                TempData[MyConstants.Messages.Success] = $"Initiative {model.Title} was Deleted!";
                return RedirectToAction("GetInitiatives", "Initiative");
            }
            return RedirectToAction("UpdateInitiative", new { id = model.InitiativeId });
        }
    }
}
