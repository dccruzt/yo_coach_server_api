using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models.Repositories
{
    public class InstallationRepository : BaseRepository
    {
        public static Installation Register(string userId, Installation installation)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    // when is an Android device its necessary disable all the installations the user has.
                    if(installation.DeviceType.Equals(DeviceType.ANDROID))
                    {
                        var installations = context.Installation.Where(x => x.UserId.Equals(userId)).ToList();
                        foreach (var inst in installations)
                        {
                            inst.Enabled = false;
                        }
                    }
                    installation.UserId = userId;
                    installation.Id = Guid.NewGuid().ToString();
                    installation.Enabled = true;
                    installation.CreatedAt = DateTime.Now;
                    installation.UpdatedAt = DateTime.Now;

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
                        var installationsByUser = context.Installation.Where(x => x.UserId.Equals(user.Id)).ToList();
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

        public static List<Installation> getInstallations(string userId)
        {
            try
            {
                using (var context = new YoCoachServerContext())
                {
                    var installations = context.Installation.Where(x => x.UserId.Equals(userId)).ToList();
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