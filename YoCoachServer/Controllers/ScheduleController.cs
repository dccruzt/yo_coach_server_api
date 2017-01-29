using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoCoachServer.Models.BindingModels;
using YoCoachServer.Models.Repositories;

namespace YoCoachServer.Controllers
{
    public class ScheduleController : ApiController
    {
        [Authorize]
        public IHttpActionResult SaveScheduleByCoach(SaveScheduleByCoachBindingModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var schedule = ScheduleRepository.SaveScheduleByCoach(model.CoachId, model.ClientId, model.Schedule);

            if (schedule == null)
            {
                //return GetErrorResult(result);
            }

            return Ok(schedule);
        }
    }
}