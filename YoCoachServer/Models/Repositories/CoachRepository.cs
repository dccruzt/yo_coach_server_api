using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Models.Repositories
{
    public class CoachRepository
    {
        public static List<Client> ListClients(string coachId)
        {
            try
            {
                using ( var context = new YoCoachServerContext())
                {
                    var clients = context.ClientCoach.Where(x => x.CoachId.Equals(coachId))
                        .Select(x => x.Client)
                        .ToList();

                    return clients;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async static Task registerClient(string coachId, RegisterClientBindingModel model, ApplicationUserManager userManager)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    //Check if the user exists
                    Client client = null;
                    ApplicationUser userClient = await userManager.FindByNameAsync(model.PhoneNumberClient);
                    if(userClient != null)
                    {
                        client = context.Client.Find(userClient.Id);
                    }
                    //if the client doesnt exist register into the aspnetusers table
                    if(client == null)
                    {
                        client = UserRepository.CreateUserClientByCoach(coachId, model);
                        var user = new ApplicationUser()
                        {
                            UserName = model.PhoneNumberClient,
                            PhoneNumber = model.PhoneNumberClient,
                            Type = "CL",
                            Client = client
                        };
                        IdentityResult result = await userManager.CreateAsync(user, "password");
                        if (!result.Succeeded)
                        {
                            
                        }
                    }//If the client exists just create a row clientcoach.
                    else
                    {
                        var clientCoach = new ClientCoach()
                        {
                            CoachId = coachId,
                            Client = client,
                            NickName = model.NickName,
                            Code = model.Code,
                            IsExpired = false
                        };
                        context.ClientCoach.Add(clientCoach);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}