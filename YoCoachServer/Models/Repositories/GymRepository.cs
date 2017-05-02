using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Models.Repositories
{
    public class GymRepository : BaseRepository
    {
        public static List<Gym> ListGyms(string coachId)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    return context.Gym.Where(x => x.Coach.Id.Equals(coachId)).Include(CREDIT).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Gym SaveGym(string coachId, Gym gym)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var coach = context.Coach.Find(coachId);
                    if (coach != null)
                    {
                        var credit = CreditRepository.createCreditForGym(gym);

                        gym.Id = Guid.NewGuid().ToString();
                        gym.Credit = credit;
                        gym.CreatedAt = DateTimeOffset.Now;
                        gym.UpdatedAt = DateTimeOffset.Now;

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