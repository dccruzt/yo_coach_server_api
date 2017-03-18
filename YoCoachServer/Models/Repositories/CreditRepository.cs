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
        public static Credit createCreditForGym(NewGymBindingModel model)
        {
            try
            {
                //
                var credit = new Credit()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreditPolicy = model.Credit.CreditPolicy,
                    UnitValue = model.Credit.UnitValue
                };

                if (model.Credit.CreditPolicy.Equals(CreditPolicy.PRE))
                {
                    credit.ExpiresAt = model.Credit.ExpiresAt;
                }
                if (model.Credit.CreditPolicy.Equals(CreditPolicy.POST))
                {
                    credit.DayOfPayment = model.Credit.DayOfPayment;
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
                    //Id = Guid.NewGuid().ToString(),
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

        public static ClientDebit createClientDebit(string scheduleId, Client client)
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