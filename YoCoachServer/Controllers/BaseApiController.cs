using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using YoCoachServer.Models;

namespace YoCoachServer.Controllers
{
    public class BaseApiController : ApiController
    {
        public static String ADMIN = "ADMIN";
        public const String COACH = "CO";
        public static String STUDENT = "ST";

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationUser _currentUser;

        public ApplicationUser CurrentUser
        {
            get
            {
                if (_currentUser != null)
                {
                    return _currentUser;
                }
                try
                {
                    _currentUser = UserManager.FindByName(Thread.CurrentPrincipal.Identity.Name);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return _currentUser;
            }
            set { _currentUser = value; }
        }

        public IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return null;
        }
    }
}