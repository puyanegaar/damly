using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Sarfasl;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PunasMarketing.Models.Enums;

namespace PunasMarketing.Controllers
{
    public class SarfaslController : Controller
    {
        private readonly SarfaslRepository _sarfaslRepo;

        public SarfaslController(SarfaslRepository sarfaslRepo)
        {
            _sarfaslRepo = sarfaslRepo;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowSarfasls)]
        public ActionResult Sarfasls()
        {
            return View(GetSarfaslListViewModel());
        }

        private SarfaslListViewModel GetSarfaslListViewModel()
        {
            var sarfasls = _sarfaslRepo.Select();

            List<SarfaslHierarchy> sarfaslHierarchies = new List<SarfaslHierarchy>();

            foreach (var groupSarfasl in sarfasls.Where(i => i.ParentId == null))
            {
                SarfaslHierarchy groupSarfaslHierarchy = new SarfaslHierarchy
                {
                    SarfaslId = groupSarfasl.Id,
                    Name = groupSarfasl.Name,
                    Code = groupSarfasl.Code.ToString(),
                    Coding = groupSarfasl.Coding,
                    IsConstant = groupSarfasl.IsConstant,
                    Children = new List<SarfaslHierarchy>()
                };

                foreach (var kolSarfasl in sarfasls.Where(i => i.ParentId == groupSarfasl.Id))
                {
                    SarfaslHierarchy kolSarfaslHierarchy = new SarfaslHierarchy
                    {
                        SarfaslId = kolSarfasl.Id,
                        Name = kolSarfasl.Name,
                        Code = kolSarfasl.Code.ToString(),
                        Coding = kolSarfasl.Coding,
                        IsConstant = kolSarfasl.IsConstant,
                        Children = new List<SarfaslHierarchy>()
                    };

                    foreach (var moeenSarfasl in sarfasls.Where(i => i.ParentId == kolSarfasl.Id))
                    {
                        SarfaslHierarchy moeenSarfaslHierarchy = new SarfaslHierarchy
                        {
                            SarfaslId = moeenSarfasl.Id,
                            Name = moeenSarfasl.Name,
                            Code = moeenSarfasl.Code.ToString("00"),
                            Coding = moeenSarfasl.Coding,
                            IsConstant = moeenSarfasl.IsConstant
                        };

                        kolSarfaslHierarchy.Children.Add(moeenSarfaslHierarchy);
                    }

                    groupSarfaslHierarchy.Children.Add(kolSarfaslHierarchy);
                }

                sarfaslHierarchies.Add(groupSarfaslHierarchy);
            }

            SarfaslListViewModel viewModel = new SarfaslListViewModel
            {
                Sarfasls = sarfasls.ToList(),
                SarfaslHierarchies = sarfaslHierarchies.ToList()
            };

            return viewModel;
        }

        [AccessControl(ActionsEnum.AddSarfasl)]
        [HttpGet]
        public ActionResult AddSarfaslModal()
        {
            return PartialView("_AddSarfasl", GetAddSarfaslViewModel());
        }

