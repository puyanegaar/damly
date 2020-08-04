using PunasMarketing.Helpers.Utilities;
using System.Web.Mvc;
using PunasMarketing.Helpers.Filters;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;

namespace PunasMarketing.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly ProductCategoryRepository _blProductCategory;

        public ProductCategoryController(ProductCategoryRepository blCategor)
        {
            _blProductCategory = blCategor;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowProductCategories)]
        public ActionResult Categories()
        {
            return View(_blProductCategory.Select());
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddProductCategory)]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddProductCategory)]
        public ActionResult AddCategory(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                if (_blProductCategory.Add(productCategory))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return RedirectToAction("Categories");
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                }
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Error, position: ToastPosition.TopCenter);

            }

            return View(productCategory);
        }


        [HttpGet]
        [AccessControl(ActionsEnum.UpdateProductCategory)]
        public ActionResult UpdateCategory(int id)
        {
            ProductCategory category = _blProductCategory.Find(id);
            if (category == null)
            {
                return View("_Error404");
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateProductCategory)]
        public ActionResult UpdateCategory(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                if (_blProductCategory.Update(productCategory))
                {
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ویرایش شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return RedirectToAction("categories");
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

            return View(productCategory);

        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteProductCategory)]
        public JsonResult DeleteCategory(int id)
        {
            if (_blProductCategory.Delete(id, out bool isUsed))
            {
                return Json(new JsonData() { Success = true });
            }
            else
            {
                return Json(new JsonData() { Success = false, IsUsed = isUsed });
            }
        }

    }
}