using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models.Repositories
{
    public class UserRepository
    {
        public static void RegisterUser(ApplicationUser user)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    if (user.Type.Equals("CL"))
                    {
                        var client = new Client()
                        {
                            ClientId = user.Id
                            
                        };
                        context.Client.Add(client);
                    }
                    if (user.Type.Equals("CO"))
                    {
                        var coach = new Coach()
                        {
                            CoachId = user.Id,
                            IsVisibleForClients = true,
                            TimeToCancel = 0
                        };
                        context.Coach.Add(coach);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}