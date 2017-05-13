using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using YoCoachServer.Models;
using System.Net;
using YoCoachServer.Helpers;

namespace YoCoachServer.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

                ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    string errorString = JsonHelper.serializeObject(new ErrorResult(ErrorHelper.INVALID_LOGIN, ErrorHelper.INFO_INVALID_LOGIN));
                    //context.SetError("invalid_grant", "The user name or password is incorrect.");
                    context.SetError(new string(' ', errorString.Length - 12));
                    context.Response.Write(errorString);
                    return;
                }

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = CreateProperties(user.Id, user.Type, user.UserName, user.Name, user.Email, (user.Birthday.HasValue ? user.Birthday.Value : new DateTimeOffset()), user.PhoneNumber, (user.Picture == null) ? "" : user.Picture.ToString());
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            catch (Exception ex)
            {
                string errorString = JsonHelper.serializeObject(new ErrorResult(ErrorHelper.EXCEPTION, ex.Message));
                context.Response.Write(errorString);
                throw;
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string id, string type, string userName, string name, string email, DateTimeOffset birthday, string phoneNumber, string picture)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "id", id },
                { "type", type },
                { "user_name", userName },
                { "name", (name == null) ? "" : name },
                { "email", (email == null) ? "" : email },
                { "birthday", birthday.ToString() },
                { "picture", picture }
            };
            return new AuthenticationProperties(data);
        }
    }
}