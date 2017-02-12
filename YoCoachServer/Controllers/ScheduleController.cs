using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using YoCoachServer.Models;
using YoCoachServer.Models.BindingModels;
using YoCoachServer.Models.Repositories;

namespace YoCoachServer.Controllers
{
    public class ScheduleController : BaseApiController
    {
        [Authorize]
        public IHttpActionResult SaveScheduleByCoach(SaveScheduleByCoachBindingModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser user = CurrentUser;
            if(user != null && user.Type.Equals("CO"))
            {
                var schedule = ScheduleRepository.SaveScheduleByCoach(user.Id, model.ClientId, model.Schedule);
                return Ok(schedule);
            }

            //if (schedule == null)
            //{
            //return GetErrorResult(result);
            //}

            return Ok();
        }

        public IHttpActionResult ListCoachSchedules(ListCoachSchedulesBindingModel model)
        {
            var schedules = ScheduleRepository.ListCoachSchedule("s", model.Date);
            return Ok(schedules);
        }
    }
}