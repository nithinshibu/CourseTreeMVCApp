using CourseTreeMVCApp.Data;
using CourseTreeMVCApp.Data.DbContext;
using CourseTreeMVCApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginModel = CourseTreeMVCApp.Models.LoginModel;

namespace CourseTreeMVCApp.Controllers
{
    public class UserAuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserAuthController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        //ValidateAntiForgeryToken attribute denotes action filter, request made to the action methods that have this filter applied are blocked
        //unless the request includes a valid anti-forgery token
        //This feature prevent cross site request forgery
        //Making sure that the a form post comes from an appropriate form 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            loginModel.LoginInValid = "true";
            //ModelState.IsValid checks whether the form values are bound to the appropriate model
            //All the validations specified through data annotations are satisfied or not
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password,loginModel.RememberMe,lockoutOnFailure:false);

                if (result.Succeeded)
                {
                    loginModel.LoginInValid = "";
                }
                else
                {
                    //if the user is not authenticated then we can write the code to add an error message to the model state object
                    //which will result in an error message being displayed to the user 
                    //and the login dialog will stay on the screen so that the user can make another attempt to login
                    //This error message will be displayed where we have coded our summary error messages to be displayed on the login dialog
                    ModelState.AddModelError(string.Empty, "Invalid Login attempt");
                }

            }
            //We have to return the code contained in the _UserLoginPartial partial view back to the client
            //Note if the user's login attempt failed our login model dialog which is inside the _UserLoginPartial partial view will be displayed again  
            //It will contain the feedback regarding the reasons why the  user's login attempt failed.
            //It will provide the user with the means to make another at logging on to the system.
            return PartialView("_UserLoginPartial",loginModel);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            //if the calling code doesn't provide an argument when calling the logout method the relevant parameter value will be null
            await _signInManager.SignOutAsync();
            if(returnUrl != null)
            {
                //the client will be redirected to the specific location specified by the returnUrl value.
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegistrationModel registrationModel)
        {
            registrationModel.RegistrationInValid = "true";

            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = registrationModel.Email,
                    Email = registrationModel.Email,
                    PhoneNumber = registrationModel.PhoneNumber,
                    FirstName = registrationModel.FirstName,
                    LastName = registrationModel.LastName,
                    Address1 = registrationModel.Address1,  
                    Address2 = registrationModel.Address2,
                    PostCode = registrationModel.PostCode
                }; 

                var result = await _userManager.CreateAsync(user,registrationModel.Password);//This is an identity result
                if (result.Succeeded)
                {
                    registrationModel.RegistrationInValid = "";

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return PartialView("_UserRegistrationPartial",registrationModel);
                }

                //Instead of hardcoding we can use AddErrorsToModelState() method
                //ModelState.AddModelError("", "Registration Attempt Failed!");

                //This will be displayed in the div of _UserRegistrationPartial
                //<div asp-validation-summary="ModelOnly" class="text-danger"></div>
                AddErrorsToModelState(result);

            }
            return PartialView("_UserRegistrationPartial", registrationModel);
        }


        [AllowAnonymous]
        public async Task<bool> UserNameExists(string userName)
        {
            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    bool userNameExists = await _context.Users.AnyAsync(x => x.UserName.ToUpper() == userName.ToUpper());
                    if (userNameExists)
                    {
                        return true;
                    }
                }
                
                return false;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void AddErrorsToModelState(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

        }


    }
}
