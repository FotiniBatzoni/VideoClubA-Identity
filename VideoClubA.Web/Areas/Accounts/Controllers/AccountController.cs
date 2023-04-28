using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VideoClubA.Core.Interfaces;
using VideoClubA.Web.Areas.Accounts.Models;
using VideoClubA.Web.Helpers;

namespace VideoClubA.Web.Areas.Accounts.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICustomerSevice _customerDb;

        public AccountController(ICustomerSevice customerDb)
        {
            _customerDb = customerDb;
        }

        [HttpGet]
        [Area("Accounts")]
        public IActionResult Index()
        {
            var login = new LoginViewModel();
            return View(login);
        }

        [HttpPost]
        [Area("Accounts")]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            var nameParts =  ConvertUsername.ToFirstAndLastName(login.UserName);
            string firstName = nameParts[0];
            string lastName = nameParts[1];

            var user = _customerDb.GetCustomer(firstName, lastName);

            if(!user.IsAdmin)
            {
                return View("AccessDenied");
            }
            else
            {
               
                var claims = new List<Claim>
                {
                    new Claim("Admin","Admin")
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties()
                {
                    IsPersistent = login.RememberMe
                };

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);
            }


             return RedirectToAction("MovieGallery", "Movie", new { area = "Movies" });
    
        }
    }
}
