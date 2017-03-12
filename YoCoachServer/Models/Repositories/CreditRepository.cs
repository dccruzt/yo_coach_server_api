using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models.Repositories
{
    public class CreditRepository
    {
        public static Credit createCreditForGym(CreditPolicy creditPolicy, double unitValue, string expiresAt, int dayOfPayment)
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
                    credit.ExpiresAt = expiresAt;
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

        public static Credit createCreditForClientDebit()
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

        public static ClientDebit createClientDebit(Client client)
        {
            try
            {
                var clientDebit = new ClientDebit()
                {
                    Id = Guid.NewGuid().ToString(),
                    Balance = createCreditForClientDebit(),
                    Client = client
                };
                return clientDebit;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}