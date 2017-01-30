using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Helpers;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models.Repositories
{
    public class ScheduleRepository
    {
        public static Schedule SaveScheduleByCoach(string coachId, string clientId, Schedule schedule)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var coach = context.Coach.Find(coachId);
                    var client = context.Client.Find(clientId);
                    if (coach != null && client != null)
                    {
                        schedule.Id = Guid.NewGuid().ToString();
                        schedule.IsConfirmed = false;
                        schedule.PaymentState = StatePayment.PENDING;
                        schedule.ScheduleState = StateSchedule.SCHEDULED;
                        schedule.Coach = coach;
                        schedule.Clients.Add(client);
                        context.Schedule.Add(schedule);

                        context.SaveChanges();
                        schedule = JsonHelper.parseScheduleWithoutObjects(schedule);
                        return schedule;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<Schedule> ListCoachSchedule(string coachId, string date)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedules = context.Schedule.Where(x => x.Coach.CoachId.Equals(coachId)).ToList();
                    if (schedules != null)
                    {
                        foreach (var schedule in schedules)
                        {
                            DateTimeOffset dto = JsonConvert.DeserializeObject<DateTimeOffset>(schedule.StartTime);
                            DateTime utc = dto.UtcDateTime;

                        }
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