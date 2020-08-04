using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public static class UserIdentity
{
    public static int Getuserid()
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            int id = int.Parse(HttpContext.Current.User.Identity.Name);
            return id;
        }
        else
        {
            return 0;
        }
    }
}
