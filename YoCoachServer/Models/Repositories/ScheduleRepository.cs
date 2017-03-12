using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Helpers;
using YoCoachServer.Models.Enums;
using YoCoachServer.Utils;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

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
                        schedule.ScheduleState = ScheduleState.SCHEDULED;
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

        public static List<Schedule> ListCoachSchedule(string coachId, DateTimeOffset date)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedules = context.Schedule.Where(x => x.Coach.CoachId.Equals(coachId)).ToList();
                    if (schedules != null)
                    {
                        var schedulesByDay = new List<Schedule>();
                        foreach (var schedule in schedules)
                        {
                            
                            DateTimeOffset datea = (schedule.StartTime.HasValue) ? schedule.StartTime.Value : new DateTimeOffset();
                            if (DateUtils.SameDate(datea, date))
                            {
                                schedulesByDay.Add(schedule);
                            }
                        }
                        return schedulesByDay;
                    }
                    return schedules;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void MarkScheduleAsCompleted(string coachId, MarkScheduleBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedule = context.Schedule.FirstOrDefault(x => x.Coach.CoachId.Equals(coachId) && x.Id.Equals(model.ScheduleId));
                    if (schedule != null)
                    {
                        schedule.ScheduleState = ScheduleState.COMPLETED;

                        var invoice = new Invoice()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CreatedAt = DateTime.Now.ToString(),
                            UpdateAt = DateTime.Now.ToString(),
                            UnitExpent = model.UnitExpent
                        };

                        //var credit = context.Credit.FirstOrDefault(x => x.);

                        if (schedule.Gym.Credit.Amount.HasValue)
                        {
                            schedule.Gym.Credit.Amount += model.UnitExpent;
                        }
                        else
                        {
                            schedule.Gym.Credit.Amount = model.UnitExpent;
                        }
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