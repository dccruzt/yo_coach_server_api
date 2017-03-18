﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class ClientDebit
    {
        [Key, ForeignKey("Schedule")]
        public string Id { get; set; }

        public virtual Client Client { get; set; }
        public virtual Credit Balance { get; set; }

        public virtual Schedule Schedule { get; set; }
    }
}