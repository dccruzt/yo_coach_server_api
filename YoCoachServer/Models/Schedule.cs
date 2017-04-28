using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using YoCoachServer.Models.Attributes;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models
{
    public class Schedule : YoCoachObject
    {
        public Schedule()
        {
            Students = new List<Student>();
        }

        public String Id { get; set; }

        [JsonProperty("start_time")]
        [CheckDateRange(ErrorMessage = "Value for StartTime must be greater than today.")]
        public DateTimeOffset? StartTime { get; set; }

        [JsonProperty("end_time")]
        public DateTimeOffset? EndTime { get; set; }

        [JsonProperty("total_value")]
        [Range(10, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
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
        [JsonProperty("coach_id")]
        public string CoachId { get; set; }

        [JsonIgnore]
        [JsonProperty("gym_id")]
        public string GymId { get; set; }

        [JsonIgnore]
        public virtual Coach Coach { get; set; }
        
        public virtual Gym Gym { get; set; }

        public virtual ICollection<Student> Students { get; set; }
        
        public virtual StudentDebit StudentDebit { get; set; }
    }
}