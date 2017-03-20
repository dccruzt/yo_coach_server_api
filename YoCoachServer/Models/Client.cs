using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models
{
    public class Client : YoCoachObject
    {
        public Client()
        {
            Schedules = new List<Schedule>();
            ClientCoaches = new List<ClientCoach>();
        }

        [Key, ForeignKey("User")]
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("allow_login_with_code")]
        public bool? AllowLoginWithCode { get; set; }

        [JsonIgnore]
        [JsonProperty("code_to_access")]
        public string CodeToAccess { get; set; }

        [JsonProperty("code_created_at")]
        public DateTimeOffset? CodeCreatedAt { get; set; }


        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        [JsonIgnore]
        public virtual ICollection<ClientCoach> ClientCoaches { get; set; }
    }
}