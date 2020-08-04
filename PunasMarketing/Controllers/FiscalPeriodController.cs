using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;

namespace PunasMarketing.Controllers
{
    public class FiscalPeriodController : Controller
    {
        private readonly FiscalRepository _fiscalRepo;

        public FiscalPeriodController(FiscalRepository fiscalRepo)
        {
            _fiscalRepo = fiscalRepo;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.FiscalSettings)]
        public ActionResult FiscalPeriods()
        {
            var Fiscal = _fiscalRepo.Where(m => m.IsClosed == false);
            if (Fiscal.Any())
            {
                return View(Fiscal.FirstOrDefault());
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        [AccessControl(ActionsEnum.FiscalSettings)]
        public ActionResult FiscalPeriodsOp(short FiscalId, string StartDate, string EndDate, string vat, string FiscalYearName)
        {
            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate) && !string.IsNullOrEmpty(vat))
            {
                var sd = StartDate.ToMiladiDate().GetStartDate();
                var ed = EndDate.ToMiladiDate().GetEndDate();
                if (ed < sd)
                {
                    TempData["saveMassage"] = Notification.Show("محدوده درستی انتخاب نشده است", type: ToastType.Warning, position: ToastPosition.TopCenter);
                    return RedirectToAction("FiscalPeriods");
                }
                if (FiscalId == 0)
                {
                    //AddNew
                    FiscalPeriod Fiscal = new FiscalPeriod();
                    Fiscal.Id = (short)(_fiscalRepo.GetLastIdentity() + 1);
                    Fiscal.StartDate = sd;
                    Fiscal.EndDate = ed;
                    Fiscal.Vat = byte.Parse(vat);
                    Fiscal.Name = FiscalYearName;
                    Fiscal.IsClosed = false;
                    Fiscal.ClosedDate = null;
                    if (_fiscalRepo.Add(Fiscal))
                    {
                        TempData["saveMassage"] = Notification.Show("اطلاعات سال مالی با موفقیت ثبت شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                        return RedirectToAction("FiscalPeriods");
                    }
                    else
                    {
                        TempData["saveMassage"] = Notification.Show(" ثبت اطلاعات سال مالی با خطا رو به رو شد", type: ToastType.Error, position: ToastPosition.TopCenter);
                        return RedirectToAction("FiscalPeriods");
                    }


                }
                else
                {
                    //Edit
                    var Fiscal = _fiscalRepo.Find(FiscalId);
                    if (Fiscal.EndDate < DateTime.Now)
                    {
                        if (Fiscal.EndDate > ed)
                        {
                            TempData["saveMassage"] = Notification.Show("محدوده سال مالی رعایت نشده است", type: ToastType.Warning, position: ToastPosition.TopCenter);
                            return RedirectToAction("FiscalPeriods");
                        }
                    }
                    Fiscal.StartDate = sd;
                    Fiscal.EndDate = ed;
                    Fiscal.Vat = byte.Parse(vat);
                    Fiscal.Name = FiscalYearName;
                    if (_fiscalRepo.Update(Fiscal))
                    {
                        TempData["saveMassage"] = Notification.Show("اطلاعات سال مالی با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                        return RedirectToAction("FiscalPeriods");
                    }
                    else
                    {
                        TempData["saveMassage"] = Notification.Show("ویرایش اطلاعات سال مالی با خطا رو به رو شد", type: ToastType.Error, position: ToastPosition.TopCenter);
                        return RedirectToAction("FiscalPeriods");
                    }


                }
            }
            else
            {
                TempData["saveMassage"] = Notification.Show("اطلاعات سال مالی را وارد نمایید", type: ToastType.Warning, position: ToastPosition.TopCenter);
                return RedirectToAction("FiscalPeriods");
            }
        }

        [HttpGet]
        [CoustomAuthrize]
        public ActionResult ChangeFiscalYear(short FiscalId)
        {
            if (Request.Cookies.AllKeys.Contains("CurrentFiscalYear"))
            {
                HttpCookie cookie = Request.Cookies["CurrentFiscalYear"];
                cookie.Value = FiscalId.ToString();
                Response.Cookies.Add(cookie);
            }
            else
            {
                // افزودن کوکی 
                var Fiscal = _fiscalRepo.Where(m => m.Id == FiscalId).FirstOrDefault();
                var date = DateTime.Now;
                var cookie = new HttpCookie("CurrentFiscalYear", FiscalId.ToString());
                if (date < Fiscal.EndDate)
                    cookie.Expires = Fiscal.EndDate;
                else
                    cookie.Expires = date.AddMonths(1);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
