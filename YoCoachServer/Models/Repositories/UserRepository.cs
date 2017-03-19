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

        public static Client CreateUserClient()
        {
            try
            {
                var client = new Client()
                {
                    //ClientId = user.Id

                };
                return client;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Client CreateUserClientByCoach(string coachId, RegisterClientBindingModel model, string code)
        {
            try
            {
                var ClientCoachList = new List<ClientCoach>();
                var clientCoach = new ClientCoach()
                {
                    CoachId = coachId,
                    NickName = model.NickName,
                    Code = model.Code,
                    IsExpired = false,
                    ClientType = model.ClientType
                };
                ClientCoachList.Add(clientCoach);

                var client = new Client()
                {
                    CreatedAt = DateTimeOffset.Now,
                    UpdateAt = DateTimeOffset.Now,
                    AllowLoginWithCode = true,
                    CodeToAccess = code,
                    CodeCreatedAt = DateTimeOffset.Now,
                    ClientCoaches = ClientCoachList

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