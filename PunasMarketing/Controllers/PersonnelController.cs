using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Personnel;
using PunasMarketing.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PunasMarketing.Controllers
{
    public class PersonnelController : Controller
    {
        private readonly PersonnelRepository _blPersonnel;
        private readonly ProvinceRepository _blProvince;
        private readonly CityRepository _blCity;
        private readonly RegionRepository _regionRepo;
        private readonly MarketerRegionRepository _marketerRegionRepo;
        private readonly JobTitleRepository _blJobTitle;
        private readonly SectionRepository _blSection;
        private readonly AcademicDegreeRepository _blAcademic;
        private readonly ActionRepository _blAction;
        private readonly UserAccessRepository _blUserAccess;
        private readonly UserRepository _blUser;
        private readonly TafsiliRepository _blTafsili;
        private readonly FiscalRepository _fiscalRepository;

        public PersonnelController(
            PersonnelRepository personnelRepository,
            ProvinceRepository provinceRepostiory,
            CityRepository cityRepository,
            RegionRepository regionRepo,
            MarketerRegionRepository marketerRegionRepo,
            SectionRepository sectionRepostiory,
            JobTitleRepository jobTitleRepository,
            AcademicDegreeRepository academicRepository,
            ActionRepository actionRepository,
            UserAccessRepository userAccessRepository,
            UserRepository userRepository,
            TafsiliRepository tafsiliRepository,
            FiscalRepository fiscalRepository)
        {
            _blPersonnel = personnelRepository;
            _blProvince = provinceRepostiory;
            _blCity = cityRepository;
            _regionRepo = regionRepo;
            _marketerRegionRepo = marketerRegionRepo;
            _blJobTitle = jobTitleRepository;
            _blSection = sectionRepostiory;
            _blAcademic = academicRepository;
            _blAction = actionRepository;
            _blUserAccess = userAccessRepository;
            _blUser = userRepository;
            _blTafsili = tafsiliRepository;
            _fiscalRepository = fiscalRepository;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowPersonnels)]
        public ActionResult Personnels()
        {
            var viewModel = new PersonnelListViewModel
            {
                Personnels = _blPersonnel.Where(i => i.JobTitleId != 0),
                JobTitles = _blJobTitle.Select()
            };

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddPersonnel)]
        public ActionResult AddPersonnel()
        {
            var viewModel = new PersonnelViewModel
            {
                Provinces = _blProvince.Select(),
                Sections = _blSection.Where(i => i.Id != 0),
                Academics = _blAcademic.Select()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddPersonnel)]
        public ActionResult AddPersonnel(HttpPostedFileBase ImageFile, PersonnelViewModel viewModel)
        {
            var personnel = viewModel.personnel;

            viewModel.Provinces = _blProvince.Select();
            viewModel.Sections = _blSection.Select();
            viewModel.Academics = _blAcademic.Select();

            if (ModelState.IsValid)
            {
                if (_blPersonnel.CodeExisits(personnel.PersonnelCode))
                {
                    TempData["SaveMessage"] = Notification.Show("کد پرسنلی تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                if (_blPersonnel.CodeMelliExists(personnel.CodeMelli))
                {
                    TempData["SaveMessage"] = Notification.Show("کد ملی تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                if (_blPersonnel.Mobile1Exists(personnel.Mobile1))
                {
                    TempData["SaveMessage"] = Notification.Show("شماره موبایل اصلی تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                #region Upload Image
                string fileName = "";
                int flag = ImageFile.UploadImage("Image\\Personnels", "Personnel", ref fileName, 1000000);
                if (flag == 1 || flag == 0)
                {
                    personnel.ImageName = fileName;
                    personnel.BirthDate = viewModel.StrBirthDate.ToMiladiDate();
                    personnel.HireDate = viewModel.StrHireDate.ToMiladiDate();

                    if (_blPersonnel.Add(personnel))
                    {
                        int id = _blPersonnel.GetLastIdentity();
                        if (personnel.SystemUser)
                        {
                            var keyNew = Utilities.GeneratePassword(20);
                            var password = Utilities.EncodePassword(personnel.CodeMelli, keyNew);
                            User user = new User
                            {
                                PersonnelId = id,
                                Username = personnel.PersonnelCode,
                                SaltCode = keyNew.EncryptString("#+Samoosh!Group@+Generate$key+#"),
                                Password = password.EncryptString("#+Samoosh!Group@+Generate$pas+#"),
                                Samoosh = (byte)0.RandomNumber(100, 200),
                                LastLogin = DateTime.Now,
                                IsActive = false,
                                RepeatPassword = "foofoo"
                            };
                            _blUser.Add(user);
                        }

                        List<Tafsili> tafsilis = new List<Tafsili>
                        {
                            new Tafsili
                            {
                                PersonnelId = id,
                                SarfaslId = (short) SarfaslEnums.HesabDaryaftani,
                                PersonTypeId = 1,
                                TafsiliName = personnel.FullName
                            },
                            new Tafsili
                            {
                                PersonnelId = id,
                                SarfaslId = (short) SarfaslEnums.AsnadDaryftani,
                                PersonTypeId = 1,
                                TafsiliName = personnel.FullName
                            },
                            new Tafsili
                            {
                                PersonnelId = id,
                                SarfaslId = (short) SarfaslEnums.HesabPardakhtani,
                                PersonTypeId = 1,
                                TafsiliName = personnel.FullName
                            }
                        };

                        if (_blTafsili.AddRange(tafsilis))
                        {
                            TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                            ModelState.Clear();
                            return RedirectToAction("PersonnelManagement", new { id = _blPersonnel.GetLastIdentity() });
                        }
                        else
                        {
                            TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات حساب های تفصیلی", type: ToastType.Error, position: ToastPosition.TopCenter);
                            ModelState.Clear();
                            return RedirectToAction("PersonnelManagement", new { id = _blPersonnel.GetLastIdentity() });
                        }
                    }
                    else
                    {
                        fileName.deleteImage("Image\\Personnels");
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
        [AccessControl(ActionsEnum.UpdatePersonnel)]
        public ActionResult UpdatePersonnel(int? id)
        {
            if (!id.HasValue)
            {
                return View("_Error404");
            }

            Personnel dbPersonnel = _blPersonnel.Find(id.Value);
            if (dbPersonnel == null)
            {
                return View("_Error404");
            }

            var viewModel = new PersonnelViewModel
            {
                Provinces = _blProvince.Select(),
                Cities = _blCity.Select(),
                Sections = _blSection.Where(i => i.Id != 0),
                JobTitles = _blJobTitle.Where(i => i.Id != 0),
                Academics = _blAcademic.Select(),
                personnel = dbPersonnel,

                StrBirthDate = dbPersonnel.BirthDate.ToPersianDateTime().ToStringDate(),
                StrHireDate = dbPersonnel.HireDate.ToPersianDateTime().ToStringDate(),
                StrFireDate = dbPersonnel.FireDate?.ToPersianDateTime().ToStringDate()
            };

            viewModel.personnel.BirthStateId = dbPersonnel.City.ProvinceId;
            viewModel.personnel.HomeStateId = dbPersonnel.City1.ProvinceId;
            viewModel.personnel.SectionId = dbPersonnel.JobTitle.SectionId;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AccessControl(ActionsEnum.UpdatePersonnel)]
        public ActionResult UpdatePersonnel(HttpPostedFileBase ImageFile, PersonnelViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Personnel dbPersonnel = _blPersonnel.Find(viewModel.personnel.Id);
                string oldImageName = dbPersonnel.ImageName ?? "";

                viewModel.personnel.BirthDate = viewModel.StrBirthDate.ToMiladiDate();
                viewModel.personnel.HireDate = viewModel.StrHireDate.ToMiladiDate();
                viewModel.personnel.FireDate = viewModel.StrFireDate?.ToMiladiDate();

                if (dbPersonnel.PersonnelCode.Trim() != viewModel.personnel.PersonnelCode.Trim() && _blPersonnel.CodeExisits(viewModel.personnel.PersonnelCode))
                {
                    TempData["SaveMessage"] = Notification.Show("کد پرسنلی تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                if (dbPersonnel.CodeMelli.Trim() != viewModel.personnel.CodeMelli.Trim() && _blPersonnel.CodeMelliExists(viewModel.personnel.PersonnelCode))
                {
                    TempData["SaveMessage"] = Notification.Show("کد ملی تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                if (dbPersonnel.Mobile1.Trim() != viewModel.personnel.Mobile1.Trim() && _blPersonnel.Mobile1Exists(viewModel.personnel.Mobile1))
                {
                    TempData["SaveMessage"] = Notification.Show("شماره موبایل تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = "";
                    int flag = ImageFile.UploadImage("Image\\Personnels", "Personnel", ref fileName, 1000000);
                    if (flag == 1 || flag == 0)
                    {
                        viewModel.personnel.ImageName = fileName;

                        if (_blPersonnel.Update(viewModel.personnel))
                        {
                            if (viewModel.personnel.SystemUser)
                            {
                                if (!_blUser.Exists(viewModel.personnel.Id))
                                {
                                    var keyNew = Utilities.GeneratePassword(20);
                                    var password = Utilities.EncodePassword(viewModel.personnel.CodeMelli, keyNew);
                                    User user = new User
                                    {
                                        PersonnelId = viewModel.personnel.Id,
                                        Username = viewModel.personnel.PersonnelCode,
                                        SaltCode = keyNew.EncryptString("#+Samoosh!Group@+Generate$key+#"),
                                        Password = password.EncryptString("#+Samoosh!Group@+Generate$pas+#"),
                                        Samoosh = (byte)0.RandomNumber(100, 200),
                                        LastLogin = DateTime.Now,
                                        IsActive = false,
                                        RepeatPassword = "foofoo"
                                    };

                                    _blUser.Add(user);
                                }
                            }
                            else
                            {
                                if (_blUser.Exists(viewModel.personnel.Id))
                                {
                                    _blUser.Delete(viewModel.personnel.Id);
                                }
                            }

                            if (_blPersonnel.UpadteTafsili(viewModel.personnel.Id, viewModel.personnel.FullName))
                            {
                                TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                                ModelState.Clear();
                                if (!string.IsNullOrEmpty(oldImageName))
                                {
                                    oldImageName.deleteImage("Image\\Personnels");
                                }
                                return RedirectToAction("PersonnelManagement", new { id = viewModel.personnel.Id });
                            }
                            else
                            {
                                TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات حساب تفصیلی", type: ToastType.Warning, position: ToastPosition.TopCenter);
                            }
                        }
                        else
                        {
                            fileName.deleteImage("Image\\Personnels");
                            TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات پرسنل ", type: ToastType.Error, position: ToastPosition.TopCenter);
                        }
                    }
                }
                else
                {
                    viewModel.personnel.ImageName = dbPersonnel.ImageName;

                    if (_blPersonnel.Update(viewModel.personnel))
                    {
                        if (viewModel.personnel.SystemUser)
                        {
                            if (!_blUser.Exists(viewModel.personnel.Id))
                            {
                                var keyNew = Utilities.GeneratePassword(20);
                                var password = Utilities.EncodePassword(viewModel.personnel.CodeMelli, keyNew);
                                User user = new User
                                {
                                    PersonnelId = viewModel.personnel.Id,
                                    Username = viewModel.personnel.CodeMelli,
                                    SaltCode = keyNew.EncryptString("#+Samoosh!Group@+Generate$key+#"),
                                    Password = password.EncryptString("#+Samoosh!Group@+Generate$pas+#"),
                                    Samoosh = (byte)0.RandomNumber(100, 200),
                                    LastLogin = DateTime.Now,
                                    IsActive = false
                                };

                                _blUser.Add(user);
                            }
                        }
                        else
                        {
                            if (_blUser.Exists(viewModel.personnel.Id))
                            {
                                _blUser.Delete(viewModel.personnel.Id);
                            }
                        }

                        if (_blPersonnel.UpadteTafsili(viewModel.personnel.Id, viewModel.personnel.FullName))
                        {
                            TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                            ModelState.Clear();
                            return RedirectToAction("PersonnelManagement", new { id = viewModel.personnel.Id });
                        }
                        else
                        {
                            TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات حساب تفصیلی", type: ToastType.Warning, position: ToastPosition.TopCenter);
                        }
                    }
                    else
                    {
                        TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات پرسنل", type: ToastType.Warning, position: ToastPosition.TopCenter);
                    }
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            viewModel.Provinces = _blProvince.Select();
            viewModel.Cities = _blCity.Select();
            viewModel.Sections = _blSection.Select();
            viewModel.JobTitles = _blJobTitle.Select();
            viewModel.Academics = _blAcademic.Select();

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.PersonnelManagement)]
        public ActionResult PersonnelManagement(int? id)
        {
            if (!id.HasValue)
            {
                return View("_Error404");
            }

            Personnel personnel = _blPersonnel.Find(id.Value);

            if (personnel == null)
            {
                return View("_Error404");
            }

            var model = new PersonnelMngtViewModel
            {
                sections = _blSection.Select(),
                personnel = personnel,
                User = _blUser.FindById(id.Value),
                FinancialReport = _blPersonnel.GetFinancialReport(Fiscal.GetFiscalId(), personnel.Id),
                Provinces = _blProvince.Select().ToList(),
                Cities = new List<City>(),
                MarketerRegions = personnel.MarketerRegions.ToList()
            };

            var regionsCityId = model.MarketerRegions.FirstOrDefault()?.Region.CityId;
            if (regionsCityId != null)
            {
                model.Regions = _regionRepo.Where(i => regionsCityId != null && i.CityId == regionsCityId).ToList();
                model.ProvinceId = model.Regions.FirstOrDefault().City.ProvinceId;
                model.Cities = _blCity.Where(i => i.ProvinceId == model.ProvinceId).ToList();
                model.CityId = model.Regions.FirstOrDefault().CityId;
            }

            model.SectionId = model.personnel.JobTitle.SectionId;
            model.Actions = _blAction.Where(m => m.SectionId == model.SectionId);
            model.ActionsId = _blUserAccess.Where(m => m.UserId == id && m.Action.SectionId == model.SectionId).Select(m => m.ActionId).ToArray();
            model.AcademicDegrees = _blAcademic.Select();

            return View(model);
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.PersonnelManagement)]
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
                        Script = Notification.BriefShow("خطا در ثبت دسترسی ها", type: ToastType.Warning),
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
                        Script = Notification.BriefShow("خطا در ثبت دسترسی ها", type: ToastType.Warning),
                    });
                }
            }

        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetBirthCities(int? id)
        {
            PersonnelViewModel viewModel = new PersonnelViewModel();
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
            PersonnelViewModel viewModel = new PersonnelViewModel();
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
        [CoustomAuthrize]
        public ActionResult GetJobTilte(int? id)
        {
            PersonnelViewModel viewModel = new PersonnelViewModel();
            if (id.HasValue)
            {
                viewModel.JobTitles = _blJobTitle.Where(i => i.SectionId == id.Value);
            }
            else
            {
                viewModel.JobTitles = _blJobTitle.Where(i => i.SectionId == id.Value);
            }
            return Json(new JsonData
            {
                Html = this.RenderPartialToString("_JobTitles", viewModel),
                Success = true
            });
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.PersonnelManagement)]
        public ActionResult GetActionGroup(int? id, int? UserId)
        {
            var model = new PersonnelMngtViewModel();
            if (id.HasValue)
            {
                model.Actions = _blAction.Where(i => i.SectionId == id.Value);
                if (model.Actions.Any())
                {
                    model.SectionId = model.Actions.FirstOrDefault()?.SectionId ?? 0;
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
                model.SectionId = model.Actions.FirstOrDefault()?.SectionId ?? 0;
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
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeletePersonnel)]
        public JsonResult DeletePersonnel(short id)
        {
            if (_blPersonnel.Delete(id, out bool isUsed))
            {
                return Json(new JsonData { Success = true });
            }
            else
            {
                return Json(new JsonData { Success = false, IsUsed = isUsed });
            }
        }

        [HttpGet]
        [AccessControl(ActionsEnum.PersonnelManagement)]
        public ActionResult ChangePassword(int id)
        {
            User user = _blUser.FindById(id);
            user.Password = "";

            return PartialView("_ChangeUserPassword", user);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.PersonnelManagement)]
        public JsonResult ChangePassword(ChangeUserPasswordViewModel viewModel)
        {
            User dbUser = _blUser.FindById(viewModel.PersonnelId);

            if (dbUser == null)
            {
                return Json(new JsonData { Success = false });
            }

            var keyNew = Utilities.GeneratePassword(20);
            var password = Utilities.EncodePassword(viewModel.NewPassword, keyNew);
            dbUser.SaltCode = keyNew.EncryptString("#+Samoosh!Group@+Generate$key+#");
            dbUser.Password = password.EncryptString("#+Samoosh!Group@+Generate$pas+#");
            dbUser.RepeatPassword = viewModel.NewPassword;

            if (_blUser.Update(dbUser))
            {
                return Json(new JsonData { Success = true });
            }
            else
            {
                return Json(new JsonData { Success = false });
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.PersonnelManagement)]
        public JsonResult Activate(int id)
        {
            User user = _blUser.FindById(id);

            if (user == null)
            {
                return Json(new JsonData { Success = false });
            }

            user.IsActive = true;
            user.RepeatPassword = "foofoo";

            if (_blUser.Update(user))
            {
                return Json(new JsonData { Success = true });
            }
            else
            {
                return Json(new JsonData { Success = false });
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.PersonnelManagement)]
        public JsonResult Disable(int id)
        {
            User user = _blUser.FindById(id);

            if (user == null)
            {
                return Json(new JsonData { Success = false });
            }

            user.IsActive = false;
            user.RepeatPassword = "foofoo";

            if (_blUser.Update(user))
            {
                return Json(new JsonData { Success = true });
            }
            else
            {
                return Json(new JsonData { Success = false });
            }
        }
    }
}