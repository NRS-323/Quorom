using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quorom.DbTables;
using Quorom.Repositories;
using static Quorom.ViewModels.ChallengeTypeVM;

namespace Quorom.Controllers
{
    [Authorize]
    public class ChallengeTypeController : Controller
    {
        private readonly IChallengeTypeRepository _challengeTypeStore;
        private readonly IBannerRepository _bannerList;
        private readonly UserManager<QuoromUser> _userManager;
        private readonly IAuditLogRepository _log;
        public ChallengeTypeController(
            IChallengeTypeRepository challengeTypeStore,
            IBannerRepository bannerList,
            UserManager<QuoromUser> userManager,
            IAuditLogRepository log
            )
        {
            _challengeTypeStore = challengeTypeStore;
            _bannerList = bannerList;
            _userManager = userManager;
            _log = log;

        }
        [HttpGet]
        public IActionResult AddChallengeType()
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            return View();
        }
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Contributer}")]
        [HttpPost]
        public async Task<IActionResult> AddChallengeType(AddChallengeTypeVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                return View();
            }
            var user = await _userManager.GetUserAsync(User);
            var record = new ChallengeType
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
            var Id = await _challengeTypeStore.AddAsync(record);
            var log = new AuditLog()
            {
                UserId = user.Email,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Type = MyConstants.ProcessType.AddRecord,
                Table = MyConstants.DbTables.ChallengeType,
                AssociatedId = Id.ChallengeTypeId,
                CreatedOnDateTime = DateTime.Now
            };
            await _log.AddLogAsync(log);
            TempData[MyConstants.Messages.Success] = $"Challenge Type {model.Title} added!";
            return RedirectToAction("GetChallengeTypes", "ChallengeType");
        }
        [HttpGet]
        public async Task<IActionResult> GetChallengeTypes()
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var records = await _challengeTypeStore.ReadAllAsync();
            return View(records);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateChallengeType(Guid id)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var user = (await _userManager.GetUserAsync(User)).Email;
            if (!ModelState.IsValid)
            {
                return View();
            }
            var record = await _challengeTypeStore.ReadAsync(id);
            if (record != null)
            {
                var model = new UpdateChallengeTypeVM
                {
                    ChallengeTypeId = record.ChallengeTypeId,
                    Title = record.Title,
                    Description = record.Description,
                    IsActive = record.IsActive,
                };
                var log = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.ReadRecord,
                    Table = MyConstants.DbTables.ChallengeType,
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
        public async Task<IActionResult> UpdateChallengeType(UpdateChallengeTypeVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                return View();
            }
            var user = await _userManager.GetUserAsync(User);
            var record = new ChallengeType
            {
                ChallengeTypeId = model.ChallengeTypeId,
                Title = model.Title,
                Description = model.Description,
                IsActive = model.IsActive,
                UpdatedOnDateTime = DateTime.Now,
                UpdatedByUserId = user.Email,
            };
            var updatedRecord = await _challengeTypeStore.UpdateAsync(record);
            if (updatedRecord != null)
            {
                var log = new AuditLog()
                {
                    UserId = user.Email,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.UpdateRecord,
                    Table = MyConstants.DbTables.ChallengeType,
                    AssociatedId = updatedRecord.ChallengeTypeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);
                TempData[MyConstants.Messages.Success] = $"The Challenge Type {model.Title} Updated!";
                return RedirectToAction("GetChallengeTypes", "ChallengeType");
            }
            else
            {
                TempData[MyConstants.Messages.Error] = "Something went wrong!";
                return RedirectToAction("UpdateChallengeType", new { id = model.ChallengeTypeId });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Deleter},{MyConstants.QuoromRoleNames.Administrator},{MyConstants.QuoromRoleNames.SuperUser}")]
        public async Task<IActionResult> DeleteSoft(UpdateChallengeTypeVM model)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            var deletedRecord = await _challengeTypeStore.DeleteSoftAsync(model.ChallengeTypeId, user);
            if (deletedRecord != null)
            {
                var log = new AuditLog()
                {
                    UserId = user,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecordSoft,
                    Table = MyConstants.DbTables.ChallengeType,
                    AssociatedId = model.ChallengeTypeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);
                TempData[MyConstants.Messages.Success] = $"The Challenge Type {model.Title} Deleted!";
                return RedirectToAction("GetChallengeTypes");
            }
            return RedirectToAction("UpdateChallengeType", new { id = model.ChallengeTypeId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.SuperUser}")]
        public async Task<IActionResult> Delete(UpdateChallengeTypeVM deleteRecord)
        {
            var user = await _userManager.GetUserAsync(User);
            var deletedRecord = await _challengeTypeStore.DeleteAsync(deleteRecord.ChallengeTypeId);
            if (deletedRecord != null)
            {
                var log = new AuditLog()
                {
                    UserId = user.Email,
                    IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    Type = MyConstants.ProcessType.DeleteRecord,
                    Table = MyConstants.DbTables.ChallengeType,
                    AssociatedId = deleteRecord.ChallengeTypeId,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);
                TempData[MyConstants.Messages.Success] = $"The Challenge Type {deleteRecord.Title} Deleted!";
                return RedirectToAction("GetChallengeTypes");
            }
            return RedirectToAction("UpdateChallengeType", new { id = deleteRecord.ChallengeTypeId });
        }
    }
}
