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
        public static Schedule SaveScheduleByStudent(string clientId, SaveScheduleByClientBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var student = context.Student.Where(x => x.Id.Equals(clientId)).Include("User").ToList().FirstOrDefault();
                    var coach = context.Coach.Find(model.CoachId);
                    var gym = context.Gym.Find(model.GymId);
                    if (coach != null && student != null && gym != null)
                    {
                        var schedule = model.Schedule;
                        schedule.Id = Guid.NewGuid().ToString();
                        schedule.IsConfirmed = false;
                        schedule.PaymentState = StatePayment.PENDING;
                        schedule.ScheduleState = ScheduleState.SCHEDULED;
                        schedule.CreatedAt = DateTimeOffset.Now;
                        schedule.UpdatedAt = DateTimeOffset.Now;
                        schedule.Coach = coach;
                        schedule.Students.Add(student);
                        schedule.Gym = gym;
                        context.Schedule.Add(schedule);

                        context.SaveChanges();

                        var installations = CoachRepository.getInstallations(schedule.Coach);
                        if(installations != null)
                        {
                            foreach(var installation in installations)
                            {
                                var notification = NotificationRepository.CreateNotificationForSaveSchedule(
                                    installation.DeviceToken,
                                    student.User.Name + " " + NotificationMessage.NEW_SCHEDULE_TITLE,
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
                        schedule.UpdatedAt = DateTimeOffset.Now;

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
                    var schedule = context.Schedule.Where(x => x.Id.Equals(model.ScheduleId)).Include("StudentDebit").Include("Students").FirstOrDefault();
                    if(schedule != null)
                    {
                        schedule.UpdatedAt = DateTimeOffset.Now;
                        //if not exist a previous payment
                        if(schedule.StudentDebit == null)
                        {
                            if (schedule.Students != null && schedule.Students.Count != 0)
                            {
                                var clientDebit = CreditRepository.createClientDebit(schedule.Students.First(), model.AmountExpend);
                                schedule.StudentDebit = clientDebit;
                                var invoice = InvoiceRepository.createInvoiceForClientDebit(model.AmountExpend);
                                schedule.StudentDebit.Balance.Invoices.Add(invoice);
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