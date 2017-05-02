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
        public DateTimeOffset? PaidAt { get; set; }
        [JsonProperty("credits_amount")]
        public double? CreditsAmount { get; set; }

        public virtual Credit Credit { get; set; }
    }
}