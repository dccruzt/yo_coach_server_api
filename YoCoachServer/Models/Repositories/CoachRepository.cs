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
    public class CoachRepository
    {

        public static Schedule SaveSchedule(ApplicationUser coach, Schedule schedule)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    schedule.Id = Guid.NewGuid().ToString();
                    schedule.CoachId = coach.Id;
                    schedule.CreatedAt = DateTimeOffset.Now;
                    schedule.UpdatedAt = DateTimeOffset.Now;
                    schedule.IsConfirmed = true;
                    schedule.PaymentState = StatePayment.PENDING;
                    schedule.ScheduleState = ScheduleState.SCHEDULED;

                    var existingStudents = new List<Student>();
                    foreach (var student in schedule.Students)
                    {
                        var existingStudent = context.Student.Find(student.Id);
                        if (existingStudent != null)
                        {
                            existingStudents.Add(existingStudent);
                        }
                    }
                    schedule.Students = existingStudents;
                    context.Schedule.Add(schedule);

                    context.SaveChanges();

                    var installations = InstallationRepository.getInstallations(schedule.Students.ToList());
                    if (installations != null)
                    {
                        foreach (var installation in installations)
                        {
                            var notification = NotificationRepository.CreateNotificationForSaveSchedule(
                                installation.DeviceToken,
                                coach.Name + " " + NotificationMessage.NEW_SCHEDULE_TITLE,
                                NotificationMessage.NEW_SCHEDULE_BODY,
                                NotificationType.SAVE_SCHEDULE);

                            NotificationHelper.SendNotfication(notification);
                        }
                    }
                    return schedule;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<Schedule> ListSchedules(string coachId, String date)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var schedules = context.Schedule.Where(x => x.CoachId.Equals(coachId)).Include("Gym").Include("Students").ToList();
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

        public async static Task<Object> RegisterStudent(string coachId, StudentCoach studentCoach, ApplicationUserManager userManager)
        {
            try
            {
                using (var context = new YoCoachServerContext())
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
                            Type = "ST"
                        };

                        var result = await userManager.CreateAsync(user, code);
                        if (result.Succeeded)
                        {
                            var roleResult = await userManager.AddToRoleAsync(user.Id, "ST");
                            var student = new Student();
                            student.Id = user.Id;
                            student.CreatedAt = DateTime.Now;
                            student.UpdatedAt = DateTime.Now;
                            student.CodeCreatedAt = DateTime.Now;
                            student.AllowLoginWithCode = true;
                            student.Code = code;
                            studentCoach.CoachId = coachId;
                            studentCoach.StudentId = student.Id;
                            studentCoach.CreatedAt = DateTime.Now;
                            studentCoach.UpdatedAt = DateTime.Now;
                            student.StudentCoaches.Add(studentCoach);
                            context.Student.Add(student);
                            context.SaveChanges();
                            await SMSHelper.sendSms(studentCoach.PhoneNumber, code);

                            student.User = user;
                            return studentCoach;
                        }
                    }
                    else
                    {
                        // check if the coach already has the student registered
                        var oldStudent = context.StudentCoach.Where(x => x.CoachId.Equals(coachId) && x.StudentId.Equals(user.Id)).ToList();
                        if (oldStudent != null && oldStudent.Count > 0)
                        {
                            IDictionary<string, string> error = new Dictionary<string, string>
                            {
                                { "code", ErrorHelper.EXISTING_USER },
                                { "message", ErrorHelper.INFO_EXISTING_USER }
                            };
                            return error;
                        }
                        // check the user role, remaining that must be added only users with student profile
                        if (user.Type.Equals("CO"))
                        {
                            IDictionary<string, string> error = new Dictionary<string, string>
                            {
                                { "code", ErrorHelper.INVALID_ROL },
                                { "message", ErrorHelper.INFO_INVALID_ROL }
                            };
                            return error;
                        }
                        studentCoach.CoachId = coachId;
                        studentCoach.StudentId = user.Id;
                        context.SaveChanges();
                    }
                    return studentCoach;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<Installation> getInstallations(Coach coach)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var installations = context.Installation.Where(x => x.User.Id.Equals(coach.Id)).ToList();
                    return installations;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}