        private SarfaslViewModel GetAddSarfaslViewModel()
        {
            SarfaslViewModel viewModel = new SarfaslViewModel
            {
                AllSarfasls = _sarfaslRepo.Select()
            };

            ViewBag.Sarfasls = _sarfaslRepo.Select().Select(i => new
            {
                i.ParentId,
                i.Code
            });

            return viewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddSarfasl)]
        public ActionResult AddSarfaslModal(SarfaslViewModel sarfaslViewModel)
        {
            if (ModelState.IsValid)
            {
                Sarfasl sarfaslToAdd = sarfaslViewModel.Sarfasl;
                short parentId = sarfaslViewModel.KolSarfaslId ?? sarfaslViewModel.GroupSarfaslId;
                sarfaslToAdd.ParentId = parentId;
                sarfaslToAdd.IsConstant = false;
                sarfaslToAdd.IsMoeen = sarfaslViewModel.KolSarfaslId.HasValue;

                if (_sarfaslRepo.CodeExists(sarfaslToAdd.ParentId, sarfaslToAdd.Code))
                {
                    TempData["SaveMessage"] = Notification.Show("کد حساب تکراری است.", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return RedirectToAction("Sarfasls");
                }

                if (_sarfaslRepo.CodingExists(sarfaslToAdd.Coding))
                {
                    TempData["SaveMessage"] = Notification.Show("کدینگ تکراری است.", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return RedirectToAction("Sarfasls");
                }

                if (_sarfaslRepo.Add(sarfaslToAdd))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    ModelState.Clear();
                    return RedirectToAction("Sarfasls");
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

            return RedirectToAction("Sarfasls");
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateSarfasl)]
        public ActionResult UpdateSarfaslModal(int id)
        {
            Sarfasl theSarfasl = _sarfaslRepo.Find((short)id);
            byte groupId = byte.Parse(theSarfasl.Coding[0].ToString());
            short groupSarfaslId = _sarfaslRepo.Select().First(i => i.ParentId == null && i.Code == groupId).Id;
            short? kolSarfaslId = null;
            if (theSarfasl.IsMoeen)
            {
                kolSarfaslId = theSarfasl.ParentId;
                theSarfasl.StrCode = theSarfasl.Code.ToString("00");
            }
            else
            {
                theSarfasl.StrCode = theSarfasl.Code.ToString();
            }

            SarfaslViewModel viewModel = new SarfaslViewModel
            {
                Sarfasl = _sarfaslRepo.Find((short)id),
                AllSarfasls = _sarfaslRepo.Select(),
                GroupSarfaslId = groupSarfaslId,
                KolSarfaslId = kolSarfaslId
            };
            viewModel.Sarfasl.StrCode = theSarfasl.StrCode;


            ViewBag.Sarfasls = _sarfaslRepo.Select().Select(i => new
            {
                i.ParentId,
                i.Code
            });
            return PartialView("_UpdateSarfasl", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateSarfasl)]
        public ActionResult UpdateSarfaslModal(SarfaslViewModel sarfaslViewModel)
        {
            if (ModelState.IsValid)
            {
                Sarfasl newSarfasl = sarfaslViewModel.Sarfasl;
                short parentId = sarfaslViewModel.KolSarfaslId ?? sarfaslViewModel.GroupSarfaslId;
                newSarfasl.ParentId = parentId;
                newSarfasl.IsMoeen = sarfaslViewModel.KolSarfaslId.HasValue;
                newSarfasl.Code = byte.Parse(newSarfasl.StrCode);

                var oldSarfasl = _sarfaslRepo.Find(newSarfasl.Id);
                if (oldSarfasl.Code != newSarfasl.Code && _sarfaslRepo.CodeExists(newSarfasl.ParentId, newSarfasl.Code))
                {
                    TempData["SaveMessage"] = Notification.Show("کد حساب تکراری است.", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return RedirectToAction("Sarfasls");
                }

                if (oldSarfasl.Coding != newSarfasl.Coding && _sarfaslRepo.CodingExists(newSarfasl.Coding))
                {
                    TempData["SaveMessage"] = Notification.Show("کدینگ تکراری است.", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return RedirectToAction("Sarfasls");
                }

                if (_sarfaslRepo.Update(newSarfasl))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    ModelState.Clear();
                    return RedirectToAction("Sarfasls");
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return RedirectToAction("Sarfasls");
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetKolSarfasls(int? id, int? sarfaslId)
        {
            if (id.HasValue)
            {
                SarfaslViewModel viewModel = new SarfaslViewModel();
                viewModel.KolSarfasls = _sarfaslRepo.Where(i => i.ParentId == id.Value);

                short? kolSarfaslId = null;
                if (sarfaslId.HasValue)
                {
                    Sarfasl theSarfasl = _sarfaslRepo.Find((short)sarfaslId);
                    kolSarfaslId = theSarfasl.ParentId;
                }

                viewModel.KolSarfaslId = kolSarfaslId;

                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_KolDropDown", viewModel)
                });
            }
            else
            {
                SarfaslViewModel viewModel = new SarfaslViewModel();
                viewModel.KolSarfasls = new List<Sarfasl>();
                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_KolDropDown", viewModel)
                });
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteSarfasl)]
        public JsonResult DeleteSarfasl(short id)
        {
            Sarfasl sarfasl = _sarfaslRepo.Find(id);
            if (sarfasl.IsConstant)
            {
                return Json(new JsonData { Success = false });
            }

            if (_sarfaslRepo.Delete(sarfasl, out bool isUsed))
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