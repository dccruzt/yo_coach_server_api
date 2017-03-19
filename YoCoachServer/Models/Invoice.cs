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
        [JsonProperty("amount_expend")]
        public double? AmountExpend { get; set; }

        public virtual Credit Credit { get; set; }
    }
}