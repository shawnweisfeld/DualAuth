using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;

namespace DualAuth.Controllers
{
    public class AccountController : Controller
    {
        public void SignIn(string authType)
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                string callbackUrl = Url.Action("SignInCallback", "Account", routeValues: new { authType = authType }, protocol: Request.Url.Scheme);

                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = callbackUrl },
                   authType);
            }
        }

        public ActionResult SignInCallback(string authType)
        {
            //NOTE: I am throwing the value into session state
            //depending on your application you will probably want to store this in something more durable (i.e. Redis Cache)
            Session["AuthType"] = authType;

            return RedirectToAction("Index", "Home");
        }

        public void SignOut()
        {
            string callbackUrl = Url.Action("SignOutCallback", "Account", routeValues: null, protocol: Request.Url.Scheme);

            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                Session["AuthType"].ToString(), CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
