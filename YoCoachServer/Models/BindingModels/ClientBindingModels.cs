﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models.BindingModels
{
    public class ValuesBindingModel
    {
        [JsonProperty("student_type")]
        public StudentType? StudentType { get; set; }

        [JsonProperty("credits_pre_quantity")]
        public double? CreditsPreQuantity { get; set; }

        [JsonProperty("credits_post_quantity")]
        public double? CreditsPostQuantity { get; set; }

        [JsonProperty("total_amount")]
        public double? TotalAmount { get; set; }

        [JsonProperty("confirmed_amount")]
        public double? ConfirmedAmount { get; set; }

        [JsonProperty("not_confirmed_amount")]
        public double? NotConfirmedAmount { get; set; }
    }

    public class GetValuesBindingModel
    {
        [Required]
        [JsonProperty("student_id")]
        public string StudentId { get; set; }

        public string Date { get; set; }
    }
}