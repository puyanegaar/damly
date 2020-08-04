using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Offer;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PunasMarketing.Controllers
{
    public class OfferController : Controller
    {
        private readonly CustomerCategoryRepository _customerCategoryRepos;
        private readonly ProductRepository _productRepo;
        private readonly OfferTypeRepository _blOfferType;
        private readonly OfferRepository _blOffer;
        private readonly OfferItemRepository _blOfferItem;

        public OfferController(
            CustomerCategoryRepository customerCategoryRepos,
            ProductRepository productRepo,
            OfferTypeRepository blOfferType,
            OfferRepository blOffer,
            OfferItemRepository blOfferItem)
        {
            _customerCategoryRepos = customerCategoryRepos;
            _productRepo = productRepo;
            _blOfferType = blOfferType;
            _blOffer = blOffer;
            _blOfferItem = blOfferItem;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowOffers)]
        public ActionResult Offers()
        {
            Session["OfferItem"] = null;
            return View(_blOffer.Select().OrderByDescending(i => i.Id));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddOffer)]
        public ActionResult AddOffer()
        {
            OfferViewModel viewModel = new OfferViewModel();

            viewModel.CustomerCategories = _customerCategoryRepos.Select();
            viewModel.Products = _productRepo.Where(i => i.IsAvailable && i.IsSellable);
            viewModel.OfferTypes = _blOfferType.Select();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddOffer)]
        public ActionResult AddOffer(OfferViewModel viewModel, HttpPostedFileBase ImageFile, List<int> cat)
        {
            viewModel.CustomerCategories = _customerCategoryRepos.Select();
            viewModel.Products = _productRepo.Where(i => i.IsAvailable && i.IsSellable);
            viewModel.OfferTypes = _blOfferType.Select();
            Offer offer = viewModel.Offer;

            if (ModelState.IsValid)
            {
                if (ImageFile == null)
                {
                    TempData["SaveMessage"] = Notification.Show("تصویر بارگذاری نشده است", type: ToastType.Error, position: ToastPosition.TopCenter);
                    return View(viewModel);
                }
                #region Upload Image
                string fileName = "";
                int flag = ImageFile.UploadImage("Image\\Offers", "Offer", ref fileName, 1000000);
                if (flag == 1 || flag == 0)
                {
                    offer.ImageName = fileName;
                    offer.StartDate = viewModel.StartDate.ToMiladiDate().GetStartDate();
                    offer.ExpDate = viewModel.ExpDate.ToMiladiDate().GetEndDate();
                    if (cat != null)
                    {
                        offer.ForCustomerCategories = string.Join(",", cat);
                    }
                    var offerId = _blOffer.Add(offer);

                    List<OfferItem> list = new List<OfferItem>();

                    if (Session["OfferItem"] != null)
                    {
                        list = Session["OfferItem"] as List<OfferItem>;
                    }

                    if (list == null || !list.Any())
                    {
                        TempData["SaveMessage"] = Notification.Show("لیست جشنواره خالی است!",
                            type: ToastType.Error, position: ToastPosition.TopCenter);
                        return View(viewModel);
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            item.OfferId = offerId;

                            item.Product = null;
                            item.Product1 = null;

                            if (!_blOfferItem.Add(item))
                            {
                                TempData["SaveMessage"] = Notification.Show("خطایی رخ داده است . بعدا امتحان کنید",
                                    type: ToastType.Error, position: ToastPosition.TopCenter);
                                return View(viewModel);
                            }
                        }
                    }

                    Session["OfferItem"] = null;
                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return RedirectToAction("Offers");
                }
                else
                {
                    TempData["SaveMessage"] = Notification.Show(flag.UploadMessage(), type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
                #endregion
            }

            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateOffer)]
        public ActionResult UpdateOffer(short id)
        {
            var offer = _blOffer.Find(id);
            if (offer == null)
            {
                return View("_Error404");
            }

            OfferViewModel viewModel = new OfferViewModel
            {
                CustomerCategories = _customerCategoryRepos.Select(),
                Products = _productRepo.Where(i => i.IsAvailable && i.IsSellable),
                OfferTypes = _blOfferType.Select(),
                Offer = offer,
                StartDate = offer.StartDate.ToPersianDateTime().ToStringDate(),
                ExpDate = offer.ExpDate.ToPersianDateTime().ToStringDate()
            };


            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateOffer)]
        public ActionResult UpdateOffer(OfferViewModel viewModel, HttpPostedFileBase ImageFile, List<int> cat)
        {
            viewModel.CustomerCategories = _customerCategoryRepos.Select();
            viewModel.Products = _productRepo.Where(i => i.IsAvailable && i.IsSellable);
            viewModel.OfferTypes = _blOfferType.Select();
            Offer offer = viewModel.Offer;

            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    var oldOffer = _blOffer.Find(offer.Id);
                    System.IO.File.Delete("/Content/Upload/Image/Offers/" + oldOffer.ImageName);

                    string fileName = "";
                    int flag = ImageFile.UploadImage("Image\\Offers", "Offer", ref fileName, 1000000);
                    if (flag == 1 || flag == 0)
                    {
                        offer.ImageName = fileName;
                        offer.StartDate = viewModel.StartDate.ToMiladiDate().GetStartDate();
                        offer.ExpDate = viewModel.ExpDate.ToMiladiDate().GetEndDate();

                        if (cat != null)
                        {
                            if (cat.Any())
                            {
                                offer.ForCustomerCategories = string.Join(",", cat);
                            }
                            else
                            {
                                offer.ForCustomerCategories = string.Empty;

                            }
                        }
                        else
                        {
                            offer.ForCustomerCategories = string.Empty;
                        }

                        var offerId = _blOffer.Update(offer);

                        TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                        return RedirectToAction("Offers");
                    }

                }
                else
                {
                    offer.StartDate = viewModel.StartDate.ToMiladiDate();
                    offer.ExpDate = viewModel.ExpDate.ToMiladiDate();
                    if (cat != null)
                    {
                        if (cat.Any())
                        {
                            offer.ForCustomerCategories = string.Join(",", cat);
                        }
                        else
                        {
                            offer.ForCustomerCategories = string.Empty;
                        }
                    }
                    else
                    {
                        offer.ForCustomerCategories = string.Empty;
                    }

                    var offerId = _blOffer.Update(offer);

                    TempData["SaveMessage"] = Notification.Show("اطلاعات با موفقیت ذخیره شد", type: ToastType.Success, position: ToastPosition.TopCenter);
                    return RedirectToAction("Offers");
                }

            }

            TempData["SaveMessage"] = Notification.Show("خطایی رخ داده است . بعدا امتحان کنید", type: ToastType.Error, position: ToastPosition.TopCenter);
            return View(viewModel);
        }

        [AjaxOnly]
        [HttpGet]
        [CoustomAuthrize]
        public ActionResult DeleteOfferItem(int id)
        {
            List<OfferItem> list = Session["OfferItem"] as List<OfferItem>;
            list.RemoveAt(id);
            Session["OfferItem"] = list;
            return PartialView("_OfferItemList", list);
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.UpdateOffer)]
        public ActionResult DeleteOfferItemInUpdateMode(int id)
        {
            if (_blOfferItem.Delete(id, out bool isUsed))
            {
                return Json(new JsonData() { Success = true });
            }
            else
            {
                return Json(new JsonData() { Success = false, IsUsed = isUsed });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CoustomAuthrize]
        public ActionResult AddOfferItem(OfferItem offerItems)
        {
            List<OfferItem> list = new List<OfferItem>();
            if (Session["OfferItem"] != null)
            {
                list = Session["OfferItem"] as List<OfferItem>;
            }

            var product = new Product();
            var Giftedproduct = new Product();
            if (offerItems.ProductId.HasValue)
            {
                short id = offerItems.ProductId.Value;
                product = _productRepo.Find(id);
            }
            else
            {
                product = null;
            }
            if (offerItems.GiftProductId.HasValue)
            {
                short id = offerItems.GiftProductId.Value;
                Giftedproduct = _productRepo.Find(id);
            }
            else
            {
                Giftedproduct = null;
            }

            list.Add(new OfferItem
            {
                DiscountAmount = offerItems.DiscountAmount,
                ProductId = offerItems.ProductId,
                Product1 = product,
                Product = Giftedproduct,
                DiscountPercent = offerItems.DiscountPercent,
                GiftProductCount = offerItems.GiftProductCount,
                GiftProductId = offerItems.GiftProductId,
                MinProductCount = offerItems.MinProductCount,
                OfferTypeId = offerItems.OfferTypeId
            });

            Session["OfferItem"] = list;

            return PartialView("_OfferItemList", list);
        }


        [ChildActionOnly]
        [CoustomAuthrize]
        public ActionResult OfferItemListInSession()
        {
            List<OfferItem> list = new List<OfferItem>();

            if (Session["OfferItem"] != null)
            {
                list = Session["OfferItem"] as List<OfferItem>;
            }

            return PartialView("_OfferItemList", list);

        }

        [HttpGet]
        [CoustomAuthrize]
        public ActionResult OfferItemList(short id)
        {
            var list = _blOfferItem.GetOfferItemByOfferId(id);

            return PartialView("_UpdateOfferItemList", list);

        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteOffer)]
        public JsonResult DeleteOffer(short id)
        {
            if (_blOfferItem.DeleteByOfferId(id))
            {
                if (_blOffer.Delete(id, out bool isUsed))
                {
                    return Json(new JsonData() { Success = true });
                }
                else
                {
                    return Json(new JsonData() { Success = false, IsUsed = isUsed });
                }
            }

            return Json(new JsonData() { Success = false });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateOffer)]
        public ActionResult AddOfferItemInUpdateMode(OfferItem offerItems)
        {
            if (ModelState.IsValid)
            {
                if (_blOfferItem.Add(offerItems))
                {
                    return PartialView("_OfferItemList", _blOfferItem.GetOfferItemByOfferId(offerItems.OfferId));
                }
            }

            return PartialView("_OfferItemList", _blOfferItem.GetOfferItemByOfferId(offerItems.OfferId));
        }

    }
}