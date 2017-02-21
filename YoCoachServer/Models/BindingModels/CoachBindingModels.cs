using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models.BindingModels
{
    public class CoachBindingModels
    {
        public class RegisterClientBindingModel
        {
            [Required]
            [JsonProperty("phone_number_client")]
            public string PhoneNumberClient { get; set; }

            [JsonProperty("nick_name")]
            public string NickName { get; set; }

            [Required]
            public string Code { get; set; }
            
            public bool IsExpired { get; set; }
        }

        public class CoachClientBindingModel
        {
            [Required]
            public string Id { get; set; }

            [Required]
            [JsonProperty("nick_name")]
            public string NickName { get; set; }
        }
    }
}