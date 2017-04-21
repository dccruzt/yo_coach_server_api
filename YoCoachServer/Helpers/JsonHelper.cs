using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models;
using YoCoachServer.Utils;

namespace YoCoachServer.Helpers
{
    public class JsonHelper
    {
        public static JsonSerializerSettings settings;

        public static void configureSettings()
        {
            settings = new JsonSerializerSettings();
            settings.ContractResolver = new JsonLowerCaseUnderscoreContractResolver();
        }

        public static Schedule parseScheduleWithoutObjects(Schedule schedule)
        {
            if (schedule.Coach != null)
            {
                schedule.Coach = null;
            }
            return schedule;
        }

        public static String serializeObject(Object obj)
        {
            try
            {
                configureSettings();
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
                return json;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}