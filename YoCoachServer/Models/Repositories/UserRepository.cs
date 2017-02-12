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
                using (var context = new YoCoachServerContext())
                {
                    var coach = new Coach()
                    {
                        //CoachId = user.Id,
                        IsVisibleForClients = true,
                        TimeToCancel = 0
                    };
                    //context.Coach.Add(coach);
                    //context.SaveChanges();
                    return coach;
                }
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
                using (var context = new YoCoachServerContext())
                {
                    var client = new Client()
                    {
                        //ClientId = user.Id

                    };
                    //context.Client.Add(client);
                    //context.SaveChanges();
                    return client;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}