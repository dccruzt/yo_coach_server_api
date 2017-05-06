using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using YoCoachServer.Helpers;
using YoCoachServer.Models;
using YoCoachServer.Models.BindingModels;
using YoCoachServer.Models.Repositories;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Controllers
{
    [Authorize(Roles = COACH)]
    public class ScheduleController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult MarkAsCompleted(MarkScheduleBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Content(HttpStatusCode.BadRequest,
                        new ErrorResult(ErrorHelper.INVALID_BODY, ErrorHelper.GetModelErrors(ModelState)));
                }
                var result = ScheduleRepository.MarkAsCompleted(model.Id, model.CreditsAmount);
                if (result is Schedule)
                {
                    return Ok(result);
                }else if (result is ErrorResult)
                {
                    return Content(HttpStatusCode.BadRequest, result);
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

        [HttpPost]
        public IHttpActionResult ReceivePayment(PayScheduleBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Content(HttpStatusCode.BadRequest,
                        new ErrorResult(ErrorHelper.INVALID_BODY, ErrorHelper.GetModelErrors(ModelState)));
                }
                var result = ScheduleRepository.ReceivePayment(CurrentUser.Id, model);
                if (result is Schedule)
                {
                    return Ok(result);
                }
                else if (result is ErrorResult)
                {
                    return Content(HttpStatusCode.BadRequest, result);
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