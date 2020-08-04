using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.OtherTafsili;
using System.Linq;
using System.Web.Mvc;
using PunasMarketing.Models.Enums;

namespace PunasMarketing.Controllers
{
    public class OtherTafsiliController : Controller
    {
        private readonly OtherTafsiliRepository _otherTafsiliRepo;
        private readonly SarfaslRepository _sarfaslRepo;
        private readonly TafsiliRepository _tafsiliRepo;

        public OtherTafsiliController(
            OtherTafsiliRepository otherTafsiliRepo,
            SarfaslRepository sarfaslRepo,
            TafsiliRepository tafsiliRepo)
        {
            _otherTafsiliRepo = otherTafsiliRepo;
            _sarfaslRepo = sarfaslRepo;
            _tafsiliRepo = tafsiliRepo;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowTafsilies)]
        public ActionResult OtherTafsilis()
        {
            return View(GetOtherTafsiliListViewModel());
        }

        private OtherTafsiliListViewModel GetOtherTafsiliListViewModel()
        {
            OtherTafsiliListViewModel viewModel = new OtherTafsiliListViewModel
            {
                OtherTafsilis = _otherTafsiliRepo.Select(),
                MoeenSarfasls = _sarfaslRepo.Where(i => i.IsMoeen)
            };

            return viewModel;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddTafsili)]
        public ActionResult AddOtherTafsiliModal()
        {
            AddUpdateOtherTafsiliViewModel viewModel = new AddUpdateOtherTafsiliViewModel
            {
                OtherTafsili = new OtherTafsili(),
                MoeenSarfasls = _sarfaslRepo.Where(i => i.IsMoeen).OrderBy(i => i.Coding)
            };
            return PartialView("_AddOtherTafsili", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddTafsili)]
        public ActionResult AddOtherTafsiliModal(AddUpdateOtherTafsiliViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (_otherTafsiliRepo.Add(viewModel.OtherTafsili))
                {
                    short otherTafsiliMaxId = _otherTafsiliRepo.GetMaxId();
                    Tafsili newTafsili = new Tafsili
                    {
                        OtherTafsiliId = otherTafsiliMaxId,
                        SarfaslId = viewModel.OtherTafsili.SarfaslId,
                        TafsiliName = viewModel.OtherTafsili.Name
                    };

                    if (_tafsiliRepo.Add(newTafsili))
                    {
                        TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                        ModelState.Clear();
                        return RedirectToAction("OtherTafsilis");
                    }
                    else
                    {
                        TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات حسابهای تفصیلی", type: ToastType.Error, position: ToastPosition.TopCenter);
                    }
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات سایر تفصیلی ها", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return RedirectToAction("OtherTafsilis");
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateTafsili)]
        public ActionResult UpdateOtherTafsiliModal(short id)
        {
            OtherTafsili otherTafsili = _otherTafsiliRepo.Find(id);
            if (otherTafsili == null)
            {
                return View("_Error404");
            }

            AddUpdateOtherTafsiliViewModel viewModel = new AddUpdateOtherTafsiliViewModel
            {
                OtherTafsili = otherTafsili,
                MoeenSarfasls = _sarfaslRepo.Where(i => i.IsMoeen).OrderBy(i => i.Coding)
            };

            Tafsili theTafsili = _tafsiliRepo.FindByOtherTafsiliId(id);
            if (theTafsili?.SarfaslId != null)
            {
                viewModel.OtherTafsili.SarfaslId = theTafsili.SarfaslId.Value;
            }

            return PartialView("_UpdateOtherTafsili", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateTafsili)]
        public ActionResult UpdateOtherTafsiliModal(AddUpdateOtherTafsiliViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                OtherTafsili otherTafsili = viewModel.OtherTafsili;

                if (_otherTafsiliRepo.Update(otherTafsili))
                {
                    Tafsili tafsili = _tafsiliRepo.FindByOtherTafsiliId(otherTafsili.Id);
                    if (tafsili == null)
                    {
                        TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                        return RedirectToAction("OtherTafsilis", GetOtherTafsiliListViewModel());
                    }

                    tafsili.SarfaslId = otherTafsili.SarfaslId;
                    tafsili.TafsiliName = otherTafsili.Name;

                    if (_tafsiliRepo.Update(tafsili))
                    {
                        TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                        ModelState.Clear();
                        return RedirectToAction("OtherTafsilis", GetOtherTafsiliListViewModel());
                    }
                    else
                    {
                        TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات حساب های تفصیلی", type: ToastType.Error, position: ToastPosition.TopCenter);
                    }
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات سایر تفصیلیها", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return RedirectToAction("OtherTafsilis", GetOtherTafsiliListViewModel());
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteTafsili)]
        public JsonResult DeleteOtherTafsili(short id)
        {
            if (_otherTafsiliRepo.Delete(id, out bool isUsed))
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