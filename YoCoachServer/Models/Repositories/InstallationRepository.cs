using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models.Repositories
{
    public class InstallationRepository
    {
        public static Installation Register(String userId, Installation installation)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {

                    var client = context.Client.Where(x => x.Id.Equals(userId)).Include("User").ToList();
                    if (client != null)
                    {
                        installation.User = client.FirstOrDefault().User;
                    }
                    else
                    {
                        var coach = context.Coach.Where(x => x.Id.Equals(userId)).Include("User").ToList();
                        installation.User = coach.FirstOrDefault().User;
                    }

                    installation.Id = Guid.NewGuid().ToString();
                    

                    context.Installation.Add(installation);
                    context.SaveChanges();
                    return installation;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}