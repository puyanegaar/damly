using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;

namespace PunasMarketing.Areas.WebApi
{
    public class ApiAuth
    {
        public static bool IsInRole(HttpRequestMessage request, string role)
        {
            if (request.GetRequestContext().Principal is ClaimsPrincipal principal)
            {
                return principal.IsInRole(role);
            }

            return false;
        }

        public static int GetUserId(HttpRequestMessage request)
        {
            if (request.GetRequestContext().Principal is ClaimsPrincipal principal)
            {
                string userId = principal.Claims.FirstOrDefault(i => i.Type == "user_id")?.Value;

                try
                {
                    if (!string.IsNullOrWhiteSpace(userId))
                    {
                        return int.Parse(userId);
                    }
                }
                catch
                {
                    // ignored
                }
            }

            return -1; // invalid user id
        }
    }
}