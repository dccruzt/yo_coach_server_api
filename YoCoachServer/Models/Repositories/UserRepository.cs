using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}