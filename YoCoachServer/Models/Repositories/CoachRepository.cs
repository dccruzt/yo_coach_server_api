using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using YoCoachServer.Helpers;
using YoCoachServer.Models.Enums;
using YoCoachServer.Utils;
using static YoCoachServer.Models.BindingModels.CoachBindingModels;

namespace YoCoachServer.Models.Repositories
{
    public class CoachRepository
    {
        public static List<Client> ListClients(string coachId, ApplicationUserManager userManager)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var clientCoaches = context.ClientCoach.Where(x => x.CoachId.Equals(coachId)).Include("Client").ToList();
                    var modelList = new List<Client>();
                    if (clientCoaches != null)
                    {
                        foreach (var clientCoach in clientCoaches)
                        {
                            //ApplicationUser userClient = await userManager.FindByIdAsync(clientCoach.ClientId);
                            var client = context.Client.Where(x => x.Id.Equals(clientCoach.ClientId)).Include("User").FirstOrDefault();
                            if(client != null)
                            {
                                //var model = new ClientBindingModel()
                                //{
                                //    Id = clientCoach.ClientId,
                                //    NickName = clientCoach.NickName,
                                //    PhoneNumber = client.User.PhoneNumber,
                                //    ClientType = clientCoach.ClientType,
                                //    Picture = client.User.Picture,
                                //    Age = client.User.Age,
                                //    Email = client.User.Email,
                                //    CreatedAt = clientCoach.Client.CreatedAt,
                                //    UpdateAt = clientCoach.Client.UpdateAt,
                                //};
                                modelList.Add(client);
                            }
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
                        client = context.Client.Where(x => x.Id.Equals(userClient.Id)).Include("User").FirstOrDefault();
                    }
                    //if the client doesnt exist, register into the aspnetusers table
                    if(client == null)
                    {
                        var code = StringUtils.GenerateCode();
                        client = UserRepository.CreateUserClientByCoach(coachId, model, code);
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
                        IdentityResult result = await userManager.CreateAsync(user, code);
                        if (result.Succeeded)
                        {
                            await SMSHelper.sendSms(model.PhoneNumber, code);
                            client.User = user;
                            return client;
                        }
                    }//If the client exists just create a row clientcoach.
                    else
                    {
                        var oldClientCoach = context.ClientCoach.FirstOrDefault(x => x.CoachId.Equals(coachId) && x.ClientId.Equals(client.Id));
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