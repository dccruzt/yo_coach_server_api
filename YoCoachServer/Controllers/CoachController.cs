using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using YoCoachServer.Models.BindingModels;
using YoCoachServer.Models.Repositories;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Controllers
{
    [Authorize]
    public class CoachController : BaseApiController
    {
        public IHttpActionResult ListClients()
        {
            try
            {
                var clients = CoachRepository.ListClients(CurrentUser.Id, UserManager);
                return Ok(clients);
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

        public IHttpActionResult AddGym(NewGymBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (CurrentUser.Id != null && CurrentUser.Type.Equals("CO"))
                {
                    var gym = GymRepository.AddGym(CurrentUser.Id, model);
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

        public IHttpActionResult FetchClientValues(GetValuesBindingModel model)
        {
            try
            {
                if (CurrentUser.Id != null && CurrentUser.Type.Equals("CO"))
                {
                    ValuesBindingModel valuesModel = ClientRepository.FetchClientValues(CurrentUser.Id, model);
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
