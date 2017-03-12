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

        public static void AddGym(string coachId, NewGymBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var coach = context.Coach.Find(coachId);
                    if (coach != null)
                    {
                        var credit = new Credit()
                        {
                            Id = Guid.NewGuid().ToString(),
                            UnitValue = model.Credit.UnitValue
                        };

                        var gym = new Gym()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = model.Name,
                            Address = model.Address,
                            Credit = credit
                        };
                        coach.Gyms.Add(gym);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}