using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Models.Repositories
{
    public class GymRepository
    {
        public static List<Gym> ListGyms(string coachId)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    return context.Gym.Where(x => x.Coach.CoachId.Equals(coachId)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Credit CreateCredit(NewCreditBindingModel model)
        {
            try
            {
                var credit = new Credit();
                credit.CreditPolicy = model.CreditPolicy;
                credit.UnitValue = model.UnitValue;
                
                if (model.CreditPolicy.Equals(CreditPolicy.PRE))
                {
                    credit.ExpiresAt = model.ExpiresAt;
                }
                if (model.CreditPolicy.Equals(CreditPolicy.POST))
                {
                    credit.DayOfPayment = model.DayOfPayment;
                }
                return credit;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Gym AddGym(string coachId, NewGymBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var coach = context.Coach.Find(coachId);
                    if (coach != null)
                    {
                        
                        var gym = new Gym()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = model.Name,
                            Address = model.Address,
                            Credit = CreateCredit(model.Credit)
                        };
                        coach.Gyms.Add(gym);
                        context.SaveChanges();
                        gym.Coach = null;
                        return gym;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}