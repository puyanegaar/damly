using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.JobTitle;
using System.Web.Mvc;
using PunasMarketing.Helpers.Filters;
using PunasMarketing.Models.Enums;

namespace PunasMarketing.Controllers
{
    public class JobTitleController : Controller
    {
        
        private readonly JobTitleRepository _blJobTitle;
        private readonly SectionRepository _blSection;

        public JobTitleController(JobTitleRepository blJobTitle, SectionRepository blSection)
        {
            _blJobTitle = blJobTitle;
            _blSection = blSection;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowJobTitles)]
        public ActionResult JobTitles()
        {
           return View(_blJobTitle.Select());
        }


        [HttpGet]
        [AccessControl(ActionsEnum.AddJobTitle)]
        public ActionResult AddJobTitle()
        {
            CreateJobTitleViewModel model = new CreateJobTitleViewModel { Sections = _blSection.Select() };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddJobTitle)]
        public ActionResult AddJobTitle(JobTitle jobTitleModel)
        {
            if (ModelState.IsValid)
            {
                if (_blJobTitle.Add(jobTitleModel))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    ModelState.Clear();
                    return RedirectToAction("JobTitles");
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            CreateJobTitleViewModel model = new CreateJobTitleViewModel { Sections = _blSection.Select() };
            return View(model);
        }


        [HttpGet]
        [AccessControl(ActionsEnum.UpdateJobTitle)]
        public ActionResult UpdateJobTitle(int id)
        {
            if (id == 0)
            {
                return View("_Error404");
            }

            var jobTitle = _blJobTitle.Find(id);
            if (jobTitle == null)
            {
                return View("_Error404");
            }

            CreateJobTitleViewModel model = new CreateJobTitleViewModel
            {
                Sections = _blSection.Select(),
                JobTitleModel = jobTitle
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateJobTitle)]
        public ActionResult UpdatejobTitle(JobTitle jobTitleModel)
        {
            if (ModelState.IsValid)
            {
                if (jobTitleModel.Id == 0)
                {
                    TempData["SaveMessage"] = Notification.Show("امکان ویرایش عنوان شغلی بازاریاب وجود ندارد.", type: ToastType.Warning, position: ToastPosition.TopCenter);
                    return RedirectToAction("JobTitles");
                }

                if (_blJobTitle.Update(jobTitleModel))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    ModelState.Clear();
                    return RedirectToAction("JobTitles");
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات", type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            CreateJobTitleViewModel model = new CreateJobTitleViewModel { Sections = _blSection.Select() };
            return View(model);

        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteJobTitle)]
        public JsonResult DeleteJobTitle(int id)
        {
            if (id == 0)
            {
                return Json(new JsonData { Success = false });
            }

            if (_blJobTitle.Delete(id, out bool isUsed))
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