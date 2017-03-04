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

            public string Code { get; set; }
            
            public bool IsExpired { get; set; }
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
            [JsonProperty("nick_name")]
            public string Name { get; set; }

            [Required]
            [JsonProperty("nick_name")]
            public string Address { get; set; }

            [Required]
            [JsonProperty("credit_type")]
            public CreditType? CreditType { get; set; }

            [Required]
            [JsonProperty("credit_purchase")]
            public NewCreditPurchaseBindingModel CreditPurchase { get; set; }
        }

        public class NewCreditPurchaseBindingModel
        {
            [Required]
            [JsonProperty("unit_value")]
            public double? UnitValue { get; set; }

            [Required]
            [JsonProperty("total_quantity")]
            public double? TotalQuantity { get; set; }

            [Required]
            [JsonProperty("purchase_date")]
            public string PurchaseDate { get; set; }

            [JsonProperty("expiration_date")]
            public string ExpirationDate { get; set; }
        }
    }
}