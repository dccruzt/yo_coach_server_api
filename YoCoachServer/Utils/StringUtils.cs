using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using YoCoachServer.Helpers;

namespace YoCoachServer.Utils
{
    public class StringUtils
    {
        private static string LINk_APP = "https://play.google.com/store/apps/details?id=upc.edu.pe.desaplg";

        public static string GenerateCode()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                GenerateCode();
            }
            return r;
        }

        public static string getSMSMessage(string code)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(MessageHelper.MSG_REGISTER_CLIENT, LINk_APP, code);

            return builder.ToString();
        }
    }
}