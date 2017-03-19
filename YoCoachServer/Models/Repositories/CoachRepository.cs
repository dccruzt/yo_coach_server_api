using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using YoCoachServer.Models.Enums;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Models.Repositories
{
    public class CoachRepository
    {
        public static async Task<List<ClientBindingModel>> ListClients(string coachId, ApplicationUserManager userManager)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var clientCoaches = context.ClientCoach.Where(x => x.CoachId.Equals(coachId)).ToList();
                    var modelList = new List<ClientBindingModel>();
                    if (clientCoaches != null)
                    {
                        foreach (var clientCoach in clientCoaches)
                        {
                            ApplicationUser userClient = await userManager.FindByIdAsync(clientCoach.ClientId);
                            var model = new ClientBindingModel()
                            {
                                Id = clientCoach.ClientId,
                                NickName = clientCoach.NickName,
                                PhoneNumber = userClient.PhoneNumber,
                                ClientType = clientCoach.ClientType,
                                Picture = userClient.Picture,
                                Age = userClient.Age,
                                Email = userClient.Email
                            };
                            modelList.Add(model);
                        }
                    }
                    return modelList;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async static Task<Client> RegisterClient(string coachId, RegisterClientBindingModel model, ApplicationUserManager userManager)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    //Check if the user exists
                    Client client = null;
                    ApplicationUser userClient = await userManager.FindByNameAsync(model.PhoneNumber);
                    if(userClient != null)
                    {
                        client = context.Client.Find(userClient.Id);
                    }
                    //if the client doesnt exist, register into the aspnetusers table
                    if(client == null)
                    {
                        client = UserRepository.CreateUserClientByCoach(coachId, model);
                        var user = new ApplicationUser()
                        {
                            UserName = model.PhoneNumber,
                            PhoneNumber = model.PhoneNumber,
                            Name = model.NickName,
                            Email = model.Email,
                            Type = "CL",
                            Age = model.Age,
                            Client = client
                        };
                        IdentityResult result = await userManager.CreateAsync(user, "password");
                        if (!result.Succeeded)
                        {
                            
                        }
                    }//If the client exists just create a row clientcoach.
                    else
                    {
                        var oldClientCoach = context.ClientCoach.FirstOrDefault(x => x.CoachId.Equals(coachId) && x.ClientId.Equals(client.ClientId));
                        if(oldClientCoach == null)
                        {
                            var clientCoach = new ClientCoach()
                            {
                                CoachId = coachId,
                                Client = client,
                                NickName = model.NickName,
                                Code = model.Code,
                                IsExpired = false,
                                ClientType = model.ClientType
                            };
                            context.SaveChanges();
                        }
                    }
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