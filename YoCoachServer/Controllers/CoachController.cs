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
    [Authorize(Roles = COACH)]
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
                return Content(HttpStatusCode.BadRequest,
                        new ErrorResult(ErrorHelper.DATABASE_ERROR, ErrorHelper.INFO_DATABASE_ERROR));
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
        public async Task<IHttpActionResult> RegisterStudent(StudentCoach studentCoach)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return Content(HttpStatusCode.BadRequest,
                        new ErrorResult(ErrorHelper.INVALID_BODY, ErrorHelper.GetModelErrors(ModelState)));
                }

                var result = await CoachRepository.RegisterStudent(CurrentUser.Id, studentCoach, UserManager);
                if(result != null)
                {
                    if (result is StudentCoach)
                    {
                        return Ok(result);
                    }
                    else if (result is IDictionary<string, string>)
                    {
                        var error = (IDictionary<string, string>)result;
                        return Content(HttpStatusCode.BadRequest,
                            new ErrorResult(error["code"], error["message"]));
                    }
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

        [HttpGet]
        public IHttpActionResult ListGyms()
        {
            try
            {
                var gyms = GymRepository.ListGyms(CurrentUser.Id);
                return Ok(gyms);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,
                    new ErrorResult(ErrorHelper.EXCEPTION, ex.StackTrace));
            }
        }

        [HttpPost]
        public IHttpActionResult SaveGym(Gym gym)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Content(HttpStatusCode.BadRequest,
                        new ErrorResult(ErrorHelper.INVALID_BODY, ErrorHelper.GetModelErrors(ModelState)));
                }

                var result = GymRepository.SaveGym(CurrentUser.Id, gym);
                if (result != null)
                {
                    return Ok(gym);
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