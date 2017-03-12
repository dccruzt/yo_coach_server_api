using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class Invoice : YoCoachObject
    {
        public string Id { get; set; }
        [JsonProperty("paid_at")]
        public string PaidAt { get; set; }
        [JsonProperty("unit_expent")]
        public double? UnitExpent { get; set; }

        public virtual Credit Credit { get; set; }
    }
}