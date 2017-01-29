using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class UseOfCredit
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public double? Quantity { get; set; }
    }
}