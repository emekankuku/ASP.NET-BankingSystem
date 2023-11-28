using BankingSystem.DTO;
using BankingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View(new RegistrationModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegistrationModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.username);
            if (userExists != null)
                return RedirectToAction("UserExists");

            User user = new User()
            {
                Email = model.email,
                UserName = model.username,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var newUser = await userManager.CreateAsync(user, model.password);
            if (newUser.Succeeded)
                return RedirectToAction("Login");
            return RedirectToAction("RegistrationUnsuccessful");
        }

        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.password))
            {
                var result = await signInManager.PasswordSignInAsync(model.username, model.password, false, false);
                if (result.Succeeded)
                {
                    //await userManager.AddClaimAsync(user, new Claim("UserRole", "Admin"));
                    return RedirectToAction("Index", "Home");
                    //return RedirectToAction("LoginSuccessful");

                }
            }
            return RedirectToAction("Unauthorized");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Unauthorized()
        {
            return View();
        }

        [Authorize]
        public IActionResult LoginSuccessful()
        {
            return View();
        }

        public IActionResult UserExists()
        {
            return View();
        }

        public IActionResult RegistrationUnsuccessful()
        {
            return View();
        }
    }
}
