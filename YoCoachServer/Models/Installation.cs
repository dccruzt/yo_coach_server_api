using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class Installation
    {
        [JsonProperty("installation_id")]
        public String InstallationId { get; set; }
        [JsonProperty("user_id")]
        public String UserId { get; set; }
        [JsonProperty("device_type")]
        public String DeviceType { get; set; }
        [JsonProperty("application_version")]
        public String ApplicationVersion { get; set; }
        public String Badge { get; set; }
        [JsonProperty("device_token")]
        public String DeviceToken { get; set; }
        [JsonProperty("device_id")]
        public String DeviceId { get; set; }
        public bool Enabled { get; set; }
    }
}