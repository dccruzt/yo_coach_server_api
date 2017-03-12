using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models.Repositories
{
    public class CreditRepository
    {
        public static Credit createCreditForGym(CreditPolicy creditPolicy, double unitValue, DateTime expiresAt, int dayOfPayment)
        {
            try
            {
                var credit = new Credit()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreditPolicy = creditPolicy,
                    UnitValue = unitValue
                };

                if (creditPolicy.Equals(CreditPolicy.PRE))
                {
                    credit.ExpiresAt = expiresAt.ToString();
                }
                if (creditPolicy.Equals(CreditPolicy.POST))
                {
                    credit.DayOfPayment = dayOfPayment;
                }
                return credit;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Credit createCreditForClientDebit(double unitValue, DateTime expiresAt, int dayOfPayment)
        {
            try
            {
                var credit = new Credit()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreditPolicy = CreditPolicy.POST,
                    UnitValue = 1
                };
                return credit;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}