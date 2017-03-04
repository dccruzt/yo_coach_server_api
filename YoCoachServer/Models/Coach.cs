using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class Coach
    {
        public Coach()
        {
            Schedules = new List<Schedule>();
            Gyms = new List<Gym>();
        }

        [Key, ForeignKey("User")]
        [JsonProperty("coach_id")]
        public string CoachId { get; set; }
        [JsonProperty("time_to_cancel")]
        public int? TimeToCancel { get; set; }
        [JsonProperty("is_visible_for_clients")]
        public bool? IsVisibleForClients { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Gym> Gyms { get; set; }
        public virtual ICollection<ClientCoach> ClientCoaches { get; set; }
    }
}