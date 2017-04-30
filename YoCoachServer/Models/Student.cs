using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models
{
    public class Student : YoCoachObject
    {
        public Student()
        {
            Schedules = new List<Schedule>();
            StudentCoaches = new List<StudentCoach>();
        }

        [Key, ForeignKey("User")]
        public string Id { get; set; }

        [JsonIgnore]
        [JsonProperty("allow_login_with_code")]
        public bool? AllowLoginWithCode { get; set; }

        [JsonIgnore]
        [JsonProperty("code_to_access")]
        public string Code { get; set; }

        [JsonIgnore]
        [JsonProperty("code_created_at")]
        public DateTimeOffset? CodeCreatedAt { get; set; }

        public virtual ApplicationUser User { get; set; }

        [JsonIgnore]
        public virtual ICollection<Schedule> Schedules { get; set; }
        [JsonIgnore]
        public virtual ICollection<StudentCoach> StudentCoaches { get; set; }
    }
}