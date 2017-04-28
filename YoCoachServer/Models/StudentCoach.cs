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
    public class StudentCoach
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

        public string Code { get; set; }

        [JsonProperty("is_expired")]
        public bool? IsExpired { get; set; }

        [JsonProperty("client_type")]
        public StudentType? StudentType { get; set; }

        public virtual Student Student { get; set; }
        public virtual Coach Coach { get; set; }
    }
}