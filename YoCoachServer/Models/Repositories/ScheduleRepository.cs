using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                    var coach = context.Coach.Where(x => x.Id.Equals(coachId)).Include("User").ToList().FirstOrDefault();
                    var client = context.Client.Find(model.ClientId);
                    var gym = context.Gym.Find(model.GymId);
                    if (coach != null && client != null && gym != null)
                    {
                        Schedule schedule = null;
                        if (model.Schedule.Id != null)
                        {
                            schedule = context.Schedule.Find(model.Schedule.Id);
                        }
                        else
                        {
                            schedule = model.Schedule;
                            schedule.Id = Guid.NewGuid().ToString();
                            schedule.CreatedAt = DateTimeOffset.Now;
                        }
                        schedule.IsConfirmed = true;
                        schedule.PaymentState = StatePayment.PENDING;
                        schedule.ScheduleState = ScheduleState.SCHEDULED;
                        schedule.UpdateAt = DateTimeOffset.Now;
                        schedule.Coach = coach;
                        schedule.Clients.Add(client);
                        schedule.Gym = gym;
                        context.Schedule.Add(schedule);

                        context.SaveChanges();
                        schedule = JsonHelper.parseScheduleWithoutObjects(schedule);

                        var installations = InstallationRepository.getInstallations(client.Id);
                        if (installations != null)
                        {
                            foreach (var installation in installations)
                            {
                                var notification = NotificationRepository.CreateNotificationForSaveSchedule(
                                    installation.DeviceToken,
                                    coach.User.Name + " " + NotificationMessage.NEW_SCHEDULE_TITLE,
                                    NotificationMessage.NEW_SCHEDULE_BODY,
                                    NotificationType.SAVE_SCHEDULE);

                                NotificationHelper.SendNotfication(notification);
                            }
                        }
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

        public static Schedule SaveScheduleByClient(string clientId, SaveScheduleByClientBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var client = context.Client.Where(x => x.Id.Equals(clientId)).Include("User").ToList().FirstOrDefault();
                    var coach = context.Coach.Find(model.CoachId);
                    var gym = context.Gym.Find(model.GymId);
                    if (coach != null && client != null && gym != null)
                    {
                        var schedule = model.Schedule;
                        schedule.Id = Guid.NewGuid().ToString();
                        schedule.IsConfirmed = false;
                        schedule.PaymentState = StatePayment.PENDING;
                        schedule.ScheduleState = ScheduleState.SCHEDULED;
                        schedule.CreatedAt = DateTimeOffset.Now;
                        schedule.UpdateAt = DateTimeOffset.Now;
                        schedule.Coach = coach;
                        schedule.Clients.Add(client);
                        schedule.Gym = gym;
                        context.Schedule.Add(schedule);

                        context.SaveChanges();
                        schedule = JsonHelper.parseScheduleWithoutObjects(schedule);

                        var installations = InstallationRepository.getInstallations(coach.Id);
                        if(installations != null)
                        {
                            foreach(var installation in installations)
                            {
                                var notification = NotificationRepository.CreateNotificationForSaveSchedule(
                                    installation.DeviceToken,
                                    client.User.Name + " " + NotificationMessage.NEW_SCHEDULE_TITLE,
                                    NotificationMessage.NEW_SCHEDULE_BODY,
                                    NotificationType.SAVE_SCHEDULE);

                                NotificationHelper.SendNotfication(notification);
                            }
                        }
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

        public static List<Schedule> ListCoachSchedule(string coachId, ListCoachSchedulesBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedules = context.Schedule.Where(x => x.Coach.Id.Equals(coachId)).Include("Gym").Include("Clients").ToList();
                    if (model != null)
                    {
                        if (schedules != null)
                        {
                            var schedulesByDay = new List<Schedule>();
                            foreach (var schedule in schedules)
                            {

                                DateTimeOffset dateStart = (schedule.StartTime.HasValue) ? schedule.StartTime.Value : new DateTimeOffset();
                                if (DateUtils.SameDate(dateStart, model.Date.Value))
                                {
                                    schedulesByDay.Add(schedule);
                                }
                            }
                            return schedulesByDay;
                        }
                    }
                    return schedules;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<Schedule> ListSchedulesForClient(string coachId)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedules = context.Schedule.Where(x => x.Coach.Id.Equals(coachId)).Include("Gym").ToList();
                    return schedules;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Schedule MarkAsCompleted(string coachId, ScheduleDetailBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedule = context.Schedule.Where(x => x.Coach.Id.Equals(coachId) && x.Id.Equals(model.ScheduleId)).Include("Gym").FirstOrDefault();
                    if (schedule != null)
                    {
                        //Change the schedule state
                        schedule.ScheduleState = ScheduleState.COMPLETED;
                        schedule.UpdateAt = DateTimeOffset.Now;

                        //Verify if the schedule has gym
                        if (schedule.Gym != null)
                        {
                            var gym = context.Gym.Where(x => x.Id.Equals(schedule.Gym.Id)).Include("Credit").FirstOrDefault();
                            if(gym != null && gym.Credit != null)
                            {
                                var credit = context.Credit.Where(x => x.Id.Equals(gym.Credit.Id)).Include("Invoices").FirstOrDefault();
                                if (credit != null)
                                {
                                    //Create Invoice for the credit of the gym
                                    var invoice = InvoiceRepository.createInvoiceForGym(model.AmountExpend);
                                    credit.Invoices.Add(invoice);

                                    //Increase the total credit amount
                                    if (credit.Amount.HasValue)
                                    {
                                        credit.Amount += model.AmountExpend;
                                    }
                                    else
                                    {
                                        credit.Amount = model.AmountExpend;
                                    }
                                }
                            }
                        }
                        context.SaveChanges();
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

        public static Schedule ReceivePayment(string coachId, ScheduleDetailBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedule = context.Schedule.Where(x => x.Id.Equals(model.ScheduleId)).Include("ClientDebit").Include("Clients").FirstOrDefault();
                    if(schedule != null)
                    {
                        schedule.UpdateAt = DateTimeOffset.Now;
                        //if not exist a previous payment
                        if(schedule.ClientDebit == null)
                        {
                            if (schedule.Clients != null && schedule.Clients.Count != 0)
                            {
                                var clientDebit = CreditRepository.createClientDebit(schedule.Clients.First(), model.AmountExpend);
                                schedule.ClientDebit = clientDebit;
                                var invoice = InvoiceRepository.createInvoiceForClientDebit(model.AmountExpend);
                                schedule.ClientDebit.Balance.Invoices.Add(invoice);
                            }
                        }
                        else
                        {
                            var clientDebit = context.ClientDebit.Where(x => x.Id.Equals(schedule.Id)).Include("Balance").FirstOrDefault();
                            if(clientDebit != null && clientDebit.Balance != null)
                            {
                                var balance = context.Credit.Where(x => x.Id.Equals(clientDebit.Balance.Id)).Include("Invoices").FirstOrDefault();
                                if(balance != null)
                                {
                                    balance.Amount += model.AmountExpend;
                                    var invoice = InvoiceRepository.createInvoiceForClientDebit(model.AmountExpend);
                                    balance.Invoices.Add(invoice);
                                }
                            }
                        }
                        context.SaveChanges();
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
    }
}