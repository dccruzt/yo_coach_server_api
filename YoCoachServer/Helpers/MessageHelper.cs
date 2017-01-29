using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoCoachServer.Helpers
{
    public class MessageHelper
    {
        public static string INVALID_DESERIALIZATION = "The values of json were sent with errors or some error ocurred with the deserealization";
        public static string INVALID_CREDENTIALS = "The credentials are wrong.";
        public static string INVALID_INPUT = "The input data coud not be found.";
    }
}