using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Models.Repositories
{
    public class UserRepository
    {
        public static Coach CreateUserCoach()
        {
            try
            {
                var coach = new Coach()
                {
                    //CoachId = user.Id,
                    IsVisibleForClients = true,
                    TimeToCancel = 0
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
                    UpdateAt = DateTimeOffset.Now,

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
                    Code = studentCoach.Code,
                    IsExpired = false,
                    StudentType = studentCoach.StudentType
                };
                StudentCoachList.Add(clientCoach);

                var client = new Student()
                {
                    CreatedAt = DateTimeOffset.Now,
                    UpdateAt = DateTimeOffset.Now,
                    AllowLoginWithCode = true,
                    CodeToAccess = code,
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