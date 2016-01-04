using BrockAllen.MembershipReboot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ServerHost.Controllers
{
    public class HomeController : Controller
    {
        UserAccountService _userAccountService;
        //AuthenticationService _authService;


        public HomeController(UserAccountService acctService) //, AuthenticationService authServ)
        {
            this._userAccountService = acctService;
            //this._authService = authServ;
        }

        // GET: Home
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password, string returnUrl)
        {
            /*

            if(username == "admin" && password == "pass")
            {
                var claims = new Claim[]
                {
                    new Claim("name", "Zohaib"),
                    new Claim("role", "Admin")

                };
                var id = new ClaimsIdentity(claims, "Cookies");
                Request.GetOwinContext().Authentication.SignIn(id);
                return Redirect(returnUrl);
            }
            
    */
            BrockAllen.MembershipReboot.UserAccount account;
            this._userAccountService.Authenticate(username, password, out account);
            if (account != null)    
            {
                var acctClaims = account.Claims;
                var claims = new Claim[]
               {
                    new Claim("name", account.Username),
                    new Claim("role", "UsersAdmin")

               };
                var id = new ClaimsIdentity(claims, "Cookies");
                Request.GetOwinContext().Authentication.SignIn(id);
                return Redirect(returnUrl);
            }
    

            return View();
        }
    }
}