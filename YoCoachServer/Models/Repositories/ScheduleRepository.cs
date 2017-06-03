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
        public static Object SaveSchedule(ApplicationUser user, SaveScheduleBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var gym = context.Gym.Where(x => x.Id.Equals(model.GymId)).Include(x => x.Credit).ToList().FirstOrDefault();
                    var isCoach = user.Type.Equals(COACH) ? true : false;
                    var schedule = ScheduleRepository.CreateSchedule(model, isCoach);

                    var studentSchedules = new List<StudentSchedule>();
                    foreach (var student in model.Students)
                    {
                        var existingStudent = context.Student.Where(x => x.Id.Equals(student.Id)).Include(x => x.User).ToList().FirstOrDefault();
                        if (existingStudent != null)
                        {
                            var studentSchedule = new StudentSchedule()
                            {
                                StudentId = existingStudent.Id,
                                Credit = CreditRepository.createCreditForStudentPayment(0)
                            };
                            studentSchedules.Add(studentSchedule);
                        }
                    }

                    if (gym != null && studentSchedules.Count > 0)
                    {
                        schedule.Gym = gym;
                        schedule.StudentSchedules = studentSchedules;
                        context.Schedule.Add(schedule);

                        context.SaveChanges();

                        List<Installation> installations = null;
                        installations = isCoach ? InstallationRepository.getInstallations(studentSchedules) : InstallationRepository.getInstallations(model.CoachId);
                        if (installations != null)
                        {
                            foreach (var installation in installations)
                            {
                                var notification = NotificationRepository.CreateNotificationForSaveSchedule(
                                    installation.DeviceToken,
                                    user.Name + " " + NotificationMessage.NEW_SCHEDULE_TITLE,
                                    NotificationMessage.NEW_SCHEDULE_BODY,
                                    NotificationType.SAVE_SCHEDULE);

                                NotificationHelper.SendNotfication(notification);
                            }
                        }
                        schedule = StudentRepository.FillStudentViewModel(schedule);
                        return schedule;
                    }
                    return new ErrorResult(ErrorHelper.DATABASE_ERROR, ErrorHelper.INFO_DATABASE_ERROR);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Schedule CreateSchedule(SaveScheduleBindingModel model, bool isCoach)
        {
            try
            {
                var schedule = new Schedule()
                {
                    Id = Guid.NewGuid().ToString(),
                    CoachId = model.CoachId,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    TotalValue = model.TotalValue,
                    GymId = model.GymId,
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now,
                    IsConfirmed = isCoach ? true : false,
                    PaymentState = StatePayment.PENDING,
                    ScheduleState = isCoach ? ScheduleState.SCHEDULED : ScheduleState.PENDING
                };
                return schedule;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

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
                        //schedule.Students.Add(student);
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

        public static StudentSchedule ReceivePayment(string coachId, PayScheduleBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var studentSchedule = context.StudentSchedule.Where(x => x.ScheduleId.Equals(model.ScheduleId) && x.StudentId.Equals(model.StudentId)).Include(CREDIT).FirstOrDefault();

                    //if not exist a previous payment
                    if (studentSchedule.Credit == null)
                    {
                        studentSchedule = CreditRepository.createStudentPayment(model.ScheduleId, model.StudentId, model.CreditsAmount);
                        var invoice = InvoiceRepository.createInvoiceForStudentPayment(model.CreditsAmount);
                        studentSchedule.Credit.Invoices.Add(invoice);
                        context.SaveChanges();
                        return studentSchedule;
                    }
                    else
                    {
                        studentSchedule.Credit.Amount += model.CreditsAmount;
                        var invoice = InvoiceRepository.createInvoiceForStudentPayment(model.CreditsAmount);
                        studentSchedule.Credit.Invoices.Add(invoice);
                        context.SaveChanges();
                        return studentSchedule;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Object UpdateSchedule(String id, Schedule schedule, String coachId)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var existingSchedule = context.Schedule.Find(id);
                    var coach = context.Coach.Find(coachId);
                    if(existingSchedule != null && coach != null)
                    {
                        if(schedule.ScheduleState != null)
                        {
                            if (schedule.ScheduleState.Equals(ScheduleState.MISSED) && coach.HasPenality.Value && coach.PenalityPercent != null)
                            {
                                existingSchedule.TotalValue = existingSchedule.TotalValue * coach.PenalityPercent;
                            }
                            existingSchedule.ScheduleState = schedule.ScheduleState;
                            existingSchedule.UpdatedAt = DateTimeOffset.Now;
                            context.SaveChanges();
                            return schedule;
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
    }
}