using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class StudentSchedule
    {
        [JsonProperty("schedule_id")]
        [Key, Column(Order = 0)]
        public string ScheduleId { get; set; }

        [JsonProperty("student_id")]
        [Key, Column(Order = 1)]
        public string StudentId { get; set; }

        [JsonProperty("Balance")]
        public virtual Credit Credit { get; set; }

        public virtual Schedule Schedule { get; set; }

        public virtual Student Student { get; set; }
    }
}