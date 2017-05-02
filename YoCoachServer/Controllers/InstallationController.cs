using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoCoachServer.Helpers;
using YoCoachServer.Models;
using YoCoachServer.Models.Repositories;

namespace YoCoachServer.Controllers
{
    [Authorize(Roles = COACH_STUDENT)]
    public class InstallationController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult Register(Installation installation)
        {
            try
            {
                var result = InstallationRepository.Register(CurrentUser.Id, installation);
                if (result != null)
                {
                    return Ok(result);
                }
                return Content(HttpStatusCode.BadRequest,
                        new ErrorResult(ErrorHelper.DATABASE_ERROR, ErrorHelper.INFO_DATABASE_ERROR));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,
                    new ErrorResult(ErrorHelper.EXCEPTION, ex.StackTrace));
            }
        }
    }
}