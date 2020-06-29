using APP.Data;
using APP.Services;
using APP.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Web;

namespace APP.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;
        public AuthController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<AuthController> logger,
            IServiceProvider serviceProvider
        )
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Index(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Await.Warning", "CS4014:Await.Warning")]
        [HttpPost("login")]
        public async Task<IActionResult> Index(LoginViewModel model, string returnUrl = null)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Username);

            if (user == null)
            {
                user = await userManager.FindByNameAsync(model.Username);
            }

            if(null == user)
            {
                ViewBag.InvalidError = 1;
                return View(model);
            }

            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                ViewBag.errorMessage = "You must have a confirmed email to log on.";
                return View("Error");
            }

            var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password,
               false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var userService = serviceProvider.GetRequiredService<IUserService>();
                var userLoginDetailService = serviceProvider.GetRequiredService<IUserLoginDetailService>();

                logger.LogInformation("User logged in.");
                user.LastLoginTime = DateTime.Now;
                await userService.UpdateUserAsync(user);
                userLoginDetailService.AddUserLoginDetail(new UserLoginDetail
                {
                    UserId = user.Id,
                    CreatedDate = user.LastLoginTime ?? DateTime.Now,
                    LoginTime = user.LastLoginTime ?? DateTime.Now
                });

                return RedirectToLocal(returnUrl);
            }


            if (result.IsLockedOut)
            {
                logger.LogWarning("User account locked out.");
                ViewBag.errorMessage = "Account locked";
                return View("Error");
            }

            if (result.RequiresTwoFactor)
            {
                var tok = userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 1234);
                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
            }

            ViewBag.InvalidError = 1;
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SendCode(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
                return RedirectToAction("Index", new { ReturnUrl = returnUrl });

            // Generate the token and send it
            var code = await userManager.GenerateTwoFactorTokenAsync(user, "Email");

            if (string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }



            return RedirectToAction("VerifyCode");
        }

        [HttpGet("Auth/VerifyCode")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode()
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(string token)
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                // Redirect to home
            }

            var result = await signInManager.TwoFactorSignInAsync("Email", token, false, false);

            if (result.Succeeded)
            {
                return RedirectToLocal("");
            }

            if (result.IsLockedOut)
            {
                logger.LogWarning(7, "User account locked out.");
                return BadRequest("Locked");
            }

            return View();
        }


        [HttpGet]
        public IActionResult ForgetPassword(string returnUrl = null)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string email, string returnUrl = null)
        {
            if(email == null)
            {
                ViewData["error"] = "Email is required";
                return View();
            }
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ViewData["error"] = "User doesnot exist";
                return View();
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedCode = HttpUtility.UrlEncode(token);

            var callbackurl = Url.Action(
               controller: "Auth",
               action: "ResetPassword",
               values: new { userId = user.Id, token = token },
               protocol: Request.Scheme);

         
            return RedirectToAction("Index", "Auth");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            var model = new ResetPasswordViewModel();
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new Exception("User doesnot exists");
                }
                model.Email = user.Email;
                model.Token = token;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return RedirectToAction("Index", "Auth");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await this.userManager.FindByEmailAsync(model.Email);
            var result = await userManager.ResetPasswordAsync(
                                        user, model.Token, model.Password);
            if (result.Succeeded)
                return RedirectToAction("Index", "Auth");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout ()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Register new user
        public async Task<IActionResult> Add()
        {
            var userEntity = new User { FirstName ="Nirdesh", LastName = "Acharya",UserName = "nirdesh", Email = "nirdesh.acharay@gmail.com", EmailConfirmed = true, TwoFactorEnabled = false };
            var result = await userManager.CreateAsync(userEntity, "Password@1");
            if (result.Succeeded)
            {
                logger.LogInformation("User created a new account with password.");

                return RedirectToLocal("login");
            }
            var user = await userManager.FindByEmailAsync("nirdesh.acharya@gmail.com");
            await userManager.AddToRoleAsync(user, "Admin");

            return Ok(result);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [Route("Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}

