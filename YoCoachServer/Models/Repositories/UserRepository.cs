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
                        return new ErrorResult(ErrorHelper.ACCOUNT_ERROR, ErrorHelper.GetIdentityErrors(result));
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
                    return new ErrorResult(ErrorHelper.DATABASE_ERROR, ex.Message);
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

        public static Student CreateUserClient()
        {
            try
            {
                var client = new Student()
                {
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now,

                };
                return client;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Student CreateStudentByCoach(string coachId, StudentCoach studentCoach, string code)
        {
            try
            {
                var StudentCoachList = new List<StudentCoach>();
                var clientCoach = new StudentCoach()
                {
                    CoachId = coachId,
                    Name = studentCoach.Name,
                    StudentType = studentCoach.StudentType
                };
                StudentCoachList.Add(clientCoach);

                var client = new Student()
                {
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now,
                    AllowLoginWithCode = true,
                    Code = code,
                    CodeCreatedAt = DateTimeOffset.Now,
                    StudentCoaches = StudentCoachList

                };
                return client;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}