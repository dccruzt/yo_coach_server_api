using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoCoachServer.Models.Repositories
{
    public class NotificationRepository
    {
        public static YoCocahNotification CreateNotification(String deviceToken)
        {
            try
            {
                var notification = new Notification(){
                    Title = "Tile yocoach",
                    Body = "body yocoach"
                };

                var yoCoachNotification = new YoCocahNotification()
                {
                    To = deviceToken,
                    Notification = notification
                };

                return yoCoachNotification;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}