using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class Client
    {
        [Key, ForeignKey("User")]
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}