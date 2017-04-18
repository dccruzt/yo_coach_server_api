using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoCoachServer.Models.Enums;

namespace YoCoachServer.Models.Repositories
{
    public class NotificationRepository
    {
        public static YoCocahNotification CreateNotificationForSaveSchedule(String deviceToken, String title, String body, NotificationType notificationType)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("notification_type", NotificationType.SAVE_SCHEDULE);

                var notification = new Notification() {
                    Title = title,
                    Body = body
                };

                var yoCoachNotification = new YoCocahNotification()
                {
                    To = deviceToken,
                    Data = dictionary,
                    Notification = notification,
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