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
    public class Client
    {
        public Client()
        {
            Schedules = new List<Schedule>();
            ClientCoaches = new List<ClientCoach>();
        }

        [Key, ForeignKey("User")]
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        [JsonProperty("client_type")]
        public ClientType? ClientType { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<ClientCoach> ClientCoaches { get; set; }
    }
}