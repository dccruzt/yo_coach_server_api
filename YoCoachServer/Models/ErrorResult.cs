using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class ErrorResult
    {
        public String Code { get; set; }
        public String Detail { get; set; }

        public ErrorResult(String Code, String Detail)
        {
            this.Code = Code;
            this.Detail = Detail;
        }
    }
}