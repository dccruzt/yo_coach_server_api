using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
                    schedule.UpdateAt = DateTimeOffset.Now;
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

        public async static Task<Student> RegisterStudent(string coachId, StudentCoach studentCoach, ApplicationUserManager userManager)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    //Check if the user exists
                    Student student = null;
                    ApplicationUser userClient = await userManager.FindByNameAsync(studentCoach.PhoneNumber);
                    if(userClient != null)
                    {
                        student = context.Student.Where(x => x.Id.Equals(userClient.Id)).Include("User").FirstOrDefault();
                    }
                    //if the student doesnt exist, register into the aspnetusers table
                    if(student == null)
                    {
                        var code = StringHelper.GenerateCode();
                        student = UserRepository.CreateStudentByCoach(coachId, studentCoach, code);
                        var user = new ApplicationUser()
                        {
                            UserName = studentCoach.PhoneNumber,
                            PhoneNumber = studentCoach.PhoneNumber,
                            Name = studentCoach.Name,
                            Email = studentCoach.Email,
                            Type = "CL",
                            Birthday = studentCoach.Birthday,
                            Student = student
                        };
                        IdentityResult result = await userManager.CreateAsync(user, code);
                        if (result.Succeeded)
                        {
                            await SMSHelper.sendSms(studentCoach.PhoneNumber, code);
                            student.User = user;
                            return student;
                        }
                    }//If the student exists just create a row clientcoach.
                    else
                    {
                        var oldClientCoach = context.StudentCoach.FirstOrDefault(x => x.CoachId.Equals(coachId) && x.StudentId.Equals(student.Id));
                        if(oldClientCoach == null)
                        {
                            var clientCoach = new StudentCoach()
                            {
                                CoachId = coachId,
                                Student = student,
                                Name = studentCoach.Name,
                                Code = studentCoach.Code,
                                IsExpired = false,
                                StudentType = studentCoach.StudentType
                            };
                            context.SaveChanges();
                        }
                    }
                    return student;
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