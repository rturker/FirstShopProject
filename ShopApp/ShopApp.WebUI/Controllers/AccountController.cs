using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.Business.Abstract;
using ShopApp.WebUI.EmailServices;
using ShopApp.WebUI.Extensions;
using ShopApp.WebUI.identity;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;
        private ICartService _cartService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, ICartService cartService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
            this._cartService = cartService;
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl = null)
        {
            return View(new LoginModel
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //var user = await _userManager.FindByNameAsync(model.UserName);
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found!");
                return View(model);
            }

            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Your account has not been verified. Confirm your account.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);//3. false ise tarayici kapaninca cookie siler

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl??"~/");
            }

            ModelState.AddModelError("", "Username or password uncorrect!");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                FirstName = model.FirsName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "customer");
                // benzersiz sayi uretme - token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });
                // emaile gonderme islemi
                await _emailSender.SendEmailAsync(model.Email, "Confirm your account.", $"Please <a href='https://localhost:44367{url}'>click here</a> to confirm your email account.");
                return RedirectToAction("Login", "Account");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData.Put("message", new AlertMessage()
            {
                Title = "Signed out.",
                Message = "Signed out",
                AlertType = "warning"
            });
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if(userId == null || token == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Invalid token or user",
                    Message = "Invalid token or user",
                    AlertType = "danger"
                });
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    // create cart object------------------------
                    _cartService.InitialzieCart(user.Id);

                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Your account has been approved.",
                        Message = "Your account has been approved.",
                        AlertType = "success"
                    });
                    return View();
                }
            }
            TempData.Put("message", new AlertMessage()
            {
                Title = "Your account is not verified!",
                Message = "Your account is not verified!",
                AlertType = "danger"
            });
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Email is empty or misspelled.",
                    Message = "Email is empty or misspelled.",
                    AlertType = "danger"
                });
                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);
            if(user == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "User not found!",
                    Message = "User not found!",
                    AlertType = "danger"
                });
                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = code
            });
            // emaile gonderme islemi
            await _emailSender.SendEmailAsync(Email, "Reset your password.", $"Please <a href='https://localhost:44367{url}'>click here</a> to reset your password.");
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if(User == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new ResetPasswordModel { Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Email is incorrect.",
                    Message = "Email is incorrect.",
                    AlertType = "danger"
                });
                return View(model);
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Your password has been reset.",
                    Message = "Your password has been reset.",
                    AlertType = "success"
                });
                return RedirectToAction("Login", "Account");
            }
            TempData.Put("message", new AlertMessage()
            {
                Title = "Error",
                Message = "Error",
                AlertType = "danger"
            });
            return View(model);
        }

        private void CreateMessage(string message, string alertType)
        {
            var msg = new AlertMessage()
            {
                Message = message,
                AlertType = alertType
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);
        }

    }
}
