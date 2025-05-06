using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quorom.Databases;
using Quorom.DbTables;
using Quorom.Repositories;
using Quorom.ViewModels;
using System.Security.Claims;

namespace Quorom.Controllers
{
    [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Administrator}")]
    public class UserController : Controller
    {
        private readonly QuoromDbContext _db;
        private readonly UserManager<QuoromUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IBannerRepository _bannerList;

        public UserController(QuoromDbContext db, UserManager<QuoromUser> userManager, RoleManager<IdentityRole> roleManager, IBannerRepository bannerList)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _bannerList = bannerList;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            var userList = _db.QuoromUsers.ToList();

            foreach (var user in userList)
            {
                var user_Role = await _userManager.GetRolesAsync(user) as List<string>;
                user.Role = String.Join(", ", user_Role);
                var user_Claim = _userManager.GetClaimsAsync(user).GetAwaiter().GetResult().Select(u => u.Type);
                user.UserClaim = String.Join(", ", user_Claim);
            }
            return View(userList);
        }

        public async Task<IActionResult> ManageRole(string userId)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            QuoromUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                TempData[MyConstants.Messages.Error] = $"This {userId} was NOT Found";
                return NotFound();
            }
            List<string> existingUserRoles = await _userManager.GetRolesAsync(user) as List<string>;
            var model = new RoleVM()
            {
                User = user
            };
            foreach (var role in _roleManager.Roles)
            {
                RoleSelection roleSelection = new()
                {
                    RoleName = role.Name
                };
                if (existingUserRoles.Any(x => x == role.Name))
                {
                    roleSelection.IsSelected = true;
                }
                model.RoleList.Add(roleSelection);
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRole(RoleVM model)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            QuoromUser user = await _userManager.FindByIdAsync(model.User.Id);
            if (user == null)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                TempData[MyConstants.Messages.Error] = $"This {model.User.Id} was NOT Found";
                return NotFound();
            }
            var oldUserRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, oldUserRoles);
            if (!result.Succeeded)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                TempData[MyConstants.Messages.Error] = "Error while removing roles";
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user, model.RoleList.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                TempData[MyConstants.Messages.Error] = "Error while adding roles";
                return View(model);
            }

            TempData[MyConstants.Messages.Success] = "Roles Assigned Successfully";
            return RedirectToAction(nameof(GetUsers));
        }

        public async Task<IActionResult> ManageUserClaim(string userId)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            QuoromUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                TempData[MyConstants.Messages.Error] = $"This {userId} was NOT Found";
                return NotFound();
            }
            var existingUserClaims = await _userManager.GetClaimsAsync(user);
            var model = new ClaimsVM()
            {
                User = user
            };
            foreach (Claim claim in MyConstants.ClaimStore.claimList)
            {
                ClaimSelection userClaim = new()
                {
                    ClaimType = claim.Type
                };
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.ClaimList.Add(userClaim);
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserClaim(ClaimsVM model)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            QuoromUser user = await _userManager.FindByIdAsync(model.User.Id);
            if (user == null)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                TempData[MyConstants.Messages.Error] = $"This {model.User.Id} was NOT Found";
                return NotFound();
            }
            var oldUserClaims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, oldUserClaims);
            if (!result.Succeeded)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                TempData[MyConstants.Messages.Error] = "Error while removing claims";
                return View(model);
            }

            result = await _userManager.AddClaimsAsync(user, model.ClaimList.Where(x => x.IsSelected).Select(y => new Claim(y.ClaimType, y.IsSelected.ToString())));
            if (!result.Succeeded)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                TempData[MyConstants.Messages.Error] = "Error while adding claims";
                return View(model);
            }

            TempData[MyConstants.Messages.Success] = "Claims Assigned Successfully";
            return RedirectToAction(nameof(GetUsers));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUnlock(string userId)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            QuoromUser user = _db.QuoromUsers.FirstOrDefault(user => user.Id == userId);
            if (user == null)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                TempData[MyConstants.Messages.Error] = $"This {userId} was NOT Found";
                return NotFound();
            }
            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now;
                TempData[MyConstants.Messages.Success] = $"This {userId} was successfully Unlocked";
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddYears(1000);
                TempData[MyConstants.Messages.Success] = $"This {userId} was successfully Locked";

            }
            _db.SaveChanges();
            return RedirectToAction(nameof(GetUsers));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            QuoromUser user = _db.QuoromUsers.FirstOrDefault(user => user.Id == userId);
            if (user == null)
            {
                ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
                TempData[MyConstants.Messages.Error] = $"This {userId} was NOT Found";
                return NotFound();
            }
            _db.QuoromUsers.Remove(user);
            _db.SaveChanges();
            TempData[MyConstants.Messages.Success] = $"This {userId} was successfully Deleted";
            return RedirectToAction(nameof(GetUsers));
        }
    }

}
