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
        public string CreatedAt { get; set; }
        [JsonProperty("update_at")]
        public string UpdateAt { get; set; }
    }
}