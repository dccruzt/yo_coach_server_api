using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Models.Repositories
{
    public class CreditRepository
    {
        public static Credit createCreditForGym(Gym gym)
        {
            try
            {
                var credit = new Credit()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreditPolicy = gym.Credit.CreditPolicy,
                    UnitValue = gym.Credit.UnitValue
                };

                if (gym.Credit.CreditPolicy.Equals(CreditPolicy.PRE))
                {
                    credit.ExpiresAt = gym.Credit.ExpiresAt;
                }
                if (gym.Credit.CreditPolicy.Equals(CreditPolicy.POST))
                {
                    credit.DayOfPayment = gym.Credit.DayOfPayment;
                }
                return credit;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Credit createCreditForStudentPayment(double amount)
        {
            try
            {
                var credit = new Credit()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreditPolicy = CreditPolicy.POST,
                    Amount = amount,
                    UnitValue = 1
                };
                return credit;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static StudentSchedule createStudentPayment(string scheduleId, string studentId, double amount)
        {
            try
            {
                var studentPayment = new StudentSchedule()
                {
                    ScheduleId = scheduleId,
                    StudentId = studentId,
                    Credit = createCreditForStudentPayment(amount)
                };
                return studentPayment;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}