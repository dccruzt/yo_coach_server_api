using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class YoCoachObject
    {
        [JsonProperty("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }
        [JsonProperty("update_at")]
        public DateTimeOffset? UpdatedAt { get; set; }
        [JsonProperty("remove_at")]
        public DateTimeOffset? RemovedAt { get; set; }
    }
}