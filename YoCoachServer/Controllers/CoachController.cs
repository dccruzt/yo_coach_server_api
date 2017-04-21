using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using YoCoachServer.Helpers;
using YoCoachServer.Models.BindingModels;
using YoCoachServer.Models.Repositories;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Controllers
{
    [Authorize]
    public class CoachController : BaseApiController
    {
        public IHttpActionResult SaveSchedule(SaveScheduleByCoachBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //ApplicationUser current = await UserManager.FindByNameAsync(Thread.CurrentPrincipal.Identity.Name);

                if (CurrentUser != null && CurrentUser.Type.Equals("CO"))
                {
                    var schedule = ScheduleRepository.SaveScheduleByCoach(CurrentUser.Id, model);
                    if (schedule != null)
                    {
                        return Ok(schedule);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult ListSchedules(ListCoachSchedulesBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (CurrentUser != null && CurrentUser.Type.Equals("CO"))
                {
                    var schedules = ScheduleRepository.ListCoachSchedule(CurrentUser.Id, model);
                    return Ok(schedules);
                }
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult ListClients()
        {
            try
            {
                var clientCoaches = CoachRepository.ListClients(CurrentUser.Id, UserManager);
                return Ok(clientCoaches);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public async Task<IHttpActionResult> RegisterClient(RegisterClientBindingModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                if(CurrentUser.Id != null && CurrentUser.Type.Equals("CO"))
                {
                    var client = await CoachRepository.RegisterClient(CurrentUser.Id, model, UserManager);
                    return Ok(client);
                }
                return InternalServerError(null);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
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
                return InternalServerError(ex);
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
                return InternalServerError(ex);
            }
        }
    }
}
