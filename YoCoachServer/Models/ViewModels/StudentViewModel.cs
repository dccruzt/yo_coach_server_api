using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models.ViewModels
{
    public class StudentViewModel :YoCoachObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] Picture { get; set; }
        public DateTimeOffset? Birthday { get; set; }
        public string Email { get; set; }
        [JsonProperty("user_name")]
        public string UserName { get; set; }
    }
}