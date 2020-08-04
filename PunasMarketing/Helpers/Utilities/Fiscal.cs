using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PunasMarketing.Models.DomainModel;


public static class Fiscal
{
    public static short GetFiscalId()
    {
        short FiscalId = 0;
        if (HttpContext.Current.Request.Cookies.AllKeys.Contains("CurrentFiscalYear"))
        {
            string value = HttpContext.Current.Request.Cookies["CurrentFiscalYear"].Value;
            FiscalId = short.Parse(value);
        }
        else
        {
            using (gsharing_DamliEntities db = new gsharing_DamliEntities())
            {
                // افزودن کوکی 
                var Fiscal = db.FiscalPeriods.Where(m => m.IsClosed == false).FirstOrDefault();
                var date = DateTime.Now;
                var cookie = new HttpCookie("CurrentFiscalYear", Fiscal.Id.ToString());
                if (date < Fiscal.EndDate)
                    cookie.Expires = Fiscal.EndDate;
                else
                    cookie.Expires = date.AddMonths(1);
                cookie.HttpOnly = true;
                HttpContext.Current.Response.Cookies.Add(cookie);
                FiscalId = Fiscal.Id;
            }


        }
        return FiscalId;

    }

    
}
