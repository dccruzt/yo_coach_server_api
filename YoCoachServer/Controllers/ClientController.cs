using Newtonsoft.Json;
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
    [Authorize]
    public class ClientController : BaseApiController
    {
        public IHttpActionResult ListCoaches()
        {
            try
            {
                if (CurrentUser.Id != null && CurrentUser.Type.Equals("CL"))
                {
                    var coaches = ClientRepository.ListCoaches(CurrentUser.Id);
                    return Ok(coaches);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult ListSchedules(ListSchedulesBindingModel model)
        {
            try
            {
                if (CurrentUser.Id != null && CurrentUser.Type.Equals("CL"))
                {
                    var schedules = ScheduleRepository.ListSchedulesForClient(model.coachId);
                    return Ok(schedules);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult SaveSchedule(SaveScheduleByClientBindingModel model)
        {
            try
            {
                if (CurrentUser.Id != null && CurrentUser.Type.Equals("CL"))
                {
                    var schedule = ScheduleRepository.SaveScheduleByClient(CurrentUser.Id, model);
                    return Ok(schedule);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}