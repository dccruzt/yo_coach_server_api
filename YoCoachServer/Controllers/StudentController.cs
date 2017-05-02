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
    public class StudentController : BaseApiController
    {
        public IHttpActionResult ListCoaches()
        {
            try
            {
                if (CurrentUser.Id != null && CurrentUser.Type.Equals("CL"))
                {
                    var coaches = StudentRepository.ListCoaches(CurrentUser.Id);
                    return Ok(coaches);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult ListSchedules(String date = null)
        {
            try
            {
                if (CurrentUser.Id != null && CurrentUser.Type.Equals("CL"))
                {
                    var schedules = StudentRepository.ListSchedules(date);
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
                    var schedule = ScheduleRepository.SaveScheduleByStudent(CurrentUser.Id, model);
                    return Ok(schedule);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult FetchValues(GetValuesBindingModel model)
        {
            try
            {
                if (CurrentUser.Id != null && CurrentUser.Type.Equals("CO"))
                {
                    ValuesBindingModel valuesModel = StudentRepository.FetchClientValues(CurrentUser.Id, model);
                    return Ok(valuesModel);
                }
                return InternalServerError(null);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}