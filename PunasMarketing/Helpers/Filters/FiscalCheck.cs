using PunasMarketing.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PunasMarketing.Helpers.Filters
{
    public class FiscalCheck : ActionFilterAttribute
    {

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var date = DateTime.Now;
        //    using (gsharing_DamliEntities db = new gsharing_DamliEntities())
        //    {
        //        if (HttpContext.Current.Request.Cookies.AllKeys.Contains("CurrentFiscalYear"))
        //        {
        //            short FiscalId = short.Parse(HttpContext.Current.Request.Cookies["CurrentFiscalYear"].Value);

        //            var Fiscal = db.FiscalPeriods.Find(FiscalId);

        //            if (Fiscal.IsClosed)
        //            {
        //                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "", action = "", MessageType = 1 }));
        //            }
        //            else if (Fiscal.EndDate < date)
        //            {
        //                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "", action = "", MessageType = 2 }));
        //            }
        //        }
        //        else
        //        {
        //            var Fiscal = db.FiscalPeriods.Where(m => m.IsClosed == false);
                    
        //            if(Fiscal.Any())
        //            {
        //                var F = Fiscal.FirstOrDefault();
        //                if(F.EndDate<date)
        //                {
        //                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "", action = "", MessageType = 2 }));
        //                }
        //            }
        //            else
        //            {
        //                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "", action = "", MessageType = 3 }));
        //            }
                    
        //        }
        //    }
        //    base.OnActionExecuting(filterContext);
        //}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var date = DateTime.Now;
            using (gsharing_DamliEntities db = new gsharing_DamliEntities())
            {
                var CurrentFiscalId = Fiscal.GetFiscalId();
                var CurrentFiscal = db.FiscalPeriods.Find(CurrentFiscalId);
                if (CurrentFiscal.IsClosed)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        var urlHelper = new UrlHelper(filterContext.RequestContext);
                        filterContext.HttpContext.Response.StatusCode = 403;
                        filterContext.Result = new JsonResult
                        {
                            Data = new
                            {
                                Url = urlHelper.Action("ClosedFiscalError", "Account")
                            },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "ClosedFiscalError", MessageType = 1 }));
                    }
                   
                }
                else if (CurrentFiscal.EndDate < date)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        var urlHelper = new UrlHelper(filterContext.RequestContext);
                        filterContext.HttpContext.Response.StatusCode = 403;
                        filterContext.Result = new JsonResult
                        {
                            Data = new
                            {
                                Url = urlHelper.Action("ExpiredFiscalError", "Account")
                            },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "ExpiredFiscalError", MessageType = 2 }));
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}