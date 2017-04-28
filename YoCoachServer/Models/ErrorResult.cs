using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models
{
    public class ErrorResult
    {
        public String Code { get; set; }
        public String Message { get; set; }

        public ErrorResult(String code, String message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}