using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using YoCoachServer.Utils;

namespace YoCoachServer.Helpers
{
    public class SMSHelper
    {
        private static string accountSid = ConfigurationManager.AppSettings["AccountSid"].ToString();
        private static string authToken = ConfigurationManager.AppSettings["AuthToken"].ToString();
        private static string twilioNumber = ConfigurationManager.AppSettings["TwilioNumber"].ToString();

        public static async Task sendSms(string phoneNumber, string code)
        {
            try
            {
                TwilioClient.Init(accountSid, authToken);

                var message = await MessageResource.CreateAsync(
                    to: new PhoneNumber("+" + phoneNumber),
                    from: new PhoneNumber(twilioNumber),
                    body: StringUtils.getSMSMessage(code));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}