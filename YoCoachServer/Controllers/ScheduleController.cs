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

        public IHttpActionResult ListCoachSchedules()
        {
            var schedules = ScheduleRepository.ListCoachSchedule("67dcb4ee-4397-42ac-b682-1a3c0d1c18b4", "2012-04-23T18:25:43.511Z");
            return Ok();
        }
    }
}