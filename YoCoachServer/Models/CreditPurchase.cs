using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models
{
    public class CreditPurchase
    {
        public string Id { get; set; }
        [JsonProperty("unit_value")]
        public double? UnitValue { get; set; }
        [JsonProperty("total_quantity")]
        public double? TotalQuantity { get; set; }
        [JsonProperty("available_quantity")]
        public double? AvailableQuantity { get; set; }
        [JsonProperty("purchase_date")]
        public string PurchaseDate { get; set; }
        [JsonProperty("expiration_date")]
        public string ExpirationDate { get; set; }

        public virtual Gym Gym { get; set; }
        public virtual ICollection<UseOfCredit> UseOfCredits { get; set; }
    }
}