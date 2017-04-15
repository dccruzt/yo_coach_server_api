using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using YoCoachServer.Models;
using YoCoachServer.Models.BindingModels;
using YoCoachServer.Models.Repositories;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Controllers
{
    [Authorize]
    public class ScheduleController : BaseApiController
    {
        public IHttpActionResult MarkAsCompleted(ScheduleDetailBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if(CurrentUser != null && CurrentUser.Type.Equals("CO"))
                {
                    var schedule = ScheduleRepository.MarkAsCompleted(CurrentUser.Id, model);
                    if (schedule != null)
                    {
                        return Ok(schedule);
                    }
                }
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
                throw;
            }
        }

        public IHttpActionResult ReceivePayment(ScheduleDetailBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (CurrentUser != null && CurrentUser.Type.Equals("CO"))
                {
                    var schedule = ScheduleRepository.ReceivePayment(CurrentUser.Id, model);
                    if(schedule != null)
                    {
                        return Ok(schedule);
                    }
                }
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
                throw;
            }
        }
    }
}