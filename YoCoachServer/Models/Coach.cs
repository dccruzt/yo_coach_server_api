﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class Coach : YoCoachObject
    {
        public Coach()
        {
            Schedules = new List<Schedule>();
            Gyms = new List<Gym>();
        }

        [Key, ForeignKey("User")]
        public string Id { get; set; }
        [JsonProperty("time_to_cancel")]
        public int? TimeToCancel { get; set; }
        [JsonProperty("is_visible_for_clients")]
        public bool? IsVisibleForClients { get; set; }
        [JsonProperty("has_penality")]
        public bool? HasPenality { get; set; }
        [JsonProperty("penalty_percent")]
        public Double? PenalityPercent { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Gym> Gyms { get; set; }
        [JsonIgnore]
        public virtual ICollection<StudentCoach> StudentCoaches { get; set; }
    }
}