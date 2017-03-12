using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models
{
    public class Gym
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual Coach Coach { get; set; }
        public virtual Credit Credit { get; set; }
    }
}