using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using YoCoachServer.Helpers;
using YoCoachServer.Models;
using YoCoachServer.Models.BindingModels;
using YoCoachServer.Models.Repositories;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Controllers
{
    [Authorize(Roles = "coach")]
    public class CoachController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult SaveSchedule(Schedule schedule)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Content(HttpStatusCode.BadRequest,
                        new ErrorResult(ErrorHelper.INVALID_BODY, ErrorHelper.GetModelErrors(ModelState)));
                }
                var result = CoachRepository.SaveSchedule(CurrentUser, schedule);
                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,
                    new ErrorResult(ErrorHelper.EXCEPTION, ex.StackTrace));
            }
        }

        [HttpGet]
        public IHttpActionResult ListSchedules(String date = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Content(HttpStatusCode.BadRequest,
                        new ErrorResult(ErrorHelper.INVALID_URI, ErrorHelper.GetModelErrors(ModelState)));
                }

                var schedules = CoachRepository.ListSchedules(CurrentUser.Id, date);
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,
                    new ErrorResult(ErrorHelper.EXCEPTION, ex.StackTrace));
            }
        }

        [HttpGet]
        public IHttpActionResult ListStudents()
        {
            try
            {
                var clientCoaches = CoachRepository.ListStudents(CurrentUser.Id);
                return Ok(clientCoaches);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,
                    new ErrorResult(ErrorHelper.EXCEPTION, ex.StackTrace));
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> RegisterStudent(RegisterClientBindingModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                if(CurrentUser.Id != null && CurrentUser.Type.Equals("CO"))
                {
                    var client = await CoachRepository.RegisterStudent(CurrentUser.Id, model, UserManager);
                    return Ok(client);
                }
                return InternalServerError(null);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,
                    new ErrorResult(ErrorHelper.EXCEPTION, ex.StackTrace));
            }
        }

        public IHttpActionResult ListGyms()
        {
            try
            {
                if (CurrentUser.Id != null && CurrentUser.Type.Equals("CO"))
                {
                    var gyms = GymRepository.ListGyms(CurrentUser.Id);
                    return Ok(gyms);
                }
                return InternalServerError(null);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,
                    new ErrorResult(ErrorHelper.EXCEPTION, ex.StackTrace));
            }
        }

        public IHttpActionResult SaveGym(NewGymBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (CurrentUser.Id != null && CurrentUser.Type.Equals("CO"))
                {
                    var gym = GymRepository.SaveGym(CurrentUser.Id, model);
                    if(gym != null)
                    {
                        return Ok(gym);
                    }
                }
                return InternalServerError(null);
                
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,
                    new ErrorResult(ErrorHelper.EXCEPTION, ex.StackTrace));
            }
        }
    }
}
