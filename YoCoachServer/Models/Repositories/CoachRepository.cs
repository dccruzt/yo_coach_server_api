using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using YoCoachServer.Helpers;
using YoCoachServer.Models.BindingModels;
using YoCoachServer.Models.Enums;
using YoCoachServer.Utils;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Models.Repositories
{
    public class CoachRepository : BaseRepository
    {

        public static Object SaveSchedule(ApplicationUser user, SaveScheduleBindingModel model)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedule = ScheduleRepository.CreateSchedule(model, true);

                    var gym = context.Gym.Where(x => x.Id.Equals(schedule.GymId)).Include(x => x.Credit).ToList().FirstOrDefault();
                    var studentSchedules = new List<StudentSchedule>();
                    foreach (var student in model.Students)
                    {
                        var existingStudent = context.Student.Where(x => x.Id.Equals(student.Id)).Include(x => x.User).ToList().FirstOrDefault() ;
                        if (existingStudent != null)
                        {
                            var studentSchedule = new StudentSchedule()
                            {
                                StudentId = existingStudent.Id
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

                        var installations = InstallationRepository.getInstallations(studentSchedules);
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

        public static List<Schedule> ListSchedules(string coachId, String date, ScheduleState? scheduleState)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedules = context.Schedule.Where(x => x.CoachId.Equals(coachId)).Include(x => x.Gym.Credit).Include(x => x.StudentSchedules.Select(y => y.Student.User)).ToList();
                    if (date != null)
                    {
                        DateTimeOffset filterDate = DateTimeOffset.Parse(date);
                        var schedulesByDay = new List<Schedule>();
                        foreach (var schedule in schedules)
                        {
                            DateTimeOffset dateStart = (schedule.StartTime.HasValue) ? schedule.StartTime.Value : new DateTimeOffset();
                            if (DateUtils.SameDate(dateStart, filterDate))
                            {
                                schedulesByDay.Add(schedule);
                            }
                        }
                        schedules = schedulesByDay;
                    }
                    if(scheduleState != null)
                    {
                        schedules = schedules.Where(x => x.ScheduleState.Equals(scheduleState)).ToList();
                    }

                    var schedulesWithStudent = new List<Schedule>();
                    foreach (var schedule in schedules)
                    {
                        var scheduleWithStudent = schedule;
                        scheduleWithStudent = StudentRepository.FillStudentViewModel(scheduleWithStudent);
                        schedulesWithStudent.Add(scheduleWithStudent);
                    }
                    schedules = schedulesWithStudent;
                    return schedules;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<StudentCoach> ListStudents(string coachId)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var clientCoaches = context.StudentCoach.Where(x => x.CoachId.Equals(coachId)).ToList();
                    return clientCoaches;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async static Task<Object> RegisterStudent(YoCoachServerContext context, string coachId, StudentCoach studentCoach, ApplicationUserManager userManager)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    // check if the user exists
                    var user = context.Users.FirstOrDefault(x => x.UserName.Equals(studentCoach.PhoneNumber));

                    // if the student doesnt exist, register into the users, students and studentCoach tables
                    if (user == null)
                    {
                        var code = StringHelper.GenerateCode();
                        user = new ApplicationUser()
                        {
                            UserName = studentCoach.PhoneNumber,
                            Type = STUDENT
                        };
                        var result = await userManager.CreateAsync(user, code);
                        if (result.Succeeded)
                        {
                            var roleResult = await userManager.AddToRoleAsync(user.Id, STUDENT);
                            var student = UserRepository.CreateStudentAndStudentCoach(studentCoach, code, coachId, user.Id);
                            context.Student.Add(student);
                            context.SaveChanges();
                            transaction.Commit();

                            await SMSHelper.sendSms(studentCoach.PhoneNumber, code);
                            return studentCoach;
                        }
                    }
                    // check if the coach already has the student registered
                    var oldStudent = context.StudentCoach.Where(x => x.CoachId.Equals(coachId) && x.StudentId.Equals(user.Id)).ToList();
                    if (oldStudent != null && oldStudent.Count > 0)
                    {
                        return new ErrorResult(ErrorHelper.INVALID_USER, ErrorHelper.INFO_INVALID_USER);
                    }
                    // check the user role, remaining that must be added only users with student profile
                    if (!user.Type.Equals(STUDENT))
                    {
                        return new ErrorResult(ErrorHelper.INVALID_ROL, ErrorHelper.INFO_INVALID_ROL);
                    }
                    studentCoach = UserRepository.CreateStudentCoach(studentCoach, coachId, user.Id);
                    context.StudentCoach.Add(studentCoach);
                    context.SaveChanges();
                    transaction.Commit();
                    return studentCoach;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new ErrorResult(ErrorHelper.DATABASE_ERROR, ex.StackTrace);
                }
            }
        }
    }
}