using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models.Enums
{
    public enum StatePayment
    {
        PENDING,
        PAYED
    }

    public enum ScheduleState
    {
        SCHEDULED,
        CANCELED,
        MISSED,
        COMPLETED
    }

    public enum CreditPolicy
    {
        PRE,
        POST
    }

    public enum ClientType
    {
        MONTHLY,
        LOOSE
    }
}