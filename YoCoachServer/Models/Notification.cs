using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class YoCocahNotification
    {
        public string To { get; set; }
        public Notification Notification { get; set; }
        public string Data { get; set; }
    }

    public class Notification
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Icon { get; set; }
    }
}