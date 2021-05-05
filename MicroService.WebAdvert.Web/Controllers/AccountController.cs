using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.WebAdvert.Web
{
    /// <summary>
    /// https://github.com/aws/aws-aspnet-cognito-identity-provider/tree/master/samples/Samples/Areas/Identity/Pages/Account
    /// </summary>
    public class AccountController : Controller
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        // private readonly CognitoUserManager<CognitoUser> _cognitoUserManager;
        private readonly CognitoUserPool _cognitoUserPool;

        public AccountController(SignInManager<CognitoUser> signInManager,
            UserManager<CognitoUser> userManager,
            CognitoUserPool cognitoUserPool)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            // _cognitoUserManager = userManager as CognitoUserManager<CognitoUser>;
            _cognitoUserPool = cognitoUserPool;
        }


        public IActionResult Signup()
        {
            return View(new SignUpModel());
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _cognitoUserPool.GetUser(model.Email);
                if (user.Status != null)
                {
                    ModelState.AddModelError(nameof(SignUpModel.Email), "User already exists");
                    return View(model);
                }
                //Name is required field
                user.Attributes.Add("name", model.Email);//CognitoAttributesConstants.Name

                //var result1 = await _cognitoUserManager.CreateAsync(user, model.Password);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                    return RedirectToAction("Confirm", new ConfirmModel { Email = model.Email });

            }
            return View(model);
        }

        public IActionResult Confirm(ConfirmModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("Confirm")]
        public async Task<IActionResult> ConfirmPost(ConfirmModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("User", "User with given email address was not found!");
                    return View(model);
                }
                //Name is required field
                var result = await _userManager.ConfirmEmailAsync(user, model.Code);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                {
                    foreach (var item in result.Errors)
                        ModelState.AddModelError(item.Code, item.Description);
                }
            }
            return View(model);
        }

        public IActionResult Login(LoginModel model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginUser(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    ModelState.AddModelError("Login", "Email and Password does not match");
            }
            return View(model);
        }
    }
}
