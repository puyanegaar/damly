using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Marketer;
using PunasMarketing.ViewModels.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PunasMarketing.Controllers
{
    public class MarketerController : Controller
    {
        private readonly PersonnelRepository _blPersonnel;
        private readonly ProvinceRepository _blProvince;
        private readonly CityRepository _blCity;
        private readonly RegionRepository _regionRepo;
        private readonly JobTitleRepository _blJobTitle;
        private readonly SectionRepository _blSection;
        private readonly AcademicDegreeRepository _blAcademic;
        private readonly ActionRepository _blAction;
        private readonly UserAccessRepository _blUserAccess;
        private readonly TafsiliRepository _bltafsili;
        private readonly UserRepository _bluser;
        private readonly FiscalRepository _fiscalRepository;
        private readonly MarketerRegionRepository _marketerRegionRepo;

        public MarketerController(
            PersonnelRepository PersonnelRepository,
            ProvinceRepository provinceRepostiory,
            CityRepository cityRepository,
            RegionRepository regionRepo,
            SectionRepository sectionRepostiory,
            JobTitleRepository jobTitleRepository,
            AcademicDegreeRepository AcademicRepository,
            ActionRepository actionRepository,
            UserAccessRepository userAccessRepository,
            TafsiliRepository tafsiliRepository,
            UserRepository userRepository,
            FiscalRepository fiscalRepository,
            MarketerRegionRepository marketerRegionRepo)
        {
            _blPersonnel = PersonnelRepository;
            _blProvince = provinceRepostiory;
            _blCity = cityRepository;
            _regionRepo = regionRepo;
            _blJobTitle = jobTitleRepository;
            _blSection = sectionRepostiory;
            _blAcademic = AcademicRepository;
            _blAction = actionRepository;
            _blUserAccess = userAccessRepository;
            _bltafsili = tafsiliRepository;
            _bluser = userRepository;
            _fiscalRepository = fiscalRepository;
            _marketerRegionRepo = marketerRegionRepo;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowMarketers)]
        public ActionResult Marketers()
        {
            var viewModel = new PersonnelListViewModel
            {
                Personnels = _blPersonnel.Where(i => i.JobTitleId == 0),
                JobTitles = _blJobTitle.Select()
            };

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddMarketer)]
        public ActionResult AddMarketer()
        {
            var model = new MarketerViewModel();
            model.Provinces = _blProvince.Select();
            model.Academics = _blAcademic.Select();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddMarketer)]
        public ActionResult AddMarketer(HttpPostedFileBase ImageFile, MarketerViewModel viewModel)
        {
            Personnel marketer = viewModel.Marketer;

            viewModel.Provinces = _blProvince.Select();
            viewModel.Academics = _blAcademic.Select();

            if (ModelState.IsValid)
            {
                if (_blPersonnel.CodeExisits(marketer.PersonnelCode))
                {
                    TempData["SaveMessage"] = Notification.Show("کد پرسنلی تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                if (_blPersonnel.CodeMelliExists(marketer.CodeMelli))
                {
                    TempData["SaveMessage"] = Notification.Show("کد ملی تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                if (_blPersonnel.Mobile1Exists(marketer.Mobile1))
                {
                    TempData["SaveMessage"] = Notification.Show("شماره موبایل اصلی تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                #region Upload Image
                string fileName = "";
                int flag = ImageFile.UploadImage("Image\\Marketers", "Marketer", ref fileName, 1000000);
                if (flag == 1 || flag == 0)
                {
                    marketer.ImageName = fileName;
                    marketer.BirthDate = viewModel.StrBirthDate.ToMiladiDate();
                    marketer.HireDate = viewModel.StrHireDate.ToMiladiDate();

                    marketer.SectionId = 0;
                    marketer.JobTitleId = 0;
                    marketer.IsGettingSalary = false;

                    if (_blPersonnel.Add(marketer))
                    {
                        int id = _blPersonnel.GetLastIdentity();

                        #region Add User

                        var keyNew = Utilities.GeneratePassword(20);
                        var password = Utilities.EncodePassword(marketer.CodeMelli, keyNew);
                        User user = new User
                        {
                            PersonnelId = id,
                            Username = marketer.PersonnelCode,
                            SaltCode = keyNew.EncryptString("#+Samoosh!Group@+Generate$key+#"),
                            Password = password.EncryptString("#+Samoosh!Group@+Generate$pas+#"),
                            Samoosh = (byte)0.RandomNumber(100, 200),
                            RepeatPassword = "foofoo",
                            LastLogin = DateTime.Now,
                            IsActive = false
                        };
                        _bluser.Add(user);
                        #endregion


                        List<Tafsili> tafsilis = new List<Tafsili>
                        {
                            new Tafsili
                            {
                                PersonnelId = id,
                                SarfaslId = (short) SarfaslEnums.HesabDaryaftani,
                                PersonTypeId = 1,
                                TafsiliName = marketer.FullName
                            },
                            new Tafsili
                            {
                                PersonnelId = id,
                                SarfaslId = (short) SarfaslEnums.AsnadDaryftani,
                                PersonTypeId = 1,
                                TafsiliName = marketer.FullName
                            },
                            new Tafsili
                            {
                                PersonnelId = id,
                                SarfaslId = (short) SarfaslEnums.HesabPardakhtani,
                                PersonTypeId = 1,
                                TafsiliName = marketer.FullName
                            }
                        };

                        _bltafsili.AddRange(tafsilis);

                        if (_bltafsili.AddRange(tafsilis))
                        {
                            TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                            ModelState.Clear();
                            return RedirectToAction("MarketerManagement", new { id = _blPersonnel.GetLastIdentity() });
                        }
                        else
                        {
                            TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات حساب های تفصیلی", type: ToastType.Error, position: ToastPosition.TopCenter);
                            ModelState.Clear();
                            return RedirectToAction("MarketerManagement", new { id = _blPersonnel.GetLastIdentity() });
                        }
                    }
                    else
                    {
                        fileName.deleteImage("Image\\Marketers");
                        TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                    }
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show(flag.UploadMessage(), type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
                #endregion
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning);
            }

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateMarketer)]
        public ActionResult UpdateMarketer(int id)
        {
            var dbMarketer = _blPersonnel.Find(id);
            if (dbMarketer == null)
            {
                return View("_Error404");
            }

            dbMarketer.BirthStateId = _blProvince.GetProvinceId(dbMarketer.City.ProvinceId);
            dbMarketer.HomeStateId = _blProvince.GetProvinceId(dbMarketer.City1.ProvinceId);
            MarketerViewModel viewModel = new MarketerViewModel
            {
                Marketer = dbMarketer,
                Provinces = _blProvince.Select(),
                Cities = _blCity.Select(),
                Academics = _blAcademic.Select(),
                StrBirthDate = dbMarketer.BirthDate.ToPersianDateTime().ToStringDate(),
                StrHireDate = dbMarketer.HireDate.ToPersianDateTime().ToStringDate(),
                StrFireDate = dbMarketer.FireDate?.ToPersianDateTime().ToStringDate()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AccessControl(ActionsEnum.UpdateMarketer)]
        public ActionResult UpdateMarketer(HttpPostedFileBase ImageFile, MarketerViewModel viewModel)
        {
            Personnel marketer = viewModel.Marketer;
            Personnel dbMarketer = _blPersonnel.Find(marketer.Id);

            dbMarketer.BirthStateId = _blProvince.GetProvinceId(dbMarketer.City.ProvinceId);
            dbMarketer.HomeStateId = _blProvince.GetProvinceId(dbMarketer.City1.ProvinceId);
            viewModel.Marketer = dbMarketer;
            viewModel.Provinces = _blProvince.Select();
            viewModel.Cities = _blCity.Select();
            viewModel.Academics = _blAcademic.Select();

            marketer.BirthDate = viewModel.StrBirthDate.ToMiladiDate();
            marketer.HireDate = viewModel.StrHireDate.ToMiladiDate();
            marketer.FireDate = viewModel.StrFireDate?.ToMiladiDate();

            string oldImageName = dbMarketer.ImageName ?? "";

            if (ModelState.IsValid)
            {
                if (dbMarketer.PersonnelCode.Trim() != marketer.PersonnelCode.Trim() && _blPersonnel.CodeExisits(marketer.PersonnelCode))
                {
                    TempData["SaveMessage"] = Notification.Show("کد پرسنلی تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                if (dbMarketer.CodeMelli.Trim() != marketer.CodeMelli.Trim() && _blPersonnel.CodeMelliExists(marketer.PersonnelCode))
                {
                    TempData["SaveMessage"] = Notification.Show("کد ملی تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                if (dbMarketer.Mobile1.Trim() != marketer.Mobile1.Trim() && _blPersonnel.Mobile1Exists(marketer.Mobile1))
                {
                    TempData["SaveMessage"] = Notification.Show("شماره موبایل تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }


                marketer.SectionId = 0;
                marketer.JobTitleId = 0;
                marketer.IsGettingSalary = false;


                #region Upload
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = "";
                    int flag = ImageFile.UploadImage("Image\\Marketers", "Marketer", ref fileName, 1000000);
                    if (flag == 1 || flag == 0)
                    {
                        marketer.ImageName = fileName;

                        if (_blPersonnel.Update(marketer))
                        {
                            if (_blPersonnel.UpadteTafsili(viewModel.Marketer.Id, viewModel.Marketer.FullName))
                            {
                                TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                                ModelState.Clear();
                                if (!string.IsNullOrEmpty(oldImageName))
                                {
                                    oldImageName.deleteImage("Image\\Marketers");
                                }

                                return RedirectToAction("MarketerManagement", new { id = viewModel.Marketer.Id });
                            }
                            else
                            {
                                TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات حساب تفصیلی", type: ToastType.Warning, position: ToastPosition.TopCenter);
                            }
                        }
                        else
                        {
                            fileName.deleteImage("Image\\Marketers");
                            TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات ", type: ToastType.Error, position: ToastPosition.TopCenter);
                        }
                    }
                    else
                    {
                        TempData["SaveMessage"] = Notification.Show(flag.UploadMessage(), type: ToastType.Warning, position: ToastPosition.TopCenter);
                    }
                }
                else
                {
                    marketer.ImageName = dbMarketer.ImageName;

                    if (_blPersonnel.Update(marketer))
                    {
                        if (_blPersonnel.UpadteTafsili(viewModel.Marketer.Id, viewModel.Marketer.FullName))
                        {
                            TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                            ModelState.Clear();
                            return RedirectToAction("MarketerManagement", new { id = viewModel.Marketer.Id });
                        }
                        else
                        {
                            TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات حساب تفصیلی", type: ToastType.Warning, position: ToastPosition.TopCenter);
                        }
                    }
                    else
                    {
                        TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات ", type: ToastType.Error, position: ToastPosition.TopCenter);
                    }
                }
                #endregion
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning);
            }

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.MarketerManagement)]
        public ActionResult MarketerManagement(int? id)
        {
            return RedirectToAction("PersonnelManagement", "Personnel", new { id = id });
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.MarketerManagement)]
        public ActionResult AccessDefine(int UserId, short[] AccessProperty, short Section)
        {
            var UserAccess = _blUserAccess.Where(m => m.UserId == UserId && m.Action.SectionId == Section);
            short[] CurrentActions = UserAccess.Select(m => m.ActionId).ToArray();
            if (UserAccess.Any())
            {
                for (int i = 0; i < AccessProperty.Length; i++)
                {
                    if (!CurrentActions.Contains(AccessProperty[i]) && AccessProperty[i] != 0)
                    {
                        _blUserAccess.Add(AccessProperty[i], UserId, false);
                    }

                }

                //Edit
                foreach (var item in UserAccess)
                {
                    if (!AccessProperty.Contains(item.ActionId))
                    {
                        _blUserAccess.Delete(item, false);
                    }
                }

                if (_blUserAccess.Save())
                {
                    return Json(new JsonData
                    {
                        Script = Notification.BriefShow("دسترسی ها با موفقیت ثبت شد", type: ToastType.Success),
                    });
                }
                else
                {
                    return Json(new JsonData
                    {
                        Script = Notification.BriefShow("خطا در ثبت دسترسی ها", type: ToastType.Success),
                    });
                }
            }
            else
            {
                //New
                for (int i = 1; i < AccessProperty.Length; i++)
                {
                    _blUserAccess.Add(AccessProperty[i], UserId, false);
                }

                if (_blUserAccess.Save() || AccessProperty.Length == 1)
                {
                    return Json(new JsonData
                    {
                        Script = Notification.BriefShow("دسترسی ها با موفقیت ثبت شد", type: ToastType.Success),
                    });
                }
                else
                {
                    return Json(new JsonData
                    {
                        Script = Notification.BriefShow("خطا در ثبت دسترسی ها", type: ToastType.Success),
                    });
                }
            }

        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetBirthCities(int? id)
        {
            MarketerViewModel viewModel = new MarketerViewModel();
            if (id.HasValue)
            {
                viewModel.Cities = _blCity.Where(i => i.ProvinceId == id.Value);
            }
            else
            {
                viewModel.Cities = _blCity.Where(i => i.ProvinceId == 0);
            }
            return Json(new JsonData
            {
                Html = this.RenderPartialToString("_BirthCities", viewModel),
                Success = true
            });
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetHomeCities(int? id)
        {
            MarketerViewModel viewModel = new MarketerViewModel();
            if (id.HasValue)
            {
                viewModel.Cities = _blCity.Where(i => i.ProvinceId == id.Value);
            }
            else
            {
                viewModel.Cities = _blCity.Where(i => i.ProvinceId == 0);
            }
            return Json(new JsonData
            {
                Html = this.RenderPartialToString("_HomeCities", viewModel),
                Success = true
            });
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.MarketerManagement)]
        public ActionResult GetActionGroup(int? id, int? UserId)
        {
            var model = new MarketerMngtViewModel();
            if (id.HasValue)
            {
                model.Actions = _blAction.Where(i => i.SectionId == id.Value);
                if (model.Actions.Any())
                {
                    model.SectionId = model.Actions.FirstOrDefault().SectionId;
                }
                else
                {
                    model.SectionId = 0;
                }

                model.ActionsId = _blUserAccess.Where(m => m.UserId == UserId && m.Action.SectionId == model.SectionId).Select(m => m.ActionId).ToArray();
            }
            else
            {
                model.Actions = _blAction.Where(i => i.SectionId == 0);
                model.SectionId = model.Actions.FirstOrDefault().SectionId;
                model.ActionsId = _blUserAccess.Where(m => m.UserId == UserId && m.Action.SectionId == model.SectionId).Select(m => m.ActionId).ToArray();
            }
            return Json(new JsonData
            {
                Html = this.RenderPartialToString("_ActionGroup", model),
                Success = true
            });
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.MarketerManagement)]
        public ActionResult GetRegions(int cityId)
        {
            return Json(new JsonData
            {
                Html = this.RenderPartialToString("_Regions", _regionRepo.Where(
                    i => i.CityId == cityId).ToList())
            });
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.MarketerManagement)]
        public ActionResult AssignRegionsForMarketer(AssignRegionViewModel viewModel)
        {
            JsonResult warningResult = Json(new JsonData
            {
                Script = Notification.BriefShow("خطا در ثبت مناطق شهری برای بازاریاب!", type: ToastType.Warning)
            });

            List<MarketerRegion> marketerRegions = new List<MarketerRegion>();
            var regions = _regionRepo.Select();

            foreach (var regionName in viewModel.RegionsName)
            {
                var regionId = regions.FirstOrDefault(i => i.Name == regionName)?.Id;
                if (!regionId.HasValue)
                {
                    return warningResult;
                }

                marketerRegions.Add(new MarketerRegion
                {
                    MarketerId = viewModel.MarketerId,
                    RegionId = regionId.Value
                });
            }

            _marketerRegionRepo.DeleteRange(viewModel.MarketerId);

            bool isAdded = _marketerRegionRepo.AddRange(marketerRegions);
            if (!isAdded)
            {
                return warningResult;
            }

            return Json(new JsonData
            {
                Script = Notification.BriefShow("مناطق شهری با موفقیت به بازاریاب اختصاص داده شد.", type: ToastType.Success)
            });
        }
    }
}