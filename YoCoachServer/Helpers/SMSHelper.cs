using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
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
        private static string accountSid = "AC4972195f0c366e028f30888729676b87";
        private static string authToken = "d688a275ec1adbcf1194eb99a160292b";
        private static string twilioNumber = "+13193132419";

        public static async Task sendSms(string phoneNumber, string code)
        {
            TwilioClient.Init(accountSid, authToken);

            var message = await MessageResource.CreateAsync(
                to: new PhoneNumber("+" + phoneNumber),
                from: new PhoneNumber(twilioNumber),
                body: StringUtils.getSMSMessage(code));
        }
    }
}