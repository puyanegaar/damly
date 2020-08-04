using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using PunasMarketing.Areas;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;

namespace PunasMarketing.Helpers.Utilities
{
    /// <summary>  
    /// Application OAUTH Provider class.  
    /// </summary>  
    public class AppOAuthProvider : OAuthAuthorizationServerProvider
    {
        #region Private Properties  

        /// <summary>  
        /// Public client ID property.  
        /// </summary>  
        private readonly string _publicClientId;

        /// <summary>  
        /// Database Store property.  
        /// </summary>  
        //private Oauth_APIEntities databaseManager = new Oauth_APIEntities();

        #endregion

        #region Default Constructor method.  

        /// <summary>  
        /// Default Constructor method.  
        /// </summary>  
        /// <param name="publicClientId">Public client ID parameter</param>  
        public AppOAuthProvider(string publicClientId)
        {
            //TODO: Pull from configuration  
            if (publicClientId == null)
            {
                throw new ArgumentNullException(nameof(publicClientId));
            }

            // Settings.  
            _publicClientId = publicClientId;
        }

        #endregion

        #region Grant resource owner credentials override method.  

        /// <summary>  
        /// Grant resource owner credentials overload method.  
        /// </summary>  
        /// <param name="context">Context parameter</param>  
        /// <returns>Returns when task is completed</returns>  
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string userType = context.OwinContext.Get<string>("user_type");

            // Initialization.  
            string usernameVal = context.UserName;
            string passwordVal = context.Password;
            int userId = 0;

            var claims = new List<Claim>();

            if (userType == UserType.Customer)
            {
                CustomerRepository customerRepo = new CustomerRepository(new gsharing_DamliEntities());
                bool login = customerRepo.Login(usernameVal, passwordVal, ref userId);
                Customer user = customerRepo.Find(userId);

                // Verification.  
                if (login == false || user == null)
                {
                    // Settings.  
                    context.SetError("ورود ناموفق", "نام کاربری یا کلمه عبور اشتباه است.");

                    // Retuen info.
                    return;
                }

                if (!user.IsActive)
                {
                    context.SetError("ورود ناموفق", "حساب کاربری شما غیرفعال است.");
                    return;
                }
            }
            else if (userType == UserType.Marketer)
            {
                PersonnelRepository personnelRepo = new PersonnelRepository(new gsharing_DamliEntities());
                User user = new User();
                bool login = personnelRepo.Login(usernameVal, passwordVal, ref user);
                userId = user.PersonnelId;
                Personnel personnel = personnelRepo.Find(user.PersonnelId);

                if (login == false || personnel == null || personnel.JobTitleId != 0)
                {
                    // Settings.  
                    context.SetError("ورود ناموفق", "نام کاربری یا کلمه عبور اشتباه است.");

                    // Retuen info.
                    return;
                }

                if (!user.IsActive)
                {
                    context.SetError("ورود ناموفق", "حساب کاربری شما غیرفعال است.");
                    return;
                }
            }
            else
            {
                // Settings.  
                context.SetError("ورود ناموفق", "نوع کاربر صحیح نیست.");

                // Retuen info.
                return;
            }

            // Initialization.  
            //var userInfo = user;

            // Setting  
            claims.Add(new Claim(ClaimTypes.Name, usernameVal));
            claims.Add(new Claim(ClaimTypes.Role, userType));
            claims.Add(new Claim("user_id", userId.ToString()));

            // Setting Claim Identities for OAUTH 2 protocol.  
            ClaimsIdentity oAuthClaimIdentity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesClaimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);

            // Setting user authentication.  
            AuthenticationProperties properties = CreateProperties(usernameVal);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthClaimIdentity, properties);

            // Grant access to authorize user.  
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesClaimIdentity);
        }

        #endregion

        #region Token endpoint override method.  

        /// <summary>  
        /// Token endpoint override method  
        /// </summary>  
        /// <param name="context">Context parameter</param>  
        /// <returns>Returns when task is completed</returns>  
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                // Adding.  
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            // Return info.  
            return Task.FromResult<object>(null);
        }

        #endregion

        #region Validate Client authntication override method  

        /// <summary>  
        /// Validate Client authntication override method  
        /// </summary>  
        /// <param name="context">Contect parameter</param>  
        /// <returns>Returns validation of client authentication</returns>  
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string[] userType = context.Parameters.Where(x => x.Key == "user_type").Select(x => x.Value).FirstOrDefault();
            if (userType != null && userType.Length > 0 && userType[0].Trim().Length > 0)
            {
                context.OwinContext.Set("user_type", userType[0].Trim());
            }

            // Resource owner password credentials does not provide a client ID.  
            if (context.ClientId == null)
            {
                // Validate Authoorization.  
                context.Validated();
            }

            // Return info.  
            return Task.FromResult<object>(null);
        }

        #endregion

        #region Validate client redirect URI override method  

        /// <summary>  
        /// Validate client redirect URI override method  
        /// </summary>  
        /// <param name="context">Context parmeter</param>  
        /// <returns>Returns validation of client redirect URI</returns>  
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            // Verification.  
            if (context.ClientId == _publicClientId)
            {
                // Initialization.  
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                // Verification.  
                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    // Validating.  
                    context.Validated();
                }
            }

            // Return info.  
            return Task.FromResult<object>(null);
        }

        #endregion

        #region Create Authentication properties method.  

        /// <summary>  
        /// Create Authentication properties method.  
        /// </summary>  
        /// <param name="userName">User name parameter</param>  
        /// <returns>Returns authenticated properties.</returns>  
        public static AuthenticationProperties CreateProperties(string userName)
        {
            // Settings.  
            IDictionary<string, string> data = new Dictionary<string, string>
                                               {
                                                   { "userName", userName }
                                               };

            // Return info.  
            return new AuthenticationProperties(data);
        }

        #endregion
    }
}