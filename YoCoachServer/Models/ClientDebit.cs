using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class ClientDebit
    {
        public string Id { get; set; }

        public virtual Client Client { get; set; }
        public virtual Credit Balance { get; set; }

        public virtual Schedule Schedule { get; set; }
    }
}