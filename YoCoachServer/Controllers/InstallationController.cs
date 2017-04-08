using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoCoachServer.Models;
using YoCoachServer.Models.Repositories;

namespace YoCoachServer.Controllers
{
    public class InstallationController : BaseApiController
    {
        public IHttpActionResult Register(Installation installation)
        {
            try
            {
                if(CurrentUser.Id != null)
                {
                    var installationObj = InstallationRepository.Register(CurrentUser, installation);
                    if (installationObj != null)
                    {
                        return Ok(installationObj);
                    }
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