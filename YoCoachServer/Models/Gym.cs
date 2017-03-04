using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models
{
    public class Gym
    {
        public Gym()
        {
            CreditPurchases = new List<CreditPurchase>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [JsonProperty("credit_type")]
        public CreditType? CreditType { get; set; }

        public virtual Coach Coach { get; set; }
        public virtual ICollection<CreditPurchase> CreditPurchases { get; set; }
    }
}