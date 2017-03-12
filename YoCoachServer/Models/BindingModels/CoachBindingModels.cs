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
            [JsonProperty("phone_number_client")]
            public string PhoneNumberClient { get; set; }

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
            public NewCreditBindingModel Credit { get; set; }
        }

        public class NewCreditBindingModel
        {
            [Required]
            [JsonProperty("credit_policy")]
            public CreditPolicy? CreditPolicy { get; set; }

            [Required]
            [JsonProperty("unit_value")]
            public double? UnitValue { get; set; }

            [JsonProperty("expires_at")]
            public string ExpiresAt { get; set; }

            [JsonProperty("day_of_payment")]
            public int? DayOfPayment { get; set; }
        }

        public class MarkScheduleBindingModel
        {
            [JsonProperty("schedule_id")]
            public string ScheduleId { get; set; }

            public double? Amount { get; set; }
        }
    }
}