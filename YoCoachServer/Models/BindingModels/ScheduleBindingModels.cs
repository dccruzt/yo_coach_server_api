using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Attributes;

namespace YoCoachServer.Models.BindingModels
{
    public class SaveScheduleBindingModel
    {
        [Required]
        [JsonProperty("start_time")]
        [CheckDateRange(ErrorMessage = "Value for StartTime must be greater than today.")]
        public DateTimeOffset? StartTime { get; set; }

        [Required]
        [JsonProperty("end_time")]
        public DateTimeOffset? EndTime { get; set; }

        [Required]
        [JsonProperty("total_value")]
        [Range(10, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double? TotalValue { get; set; }

        [Required]
        [JsonProperty("gym_id")]
        public string GymId { get; set; }

        [Required]
        public virtual List<Student> Students { get; set; }
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