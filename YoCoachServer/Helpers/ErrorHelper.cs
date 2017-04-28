using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using YoCoachServer.Models;

namespace YoCoachServer.Helpers
{
    public class ErrorHelper
    {
        public static string INVALID_URI = "INVALID_URI";
        public static string INVALID_BODY = "INVALID_BODY";
        public static string DATABASE_ERROR = "DATABASE_ERROR";
        public static string EXCEPTION = "EXCEPTION";

        public static string INFO_DATABASE_ERROR = "Some error ocurred with the server or database, please try again.";

        public static String GetModelErrors(ModelStateDictionary modelStateDictionary)
        {
            try
            {
                String error = "";
                foreach (ModelState modelState in modelStateDictionary.Values)
                {
                    foreach (ModelError modelError in modelState.Errors)
                    {
                        error += modelError.ErrorMessage + ", ";
                    }
                }
                return error;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}