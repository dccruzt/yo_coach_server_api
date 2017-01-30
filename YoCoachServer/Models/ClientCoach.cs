using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class ClientCoach
    {
        [Key, Column(Order = 0)]
        public string ClientId { get; set; }
        [Key, Column(Order = 1)]
        public string CoachId { get; set; }
        public string NickName { get; set; }
        public string Code { get; set; }
        public bool? IsExpired { get; set; }

        public virtual Client Client { get; set; }
        public virtual Coach Coach { get; set; }
    }
}