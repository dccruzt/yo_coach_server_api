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
using YoCoachServer.Models.ViewModels;

namespace YoCoachServer.Models
{
    public class Schedule : YoCoachObject
    {
        public Schedule()
        {
            //Students = new List<Student>();
            StudentSchedules = new List<StudentSchedule>();
        }

        public String Id { get; set; }

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

        [JsonProperty("credits_amount")]
        public double? CreditsAmount { get; set; }

        [JsonIgnore]
        [JsonProperty("coach_id")]
        public string CoachId { get; set; }

        [JsonProperty("gym_id")]
        public string GymId { get; set; }

        [JsonIgnore]
        public virtual Coach Coach { get; set; }
        
        public virtual Gym Gym { get; set; }

        [JsonProperty("students")]
        public ICollection<StudentViewModel> StudentsViewModel { get; set; }

        [JsonIgnore]
        public virtual ICollection<StudentSchedule> StudentSchedules { get; set; }
    }
}