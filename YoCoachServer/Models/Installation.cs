using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models
{
    public class Installation : YoCoachObject
    {
        public String Id { get; set; }

        [JsonProperty("device_type")]
        public DeviceType? DeviceType { get; set; }

        [JsonProperty("application_version")]
        public String ApplicationVersion { get; set; }

        public int? Badge { get; set; }

        [JsonProperty("device_token")]
        public String DeviceToken { get; set; }

        [JsonProperty("device_id")]
        public String DeviceId { get; set; }

        public bool Enabled { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}