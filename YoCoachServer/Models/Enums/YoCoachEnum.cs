﻿using System;
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
        PENDING,
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

    public enum StudentType
    {
        MONTHLY,
        LOOSE
    }

    public enum DeviceType
    {
        ANDROID,
        IOS
    }

    public enum NotificationType
    {
        SAVE_SCHEDULE,
        CANCEL_SCHEDULE
    }
}