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
    public class ScheduleRepository : BaseRepository
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

                        var installations = InstallationRepository.getInstallations(schedule.Coach.Id);
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

        public static Object MarkAsCompleted(string scheduleId, double? creditsAmount)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedule = context.Schedule.Where(x => x.Id.Equals(scheduleId)).Include(GYM).FirstOrDefault();
                    if (schedule != null)
                    {
                        //Change the schedule state
                        schedule.ScheduleState = ScheduleState.COMPLETED;
                        schedule.UpdatedAt = DateTimeOffset.Now;

                        //Verify if the schedule has gym
                        if (schedule.Gym != null)
                        {
                            var gym = context.Gym.Where(x => x.Id.Equals(schedule.Gym.Id)).Include(CREDIT).FirstOrDefault();
                            if(gym != null && gym.Credit != null)
                            {
                                var credit = context.Credit.Where(x => x.Id.Equals(gym.Credit.Id)).Include(INVOICES).FirstOrDefault();
                                if (credit != null)
                                {
                                    //Create Invoice for the credit of the gym
                                    var invoice = InvoiceRepository.createInvoiceForGym(creditsAmount);
                                    credit.Invoices.Add(invoice);

                                    //Increase the total credit amount
                                    if (credit.Amount.HasValue)
                                    {
                                        credit.Amount += creditsAmount;
                                    }
                                    else
                                    {
                                        credit.Amount = creditsAmount;
                                    }
                                    context.SaveChanges();
                                    return schedule;
                                }
                            }
                        }
                    }
                    return new ErrorResult(ErrorHelper.DATABASE_ERROR, ErrorHelper.INFO_DATABASE_ERROR);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Object ReceivePayment(string coachId, PayScheduleBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var studentPayment = context.StudentPayment.FirstOrDefault(x => x.ScheduleId.Equals(model.ScheduleId) && x.StudentId.Equals(model.StudentId));
                    if(studentPayment != null)
                    {
                        //studentPayment.UpdatedAt = DateTimeOffset.Now;
                        //if not exist a previous payment
                        if(studentPayment == null)
                        {
                            studentPayment = CreditRepository.createStudentPayment(model.ScheduleId, model.StudentId, model.CreditsAmount);
                            var invoice = InvoiceRepository.createInvoiceForStudentPayment(model.CreditsAmount);
                            studentPayment.Credit.Invoices.Add(invoice);
                        }
                        else
                        {
                            if(studentPayment != null && studentPayment.Credit != null)
                            {
                                var balance = context.Credit.Where(x => x.Id.Equals(studentPayment.Credit.Id)).Include(INVOICES).FirstOrDefault();
                                if(balance != null)
                                {
                                    balance.Amount += model.CreditsAmount;
                                    var invoice = InvoiceRepository.createInvoiceForStudentPayment(model.CreditsAmount);
                                    balance.Invoices.Add(invoice);
                                }
                            }
                        }
                        context.SaveChanges();
                        return studentPayment;
                    }
                    return new ErrorResult(ErrorHelper.DATABASE_ERROR, ErrorHelper.INFO_DATABASE_ERROR); ;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}