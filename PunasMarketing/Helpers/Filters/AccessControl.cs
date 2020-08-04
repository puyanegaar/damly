using PunasMarketing.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PunasMarketing.Helpers.Filters
{
    public class AccessControl : AuthorizeAttribute
    {
        private object _Permission;
        public AccessControl(object Permission)
        : base()
        {
            _Permission = Permission;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                int Permisonid = (int)_Permission;
                int Ui = int.Parse(filterContext.HttpContext.User.Identity.Name);
                using (gsharing_DamliEntities db = new gsharing_DamliEntities())
                {
                    var u = db.Users.Find(Ui);
                    int grand = u.PersonnelId + u.Samoosh;
                    if (grand != 101)
                    {
                        var Access = db.UserAccesses.Where(m => m.UserId == u.PersonnelId && m.ActionId == Permisonid);
                        if (!Access.Any())
                        {
                            if(filterContext.HttpContext.Request.IsAjaxRequest())
                            {
                                var urlHelper = new UrlHelper(filterContext.RequestContext);
                                filterContext.HttpContext.Response.StatusCode = 403;
                                filterContext.Result = new JsonResult
                                {
                                    Data = new
                                    {
                                        Url = urlHelper.Action("AccessError", "Account")
                                    },
                                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                                };
                            }
                            else
                            {
                                //NotAuthorized
                                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "AccessError" }));
                            }
                           
                        }
                    }

                }

            }
            else
            {
                //NotLoged redirect to Login page
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }

            base.OnAuthorization(filterContext);
        }
    }
}