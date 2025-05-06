using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quorom.Databases;
using Quorom.DbTables;
using Quorom.Repositories;

namespace Quorom.Controllers
{
    [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Administrator}")]
    public class RoleController : Controller
    {
        private readonly QuoromDbContext _db;
        private readonly UserManager<QuoromUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IBannerRepository _bannerList;
        public RoleController(QuoromDbContext db, UserManager<QuoromUser> userManager, RoleManager<IdentityRole> roleManager, IBannerRepository bannerList)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _bannerList = bannerList;
        }


        [HttpGet]
        public IActionResult GetRoles()
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var roles = _db.Roles.ToList();
            return View(roles);
        }


        [HttpGet]
        public IActionResult Upsert(String roleId)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            if (String.IsNullOrEmpty(roleId))
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                //Create
                return View();
            }
            else
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                //Update
                var objFromDb = _db.Roles.FirstOrDefault(x => x.Id == roleId);
                return View(objFromDb);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(IdentityRole roleObj)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            if (await _roleManager.RoleExistsAsync(roleObj.Name))
            {

            }
            if (String.IsNullOrEmpty(roleObj.NormalizedName))
            {
                //Create
                await _roleManager.CreateAsync(new IdentityRole() { Name = roleObj.Name });
                TempData[MyConstants.Messages.Success] = $"Role {roleObj.Name} Created Successfully";
            }
            else
            {
                //Update
                var objFromDb = _db.Roles.FirstOrDefault(x => x.Id == roleObj.Id);
                objFromDb.Name = roleObj.Name;
                objFromDb.NormalizedName = roleObj.Name.ToUpper();
                var result = await _roleManager.UpdateAsync(objFromDb);
                TempData[MyConstants.Messages.Success] = $"Role {roleObj.Name} Updated Successfully";
            }
            return RedirectToAction(nameof(GetRoles));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "OnlyQuoromAdminChecker")]
        public async Task<IActionResult> Delete(string roleId)
        {
            var objFromDb = _db.Roles.FirstOrDefault(x => x.Id == roleId);
            if (objFromDb != null)
            {
                var userRolesForThisRole = _db.UserRoles.Where(x => x.RoleId == roleId).Count();
                if(userRolesForThisRole > 0)
                {
                    TempData[MyConstants.Messages.Error] = $"Cannnot Delete {objFromDb.Name}. Since Users are assigned to it.";
                    return RedirectToAction(nameof(GetRoles));
                }
                else
                {
                    TempData[MyConstants.Messages.Error] = "Role Not Found.";
                }
                var result = await _roleManager.DeleteAsync(objFromDb);
                TempData[MyConstants.Messages.Success] = $"Role {objFromDb.Name} Deleted Successfully";
            }
            return RedirectToAction(nameof(GetRoles));
        }
    }

}
