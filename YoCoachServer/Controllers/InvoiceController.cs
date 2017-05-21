using Microsoft.AspNet.Identity.Owin;
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
using YoCoachServer.Models.Enums;
using YoCoachServer.Models.Repositories;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Controllers
{
    //[Authorize(Roles = COACH)]
    public class InvoiceController : BaseApiController
    {
        [Route("api/invoice/")]
        public IHttpActionResult Get([FromUri(Name = "schedule_id")]String scheduleId = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Content(HttpStatusCode.BadRequest,
                        new ErrorResult(ErrorHelper.INVALID_URI, ErrorHelper.GetModelErrors(ModelState)));
                }

                var schedules = InvoiceRepository.ListInvoices(scheduleId);
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError,
                    new ErrorResult(ErrorHelper.EXCEPTION, ex.StackTrace));
            }
        }
    }
}