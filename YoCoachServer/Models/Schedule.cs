﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models
{
    public class Schedule : YoCoachObject
    {
        public Schedule()
        {
            Clients = new List<Client>();
        }

        public string Id { get; set; }

        [JsonProperty("start_time")]
        public DateTimeOffset? StartTime { get; set; }

        [JsonProperty("end_time")]
        public DateTimeOffset? EndTime { get; set; }

        [JsonProperty("total_value")]
        public double? TotalValue { get; set; }

        [JsonProperty("is_confirmed")]
        public bool? IsConfirmed { get; set; }

        [JsonProperty("payment_state")]
        public StatePayment? PaymentState { get; set; }

        [JsonProperty("schedule_state")]
        public ScheduleState? ScheduleState { get; set; }

        [JsonProperty("credits_quantity")]
        public double? CreditsQuantity { get; set; }


        [JsonIgnore]
        public virtual Coach Coach { get; set; }
        
        public virtual Gym Gym { get; set; }

        [JsonIgnore]
        public virtual ICollection<Client> Clients { get; set; }
        
        public virtual ClientDebit ClientDebit { get; set; }
    }
}