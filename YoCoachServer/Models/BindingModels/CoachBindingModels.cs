using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models.BindingModels
{
    public class CoachBindingModels
    {
        public class RegisterClientBindingModel
        {
            [Required]
            [JsonProperty("phone_number")]
            public string PhoneNumber { get; set; }

            [Required]
            [JsonProperty("nick_name")]
            public string NickName { get; set; }

            [Required]
            [JsonProperty("client_type")]
            public ClientType ClientType { get; set; }

            public string Email { get; set; }

            public string Code { get; set; }
            
            public bool IsExpired { get; set; }

            public int? Age { get; set; }
        }

        public class ClientBindingModel
        {
            [Required]
            public string Id { get; set; }

            [Required]
            [JsonProperty("nick_name")]
            public string NickName { get; set; }

            [Required]
            [JsonProperty("phone_number")]
            public string PhoneNumber { get; set; }

            [Required]
            [JsonProperty("client_type")]
            public ClientType? ClientType { get; set; }

            public byte[] Picture { get; set; }

            public int? Age { get; set; }

            public string Email { get; set; }
        }

        public class NewGymBindingModel
        {
            [Required]
            public string Name { get; set; }
            
            public string Address { get; set; }

            [Required]
            public Credit Credit { get; set; }
        }

        public class NewCreditBindingModel
        {
            [Required]
            public Credit Credit { get; set; }
        }

        public class ScheduleDetailBindingModel
        {
            [JsonProperty("schedule_id")]
            public string ScheduleId { get; set; }

            [JsonProperty("amount_expend")]
            public double AmountExpend { get; set; }
        }
    }
}