using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
                var clients = CoachRepository.ListClients(CurrentUser.Id);
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
                    await CoachRepository.RegisterClient(CurrentUser.Id, model, UserManager);
                    return Ok();
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
                return Ok();
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
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
