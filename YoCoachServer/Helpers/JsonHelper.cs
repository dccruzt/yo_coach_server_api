using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models;

namespace YoCoachServer.Helpers
{
    public class JsonHelper
    {
        public static Schedule parseScheduleWithoutObjects(Schedule schedule)
        {
            if (schedule.Coach != null)
            {
                schedule.Coach = null;
            }
            if (schedule.Clients != null)
            {
                schedule.Clients = null;
            }
            return schedule;
        }
    }
}