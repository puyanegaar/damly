using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Customer;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PunasMarketing.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerRepository _customerRepo;
        private readonly CustomerCategoryRepository _customerCategoryRepo;
        private readonly ProvinceRepository _provinceRepo;
        private readonly CityRepository _cityRepo;
        private readonly RegionRepository _regionRepo;
        private readonly TafsiliRepository _tafsiliRepo;
        private readonly SarfaslRepository _sarfaslRepo;
        private readonly PersonnelRepository _personnelRepo;

        public CustomerController(
            CustomerRepository customerRepo,
            CustomerCategoryRepository customerCategoryRepo,
            ProvinceRepository provinceRepo,
            CityRepository cityRepo,
            RegionRepository regionRepo,
            TafsiliRepository tafsiliRepo,
            SarfaslRepository sarfaslRepo,
            PersonnelRepository personnelRepo)
        {
            _customerRepo = customerRepo;
            _customerCategoryRepo = customerCategoryRepo;
            _provinceRepo = provinceRepo;
            _cityRepo = cityRepo;
            _regionRepo = regionRepo;
            _tafsiliRepo = tafsiliRepo;
            _sarfaslRepo = sarfaslRepo;
            _personnelRepo = personnelRepo;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowCustomers)]
        public ActionResult Customers()
        {
            CustomerViewModel viewModel = new CustomerViewModel
            {
                Customers = _customerRepo.Select().OrderByDescending(i => i.Id),
                Regions = _regionRepo.Select(),
                Cities = _cityRepo.Select(),
                CustomerCategories = _customerCategoryRepo.Select()
            };

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddCustomer)]
        public ActionResult AddCustomer()
        {
            CustomerViewModel viewModel = new CustomerViewModel
            {
                Customer = new Customer(),
                Provinces = _provinceRepo.Select(),
                Cities = new List<City>(),
                Regions = new List<Region>(),
                CustomerCategories = _customerCategoryRepo.Select(),
                Marketers = _personnelRepo.Where(i => i.JobTitleId == 0)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddCustomer)]
        public ActionResult AddCustomer(HttpPostedFileBase ImageFile, Customer customer)
        {
            var viewModel = new CustomerViewModel
            {
                Customer = customer,
                Provinces = _provinceRepo.Select(),
                Cities = _cityRepo.Where(i => i.ProvinceId == customer.ProvinceId),
                Regions = _regionRepo.Where(i => i.CityId == customer.CityId),
                CustomerCategories = _customerCategoryRepo.Select(),
                Marketers = _personnelRepo.Where(i => i.JobTitleId == 0)
            };

            if (ModelState.IsValid)
            {
                if (_customerRepo.BusinessNameExists(customer.BusinessName))
                {
                    TempData["SaveMessage"] = Notification.Show("عنوان کسب و کار تکراری است", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                if (_customerRepo.Mobile1Exists(customer.Mobile1))
                {
                    TempData["SaveMessage"] = Notification.Show("شماره موبایل اصلی تکراری است", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                if (_customerRepo.CodeMelliExists(customer.CodeMelli))
                {
                    TempData["SaveMessage"] = Notification.Show("کد ملی تکراری است", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                var keyNew = Utilities.GeneratePassword(20);
                var password = Utilities.EncodePassword(customer.CodeMelli, keyNew);
                customer.Username = customer.Mobile1;
                customer.Password = password.EncryptString("#+Samoosh!Group@+Generate$pas+#");
                customer.SaltCode = keyNew.EncryptString("#+Samoosh!Group@+Generate$key+#");
                customer.IsActive = true;

                #region Upload Image
                string fileName = "";
                int flag = ImageFile.UploadImage("Image\\Customers", "Customer", ref fileName, 1000000);
                if (flag == 1 || flag == 0)
                {
                    customer.ImageName = fileName;
                    if (_customerRepo.Add(customer))
                    {
                        int addedCustomerId = _customerRepo.GetLastIdentity();
                        List<Tafsili> tafsilis = new List<Tafsili>
                        {
                            new Tafsili
                            {
                                CustomerId = addedCustomerId,
                                SarfaslId = (short)SarfaslEnums.HesabDaryaftani,
                                PersonTypeId = 2,
                                TafsiliName = customer.OwnerName
                            },
                            new Tafsili
                            {
                                CustomerId = addedCustomerId,
                                SarfaslId = (short)SarfaslEnums.AsnadDaryftani,
                                PersonTypeId = 2,
                                TafsiliName = customer.OwnerName
                            },
                            new Tafsili
                            {
                                CustomerId = addedCustomerId,
                                SarfaslId = (short)SarfaslEnums.HesabPardakhtani,
                                PersonTypeId = 2,
                                TafsiliName = customer.OwnerName
                            }
                        };

                        if (_tafsiliRepo.AddRange(tafsilis))
                        {
                            TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                            return RedirectToAction("CustomerManagement", new { id = addedCustomerId });
                        }
                    }
                    else
                    {
                        fileName.deleteImage("Image\\Customers");
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
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.CustomerManagement)]
        public ActionResult CustomerManagement(int? id)
        {
            if (!id.HasValue)
            {
                return View("_Error404");
            }

            Customer customer = _customerRepo.Find(id.Value);

            if (customer == null)
            {
                return View("_Error404");
            }

            CustomerManagementViewModel viewModel = new CustomerManagementViewModel
            {
                Customer = customer,
                FinancialReport = _customerRepo.GetFinancialReport(Fiscal.GetFiscalId(), id.Value),
                CustomerCategories = _customerCategoryRepo.Select()
            };
            var s1 = _customerCategoryRepo.Select().FirstOrDefault();
            var s2 = _customerRepo.GetFinancialReport(Fiscal.GetFiscalId(), id.Value).FirstOrDefault();
            return View(viewModel);
        }

        //[HttpGet]
        //[AccessControl(ActionsEnum.CustmorFinancial)]
        //public ActionResult CustmorFinancial(int? id)
        //{
        //    if (!id.HasValue)
        //    {
        //        return View("_Error404");
        //    }

        //    Customer customer = _customerRepo.Find(id.Value);

        //    if (customer == null)
        //    {
        //        return View("_Error404");
        //    }

        //    List<CustomerlReport_Result> FinancialReport = _customerRepo.GetFinancialReport(Fiscal.GetFiscalId(), id.Value);
            
        //    var s1 = _customerCategoryRepo.Select().FirstOrDefault();
        //    var s2 = _customerRepo.GetFinancialReport(Fiscal.GetFiscalId(), id.Value).FirstOrDefault();
            
        //}

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateCustomer)]
        public ActionResult UpdateCustomer(int? id)
        {
            if (!id.HasValue)
            {
                return View("_Error404");
            }

            var dbCustomer = _customerRepo.Find(id.Value);
            if (dbCustomer == null)
            {
                return View("_Error404");
            }

            CustomerViewModel viewModel = new CustomerViewModel
            {
                Customer = dbCustomer,
                Provinces = _provinceRepo.Select(),
                Cities = _cityRepo.Where(i => i.ProvinceId == dbCustomer.Region.City.ProvinceId),
                Regions = _regionRepo.Where(i => i.CityId == dbCustomer.Region.CityId),
                CustomerCategories = _customerCategoryRepo.Select(),
                Marketers = _personnelRepo.Where(i => i.JobTitleId == 0)
            };

            viewModel.Customer.RegionId = dbCustomer.RegionId;
            viewModel.Customer.CityId = dbCustomer.Region.CityId;
            viewModel.Customer.ProvinceId = dbCustomer.Region.City.ProvinceId;

            return View(viewModel);
        }

        [HttpPost]
        [AccessControl(ActionsEnum.UpdateCustomer)]
        public ActionResult UpdateCustomer(HttpPostedFileBase ImageFile, CustomerViewModel viewModel)
        {
            Customer customer = viewModel.Customer;
            Customer dbCustomer = _customerRepo.Find(customer.Id);

            viewModel.Provinces = _provinceRepo.Select();
            viewModel.Cities = _cityRepo.Where(i => i.ProvinceId == dbCustomer.Region.City.ProvinceId);
            viewModel.Regions = _regionRepo.Where(i => i.CityId == dbCustomer.Region.CityId);
            viewModel.Marketers = _personnelRepo.Where(i => i.JobTitleId == 0);
            viewModel.CustomerCategories = _customerCategoryRepo.Select();

            if (ModelState.IsValid)
            {
                if (dbCustomer == null)
                {
                    TempData["SaveMessage"] = Notification.Show("مشتری با این شناسه موجود نیست", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                customer.Username = dbCustomer.Username;
                customer.Password = dbCustomer.Password;
                customer.SaltCode = dbCustomer.SaltCode;

                if (dbCustomer.BusinessName.Trim() != customer.BusinessName.Trim() &&
                    _customerRepo.BusinessNameExists(customer.BusinessName))
                {
                    TempData["SaveMessage"] = Notification.Show("عنوان کسب  کار مشتری تکراری است", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                if (dbCustomer.Mobile1.Trim() != customer.Mobile1.Trim() &&
                    _customerRepo.Mobile1Exists(customer.Mobile1))
                {
                    TempData["SaveMessage"] = Notification.Show("شماره موبایل اصلی تکراری است", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                if (dbCustomer.CodeMelli.Trim() != customer.CodeMelli.Trim() &&
                    _customerRepo.CodeMelliExists(customer.CodeMelli))
                {
                    TempData["SaveMessage"] = Notification.Show("کد ملی تکراری است", type: ToastType.Info, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }

                #region Upload Image
                string fileName = "";
                int flag = ImageFile.UploadImage("Image\\Customers", "Customer", ref fileName, 1000000);
                if (flag == 1 || flag == 0)
                {
                    customer.ImageName = fileName;
                    if (_customerRepo.Update(customer))
                    {
                        if (_customerRepo.UpadteTafsili(customer.Id, customer.OwnerName))
                        {
                            TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                            return RedirectToAction("Customers");
                        }
                        else
                        {
                            TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات حساب تفصیلی", type: ToastType.Warning, position: ToastPosition.TopCenter);
                        }
                    }
                    else
                    {
                        TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات", type: ToastType.Warning, position: ToastPosition.TopCenter);
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
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }

            return View(viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetCities(int? id)
        {
            if (id.HasValue)
            {
                CustomerViewModel viewModel = new CustomerViewModel
                {
                    Cities = _cityRepo.Where(i => i.ProvinceId == id.Value)
                };

                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_Cities", viewModel)
                });
            }
            else
            {
                CustomerViewModel viewModel = new CustomerViewModel
                {
                    Cities = _cityRepo.Where(i => i.ProvinceId == id.Value)
                };

                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_Cities", viewModel)
                });
            }
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult GetRegions(int? id)
        {
            if (id.HasValue)
            {
                CustomerViewModel viewModel = new CustomerViewModel
                {
                    Regions = _regionRepo.Where(i => i.CityId == id.Value)
                };

                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_Regions", viewModel)
                });
            }
            else
            {
                CustomerViewModel viewModel = new CustomerViewModel
                {
                    Regions = _regionRepo.Where(i => i.CityId == id.Value)
                };

                return Json(new JsonData
                {
                    Html = this.RenderPartialToString("_Regions", viewModel)
                });
            }
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.CustomerManagement)]
        public JsonResult Activate(int id)
        {
            Customer customer = _customerRepo.Find(id);

            if (customer == null)
            {
                return Json(new JsonData { Success = false });
            }

            customer.IsActive = true;
            //customer.RepeatPassword = "foofoo";

            if (_customerRepo.Update(customer))
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
        [AccessControl(ActionsEnum.CustomerManagement)]
        public JsonResult Disable(int id)
        {
            Customer customer = _customerRepo.Find(id);

            if (customer == null)
            {
                return Json(new JsonData { Success = false });
            }

            customer.IsActive = false;
            //customer.RepeatPassword = "foofoo";

            if (_customerRepo.Update(customer))
            {
                return Json(new JsonData { Success = true });
            }
            else
            {
                return Json(new JsonData { Success = false });
            }
        }

        [HttpGet]
        [AccessControl(ActionsEnum.CustomerManagement)]
        public ActionResult ChangePassword(int id)
        {
            Customer customer = _customerRepo.Find(id);

            ChangeCustomerPasswordViewModel viewModel = new ChangeCustomerPasswordViewModel
            {
                CustomerId = id,
            };

            return PartialView("_ChangeCustomerPassword", viewModel);
        }


        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.CustomerManagement)]
        public JsonResult ChangePassword(ChangeCustomerPasswordViewModel viewModel)
        {
            Customer dbCustomer = _customerRepo.Find(viewModel.CustomerId);

            if (dbCustomer == null)
            {
                return Json(new JsonData { Success = false });
            }

            var keyNew = Utilities.GeneratePassword(20);
            var password = Utilities.EncodePassword(viewModel.NewPassword, keyNew);
            dbCustomer.SaltCode = keyNew.EncryptString("#+Samoosh!Group@+Generate$key+#");
            dbCustomer.Password = password.EncryptString("#+Samoosh!Group@+Generate$pas+#");

            if (_customerRepo.Update(dbCustomer))
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
        [AccessControl(ActionsEnum.DeleteCustomer)]
        public JsonResult DeleteCustomer(int id)
        {
            if (_customerRepo.Delete(id, out bool isUsed))
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