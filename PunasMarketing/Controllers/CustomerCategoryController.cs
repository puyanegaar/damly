using System.Linq;
using System.Web.Mvc;
using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;

namespace PunasMarketing.Controllers
{
    public class CustomerCategoryController : Controller
    {
        private readonly CustomerCategoryRepository _customerCategoryRepo;

        public CustomerCategoryController(CustomerCategoryRepository customerCategoryRepo)
        {
            _customerCategoryRepo = customerCategoryRepo;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowCustomerCategories)]
        public ActionResult CustomerCategories()
        {
            return View(_customerCategoryRepo.Select().OrderByDescending(i => i.Id));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddCustomerCategory)]
        public ActionResult AddCustomerCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddCustomerCategory)]
        public ActionResult AddCustomerCategory(CustomerCategory customerCategory)
        {
            if (ModelState.IsValid)
            {
                if (_customerCategoryRepo.Add(customerCategory))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    ModelState.Clear();
                    return RedirectToAction("CustomerCategories");
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

            return View(customerCategory);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateCustomerCategory)]
        public ActionResult UpdateCustomerCategory(short id)
        {
            CustomerCategory customerCategory = _customerCategoryRepo.Find(id);
            if (customerCategory == null)
            {
                return View("_Error404");
            }

            return View(customerCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateCustomerCategory)]
        public ActionResult UpdateCustomerCategory(CustomerCategory customerCategory)
        {
            if (ModelState.IsValid)
            {
                if (_customerCategoryRepo.Update(customerCategory))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return RedirectToAction("CustomerCategories");
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ویرایش اطلاعات", type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Error, position: ToastPosition.TopCenter);

            }

            return View(customerCategory);

        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteCustomerCategory)]
        public JsonResult DeleteCustomerCategory(short id)
        {
            if (_customerCategoryRepo.Delete(id, out bool isUsed))
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