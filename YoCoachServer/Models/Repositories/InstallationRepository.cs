using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models.Repositories
{
    public class InstallationRepository
    {
        public static Installation Register(ApplicationUser currentUser, Installation installation)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    ApplicationUser user = null;
                    if(currentUser.Type.Equals("CO"))
                    {
                        user = context.Coach.Where(x => x.Id.Equals(currentUser.Id)).Include("User").ToList().FirstOrDefault().User;
                        installation.User = user;
                    }
                    else if(currentUser.Type.Equals("CL"))
                    {
                        user = context.Student.Where(x => x.Id.Equals(currentUser.Id)).Include("User").ToList().FirstOrDefault().User;
                        installation.User = user;
                    }

                    // When is an Android device its necessary disable all the installations the user has.
                    if(installation.DeviceType.Equals(DeviceType.ANDROID) && user != null)
                    {
                        var installations = context.Installation.Where(x => x.User.Id.Equals(user.Id));
                        if(installations != null)
                        {
                            foreach(var inst in installations)
                            {
                                if (inst.Enabled)
                                {
                                    inst.Enabled = false;
                                }
                            }
                        }
                    }

                    installation.Id = Guid.NewGuid().ToString();
                    installation.Enabled = true;
                    installation.CreatedAt = DateTime.Now;
                    installation.UpdateAt = DateTime.Now;

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

        public static List<Installation> getInstallations(List<Student> users)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var installations = new List<Installation>();
                    foreach(Student user in users)
                    {
                        var installationsByUser = context.Installation.Where(x => x.User.Id.Equals(user.Id)).ToList();
                        installations.AddRange(installationsByUser);
                    }
                    return installations;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}