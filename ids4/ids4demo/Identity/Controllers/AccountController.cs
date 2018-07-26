using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Data;
using Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {
        UserManager<DemoUsers> userManager;
        SignInManager<DemoUsers> signInManager;
        public AccountController(UserManager<DemoUsers> userManager,
            SignInManager<DemoUsers> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null) {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel) {
            var result = await signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, true, false);
            if (result.Succeeded) {
                if (!string.IsNullOrWhiteSpace(loginModel.ReturnUrl))
                    return Redirect(loginModel.ReturnUrl);
                else
                    return RedirectToAction(nameof(Login));
            }
            else
                return Unauthorized();
        }

    }
}