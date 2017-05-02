using Microsoft.AspNet.Identity;
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
        public static string INVALID_LOGIN = "INVALID_LOGIN";
        public static string INVALID_URI = "INVALID_URI";
        public static string INVALID_BODY = "INVALID_BODY";
        public static string INVALID_ROL = "INVALID_ROL";
        public static string INVALID_ACCOUNT = "INVALID_ACCOUNT";
        public static string INVALID_USER = "INVALID_USER";

        public static string DATABASE_ERROR = "DATABASE_ERROR";
        public static string EXCEPTION = "EXCEPTION";

        public static string INFO_INVALID_LOGIN = "The user name or password is incorrect.";
        public static string INFO_INVALID_ROL = "The user can not be added because has a profile with no permissions.";
        public static string INFO_INVALID_USER = "The user can not be added because already was registred by the same coach.";

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

        public static String GetIdentityErrors(IdentityResult identityResult)
        {
            try
            {
                String error = "";
                foreach (string identityError in identityResult.Errors)
                {
                    error += identityError + ", ";
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