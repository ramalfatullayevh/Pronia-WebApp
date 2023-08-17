using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProniaMVC.Models;
using ProniaMVC.Utilies.Enums;
using ProniaMVC.ViewModels;

namespace ProniaMVC.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager { get; }
        SignInManager<AppUser> _signInManager { get; }
        RoleManager<IdentityRole> _roleManager { get; }

        public AccountController(UserManager<AppUser> appuser, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = appuser;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser appUser = await _userManager.FindByNameAsync(registerVM.UserName);

            if (appUser != null)
            {
                ModelState.AddModelError("UserName", "Bu istifadeci adi movcuddur");
                return View();

            }
            appUser = new AppUser
            {
                FirstName = registerVM.Name,
                LastName = registerVM.Surname,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
            };

            var result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View();
            }

            var role = await _userManager.AddToRoleAsync(appUser, "Member");

            if (!role.Succeeded)
            {
                foreach (var item in role.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View();
            }

            await _signInManager.SignInAsync(appUser, true);

            return RedirectToAction("Login", "Account");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM loginVM, string? ReturnUrl)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Login or Password is wrong");
                    return View();
                }
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Login or Password is wrong");
                return View();
            }
            if (ReturnUrl == null)
            {
                return RedirectToAction("Index", "Home");

            }
            else
            {
                return Redirect(ReturnUrl);
            }

        }

        public async Task<IActionResult> Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task AddRoles()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });

                }
            }
            
        }


        //public async Task<IActionResult> GiveRole()
        //{
        //    AppUser appUser = new AppUser
        //    {
        //        FirstName = "Ramal",
        //        LastName = "Fatullayev",
        //        UserName = "ramalfatullayevh",
        //        Email = "ramal.fetullayev051002@gmail.com",
        //    };
        //    await _userManager.CreateAsync(appUser, "Ramal!1");
        //    await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(appUser.UserName), Roles.Admin.ToString());
        //    return View();
        //}

        public IActionResult AccessDenied()
        {
            return View();  
        }
    }
}

