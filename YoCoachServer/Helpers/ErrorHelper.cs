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
        public static string INVALID_MODEL = "INVALID_MODEL";

        public static String GetModelErrors(ModelStateDictionary modelStateDictionary)
        {
            try
            {
                String error = "";
                foreach (ModelState modelState in modelStateDictionary.Values)
                {
                    foreach (ModelError modelError in modelState.Errors)
                    {
                        error += modelError.ErrorMessage + " ";
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