﻿using Microsoft.AspNet.Identity.Owin;
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
using YoCoachServer.Models.Enums;
using YoCoachServer.Models.Repositories;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Controllers
{
    [Authorize(Roles = COACH_STUDENT)]
    public class ScheduleController : BaseApiController
    {
        [Route("api/schedule/")]
        public IHttpActionResult Post(SaveScheduleBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Content(HttpStatusCode.BadRequest,
                        new ErrorResult(ErrorHelper.INVALID_BODY, ErrorHelper.GetModelErrors(ModelState)));
                }
                var result = ScheduleRepository.SaveSchedule(CurrentUser, model);
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

        [Route("api/schedule/{id}")]
        public IHttpActionResult Put(String id, Schedule schedule)
        {
            try
            {
                var result = ScheduleRepository.UpdateSchedule(id, schedule, CurrentUser.Id);
                if(result is Schedule)
                {
                    return Ok();
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