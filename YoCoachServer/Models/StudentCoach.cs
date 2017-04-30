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
    public class StudentCoach : YoCoachObject
    {
        [JsonProperty("Id")]
        [Key, Column(Order = 0)]
        public string StudentId { get; set; }

        [JsonIgnore]
        [Key, Column(Order = 1)]
        public string CoachId { get; set; }
        
        public string Name { get; set; }

        public string Email { get; set; }
        
        public DateTimeOffset? Birthday { get; set; }

        [JsonProperty("student_type")]
        public StudentType? StudentType { get; set; }

        [JsonProperty("phone_number")]
        public String PhoneNumber { get; set; }

        [JsonIgnore]
        public virtual Student Student { get; set; }

        [JsonIgnore]
        public virtual Coach Coach { get; set; }
    }
}