using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quorom.DbTables;
using Quorom.Repositories;
using Quorom.Services;
using Quorom.ViewModels;
using System.Text.Encodings.Web;

namespace Quorom.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<QuoromUser> _userManager;
        private readonly SignInManager<QuoromUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UrlEncoder _urlEncoder;
        private readonly IQuoromiteRepository _recipient;
        private readonly IEmailWithMailKit _email;
        private readonly IAuditLogRepository _log;
        public AccountController
            (
            UserManager<QuoromUser> userManager,
            SignInManager<QuoromUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            UrlEncoder urlEncoder,
            IQuoromiteRepository recipient,
            IEmailWithMailKit email,
            IAuditLogRepository log
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _recipient = recipient;
            _email = email;
            _urlEncoder = urlEncoder;
            _log = log;
        }

        [Authorize(Roles = $"{MyConstants.QuoromRoleNames.Administrator}")]
        //[AllowAnonymous]
        public async Task<IActionResult> Register(string returnURL = null)
        {
            if (!_roleManager.RoleExistsAsync(MyConstants.QuoromRoleNames.Administrator).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(MyConstants.QuoromRoleNames.Administrator));
                await _roleManager.CreateAsync(new IdentityRole(MyConstants.QuoromRoleNames.Viewer));
            }

            ViewData["ReturnUrl"] = returnURL;
            RegisterVM model = new()
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem { Text = i, Value = i })
            };
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Login(string returnURL = null)
        {

            ViewData["ReturnUrl"] = returnURL;
            return View();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM viewModel, string returnURL = null)
        {
            var user = (await _userManager.GetUserAsync(User)).Email;
            ViewData["ReturnUrl"] = returnURL;
            returnURL = returnURL ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var umbonoUser = new QuoromUser
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Position = viewModel.Position,
                    UserName = viewModel.Username,
                    Email = viewModel.Email,
                    CreatedOnDateTime = DateTime.Now,
                };
                var result = await _userManager.CreateAsync(umbonoUser, viewModel.Password);
                if (result.Succeeded)
                {
                    var emailQuoromite = new Quoromite()
                    {
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        FullName = viewModel.FirstName + " " + viewModel.LastName,
                        Description = viewModel.Position,
                        Email = viewModel.Email,
                        IsActive = true,
                        CreatedByUserId = user,
                        CreatedOnDateTime = DateTime.Now,
                        UpdatedByUserId = user,
                        UpdatedOnDateTime = DateTime.Now,
                        IsDeleted = false,
                        DeletedByUserId = null,
                        DeletedOnDateTime = null,
                    };
                    await _recipient.AddAsync(emailQuoromite);

                    if (viewModel.RoleSelected != null)
                        await _userManager.AddToRoleAsync(umbonoUser, viewModel.RoleSelected);
                    else
                        await _userManager.AddToRoleAsync(umbonoUser, MyConstants.QuoromRoleNames.Viewer);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(umbonoUser);
                    var callBackURL = Url.Action("ConfirmEmail", "Account", new { userid = umbonoUser.Id, code }, protocol: HttpContext.Request.Scheme);
                    await _email.SendMailViaUser(viewModel.Email, "Quorom Account Confirmation", $"To confirm your account, click here: <a href='{callBackURL}'>link</a>", user);
                    await _signInManager.SignInAsync(umbonoUser, isPersistent: false);
                    if (!string.IsNullOrEmpty(returnURL) && Url.IsLocalUrl(returnURL))

                        return RedirectToAction("Index", "Dashboard");
                }
                AddErrors(result);
            }
            viewModel.RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem { Text = i, Value = i });

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return RedirectToAction("Login");
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await _signInManager.SignInAsync(user, isPersistent: false);
                return View("ChangePasswordConfirmation");
            }
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EnableAuthenticator()
        {
            string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
            var user = await _userManager.GetUserAsync(User);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            var token = await _userManager.GetAuthenticatorKeyAsync(user);
            string AuthUri = string.Format(AuthenticatorUriFormat, _urlEncoder.Encode("Quorom"), _urlEncoder.Encode(user.Email), token);

            var model = new TwoFactorAuthenticationVM() { Token = token, QRCodeURL = AuthUri };
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RemoveAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            return RedirectToAction(nameof(Index), "Home");
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(TwoFactorAuthenticationVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var succeeded = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, model.Code);
                if (succeeded)
                    await _userManager.SetTwoFactorEnabledAsync(user, true);
                else
                {
                    ModelState.AddModelError("Verify", "Your two factor authentication code could note be validated");
                    return View(model);
                }
                return RedirectToAction(nameof(AuthenticatorConfirmation));
            }
            return View("Error");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyAuthenticatorCode(bool rememberMe, string returnURL = null)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
                return View("Error"); ViewData["ReturnURL"] = returnURL;
            return View(new VerifyAuthenticatorVM { ReturnURL = returnURL, RememberMe = rememberMe });
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyAuthenticatorCode(VerifyAuthenticatorVM model)
        {
            model.ReturnURL = model.ReturnURL ?? Url.Content("~/");
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(model.Code, model.RememberMe, rememberClient: false);

            if (result.Succeeded)
                return LocalRedirect(model.ReturnURL);
            if (result.IsLockedOut)
                return View("Lockout");
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt!");
                return View(model);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            var authUser = (await _userManager.GetUserAsync(User)).Email;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return RedirectToAction("ForgotPasswordConfirmation");
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callBackURL = Url.Action("ResetPassword", "Account", new { userid = user.Id, code }, protocol: HttpContext.Request.Scheme);
                await _email.SendMailViaUser(model.Email, "Reset Password - Quorom", $"Umbunite! To Reset your password, click here: <a href='{callBackURL}'>link</a>", authUser);
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AuthenticatorConfirmation()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                AddErrors(result);
            }
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string code, string userId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return View("Error");
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                    return View();
            }
            return View("Error");
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM model, string returnURL = null)
        {
            ViewData["ReturnUrl"] = returnURL;
            returnURL = returnURL ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password,
                    model.RememberMe, lockoutOnFailure: true);

                if (result.IsLockedOut)
                    return View("Lockout");

                if (result.RequiresTwoFactor)
                    return RedirectToAction(nameof(VerifyAuthenticatorCode), new { returnURL, model.RememberMe });

                if (result.Succeeded)
                {
                    var user = await _userManager.GetUserAsync(User);

                    var log = new AuditLog()
                    {
                        UserId = user.Email,
                        IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        Type = MyConstants.ProcessType.Login,
                        Table = MyConstants.DbTables.Login,
                        AssociatedId = Guid.Parse(user.Id),
                        CreatedOnDateTime = DateTime.Now
                    };
                    await _log.AddLogAsync(log);

                    // 👇 Add this line to trigger the animation
                    TempData["ShowAnimation"] = true;

                    // 👇 Instead of redirecting to Dashboard immediately,
                    // redirect back to the Login view to show the animation first
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt!");
                    return View(model);
                }
            }
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
