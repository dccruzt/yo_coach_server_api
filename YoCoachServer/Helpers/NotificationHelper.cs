using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using YoCoachServer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Configuration;

namespace YoCoachServer.Helpers
{
    public class NotificationHelper
    {
        public static void SendNotfication(YoCocahNotification yoCoachNotification)
        {
            try
            {
                var fireBaseUrl = ConfigurationManager.AppSettings["FireBaseUrl"].ToString();
                var serverKey = ConfigurationManager.AppSettings["ServerKey"].ToString();
                var senderId = ConfigurationManager.AppSettings["SenderId"].ToString();

                WebRequest webRequest = WebRequest.Create(fireBaseUrl);
                webRequest.Method = "POST";
                webRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                webRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                webRequest.ContentType = "application/json";

                var json = JsonHelper.serializeObject(yoCoachNotification);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                webRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = webRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse webResponse = webRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = webResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                            }
                        }
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