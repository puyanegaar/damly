using PunasMarketing.Models.LocalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.Repositories;
using PunasMarketing.Models.DomainModel;
using System.Web.Security;

namespace PunasMarketing.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private readonly PersonnelRepository _blpersonnelRepository;
        private readonly UserRepository _blUserRepository;

        public AccountController(PersonnelRepository personnelRepository, UserRepository userRepository)
        {
            _blpersonnelRepository = personnelRepository;
            _blUserRepository = userRepository;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            if (this.IsCaptchaValid(""))
            {
                User u = new User();
                if (_blpersonnelRepository.Login(login.UserName, login.Password, ref u))
                {
                    if (!u.IsActive)
                    {
                        TempData["BadLogin"] = Notification.Show("حساب کاربری شما توسط مدیریت مسدود شده است", type: ToastType.Warning, position: ToastPosition.TopCenter);
                        return View();
                    }

                    u.LastLogin = DateTime.Now;
                    u.RepeatPassword = "123456";
                    _blUserRepository.Update(u);
                    FormsAuthentication.SetAuthCookie(u.PersonnelId.ToString(), login.Remmeber);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    TempData["BadLogin"] = Notification.Show("نام کاربری یا رمز عبور شما اشتباه می باشد.", type: ToastType.Warning, position: ToastPosition.TopCenter);
                    return View();
                }
            }
            else
            {
                TempData["BadLogin"] = Notification.Show("حاصل عبارت، صحیح نیست.", type: ToastType.Warning, position: ToastPosition.TopCenter);
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult AccessError()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UpdateError()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ClosedFiscalError()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ExpiredFiscalError()
        {
            return View();
        }
    }
}