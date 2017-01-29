﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models.BindingModels
{
    public class SaveScheduleByCoachBindingModel
    {
        
        [Required]
        [JsonProperty("coach_id")]
        public string CoachId { get; set; }

        [Required]
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("gym_id")]
        public string GymId { get; set; }

        [Required]
        public Schedule Schedule { get; set; }
    }
}