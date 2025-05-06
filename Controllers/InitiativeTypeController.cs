using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quorom.DbTables;
using Quorom.Repositories;
using static Quorom.ViewModels.InitiativeTypeVM;

namespace Quorom.Controllers
{
    [Authorize]
    public class InitiativeTypeController : Controller
    {
        private readonly IInitiativeTypeRepository _initiativeTypeStore;
        private readonly IBannerRepository _bannerList;
        private readonly UserManager<QuoromUser> _userManager;
        private readonly IAuditLogRepository _log;
        public InitiativeTypeController(
            IInitiativeTypeRepository initiativeTypeStore,
            IBannerRepository bannerList,
            UserManager<QuoromUser> userManager,
            IAuditLogRepository log
            )
        {
            _initiativeTypeStore = initiativeTypeStore;
            _bannerList = bannerList;
            _userManager = userManager;
            _log = log;

        }
        [HttpGet]
        public IActionResult AddInitiativeType()
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            return View();
        }
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Contributer}")]
        [HttpPost]
        public async Task<IActionResult> AddInitiativeType(AddInitiativeTypeVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                return View();
            }
            var user = await _userManager.GetUserAsync(User);
            var record = new InitiativeType
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
            var Id = await _initiativeTypeStore.AddAsync(record);
            var log = new AuditLog()
            {
                UserId = user.Email,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Type = MyConstants.ProcessType.AddRecord,
                Table = MyConstants.DbTables.InitiativeType,
                AssociatedId = Id.InitiativeTypeId,
                CreatedOnDateTime = DateTime.Now
            };
            await _log.AddLogAsync(log);
            TempData[MyConstants.Messages.Success] = $"Initiative Type {model.Title} added!";
            return RedirectToAction("GetInitiativeTypes", "InitiativeType");
        }
        [HttpGet]
        public async Task<IActionResult> GetInitiativeTypes()
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var records = await _initiativeTypeStore.ReadAllAsync();
            return View(records);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateInitiativeType(Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var user = (await _userManager.GetUserAsync(User)).Email;
            if (!ModelState.IsValid)
            {
                return View();
            }
            var record = await _initiativeTypeStore.ReadAsync(id);
            if (record != null)
            {
                var model = new UpdateInitiativeTypeVM
                {
                    InitiativeTypeId = record.InitiativeTypeId,
                    Title = record.Title,
                    Description = record.Description,
                    IsActive = record.IsActive,
                };
                var log = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.ReadRecord,
                    Table = MyConstants.DbTables.InitiativeType,
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
        public async Task<IActionResult> UpdateInitiativeType(UpdateInitiativeTypeVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                return View();
            }
            var user = await _userManager.GetUserAsync(User);
            var record = new InitiativeType
            {
                InitiativeTypeId = model.InitiativeTypeId,
                Title = model.Title,
                Description = model.Description,
                IsActive = model.IsActive,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user.Email,
            };
            var updatedRecord = await _initiativeTypeStore.UpdateAsync(record);
            if (updatedRecord != null)
            {
                var log = new AuditLog()
                {
                    UserId = user.Email,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.UpdateRecord,
                    Table = MyConstants.DbTables.InitiativeType,
                    AssociatedId = updatedRecord.InitiativeTypeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);
                TempData[MyConstants.Messages.Success] = $"The Initiative Type {model.Title} Updated!";
                return RedirectToAction("GetInitiativeTypes", "InitiativeType");
            }
            else
            {
                TempData[MyConstants.Messages.Error] = "Something went wrong!";
                return RedirectToAction("UpdateInitiativeType", new { id = model.InitiativeTypeId });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Deleter},{MyConstants.QuoromRoleNames.Administrator},{MyConstants.QuoromRoleNames.SuperUser}")]
        public async Task<IActionResult> DeleteSoft(UpdateInitiativeTypeVM model)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var deletedRecord = await _initiativeTypeStore.DeleteSoftAsync(model.InitiativeTypeId, user);
            if (deletedRecord != null)
            {
                var log = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecordSoft,
                    Table = MyConstants.DbTables.InitiativeType,
                    AssociatedId = model.InitiativeTypeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);
                TempData[MyConstants.Messages.Success] = $"The Initiative Type {model.Title} Deleted!";
                return RedirectToAction("GetInitiativeTypes");
            }
            return RedirectToAction("UpdateInitiativeType", new { id = model.InitiativeTypeId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.SuperUser}")]
        public async Task<IActionResult> Delete(UpdateInitiativeTypeVM deleteRecord)
        {
            var user = await _userManager.GetUserAsync(User);
            var deletedRecord = await _initiativeTypeStore.DeleteAsync(deleteRecord.InitiativeTypeId);
            if (deletedRecord != null)
            {
                var log = new AuditLog()
                {
                    UserId = user.Email,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecord,
                    Table = MyConstants.DbTables.InitiativeType,
                    AssociatedId = deleteRecord.InitiativeTypeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);
                TempData[MyConstants.Messages.Success] = $"The Initiative Type {deleteRecord.Title} Deleted!";
                return RedirectToAction("GetInitiativeTypes");
            }
            return RedirectToAction("UpdateInitiativeType", new { id = deleteRecord.InitiativeTypeId });
        }
    }
}
