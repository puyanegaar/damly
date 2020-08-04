﻿using PunasMarketing.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PunasMarketing.Helpers.Filters
{
    public class InventoryUpdateChecking : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var id = (short)filterContext.ActionParameters.First().Value;
            using (gsharing_DamliEntities db = new gsharing_DamliEntities())
            {
                var inventory = db.InventoryCheckings.Find(id);
                var date = DateTime.Now;
                int day = (int)(date - inventory.CreatedDate).TotalDays;
                int Ui = int.Parse(filterContext.HttpContext.User.Identity.Name);
                if (day != 0 && inventory.CreatedByUserId != Ui)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        var urlHelper = new UrlHelper(filterContext.RequestContext);
                        filterContext.HttpContext.Response.StatusCode = 403;
                        filterContext.Result = new JsonResult
                        {
                            Data = new
                            {
                                Url = urlHelper.Action("UpdateError", "Account")
                            },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        //NotAllowedToEdit
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "UpdateError" }));
                    }
                   
                }

            }
            base.OnActionExecuting(filterContext);
        }
    }
}