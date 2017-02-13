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
    [Authorize]
    public class ScheduleController : BaseApiController
    {
        public IHttpActionResult SaveScheduleByCoach(SaveScheduleByCoachBindingModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if(CurrentUser != null && CurrentUser.Type.Equals("CO"))
            {
                var schedule = ScheduleRepository.SaveScheduleByCoach(CurrentUser.Id, model.ClientId, model.Schedule);
                if(schedule != null)
                {
                    return Ok(schedule);
                }
            }
            return GetErrorResult(null);
        }

        public IHttpActionResult ListCoachSchedules(ListCoachSchedulesBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (CurrentUser != null && CurrentUser.Type.Equals("CO"))
            {
                var schedules = ScheduleRepository.ListCoachSchedule(CurrentUser.Id, model.Date);
                return Ok(schedules);
            }
            return GetErrorResult(null);
        }
    }
}