using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<WorldUser> _manager;

        public AuthController(SignInManager<WorldUser> manager)
        {
            _manager = manager;
        }
        public IActionResult Login(string fromPath)
        {
            //check if authenticated, return Trips
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Trips", "App");
            }
            //if not, same page
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(string returnUrl, LoginViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var signInResult = await _manager.PasswordSignInAsync(vm.Username, vm.Password, true, false);
                if(signInResult.Succeeded)
                {
                    if(string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Trips", "App");
                    }
                    else
                    {
                        return Redirect(returnUrl);             //returnUrl je dio url-a koji config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login"; u startup-u doda
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is incorrect");
                }
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _manager.SignOutAsync();
            }

            return RedirectToAction("Index", "App");
        }
    }
}
