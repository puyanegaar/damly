using System.Web.Mvc;
using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;

namespace PunasMarketing.Controllers
{
    public class SectionController : Controller
    {
        private readonly SectionRepository _blSectionRepository;

        public SectionController(SectionRepository blSectionRepository)
        {
            _blSectionRepository = blSectionRepository;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowSections)]
        public ActionResult Sections()
        {
            return View(_blSectionRepository.Select());
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddSection)]
        public ActionResult AddSection()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddSection)]
        public ActionResult AddSection(Section section)
        {
            if (ModelState.IsValid)
            {
                if (_blSectionRepository.Add(section))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return RedirectToAction("Sections");
                }

                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                ModelState.Clear();
                return View(section);
            }

            TempData["SaveMessage"] = Notification.Show("مقادیر وارد شده صحیح نمی باشند", type: ToastType.Error, position: ToastPosition.TopCenter);
            return View(section);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateSection)]
        public ActionResult UpdateSection(int id)
        {
            var section = _blSectionRepository.Find(id);
            if (section == null)
            {
                return View("_Error404");
            }

            return View(section);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateSection)]
        public ActionResult UpdateSection(Section section)
        {
            if (ModelState.IsValid)
            {
                if (section.Id == 0)
                {
                    TempData["SaveMessage"] = Notification.Show("امکان ویرایش بخش بازاریابی وجود ندارد.", type: ToastType.Warning, position: ToastPosition.TopCenter);
                    return RedirectToAction("Sections");
                }

                if (_blSectionRepository.Update(section))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    ModelState.Clear();
                    return RedirectToAction("Sections");
                }

                TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                ModelState.Clear();
                return View(section);
            }

            TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Error, position: ToastPosition.TopCenter);
            ModelState.Clear();
            return View(section);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteSection)]
        public JsonResult DeleteSection(int id)
        {
            if (id == 0) // بازاریابی
            {
                return Json(new JsonData { Success = false });
            }

            if (_blSectionRepository.Delete(id, out bool isUsed))
            {
                return Json(new JsonData { Success = true });
            }
            else
            {
                return Json(new JsonData { Success = false, IsUsed = isUsed });
            }
        }
    }
}