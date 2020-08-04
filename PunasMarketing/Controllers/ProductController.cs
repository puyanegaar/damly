using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.LocalModel;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PunasMarketing.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _blProduct;
        private readonly ProductCategoryRepository _blProductCategory;
        private readonly UnitRepository _blUnit;
        private readonly PriceTypeRepository _blProductPriceType;
        private readonly ProductPriceListRepository _blProductPriceList;
        private readonly ProductionStatusRepository _productionStatusRepo;
        private readonly TafsiliRepository _tafsiliRepo;
        private readonly WarehouseRepository _warehouseRepo;
        private readonly FactorRepository _factorRepo;
        private readonly FactorItemRepository _factorItemRepo;
        private readonly ProductImageRepository _productImageRepo;
        private readonly InvoiceRepository _invoiceRepo;
        private readonly InvoiceItemRepository _invoiceItemRepo;

        public ProductController(
            ProductRepository productRepository,
            ProductCategoryRepository categoryRepository,
            UnitRepository unitRepository,
            PriceTypeRepository priceTypeRepository,
            ProductPriceListRepository productPriceListRepository,
            ProductionStatusRepository productionStatusRepository,
            TafsiliRepository tafsiliRepo,
            WarehouseRepository warehouseRepo,
            FactorRepository factorRepo,
            FactorItemRepository factorItemRepo,
            InvoiceRepository invoiceRepo,
            InvoiceItemRepository invoiceItemRepo,

            ProductImageRepository productImageRepository)
        {
            _blProduct = productRepository;
            _blProductCategory = categoryRepository;
            _blUnit = unitRepository;
            _blProductPriceType = priceTypeRepository;
            _blProductPriceList = productPriceListRepository;
            _productionStatusRepo = productionStatusRepository;
            _tafsiliRepo = tafsiliRepo;
            _warehouseRepo = warehouseRepo;
            _factorRepo = factorRepo;
            _factorItemRepo = factorItemRepo;
            _invoiceRepo = invoiceRepo;
            _invoiceItemRepo = invoiceItemRepo;
            _productImageRepo = productImageRepository;
        }


        [HttpGet]
        [AccessControl(ActionsEnum.ShowProducts)]
        public ActionResult Products()
        {
            var viewModel = new ProductListViewModel
            {
                Products = _blProduct.Select().OrderByDescending(i => i.Id),
                ProductionStatuses = _productionStatusRepo.Select(),
                ProductCategories = _blProductCategory.Select(),
                Warehouses = _warehouseRepo.Select()
            };
            return View(viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowProducts)]
        public ActionResult ProductsByCategory(short id)
        {
            var viewModel = new ProductListViewModel
            {
                Products = _blProduct.Where(i => i.ProductCategoryId == id).OrderByDescending(i => i.Id),
                ProductionStatuses = _productionStatusRepo.Select(),
                ProductCategories = _blProductCategory.Select(),
                Warehouses = _warehouseRepo.Select()
            };
            return View("Products", viewModel);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddProduct)]
        public ActionResult AddProduct()
        {
            ProductViewModel productViewModel = new ProductViewModel
            {
                ProductCategories = _blProductCategory.Select(),
                ProductionStatuses = _productionStatusRepo.Select(),
                Warehouses = _warehouseRepo.Select(),
                Units = _blUnit.Select()
            };
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddProduct)]
        public ActionResult AddProduct(Product productModel, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (_blProduct.CodeExists(productModel.ProductCode))
                {
                    return Json(new
                    {
                        isvalid = false,
                        Message = Notification.BriefShow("کد محصول تکراری می باشد", ToastType.Error)
                    });
                }

                if (_blProduct.NameExists(productModel.Name))
                {
                    return Json(new
                    {
                        isvalid = false,
                        Message = Notification.BriefShow("نام محصول تکراری می باشد", ToastType.Error)
                    });
                }

                if (productModel.SubUnitId == null && productModel.UnitRate != null)
                {
                    return Json(new
                    {
                        isvalid = false,
                        Message = Notification.BriefShow("در صورت انتخاب واحد فرعی می توانید نسبت آن به مقدار اصلی را وارد نمایید", ToastType.Error)
                    });
                }

                if (productModel.SubUnitId != null && productModel.UnitRate == null)
                {
                    return Json(new
                    {
                        isvalid = false,
                        Message = Notification.BriefShow("نسبت واحد فرعی به اصلی را وارد نمایید", ToastType.Error)
                    });
                }

                #region Upload Image
                string fileName = "";
                int flag = ImageFile.UploadImage("Image\\Products", "ProductCatalog", ref fileName, 1000000, checkType: false);
                if (flag == 1 || flag == 0)
                {
                    productModel.CatalogFileName = fileName;
                    productModel.Id = 1;
                    if (_blProduct.Add(productModel))
                    {
                        short addedProductId = _blProduct.GetMaxId();

                        List<Tafsili> tafsilis = new List<Tafsili>
                        {
                            new Tafsili
                            {
                                ProductId = addedProductId,
                                SarfaslId = (short) SarfaslEnums.MojoodiKala,
                                TafsiliName = productModel.Name
                            },
                            new Tafsili
                            {
                                ProductId = addedProductId,
                                SarfaslId = (short) SarfaslEnums.KharidKala,
                                TafsiliName = productModel.Name
                            },
                            new Tafsili
                            {
                                ProductId = addedProductId,
                                SarfaslId = (short) SarfaslEnums.BarghashtAzKharid,
                                TafsiliName = productModel.Name
                            },
                            new Tafsili
                            {
                                ProductId = addedProductId,
                                SarfaslId = (short) SarfaslEnums.ForosheKala,
                                TafsiliName = productModel.Name
                            },
                            new Tafsili
                            {
                                ProductId = addedProductId,
                                SarfaslId = (short) SarfaslEnums.BarghashtAzForosh,
                                TafsiliName = productModel.Name
                            }
                        };

                        if (_tafsiliRepo.AddRange(tafsilis))
                        {
                            return Json(new
                            {
                                ProductId = addedProductId,
                                isvalid = true,
                                Message = Notification.BriefShow("محصول با موفقیت ثبت شد", ToastType.Success)
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                ProductId = addedProductId,
                                isvalid = true,
                                Message = Notification.BriefShow("خطا در ذخیره اطلاعات حساب های تفصیلی", ToastType.Error)
                            });
                        }
                    }
                    else
                    {
                        return Notification.jsonShow("خطا در ذخیره اطلاعات محصول",
                            type: ToastType.Error, position: ToastPosition.TopCenter);
                    }
                }
                else
                {
                    return Notification.jsonShow(flag.UploadMessage(), type: ToastType.Error, position: ToastPosition.TopCenter);
                }
                #endregion
            }
            else
            {
                return Notification.jsonShow(ModelState.GetErorr(), type: ToastType.Error, position: ToastPosition.TopCenter);
            }
        }

        [HttpGet]
        [AccessControl(ActionsEnum.UpdateProduct)]
        public ActionResult UpdateProduct(short id)
        {
            ProductViewModel productViewModel = new ProductViewModel
            {
                ProductCategories = _blProductCategory.Select(),
                Units = _blUnit.Select(),
                ProductModel = _blProduct.Find(id),
                ProductionStatuses = _productionStatusRepo.Select(),
                Warehouses = _warehouseRepo.Select()
            };
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.UpdateProduct)]
        public ActionResult UpdateProduct(Product productModel, HttpPostedFileBase ImageFile)
        {
            Product dbProduct = _blProduct.Find(productModel.Id);

            if (ModelState.IsValid)
            {
                if (dbProduct.ProductCode.Trim() != productModel.ProductCode.Trim() && _blProduct.CodeExists(productModel.ProductCode))
                {
                    return Notification.jsonShow("کد محصول تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);

                }
                if (dbProduct.Name.Trim() != productModel.Name.Trim() && _blProduct.NameExists(productModel.Name))
                {
                    return Notification.jsonShow("نام محصول تکراری می باشد", type: ToastType.Info, position: ToastPosition.TopCenter);

                }

                if (productModel.SubUnitId == null && productModel.UnitRate != null)
                {
                    return Notification.jsonShow("در صورت انتخاب واحد فرعی می توانید نسبت آن به مقدار اصلی را وارد نمایید", type: ToastType.Info, position: ToastPosition.TopCenter);
                }

                if (productModel.SubUnitId != null && productModel.UnitRate == null)
                {
                    return Notification.jsonShow("نسبت واحد فرعی به اصلی وارد نمایید", type: ToastType.Info, position: ToastPosition.TopCenter);
                }

                #region Upload Image
                string fileName = "";
                int flag = 0;
                if (!string.IsNullOrWhiteSpace(productModel.CatalogFileName))
                {
                    flag = ImageFile.UploadImage("Image\\Products", "ProductCatalog", ref fileName, 1000000, checkType: false);
                    productModel.CatalogFileName = fileName;
                }
                else // remove old file from db
                {
                    fileName.deleteImage("Image\\Products");
                    productModel.CatalogFileName = null;
                }

                if (flag == 1 || flag == 0)
                {
                    productModel.Inventory = dbProduct.Inventory;
                    productModel.PendingsCount = dbProduct.PendingsCount;

                    if (_blProduct.Update(productModel))
                    {
                        if (_blProduct.UpdateTafsili(productModel.Id, productModel.Name))
                        {
                            return Json(new { ProductId = productModel.Id, isvalid = true, Message = Notification.BriefShow("محصول با موفقیت ویرایش شد", ToastType.Success) });
                        }
                        else
                        {
                            return Json(new { ProductId = productModel.Id, isvalid = true, Message = Notification.BriefShow("خطا در ویرایش اطلاعات حساب های تفصیلی", ToastType.Warning) });
                        }
                    }
                    else
                    {
                        return Notification.jsonShow("خطا در ویرایش اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);
                    }
                }
                else
                {
                    return Notification.jsonShow(flag.UploadMessage(), type: ToastType.Warning, position: ToastPosition.TopCenter);
                }
                #endregion
            }
            else
            {
                return Notification.jsonShow(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
            }
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ProductManagement)]
        public ActionResult ProductManagement(short? id)
        {

            if (id.HasValue)
            {
                var product = _blProduct.Find(id.Value);
                if (product == null)
                {
                    return View("_Error404");
                }

                var model = new ProductManagementViewModel
                {
                    Product = product,
                    ProductionStatuses = _productionStatusRepo.Select(),
                    InvoiceItems = _invoiceItemRepo.Where(i => i.ProductId == product.Id).ToList()
                };
                model.Product.PendingsCount = _blProduct.GetLivePendingCount(id.Value, true);

                List<PriceTypeListModel> lst = new List<PriceTypeListModel>();
                var priceList = _blProductPriceList.Where(p => p.ProductId == id.Value);
                var priceType = _blProductPriceType.Select();

                if (priceList.Any())
                {
                    foreach (var item in priceType)
                    {
                        var value = priceList.Where(m => m.PriceTypeId == item.Id);
                        var bvalue = false;
                        decimal val = 0;
                        if (value.Any())
                        {
                            val = value.FirstOrDefault()?.Price ?? 0;
                            bvalue = value.FirstOrDefault()?.IsVisible ?? false;
                        }
                        lst.Add(new PriceTypeListModel { PriceTypeId = item.Id, PriceTypeTitle = item.Name, PriceValue = val, Isvisiable = bvalue });
                    }
                    model.IsUpdate = 1;
                    //edit
                }
                else
                {
                    //new
                    foreach (var item in priceType)
                    {
                        lst.Add(new PriceTypeListModel { PriceTypeId = item.Id, PriceTypeTitle = item.Name, PriceValue = 0, Isvisiable = false });
                    }
                    model.IsUpdate = 0;
                }

                model.PriceTypeList = lst;
                return View(model);
            }
            else
            {
                TempData["SaveMessage"] = Notification.Show("محصولی با این شناسه وجود ندارد", type: ToastType.Warning);
                return RedirectToAction("Products");
            }
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.ProductManagement)]
        public ActionResult AddProductPrice(short productId, string[] priceProperty, int mood)
        {
            if (!Convert.ToBoolean(mood))
            {
                //new
                for (int i = 0; i < priceProperty.Length; i++)
                {
                    string[] sp = priceProperty[i].Split('&');
                    if (!string.IsNullOrEmpty(sp[0]) && !string.IsNullOrWhiteSpace(sp[0]))
                    {
                        _blProductPriceList.Add(productId, short.Parse(sp[1]), decimal.Parse(sp[0]), Convert.ToBoolean(int.Parse(sp[2])), false);
                    }
                }

                if (_blProductPriceList.Save())
                {

                    return Json(new JsonData
                    {
                        Script = Notification.BriefShow("قیمت ها با موفقیت ثبت شد", type: ToastType.Success),
                        Success = true,
                    });
                }
                else
                {

                    return Json(new JsonData
                    {
                        Script = Notification.BriefShow("خطا در ثبت قیمت محصول", type: ToastType.Error),
                        Success = false,

                    });
                }
            }
            else
            {
                for (int i = 0; i < priceProperty.Length; i++)
                {
                    string[] sp = priceProperty[i].Split('&');
                    var propid = short.Parse(sp[1]);
                    var check = _blProductPriceList.Where(m => m.ProductId == productId && m.PriceTypeId == propid);
                    if (check.Any())
                    {
                        var c = check.Single();
                        if (!string.IsNullOrEmpty(sp[0]) && !string.IsNullOrWhiteSpace(sp[0]))
                        {
                            c.Price = decimal.Parse(sp[0]);
                            c.IsVisible = Convert.ToBoolean(int.Parse(sp[2]));
                            _blProductPriceList.Update(c, false);
                        }
                        else
                        {
                            _blProductPriceList.Delete(c, false);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(sp[0]) && !string.IsNullOrWhiteSpace(sp[0]))
                        {
                            _blProductPriceList.Add(productId, short.Parse(sp[1]), decimal.Parse(sp[0]), Convert.ToBoolean(int.Parse(sp[2])), false);
                        }
                    }

                }

                if (_blProductPriceList.Save())
                {

                    return Json(new JsonData
                    {
                        Script = Notification.BriefShow("قیمت  محصول با موفقیت ثبت شد", type: ToastType.Success),
                        Success = true,
                    });
                }
                else
                {

                    return Json(new JsonData
                    {
                        Script = Notification.BriefShow("خطا در ثبت قیمت محصول", type: ToastType.Error),
                        Success = false,

                    });
                }
                //update
            }

        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CoustomAuthrize]
        public JsonResult UpdatePendingCount(short id)
        {
            double pendingCount = _blProduct.GetLivePendingCount(id, true);

            return Json(new { newPendingCount = pendingCount });
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteProduct)]
        public JsonResult DeleteProduct(short id)
        {
            if (_blProduct.Delete(id, out bool isUsed))
            {
                return Json(new JsonData() { Success = true });
            }
            else
            {
                return Json(new JsonData() { Success = false, IsUsed = isUsed });
            }
        }

        [AjaxOnly]
        [HttpGet]
        public JsonResult GetInventory(short id)
        {
            try
            {
                var product = _blProduct.Find(id);
                var inventory = product.Inventory;

                return Json(new { inventory = inventory }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { inventory = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        [AjaxOnly]
        [HttpPost]
        [CoustomAuthrize]
        public ActionResult UploadImage(short ProductId)
        {
            ProductImage Img;
            for (int arquivo = 0; arquivo < Request.Files.Count; arquivo++)
            {

                HttpPostedFileBase file = Request.Files[arquivo];
                string fileName = "";
                if (file != null && file.ContentLength > 0)
                {
                    Img = new ProductImage();
                    int flag = file.UploadImage("Image\\Products", "Pro", ref fileName, 1000000);
                    if (flag == 1 || flag == 0)
                    {
                        Img.ImageName = fileName;
                        Img.ProductId = ProductId;
                        _productImageRepo.Add(Img, false);
                    }


                }

            }

            if (_productImageRepo.SaveChange())
            {
                TempData["SaveMessage"] = Notification.Show("تصاویر با موفقیت آپلود شد", type: ToastType.Success, position: ToastPosition.TopCenter);
            }

            else
            {
                TempData["SaveMessage"] = Notification.Show("خطا در آپلود تصاویر", type: ToastType.Error, position: ToastPosition.TopCenter);
            }
            return null;

        }

        [HttpGet]
        [CoustomAuthrize]
        public ActionResult GetImages(int Code)
        {
            try
            {
                var selectImg = _productImageRepo.Where(x => x.ProductId == Code);

                List<ImageModel> lsImg = new List<ImageModel>();
                ImageModel ob;
                foreach (var item in selectImg)
                {
                    ob = new ImageModel();
                    ob.id = item.Id;
                    ob.ProductId = item.ProductId;
                    ob.Name = item.ImageName;
                    ob.Path = "/Content/Upload/Image/Products/" + item.ImageName;
                    lsImg.Add(ob);
                }
                return Json(new { Data = lsImg }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { Data = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [CoustomAuthrize]
        public ActionResult deleteImg(int id)
        {
            _productImageRepo.Delete(id);
            return null;
        }
    }
}