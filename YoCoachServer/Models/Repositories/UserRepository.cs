using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using YoCoachServer.Helpers;
using YoCoachServer.Models.BindingModels;

namespace YoCoachServer.Models.Repositories
{
    public class UserRepository : BaseRepository
    {
        public static async Task<Object> RegisterCoach(RegisterBindingModel model, ApplicationUserManager userManager, YoCoachServerContext context)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var user = new ApplicationUser()
                    {
                        UserName = model.PhoneNumber,
                        Name = model.Name,
                        Type = COACH
                    };

                    var result = await userManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        return new ErrorResult(ErrorHelper.INVALID_ACCOUNT, ErrorHelper.GetIdentityErrors(result));
                    }
                    var roleResult = await userManager.AddToRoleAsync(user.Id, COACH);
                    var coach = CreateCoach(user);
                    context.Coach.Add(coach);
                    context.SaveChanges();
                    transaction.Commit();
                    return coach;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new ErrorResult(ErrorHelper.DATABASE_ERROR, ex.StackTrace);
                }
            }
        }
        public static Coach CreateCoach(ApplicationUser user)
        {
            try
            {
                var coach = new Coach()
                {
                    User = user,
                    IsVisibleForClients = true,
                    TimeToCancel = 0,
                    HasPenality = false,
                    PenalityPercent = 0,
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now
                };
                return coach;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Student CreateStudentAndStudentCoach(StudentCoach studentCoach, string code, string coachId, string userId)
        {
            try
            {
                studentCoach.CoachId = coachId;
                studentCoach.StudentId = userId;
                studentCoach.CreatedAt = DateTime.Now;
                studentCoach.UpdatedAt = DateTime.Now;

                var student = new Student()
                {
                    Id = userId,
                    Code = code,
                    AllowLoginWithCode = true,
                    CodeCreatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                student.StudentCoaches.Add(studentCoach);
                return student;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static StudentCoach CreateStudentCoach(StudentCoach studentCoach, string coachId, string userId)
        {
            try
            {
                studentCoach.CoachId = coachId;
                studentCoach.StudentId = userId;
                studentCoach.CreatedAt = DateTime.Now;
                studentCoach.UpdatedAt = DateTime.Now;
                return studentCoach;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}