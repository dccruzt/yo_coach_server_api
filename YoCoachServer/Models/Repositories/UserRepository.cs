using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Models.Repositories
{
    public class UserRepository
    {
        public static Coach CreateCoach(String userId)
        {
            try
            {
                var coach = new Coach()
                {
                    Id = userId,
                    CreatedAt = DateTimeOffset.Now,
                    UpdatedAt = DateTimeOffset.Now,
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