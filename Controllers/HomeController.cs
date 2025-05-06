using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quorom.DbTables;
using Quorom.Repositories;
using Quorom.ViewModels;

namespace Quorom.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBannerRepository _bannerList;
        private readonly UserManager<QuoromUser> _userManager;


        public HomeController(
            ILogger<HomeController> logger,
            IBannerRepository bannerList,
            UserManager<QuoromUser> userManager
            )
        {
            _logger = logger;
            _bannerList = bannerList;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {

            return RedirectToAction("Index", "Dashboard");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorVM { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult SystemTables()
        {
            ViewBag.BannerImageUrl = _bannerList.GetRandomBanner();
            return View();
        }

    }
}
