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
        [JsonProperty("student_id")]
        public string StudentId { get; set; }

        [JsonProperty("gym_id")]
        public string GymId { get; set; }

        [Required]
        public Schedule Schedule { get; set; }
    }

    public class SaveScheduleByClientBindingModel
    {
        [Required]
        [JsonProperty("coach_id")]
        public string CoachId { get; set; }

        [JsonProperty("gym_id")]
        public string GymId { get; set; }

        [Required]
        public Schedule Schedule { get; set; }
    }

    public class MarkScheduleBindingModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [JsonProperty("credits_amount")]
        public double CreditsAmount { get; set; }
    }

    public class PayScheduleBindingModel
    {
        [Required]
        [JsonProperty("schedule_id")]
        public string ScheduleId { get; set; }

        [Required]
        [JsonProperty("student_id")]
        public string StudentId { get; set; }

        [Required]
        [JsonProperty("credits_amount")]
        public double CreditsAmount { get; set; }
    }
}