using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Region;
using System.Web.Mvc;
using PunasMarketing.Models.Enums;

namespace PunasMarketing.Controllers
{
    public class RegionController : Controller
    {
        private readonly RegionRepository _blRegionRepository;
        private readonly CityRepository _blCityRepository;
        private readonly ProvinceRepository _blProvinceRepository;

        public RegionController(
            RegionRepository regionRepository,
            CityRepository cityRepository,
            ProvinceRepository provinceRepository)
        {
            _blRegionRepository = regionRepository;
            _blCityRepository = cityRepository;
            _blProvinceRepository = provinceRepository;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowRegions)]
        public ActionResult Regions()
        {
            return View(_blRegionRepository.Select());
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddRegion)]
        public ActionResult AddRegion()
        {
            RegionViewModel model = new RegionViewModel { Cities = _blCityRepository.Select(), Provinces = _blProvinceRepository.Select() };
            return View(model);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.AddRegion)]
        public ActionResult AddRegion(RegionViewModel regionViewModel)
        {
            if (ModelState.IsValid)
            {
                Region region = regionViewModel.RegionModel;

                if (_blRegionRepository.Add(region))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return RedirectToAction("Regions");
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
            RegionViewModel model = new RegionViewModel { Cities = _blCityRepository.Select(), Provinces = _blProvinceRepository.Select() };
            return View(model);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateRegion)]
        public ActionResult UpdateRegion(int id)
        {
            var region = _blRegionRepository.Find(id);
            if (region == null)
            {
                return View("_Error404");
            }

            region.StateId = _blProvinceRepository.GetProvinceId(region.City.ProvinceId);
            RegionViewModel viewModel = new RegionViewModel
            {
                RegionModel = region,
                Cities = _blCityRepository.Where(a=>a.Id == region.CityId),
                Provinces = _blProvinceRepository.Select()
            };
            return View(viewModel);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.UpdateRegion)]
        public ActionResult UpdateRegion(RegionViewModel regionViewModel)
        {
            if (ModelState.IsValid)
            {
                Region region = regionViewModel.RegionModel;
               
                if (_blRegionRepository.Update(region))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return RedirectToAction("Regions");
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

            RegionViewModel viewModel = new RegionViewModel
            {
                RegionModel = regionViewModel.RegionModel,
                Cities = _blCityRepository.Where(a => a.Id == regionViewModel.RegionModel.CityId),
                Provinces = _blProvinceRepository.Select()
            };
            return View(viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteRegion)]
        public JsonResult DeleteRegion(int id)
        {
            if (_blRegionRepository.Delete(id, out bool isUsed))
            {
                return Json(new JsonData { Success = true });
            }
            else
            {
                return Json(new JsonData { Success = false, IsUsed = isUsed });
            }
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetCities(int? id)
        {
            if (id.HasValue)
            {
                RegionViewModel viewModel = new RegionViewModel();
                viewModel.Cities = _blCityRepository.Where(i => i.ProvinceId == id.Value);
                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_RegionCities", viewModel)
                });
            }
            else
            {
                RegionViewModel viewModel = new RegionViewModel();
                viewModel.Cities = _blCityRepository.Where(i => i.ProvinceId == id.Value);
                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_RegionCities", viewModel)
                });
            }
        }
    }
}