using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Helpers;
using YoCoachServer.Models.BindingModels;
using YoCoachServer.Models.Enums;
using YoCoachServer.Utils;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Models.Repositories
{
    public class ScheduleRepository
    {
        public static Schedule SaveScheduleByCoach(string coachId, SaveScheduleByCoachBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var coach = context.Coach.Find(coachId);
                    var client = context.Client.Find(model.ClientId);
                    if (coach != null && client != null)
                    {
                        var schedule = model.Schedule;
                        schedule.Id = Guid.NewGuid().ToString();
                        schedule.IsConfirmed = false;
                        schedule.PaymentState = StatePayment.PENDING;
                        schedule.ScheduleState = ScheduleState.SCHEDULED;
                        //schedule.ClientDebit = CreditRepository.createClientDebit(client);
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

        public static void MarkScheduleAsCompleted(string coachId, ScheduleDetailBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedule = context.Schedule.FirstOrDefault(x => x.Coach.CoachId.Equals(coachId) && x.Id.Equals(model.ScheduleId));
                    if (schedule != null)
                    {
                        //Change the schedule state
                        schedule.ScheduleState = ScheduleState.COMPLETED;

                        //Create Invoice for the credit of the gym
                        var credit = context.Credit.FirstOrDefault(x => x.Id.Equals(schedule.Gym.Id));
                        if(credit != null)
                        {
                            var invoice = InvoiceRepository.createInvoiceForGym(model.AmountExpend.Value);
                            credit.Invoices.Add(invoice);
                        }
                        //Increase the total credit amount
                        if (credit.Amount.HasValue)
                        {
                            credit.Amount += model.AmountExpend;
                        }
                        else
                        {
                            credit.Amount = model.AmountExpend;
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

        public static void ReceivePayment(string coachId, ScheduleDetailBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedule = context.Schedule.Find(model.ScheduleId);
                    if(schedule != null)
                    {
                        if(schedule.ClientDebit != null)
                        {
                            var invoice = InvoiceRepository.createInvoiceForClientDebit(model.AmountExpend.Value);
                            schedule.ClientDebit.Balance.Invoices.Add(invoice);

                            context.SaveChanges();
                        }
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