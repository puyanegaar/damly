using Newtonsoft.Json;
using PunasMarketing.Helpers.Filters;
using PunasMarketing.Helpers.Utilities;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.LocalModel;
using PunasMarketing.Models.Repositories;
using PunasMarketing.ViewModels.Factor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace PunasMarketing.Controllers
{

    public class FactorController : Controller
    {
        private readonly FactorRepository _factorRepo;
        private readonly FactorItemRepository _blFactorItem;
        private readonly ProductRepository _productRepo;
        private readonly SupplierRepository _supplierRepo;
        private readonly CustomerRepository _customerRepo;
        private readonly PriceTypeRepository _blPriceType;
        private readonly ProductPriceListRepository _blProductPriceList;
        private readonly OfferRepository _blOffer;
        private readonly OfferItemRepository _offerItemRepo;
        private readonly PersonnelRepository _personnelRepo;
        private readonly SanadRepository _sanadRepo;
        private readonly SanadItemRepository _sanadItemRepo;
        private readonly TafsiliRepository _tafsiliRepo;
        private readonly WarehouseRepository _blWarehouse;
        private readonly InvoiceRepository _blInvoice;
        private readonly InvoiceItemRepository _blInvoiceItem;
        private readonly OrderRepository _orderRepo;
        private readonly OrderItemRepository _orderItemRepo;
        private readonly FiscalRepository _fiscalRepo;
        private readonly UnitRepository _blUnit;
        private readonly ProductRepository _blProduct;

        public FactorController(FactorRepository factorRepo, FactorItemRepository blFactorItem,
            ProductRepository productRepo, SupplierRepository supplierRepo,
            CustomerRepository customerRepo, PriceTypeRepository blPriceType,
            ProductPriceListRepository blProductPriceList, OfferRepository blOffer,
            OfferItemRepository blOfferItem, PersonnelRepository personnelRepo,
            SanadRepository sanadRepo, SanadItemRepository sanadItemRepo,
            TafsiliRepository tafsiliRepo, WarehouseRepository blWarehouse,
            InvoiceRepository blInvoice, InvoiceItemRepository blInvoiceItem,
            OrderRepository orderRepo, OrderItemRepository orderItemRepo,
            FiscalRepository fiscalRepo, UnitRepository blUnit, ProductRepository blProduct)
        {
            _factorRepo = factorRepo;
            _blFactorItem = blFactorItem;
            _productRepo = productRepo;
            _supplierRepo = supplierRepo;
            _customerRepo = customerRepo;
            _blPriceType = blPriceType;
            _blProductPriceList = blProductPriceList;
            _blOffer = blOffer;
            _offerItemRepo = blOfferItem;
            _personnelRepo = personnelRepo;
            _sanadRepo = sanadRepo;
            _sanadItemRepo = sanadItemRepo;
            _tafsiliRepo = tafsiliRepo;
            _blWarehouse = blWarehouse;
            _blInvoice = blInvoice;
            _blInvoiceItem = blInvoiceItem;
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _fiscalRepo = fiscalRepo;
            _blUnit = blUnit;
            _blProduct = blProduct;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowFactors)]
        public ActionResult BuyFactors()
        {

            var fiscalId = Fiscal.GetFiscalId();
            var select = _factorRepo.Where(i =>
                    i.PeriodId == fiscalId && i.IsDeleted == false && i.IsPreFactor == false &&
                    i.FactorTypeId == (byte)FactorType.Kharid).
                OrderByDescending(a => a.Id);
            var b = select.ToList();
            List<Factor> buyFactors=new List<Factor>();
            foreach (var a in select)
            {
                int idProduct = a.FactorItems.First().ProductId;
                int idWareHouse = _blProduct.Where(x => x.Id == idProduct).FirstOrDefault().WarehouseId;
                string wareHouseName = _blWarehouse.Where(x => x.Id == idWareHouse).FirstOrDefault().Name;
                a.WareHouse = wareHouseName;
                buyFactors.Add(a);
            }

           
            return View(select);

        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowFactors)]
        public ActionResult BuyBackFactors()
        {
            var fiscalId = Fiscal.GetFiscalId();
            return View(_factorRepo.Where(
                    i => i.PeriodId == fiscalId && i.IsDeleted == false && i.IsPreFactor == false
                         && i.FactorTypeId == (byte)FactorType.BargashAzKharid).
                OrderByDescending(a => a.Id));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowFactors)]
        public ActionResult SellFactors()
        {
            var fiscalId = Fiscal.GetFiscalId();
            return View(_factorRepo.Where(
                    i => i.PeriodId == fiscalId && i.IsDeleted == false && i.IsPreFactor == false
                         && i.FactorTypeId == (byte)FactorType.Foroosh).
                OrderByDescending(a => a.Id));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowFactors)]
        public ActionResult SellBackFactors()
        {
            var fiscalId = Fiscal.GetFiscalId();
            return View(_factorRepo.Where(
                    i => i.PeriodId == fiscalId && i.IsDeleted == false && i.IsPreFactor == false
                         && i.FactorTypeId == (byte)FactorType.BargashAzForoosh).
                OrderByDescending(a => a.Id));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowFactors)]
        public ActionResult DeletedFactors()
        {
            var fiscalId = Fiscal.GetFiscalId();
            return View(_factorRepo.Where(
                    i => i.PeriodId == fiscalId && i.IsDeleted && !i.IsPreFactor)
                .OrderByDescending(a => a.Id));
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowFactors)]
        public ActionResult SellFactorsWithoutInvoice()
        {
            IQueryable<Factor> sellFactorsWithoutInvoice =
                _factorRepo.Where(i => !i.IsPreFactor && !i.IsDeleted && !i.InvoiceId.HasValue &&
                                     i.IsConfirmed.HasValue && i.IsConfirmed.Value == true
                                     && (i.FactorTypeId == (byte)FactorType.Foroosh ||
                                     i.FactorTypeId == (byte)FactorType.BargashAzKharid))
                    .OrderByDescending(i => i.Id);

            return View(sellFactorsWithoutInvoice);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.ShowFactors)]
        public ActionResult BuyFactorsWithoutInvoice()
        {
            IQueryable<Factor> buyFactorsWithoutInvoice =
                _factorRepo.Where(i => !i.IsPreFactor && !i.IsDeleted && !i.InvoiceId.HasValue
                                          && (i.FactorTypeId == (byte)FactorType.Kharid || i.FactorTypeId == (byte)FactorType.BargashAzForoosh))
                    .OrderByDescending(i => i.Id);
            return View(buyFactorsWithoutInvoice);
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddFactor)]
        [FiscalCheck]
        public ActionResult AddBuyFactor()
        {
            FactorViewModel viewModel = new FactorViewModel
            {
                PriceTypes = _blPriceType.Select().Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }),
                SelectSuppliers = _supplierRepo.Select().Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }),
                Products = _productRepo.Where(a => a.IsAvailable && a.IsSellable),
                Units = _blUnit.Select(),
                TaxPercent = _fiscalRepo.Where(i => !i.IsClosed).FirstOrDefault()?.Vat ?? 0,
                StrCreatedDate = DateTime.Now.ToPersianDateTime().ToStringDate()
            };

            return View(viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.AddFactor)]
        [FiscalCheck]
        public JsonResult AddBuyFactor(AddFactorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                short fiscalId = Fiscal.GetFiscalId();
                int addedFactorId = _factorRepo.GetLastIdentity(fiscalId) + 1;
                string description = $"فروش طی فاکتور شماره {addedFactorId}";

                viewModel.Factor.Id = addedFactorId;
                viewModel.Factor.PeriodId = fiscalId;
                viewModel.Factor.CreatedDate = viewModel.StrCreateDate.ToMiladiDate();
                viewModel.Factor.CreatorUserId = UserIdentity.Getuserid();
                viewModel.Factor.IsBuy = true;
                viewModel.Factor.FactorTypeId = (byte)FactorType.Kharid;
                viewModel.Factor.IsPreFactor = false;
                viewModel.Factor.IsConfirmed = true;

                int factorId = _factorRepo.Add(viewModel.Factor);
                if (factorId == -1)
                {
                    return Json(new { Success = false, Message = "خطا در ثبت فاکتور" });
                }

                foreach (var item in viewModel.FactorItems)
                {
                    var unitsRate = _productRepo.Find(item.ProductId).UnitRate;
                    var totalDiscount = (decimal)item.Count * item.Discount;
                    var totalPrice = (decimal)item.Count * item.UnitPrice;
                    var finalPrice = totalPrice - totalDiscount + item.Tax;
                    FactorItem factorItem = new FactorItem
                    {
                        UnitId = item.UnitId,
                        PeriodId = fiscalId,
                        FactorId = addedFactorId,
                        ProductId = item.ProductId,
                        UnitsRate = unitsRate,
                        MainUnitCount = item.Count,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = totalPrice,
                        Discount = totalDiscount,
                        Tax = item.Tax,
                        FinalPrice = finalPrice,
                        Description = item.Description
                    };

                    _blFactorItem.Add(factorItem);
                }

                int addedSanadId = _sanadRepo.GetLastIdentity(fiscalId) + 1;

                Sanad sanad = new Sanad
                {
                    Id = addedSanadId,
                    PeriodId = fiscalId,
                    ConfirmDate = viewModel.Factor.CreatedDate,
                    CreatedByUserId = viewModel.Factor.CreatorUserId,
                    CreatedDate = viewModel.Factor.CreatedDate,
                    Description = description,
                    IsAutomatic = true,
                    IsConfirmed = true,
                    TotalAmount = viewModel.Factor.FinalPrice,
                    SanadTypeId = (byte)SandType.FactorKharid
                };

                if (!_sanadRepo.Add(sanad))
                {
                    return Json(new { Success = false, Message = "خطا در ثبت سند حسابداری" });
                }

                var factor = _factorRepo.Find(addedFactorId, fiscalId);
                factor.SanadId = addedSanadId;
                factor.PeriodId = fiscalId;

                #region calulate and set total amounts

                var sumTotalPrice = factor.FactorItems.Sum(i => i.TotalPrice);
                var sumTotalDiscount = factor.FactorItems.Sum(i => i.Discount);
                var sumTotalTax = factor.FactorItems.Sum(i => i.Tax);
                var sumFinalPrice = sumTotalPrice - sumTotalDiscount + sumTotalTax - factor.DiscountOnFactor +
                                    factor.Additions - factor.Deductions;

                factor.TotalPrice = sumTotalPrice;
                factor.TotalDiscount = sumTotalDiscount;
                factor.TotalTax = sumTotalTax;
                factor.FinalPrice = sumFinalPrice;

                #endregion

                if (!_factorRepo.Update(factor))
                {
                    return Json(new { Success = false, Message = "خطا در ثبت اطلاعات فاکتور" });
                }


                List<SanadItem> sanadItemsList = new List<SanadItem>();

                #region Supplier HesabPardakhtani

                int supplierPardakhtaniTafsilyId = _tafsiliRepo.Where(a =>
                    a.SarfaslId == (short)SarfaslEnums.HesabPardakhtani &&
                    a.SupplierId == factor.SupplierId).Select(a => a.Id).FirstOrDefault();

                SanadItem supplierPardakhtaniSanadItem = new SanadItem()
                {
                    SandId = addedSanadId,
                    PeriodId = fiscalId,
                    IssueTrackingNum = null,
                    ChequeId = null,
                    Description = description,
                    SarfaslId = (short)SarfaslEnums.HesabPardakhtani,
                    TafsiliId = supplierPardakhtaniTafsilyId,
                    Bedeh = 0,
                    Bestan = factor.FinalPrice,
                    ConfirmDate = sanad.ConfirmDate

                };

                sanadItemsList.Add(supplierPardakhtaniSanadItem);

                #endregion

                if (factor.TotalDiscount + factor.DiscountOnFactor > 0)
                {

                    #region Takhfifat

                    SanadItem takhfifatSanadItem = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.TakhfifatNagdiKharid,
                        TafsiliId = null,
                        Bedeh = 0,
                        Bestan = factor.TotalDiscount + factor.DiscountOnFactor,
                        ConfirmDate = sanad.ConfirmDate
                    };

                    sanadItemsList.Add(takhfifatSanadItem);

                    #endregion
                }

                var deductions = factor.Deductions;
                var additions = factor.Additions;
                if (additions - deductions != 0)
                {
                    #region Sayere Daramadha // برای تعدیل اضافات و کسورات فاکتور

                    SanadItem sayereDaramadha = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.SayereDaramadha,
                        TafsiliId = null,
                        Bedeh = 0, // will be set later
                        Bestan = 0, // will be set later
                        ConfirmDate = sanad.ConfirmDate
                    };

                    if (additions > deductions)
                    {
                        sayereDaramadha.Bedeh = Math.Abs(additions - deductions);
                    }
                    else
                    {
                        sayereDaramadha.Bestan = Math.Abs(additions - deductions);
                    }

                    sanadItemsList.Add(sayereDaramadha);

                    #endregion
                }

                #region MaliatBarArzeshAfzodehKharid

                SanadItem maliatSanadItem = new SanadItem
                {
                    SandId = addedSanadId,
                    PeriodId = fiscalId,
                    IssueTrackingNum = null,
                    ChequeId = null,
                    Description = description,
                    SarfaslId = (short)SarfaslEnums.MaliatBarArzeshAfzodehKharid,
                    TafsiliId = null,
                    Bedeh = factor.TotalTax,
                    Bestan = 0,
                    ConfirmDate = sanad.ConfirmDate

                };

                sanadItemsList.Add(maliatSanadItem);

                #endregion

                #region Kharid

                foreach (var item in factor.FactorItems)
                {
                    if (item.TotalPrice > 0)
                    {
                        var kharidTafsilyId = _tafsiliRepo.Where(a =>
                            a.SarfaslId == (short)SarfaslEnums.KharidKala &&
                            a.ProductId == item.ProductId).Select(a => a.Id).FirstOrDefault();

                        SanadItem kharidSanadItem = new SanadItem
                        {
                            SandId = addedSanadId,
                            PeriodId = fiscalId,
                            IssueTrackingNum = null,
                            ChequeId = null,
                            Description = description,
                            SarfaslId = (short)SarfaslEnums.KharidKala,
                            TafsiliId = kharidTafsilyId,
                            Bedeh = item.TotalPrice,
                            Bestan = 0,
                            ConfirmDate = sanad.ConfirmDate
                        };

                        sanadItemsList.Add(kharidSanadItem);
                    }
                }

                var sumBedeh = sanadItemsList.Sum(i => i.Bedeh);
                var sumBestan = sanadItemsList.Sum(i => i.Bestan);
                if (sumBestan != sumBedeh)
                {
                    return Json(new { Success = false, Message = "خطا در سند حسابداری! سند حسابداری، تراز نیست!" });
                }

                var theSanad = _sanadRepo.Find(addedSanadId, fiscalId);
                theSanad.TotalAmount = sumBestan; // or sumBedeh

                #endregion

                if (_sanadItemRepo.AddRange(sanadItemsList) && _sanadRepo.Update(theSanad))
                {
                    return Json(new { Success = true, Id = addedFactorId });
                }
                else
                {
                    return Json(new { Success = false, Message = "خطا در ثبت آیتم های سند حسابداری" });
                }
            }
            else // Model is not valid
            {
                return Json(new { Success = false, Message = ModelState.GetErorr() });
            }
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddFactor)]
        public ActionResult AddBuyBackFactor(short id, short periodId)
        {
            var factor = _factorRepo.Find(id, periodId);
            var factorItems = _blFactorItem.Where(a => a.FactorId == factor.Id).ToList();

            BackFactorViewModel viewModel = new BackFactorViewModel
            {
                Factor = factor,
                FactorItems = factorItems.Select(i => new BackFactorItems
                {
                    Description = i.Description,
                    Discount = Math.Round(i.Discount / (decimal)(i.MainUnitCount ?? 1)),
                    TotalDiscount = i.Discount,
                    FinalPrice = i.FinalPrice,
                    Count = i.MainUnitCount ?? 1,
                    MainUnitId = i.Product.MainUnitId,
                    MainUnitName = i.Product.Unit.Name,
                    SubUnitId = i.Product.SubUnitId,
                    SubUnitName = i.Product.Unit1?.Name,
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Tax = i.Tax,
                    TotalPrice = i.TotalPrice,
                    UnitPrice = i.UnitPrice,
                    UnitsRate = i.UnitsRate,
                }).ToList(),
                StrCreateDate = DateTime.Now.ToPersianDateTime().ToStringDate(),
                Description = $"فاکتور برگشت از خرید برای فاکتور خرید شماره {factor.Id}",
                TaxPercent = _fiscalRepo.Where(i => !i.IsClosed).FirstOrDefault()?.Vat ?? 0,
            };

            return View(viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.AddFactor)]
        [FiscalCheck]
        public JsonResult AddBuyBackFactor(BackFactorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var viewModelFactorId = viewModel.Factor.Id;
                var viewModelFactorPeriodId = viewModel.Factor.PeriodId;

                var dbBuyFactor = _factorRepo.Find(viewModelFactorId, viewModelFactorPeriodId);
                var dbBuyFactorItems = _blFactorItem.Where(i =>
                    i.FactorId == viewModelFactorId && i.PeriodId == viewModelFactorPeriodId);

                if (dbBuyFactor.Factors1 != null && dbBuyFactor.Factors1.Any())
                {
                    return Json(new
                    {
                        Success = false,
                        Message = $"برای این فاکتور قبلا فاکتور برگشتی صادر شده است."
                    });
                }

                if (dbBuyFactor.ReturnedPeriodId.HasValue || dbBuyFactor.ReturnForFactorId.HasValue)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = $"این فاکتور از نوع برگشتی است! امکان صدور فاکتور برگشتی برای فاکتور برگشتی وجود ندارد."
                    });
                }

                if (!dbBuyFactor.InvoiceId.HasValue)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = $"این فاکتور فاقد رسید است. فاکتور برگشتی تنها برای فاکتورهایی که برایشان رسید صادر شده، قابل ثبت است"
                    });
                }

                foreach (var viewModelFactorItem in viewModel.FactorItems)
                {
                    if (viewModelFactorItem.Count <= 0)
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = $"تعداد کالا نمی تواند کمتر یا مساوی صفر باشد."
                        });
                    }

                    if (!dbBuyFactorItems.Any(i => i.ProductId == viewModelFactorItem.ProductId))
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = $"کالاهای ثبت شده، با لیست کالاهای فاکتور شماره {viewModelFactorId}، منطبق نیستند."
                        });
                    }

                    var dbFactorItem = dbBuyFactorItems.FirstOrDefault(i => i.ProductId == viewModelFactorItem.ProductId);
                    if ((dbFactorItem?.MainUnitCount ?? 0) < viewModelFactorItem.Count)
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = $"تعداد ثبت شده برای کالای {dbFactorItem?.Product.Name}، بیش از تعداد متناظر در فاکتور اصلی است. حداکثر تعداد قابل قبول برای این کالا، {dbFactorItem?.MainUnitCount ?? 0} {dbFactorItem?.Product.Unit.Name} است."
                        });
                    }
                }

                short fiscalId = Fiscal.GetFiscalId();
                int addedFactorId = _factorRepo.GetLastIdentity(fiscalId) + 1;
                string description = $"فاکتور برگشت از خرید برای فاکتور شماره {viewModel.Factor.Id}";

                viewModel.Factor.Id = addedFactorId;
                viewModel.Factor.PeriodId = fiscalId;
                viewModel.Factor.CreatedDate = viewModel.StrCreateDate.ToMiladiDate();
                viewModel.Factor.CreatorUserId = UserIdentity.Getuserid();
                viewModel.Factor.IsBuy = true;
                viewModel.Factor.FactorTypeId = (byte)FactorType.BargashAzKharid;
                viewModel.Factor.IsPreFactor = false;
                viewModel.Factor.IsConfirmed = true;
                viewModel.Factor.SupplierId = dbBuyFactor.SupplierId;
                viewModel.Factor.ReturnForFactorId = dbBuyFactor.Id;
                viewModel.Factor.ReturnedPeriodId = dbBuyFactor.PeriodId;

                int factorId = _factorRepo.Add(viewModel.Factor);
                if (factorId == -1)
                {
                    return Json(new { Success = false, Message = "خطا در ثبت فاکتور" });
                }

                foreach (var item in viewModel.FactorItems)
                {
                    var unitsRate = _productRepo.Find(item.ProductId).UnitRate;
                    var totalDiscount = (decimal)item.Count * item.Discount;
                    var totalPrice = (decimal)item.Count * item.UnitPrice;
                    var finalPrice = totalPrice - totalDiscount + item.Tax;
                    FactorItem factorItem = new FactorItem
                    {
                        PeriodId = fiscalId,
                        FactorId = addedFactorId,
                        ProductId = item.ProductId,
                        UnitsRate = unitsRate,
                        MainUnitCount = item.Count,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = totalPrice,
                        Discount = totalDiscount,
                        Tax = item.Tax,
                        FinalPrice = finalPrice,
                        Description = item.Description
                    };

                    _blFactorItem.Add(factorItem);
                }

                int addedSanadId = _sanadRepo.GetLastIdentity(fiscalId) + 1;

                Sanad sanad = new Sanad
                {
                    Id = addedSanadId,
                    PeriodId = fiscalId,
                    ConfirmDate = viewModel.Factor.CreatedDate,
                    CreatedByUserId = viewModel.Factor.CreatorUserId,
                    CreatedDate = viewModel.Factor.CreatedDate,
                    Description = description,
                    IsAutomatic = true,
                    IsConfirmed = true,
                    TotalAmount = 0, // will be set later
                    SanadTypeId = (byte)SandType.FactorBargashtiKharid
                };

                if (!_sanadRepo.Add(sanad))
                {
                    return Json(new { Success = false, Message = "خطا در ثبت سند حسابداری" });
                }

                var factor = _factorRepo.Find(addedFactorId, fiscalId);
                factor.SanadId = addedSanadId;
                factor.PeriodId = fiscalId;

                #region calulate and set total amounts

                var sumTotalPrice = factor.FactorItems.Sum(i => i.TotalPrice);
                var sumTotalDiscount = factor.FactorItems.Sum(i => i.Discount);
                var sumTotalTax = factor.FactorItems.Sum(i => i.Tax);
                var sumFinalPrice = sumTotalPrice - sumTotalDiscount + sumTotalTax - factor.DiscountOnFactor +
                                    factor.Additions - factor.Deductions;

                factor.TotalPrice = sumTotalPrice;
                factor.TotalDiscount = sumTotalDiscount;
                factor.TotalTax = sumTotalTax;
                factor.FinalPrice = sumFinalPrice;

                #endregion

                if (!_factorRepo.Update(factor))
                {
                    return Json(new { Success = false, Message = "خطا در ثبت اطلاعات فاکتور" });
                }

                List<SanadItem> sanadItemsList = new List<SanadItem>();

                #region Supplier HesabDaryaftani

                int supplierDaryaftaniTafsilyId = _tafsiliRepo.Where(i =>
                    i.SarfaslId == (short)SarfaslEnums.HesabDaryaftani &&
                    i.SupplierId == factor.SupplierId).Select(a => a.Id).FirstOrDefault();

                SanadItem supplierHesabDaryaftaniSanadItem = new SanadItem
                {
                    SandId = addedSanadId,
                    PeriodId = fiscalId,
                    IssueTrackingNum = null,
                    ChequeId = null,
                    Description = description,
                    SarfaslId = (short)SarfaslEnums.HesabDaryaftani,
                    TafsiliId = supplierDaryaftaniTafsilyId,
                    Bedeh = factor.FinalPrice,
                    Bestan = 0,
                    ConfirmDate = sanad.ConfirmDate
                };

                sanadItemsList.Add(supplierHesabDaryaftaniSanadItem);

                #endregion

                if (factor.TotalDiscount + factor.DiscountOnFactor > 0)
                {
                    #region Takhfifat

                    SanadItem takhfifatSanadItem = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.HazinehTakhfifatNagdiForosh,
                        TafsiliId = null,
                        Bedeh = factor.TotalDiscount + factor.DiscountOnFactor,
                        Bestan = 0,
                        ConfirmDate = sanad.ConfirmDate
                    };

                    sanadItemsList.Add(takhfifatSanadItem);

                    #endregion
                }

                var deductions = factor.Deductions;
                var additions = factor.Additions;
                if (additions - deductions != 0)
                {
                    #region Sayere hazinehaye tozi va forush // برای تعدیل اضافات و کسورات فاکتور

                    SanadItem sayereHazinehayeToziVaForushSanadItem = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.SayereHazinehayeToziVaForosh,
                        TafsiliId = null,
                        Bedeh = 0, // will be set later
                        Bestan = 0, // will be set later
                        ConfirmDate = sanad.ConfirmDate
                    };

                    if (additions > deductions)
                    {
                        sayereHazinehayeToziVaForushSanadItem.Bestan = Math.Abs(additions - deductions);
                    }
                    else
                    {
                        sayereHazinehayeToziVaForushSanadItem.Bedeh = Math.Abs(additions - deductions);
                    }

                    sanadItemsList.Add(sayereHazinehayeToziVaForushSanadItem);

                    #endregion
                }

                if (factor.TotalTax > 0)
                {
                    #region MaliatBarArzeshAfzodeh

                    SanadItem maliatSanadItem = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.MaliatBarArzeshAfzodehForosh,
                        TafsiliId = null,
                        Bedeh = 0,
                        Bestan = factor.TotalTax,
                        ConfirmDate = sanad.ConfirmDate
                    };

                    sanadItemsList.Add(maliatSanadItem);

                    #endregion
                }

                #region Foroosh

                foreach (var item in factor.FactorItems)
                {
                    if (item.TotalPrice > 0)
                    {
                        var bargashtAzKharidTafsilyId = _tafsiliRepo.Where(a =>
                            a.SarfaslId == (short)SarfaslEnums.BarghashtAzKharid &&
                            a.ProductId == item.ProductId).Select(a => a.Id).FirstOrDefault();

                        SanadItem bargashtAzForoshSanadItem = new SanadItem
                        {
                            SandId = addedSanadId,
                            PeriodId = fiscalId,
                            IssueTrackingNum = null,
                            ChequeId = null,
                            Description = description,
                            SarfaslId = (short)SarfaslEnums.BarghashtAzKharid,
                            TafsiliId = bargashtAzKharidTafsilyId,
                            Bedeh = 0,
                            Bestan = item.TotalPrice,
                            ConfirmDate = sanad.ConfirmDate
                        };

                        sanadItemsList.Add(bargashtAzForoshSanadItem);
                    }
                }

                #endregion

                var sumBedeh = sanadItemsList.Sum(i => i.Bedeh);
                var sumBestan = sanadItemsList.Sum(i => i.Bestan);
                if (sumBestan != sumBedeh)
                {
                    return Json(new { Success = false, Message = "خطا در سند حسابداری! سند حسابداری، تراز نیست!" });
                }

                var theSanad = _sanadRepo.Find(addedSanadId, fiscalId);
                theSanad.TotalAmount = sumBestan; // or sumBedeh

                if (_sanadItemRepo.AddRange(sanadItemsList) && _sanadRepo.Update(theSanad))
                {
                    return Json(new { Success = true, Id = addedFactorId });
                }
                else
                {
                    return Json(new { Success = false, Message = "خطا در ثبت آیتم های سند حسابداری" });
                }
            }
            else // Model is not valid
            {
                return Json(new { Success = false, Message = ModelState.GetErorr() });
            }
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddFactor)]
        [FiscalCheck]
        public ActionResult AddSellFactor()
        {
            FactorViewModel viewModel = new FactorViewModel
            {
                PriceTypes = _blPriceType.Select().Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name }),
                SelectCustomers = _customerRepo.Select().Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.BusinessName + "-" + s.OwnerName }),
                Products = _productRepo.Where(a => a.IsAvailable && a.IsSellable),
                Units = _blUnit.Select(),
                TaxPercent = _fiscalRepo.Where(i => !i.IsClosed).FirstOrDefault()?.Vat ?? 0,
                StrCreatedDate = DateTime.Now.ToPersianDateTime().ToStringDate()
            };

            return View(viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.AddFactor)]
        [FiscalCheck]
        public JsonResult AddSellFactor(AddFactorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                short fiscalId = Fiscal.GetFiscalId();
                int addedFactorId = _factorRepo.GetLastIdentity(fiscalId) + 1;
                string description = $"فروش طی فاکتور شماره {addedFactorId}";

                viewModel.Factor.Id = addedFactorId;
                viewModel.Factor.PeriodId = fiscalId;
                viewModel.Factor.CreatedDate = viewModel.StrCreateDate.ToMiladiDate();
                viewModel.Factor.CreatorUserId = UserIdentity.Getuserid();
                viewModel.Factor.IsBuy = false;
                viewModel.Factor.FactorTypeId = (byte)FactorType.Foroosh;
                viewModel.Factor.IsPreFactor = false;
                viewModel.Factor.IsConfirmed = true;
                viewModel.Factor.MarketerId = _customerRepo.Find(viewModel.Factor.CustomerId ?? 0).MarketerId;

                int factorId = _factorRepo.Add(viewModel.Factor);
                if (factorId == -1)
                {
                    return Json(new { Success = false, Message = "خطا در ثبت فاکتور" });
                }

                foreach (var item in viewModel.FactorItems)
                {
                    var unitsRate = _productRepo.Find(item.ProductId).UnitRate;
                    var totalDiscount = (decimal)item.Count * item.Discount;
                    var totalPrice = (decimal)item.Count * item.UnitPrice;
                    var totalCommission = (decimal)item.Count * item.Commission;
                    var finalPrice = totalPrice - totalDiscount + item.Tax;
                    FactorItem factorItem = new FactorItem
                    {
                        UnitId = item.UnitId,
                        PeriodId = fiscalId,
                        FactorId = addedFactorId,
                        ProductId = item.ProductId,
                        UnitsRate = unitsRate,
                        MainUnitCount = item.Count,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = totalPrice,
                        Discount = totalDiscount,
                        Tax = item.Tax,
                        MarketerCommission = totalCommission,
                        FinalPrice = finalPrice,
                        Description = item.Description
                    };

                    _blFactorItem.Add(factorItem);
                    _productRepo.UpdateProductPending(item.ProductId, item.Count);
                }

                int addedSanadId = _sanadRepo.GetLastIdentity(fiscalId) + 1;

                Sanad sanad = new Sanad
                {
                    Id = addedSanadId,
                    PeriodId = fiscalId,
                    ConfirmDate = viewModel.Factor.CreatedDate,
                    CreatedByUserId = viewModel.Factor.CreatorUserId,
                    CreatedDate = viewModel.Factor.CreatedDate,
                    Description = description,
                    IsAutomatic = true,
                    IsConfirmed = true,
                    TotalAmount = 0, // will be set later
                    SanadTypeId = (byte)SandType.FactorForosh
                };

                if (!_sanadRepo.Add(sanad))
                {
                    return Json(new { Success = false, Message = "خطا در ثبت سند حسابداری" });
                }

                var factor = _factorRepo.Find(addedFactorId, fiscalId);
                factor.SanadId = addedSanadId;
                factor.PeriodId = fiscalId;

                #region calulate and set total amounts

                var sumTotalPrice = factor.FactorItems.Sum(i => i.TotalPrice);
                var sumTotalDiscount = factor.FactorItems.Sum(i => i.Discount);
                var sumTotalTax = factor.FactorItems.Sum(i => i.Tax);
                var sumTotalCommission = viewModel.AssignCommission ? factor.FactorItems.Sum(i => i.MarketerCommission) : 0;
                var sumFinalPrice = sumTotalPrice - sumTotalDiscount + sumTotalTax - factor.DiscountOnFactor +
                                    factor.Additions - factor.Deductions;

                factor.TotalPrice = sumTotalPrice;
                factor.TotalDiscount = sumTotalDiscount;
                factor.TotalTax = sumTotalTax;
                factor.MarketerTotalCommission = sumTotalCommission;
                factor.FinalPrice = sumFinalPrice;

                #endregion

                if (!_factorRepo.Update(factor))
                {
                    return Json(new { Success = false, Message = "خطا در ثبت اطلاعات فاکتور" });
                }

                List<SanadItem> sanadItemsList = new List<SanadItem>();

                #region Customer HesabDaryaftani

                int customerDaryaftaniTafsilyId = _tafsiliRepo.Where(i =>
                    i.SarfaslId == (short)SarfaslEnums.HesabDaryaftani &&
                    i.CustomerId == factor.CustomerId).Select(a => a.Id).FirstOrDefault();

                SanadItem customerHesabDaryaftaniSanadItem = new SanadItem
                {
                    SandId = addedSanadId,
                    PeriodId = fiscalId,
                    IssueTrackingNum = null,
                    ChequeId = null,
                    Description = description,
                    SarfaslId = (short)SarfaslEnums.HesabDaryaftani,
                    TafsiliId = customerDaryaftaniTafsilyId,
                    Bedeh = factor.FinalPrice,
                    Bestan = 0,
                    ConfirmDate = sanad.ConfirmDate
                };

                sanadItemsList.Add(customerHesabDaryaftaniSanadItem);

                #endregion

                if (factor.MarketerId.HasValue && viewModel.AssignCommission && factor.MarketerTotalCommission > 0)
                {
                    #region Marketer

                    int marketerPardakhtaniTafsilyId = _tafsiliRepo.Where(a =>
                        a.SarfaslId == (short)SarfaslEnums.HesabPardakhtani &&
                        a.PersonnelId == factor.MarketerId).Select(a => a.Id).FirstOrDefault();

                    SanadItem marketerPardakhtaniSanadItem = new SanadItem()
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.HesabPardakhtani,
                        TafsiliId = marketerPardakhtaniTafsilyId,
                        Bedeh = 0,
                        Bestan = factor.MarketerTotalCommission,
                        ConfirmDate = sanad.ConfirmDate
                    };

                    sanadItemsList.Add(marketerPardakhtaniSanadItem);

                    #endregion

                    #region Bazaryabi

                    SanadItem bazaryabiSanadItem = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.HazinehBazaryabiVaPorsant,
                        TafsiliId = null,
                        Bedeh = factor.MarketerTotalCommission,
                        Bestan = 0,
                        ConfirmDate = sanad.ConfirmDate
                    };
                    sanadItemsList.Add(bazaryabiSanadItem);

                    #endregion
                }

                if (factor.TotalDiscount + factor.DiscountOnFactor > 0)
                {
                    #region Takhfifat

                    SanadItem takhfifatSanadItem = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.HazinehTakhfifatNagdiForosh,
                        TafsiliId = null,
                        Bedeh = factor.TotalDiscount + factor.DiscountOnFactor,
                        Bestan = 0,
                        ConfirmDate = sanad.ConfirmDate

                    };

                    sanadItemsList.Add(takhfifatSanadItem);

                    #endregion
                }

                var deductions = factor.Deductions;
                var additions = factor.Additions;
                if (additions - deductions != 0)
                {
                    #region Sayere hazinehaye tozi va forush // برای تعدیل اضافات و کسورات فاکتور

                    SanadItem sayereHazinehayeToziVaForushSanadItem = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.SayereHazinehayeToziVaForosh,
                        TafsiliId = null,
                        Bedeh = 0, // will be set later
                        Bestan = 0, // will be set later
                        ConfirmDate = sanad.ConfirmDate
                    };

                    if (additions > deductions)
                    {
                        sayereHazinehayeToziVaForushSanadItem.Bestan = Math.Abs(additions - deductions);
                    }
                    else
                    {
                        sayereHazinehayeToziVaForushSanadItem.Bedeh = Math.Abs(additions - deductions);
                    }

                    sanadItemsList.Add(sayereHazinehayeToziVaForushSanadItem);

                    #endregion
                }


                if (factor.TotalTax > 0)
                {
                    #region MaliatBarArzeshAfzodeh

                    SanadItem maliatSanadItem = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.MaliatBarArzeshAfzodehForosh,
                        TafsiliId = null,
                        Bedeh = 0,
                        Bestan = factor.TotalTax,
                        ConfirmDate = sanad.ConfirmDate

                    };

                    sanadItemsList.Add(maliatSanadItem);

                    #endregion
                }

                #region Foroosh

                foreach (var item in factor.FactorItems)
                {
                    if (item.TotalPrice > 0)
                    {
                        var foroshTafsilyId = _tafsiliRepo.Where(a =>
                            a.SarfaslId == (short)SarfaslEnums.ForosheKala &&
                            a.ProductId == item.ProductId).Select(a => a.Id).FirstOrDefault();

                        SanadItem foroshSanadItem = new SanadItem
                        {
                            SandId = addedSanadId,
                            PeriodId = fiscalId,
                            IssueTrackingNum = null,
                            ChequeId = null,
                            Description = description,
                            SarfaslId = (short)SarfaslEnums.ForosheKala,
                            TafsiliId = foroshTafsilyId,
                            Bedeh = 0,
                            Bestan = item.TotalPrice,
                            ConfirmDate = sanad.ConfirmDate
                        };

                        sanadItemsList.Add(foroshSanadItem);
                    }
                }

                #endregion

                var sumBedeh = sanadItemsList.Sum(i => i.Bedeh);
                var sumBestan = sanadItemsList.Sum(i => i.Bestan);
                if (sumBestan != sumBedeh)
                {
                    return Json(new { Success = false, Message = "خطا در سند حسابداری! سند حسابداری، تراز نیست!" });
                }

                var theSanad = _sanadRepo.Find(addedSanadId, fiscalId);
                theSanad.TotalAmount = sumBestan; // or sumBedeh

                if (_sanadItemRepo.AddRange(sanadItemsList) && _sanadRepo.Update(theSanad))
                {
                    return Json(new { Success = true, Id = addedFactorId });
                }
                else
                {
                    return Json(new { Success = false, Message = "خطا در ثبت آیتم های سند حسابداری" });
                }
            }
            else // Model is not valid
            {
                return Json(new { Success = false, Message = ModelState.GetErorr() });
            }
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddFactor)]
        public ActionResult AddSellBackFactor(short id, short periodId)
        {
            var factor = _factorRepo.Find(id, periodId);
            var factorItems = _blFactorItem.Where(a => a.FactorId == factor.Id).ToList();

            BackFactorViewModel viewModel = new BackFactorViewModel
            {
                Factor = factor,
                FactorItems = factorItems.Select(i => new BackFactorItems
                {
                    Description = i.Description,
                    Discount = Math.Round(i.Discount / (decimal)(i.MainUnitCount ?? 1)),
                    TotalDiscount = i.Discount,
                    FinalPrice = i.FinalPrice,
                    Count = i.MainUnitCount ?? 1,
                    MainUnitId = i.Product.MainUnitId,
                    MainUnitName = i.Product.Unit.Name,
                    SubUnitId = i.Product.SubUnitId,
                    SubUnitName = i.Product.Unit1?.Name,
                    TotalCommission = i.MarketerCommission,
                    Commission = Math.Round(i.MarketerCommission / (decimal)(i.MainUnitCount ?? 1)),
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Tax = i.Tax,
                    TotalPrice = i.TotalPrice,
                    UnitPrice = i.UnitPrice,
                    UnitsRate = i.UnitsRate,
                }).ToList(),
                StrCreateDate = DateTime.Now.ToPersianDateTime().ToStringDate(),
                Description = $"فاکتور برگشت از فروش برای فاکتور فروش شماره {factor.Id}",
                TaxPercent = _fiscalRepo.Where(i => !i.IsClosed).FirstOrDefault()?.Vat ?? 0,
            };

            return View(viewModel);
        }

        [AjaxOnly]
        [HttpPost]
        [AccessControl(ActionsEnum.AddFactor)]
        [FiscalCheck]
        public JsonResult AddSellBackFactor(BackFactorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var viewModelFactorId = viewModel.Factor.Id;
                var viewModelFactorPeriodId = viewModel.Factor.PeriodId;

                var dbSellFactor = _factorRepo.Find(viewModelFactorId, viewModelFactorPeriodId);
                var dbSellFactorItems = _blFactorItem.Where(i =>
                    i.FactorId == viewModelFactorId && i.PeriodId == viewModelFactorPeriodId);

                if (dbSellFactor.Factors1 != null && dbSellFactor.Factors1.Any())
                {
                    return Json(new
                    {
                        Success = false,
                        Message = $"برای این فاکتور قبلا فاکتور برگشتی صادر شده است."
                    });
                }

                if (dbSellFactor.ReturnedPeriodId.HasValue || dbSellFactor.ReturnForFactorId.HasValue)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = $"این فاکتور از نوع برگشتی است! امکان صدور فاکتور برگشتی برای فاکتور برگشتی وجود ندارد."
                    });
                }

                if (!dbSellFactor.InvoiceId.HasValue)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = $"این فاکتور فاقد حواله است. فاکتور برگشتی تنها برای فاکتورهایی که برایشان حواله صادر شده، قابل ثبت است"
                    });
                }

                foreach (var viewModelFactorItem in viewModel.FactorItems)
                {
                    if (viewModelFactorItem.Count <= 0)
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = $"تعداد کالا نمی تواند کمتر یا مساوی صفر باشد."
                        });
                    }

                    if (!dbSellFactorItems.Any(i => i.ProductId == viewModelFactorItem.ProductId))
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = $"کالاهای ثبت شده، با لیست کالاهای فاکتور شماره {viewModelFactorId}، منطبق نیستند."
                        });
                    }

                    var dbFactorItem = dbSellFactorItems.FirstOrDefault(i => i.ProductId == viewModelFactorItem.ProductId);
                    if ((dbFactorItem?.MainUnitCount ?? 0) < viewModelFactorItem.Count)
                    {
                        return Json(new
                        {
                            Success = false,
                            Message = $"تعداد ثبت شده برای کالای {dbFactorItem?.Product.Name}، بیش از تعداد متناظر در فاکتور اصلی است. حداکثر تعداد قابل قبول برای این کالا، {dbFactorItem?.MainUnitCount ?? 0} {dbFactorItem?.Product.Unit.Name} است."
                        });
                    }
                }

                short fiscalId = Fiscal.GetFiscalId();
                int addedFactorId = _factorRepo.GetLastIdentity(fiscalId) + 1;
                string description = $"فاکتور برگشت از فروش برای فاکتور شماره {viewModel.Factor.Id}";

                viewModel.Factor.Id = addedFactorId;
                viewModel.Factor.PeriodId = fiscalId;
                viewModel.Factor.CreatedDate = viewModel.StrCreateDate.ToMiladiDate();
                viewModel.Factor.CreatorUserId = UserIdentity.Getuserid();
                viewModel.Factor.IsBuy = false;
                viewModel.Factor.FactorTypeId = (byte)FactorType.BargashAzForoosh;
                viewModel.Factor.IsPreFactor = false;
                viewModel.Factor.IsConfirmed = true;
                viewModel.Factor.CustomerId = dbSellFactor.CustomerId;
                viewModel.Factor.MarketerId = dbSellFactor.MarketerId;
                viewModel.Factor.ReturnForFactorId = dbSellFactor.Id;
                viewModel.Factor.ReturnedPeriodId = dbSellFactor.PeriodId;

                int factorId = _factorRepo.Add(viewModel.Factor);
                if (factorId == -1)
                {
                    return Json(new { Success = false, Message = "خطا در ثبت فاکتور" });
                }

                foreach (var item in viewModel.FactorItems)
                {
                    var unitsRate = _productRepo.Find(item.ProductId).UnitRate;
                    var totalDiscount = (decimal)item.Count * item.Discount;
                    var totalPrice = (decimal)item.Count * item.UnitPrice;
                    var totalCommission = (decimal)item.Count * item.Commission;
                    var finalPrice = totalPrice - totalDiscount + item.Tax;
                    FactorItem factorItem = new FactorItem
                    {
                        PeriodId = fiscalId,
                        FactorId = addedFactorId,
                        ProductId = item.ProductId,
                        UnitsRate = unitsRate,
                        MainUnitCount = item.Count,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = totalPrice,
                        Discount = totalDiscount,
                        Tax = item.Tax,
                        MarketerCommission = totalCommission,
                        FinalPrice = finalPrice,
                        Description = item.Description
                    };

                    _blFactorItem.Add(factorItem);
                    if (!dbSellFactor.InvoiceId.HasValue) // اگر فاکتور فروش اصلی، بدون رسیدیاحواله بود
                    {
                        _productRepo.UpdateProductPending(item.ProductId, item.Count,
                            false); // تعداد کالای در انتظار رسیدیاحواله را کاهش بده
                    }
                }

                int addedSanadId = _sanadRepo.GetLastIdentity(fiscalId) + 1;

                Sanad sanad = new Sanad
                {
                    Id = addedSanadId,
                    PeriodId = fiscalId,
                    ConfirmDate = viewModel.Factor.CreatedDate,
                    CreatedByUserId = viewModel.Factor.CreatorUserId,
                    CreatedDate = viewModel.Factor.CreatedDate,
                    Description = description,
                    IsAutomatic = true,
                    IsConfirmed = true,
                    TotalAmount = 0, // will be set later
                    SanadTypeId = (byte)SandType.FactorBargashtiForosh
                };

                if (!_sanadRepo.Add(sanad))
                {
                    return Json(new { Success = false, Message = "خطا در ثبت سند حسابداری" });
                }

                var factor = _factorRepo.Find(addedFactorId, fiscalId);
                factor.SanadId = addedSanadId;
                factor.PeriodId = fiscalId;

                #region calulate and set total amounts

                var sumTotalPrice = factor.FactorItems.Sum(i => i.TotalPrice);
                var sumTotalDiscount = factor.FactorItems.Sum(i => i.Discount);
                var sumTotalTax = factor.FactorItems.Sum(i => i.Tax);
                var sumTotalCommission = factor.FactorItems.Sum(i => i.MarketerCommission);
                var sumFinalPrice = sumTotalPrice - sumTotalDiscount + sumTotalTax - factor.DiscountOnFactor +
                                    factor.Additions - factor.Deductions;

                factor.TotalPrice = sumTotalPrice;
                factor.TotalDiscount = sumTotalDiscount;
                factor.TotalTax = sumTotalTax;
                factor.MarketerTotalCommission = sumTotalCommission;
                factor.FinalPrice = sumFinalPrice;

                #endregion

                if (!_factorRepo.Update(factor))
                {
                    return Json(new { Success = false, Message = "خطا در ثبت اطلاعات فاکتور" });
                }

                List<SanadItem> sanadItemsList = new List<SanadItem>();

                #region Customer HesabPardakhtani

                int customerPrdakhtaniTafsilyId = _tafsiliRepo.Where(i =>
                    i.SarfaslId == (short)SarfaslEnums.HesabPardakhtani &&
                    i.CustomerId == factor.CustomerId).Select(a => a.Id).FirstOrDefault();

                SanadItem customerHesabPardakhtaniSanadItem = new SanadItem
                {
                    SandId = addedSanadId,
                    PeriodId = fiscalId,
                    IssueTrackingNum = null,
                    ChequeId = null,
                    Description = description,
                    SarfaslId = (short)SarfaslEnums.HesabPardakhtani,
                    TafsiliId = customerPrdakhtaniTafsilyId,
                    Bedeh = 0,
                    Bestan = factor.FinalPrice,
                    ConfirmDate = sanad.ConfirmDate
                };

                sanadItemsList.Add(customerHesabPardakhtaniSanadItem);

                #endregion

                #region Marketer

                if (factor.MarketerId.HasValue)
                {
                    int marketerDaryaftaniTafsilyId = _tafsiliRepo.Where(a =>
                        a.SarfaslId == (short)SarfaslEnums.HesabDaryaftani &&
                        a.PersonnelId == factor.MarketerId).Select(a => a.Id).FirstOrDefault();
                    SanadItem marketerDaryaftaniSanadItem = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.HesabDaryaftani,
                        TafsiliId = marketerDaryaftaniTafsilyId,
                        Bedeh = factor.MarketerTotalCommission,
                        Bestan = 0,
                        ConfirmDate = sanad.ConfirmDate
                    };
                    sanadItemsList.Add(marketerDaryaftaniSanadItem);
                }

                #endregion

                #region Bazaryabi

                SanadItem bazaryabiSanadItem = new SanadItem
                {
                    SandId = addedSanadId,
                    PeriodId = fiscalId,
                    IssueTrackingNum = null,
                    ChequeId = null,
                    Description = description,
                    SarfaslId = (short)SarfaslEnums.HazinehBazaryabiVaPorsant,
                    TafsiliId = null,
                    Bedeh = 0,
                    Bestan = factor.MarketerTotalCommission,
                    ConfirmDate = sanad.ConfirmDate
                };
                sanadItemsList.Add(bazaryabiSanadItem);

                #endregion

                if (factor.TotalDiscount + factor.DiscountOnFactor > 0)
                {
                    #region Takhfifat

                    SanadItem takhfifatSanadItem = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.TakhfifatNagdiKharid,
                        TafsiliId = null,
                        Bedeh = 0,
                        Bestan = factor.TotalDiscount + factor.DiscountOnFactor,
                        ConfirmDate = sanad.ConfirmDate
                    };

                    sanadItemsList.Add(takhfifatSanadItem);

                    #endregion
                }

                var deductions = factor.Deductions;
                var additions = factor.Additions;
                if (additions - deductions != 0)
                {
                    #region Sayere hazinehaye tozi va forush // برای تعدیل اضافات و کسورات فاکتور

                    SanadItem sayereHazinehayeToziVaForushSanadItem = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.SayereHazinehayeToziVaForosh,
                        TafsiliId = null,
                        Bedeh = 0, // will be set later
                        Bestan = 0, // will be set later
                        ConfirmDate = sanad.ConfirmDate
                    };

                    if (additions > deductions)
                    {
                        sayereHazinehayeToziVaForushSanadItem.Bedeh = Math.Abs(additions - deductions);
                    }
                    else
                    {
                        sayereHazinehayeToziVaForushSanadItem.Bestan = Math.Abs(additions - deductions);
                    }

                    sanadItemsList.Add(sayereHazinehayeToziVaForushSanadItem);

                    #endregion
                }

                if (factor.TotalTax > 0)
                {
                    #region MaliatBarArzeshAfzodeh

                    SanadItem maliatSanadItem = new SanadItem
                    {
                        SandId = addedSanadId,
                        PeriodId = fiscalId,
                        IssueTrackingNum = null,
                        ChequeId = null,
                        Description = description,
                        SarfaslId = (short)SarfaslEnums.MaliatBarArzeshAfzodehForosh,
                        TafsiliId = null,
                        Bedeh = factor.TotalTax,
                        Bestan = 0,
                        ConfirmDate = sanad.ConfirmDate
                    };

                    sanadItemsList.Add(maliatSanadItem);

                    #endregion
                }

                #region Foroosh

                foreach (var item in factor.FactorItems)
                {
                    if (item.TotalPrice > 0)
                    {
                        var bargashtAzForoshTafsilyId = _tafsiliRepo.Where(a =>
                            a.SarfaslId == (short)SarfaslEnums.BarghashtAzForosh &&
                            a.ProductId == item.ProductId).Select(a => a.Id).FirstOrDefault();

                        SanadItem bargashtAzForoshSanadItem = new SanadItem
                        {
                            SandId = addedSanadId,
                            PeriodId = fiscalId,
                            IssueTrackingNum = null,
                            ChequeId = null,
                            Description = description,
                            SarfaslId = (short)SarfaslEnums.BarghashtAzForosh,
                            TafsiliId = bargashtAzForoshTafsilyId,
                            Bedeh = item.TotalPrice,
                            Bestan = 0,
                            ConfirmDate = sanad.ConfirmDate
                        };

                        sanadItemsList.Add(bargashtAzForoshSanadItem);
                    }
                }

                #endregion

                var sumBedeh = sanadItemsList.Sum(i => i.Bedeh);
                var sumBestan = sanadItemsList.Sum(i => i.Bestan);
                if (sumBestan != sumBedeh)
                {
                    return Json(new { Success = false, Message = "خطا در سند حسابداری! سند حسابداری، تراز نیست!" });
                }

                var theSanad = _sanadRepo.Find(addedSanadId, fiscalId);
                theSanad.TotalAmount = sumBestan; // or sumBedeh

                if (_sanadItemRepo.AddRange(sanadItemsList) && _sanadRepo.Update(theSanad))
                {
                    return Json(new { Success = true, Id = addedFactorId });
                }
                else
                {
                    return Json(new { Success = false, Message = "خطا در ثبت آیتم های سند حسابداری" });
                }
            }
            else // Model is not valid
            {
                return Json(new { Success = false, Message = ModelState.GetErorr() });
            }
        }

        [HttpGet]
        [AjaxOnly]
        [CoustomAuthrize]
        public JsonResult GetProductDetails(short productId, short customerId, short priceTypeId)
        {
            var discount = _offerItemRepo.Where(a =>
                a.ProductId == productId && a.Offer.IsActive == true && a.Offer.StartDate < DateTime.Now &&
                a.Offer.ExpDate > DateTime.Now && a.Offer.ForCustomerCategories.Contains(customerId.ToString())).Select(
                a => new
                {
                    type = a.OfferTypeId,
                    amount = a.DiscountAmount,
                    percent = a.DiscountPercent,
                    giftproduct = a.Product.Name,
                    minproductCount = a.MinProductCount,
                    gifproductId = a.GiftProductId,
                    gifproductCount = a.GiftProductCount,
                    gifId = a.GiftProductId


                }).FirstOrDefault();

            decimal gifPrice = 0;


            if (discount != null)
            {
                if (discount.type == 3)
                {
                    gifPrice = _blProductPriceList
                        .Where(p => p.PriceTypeId == priceTypeId && p.ProductId == discount.gifId)
                        .Select(p => p.Price).FirstOrDefault();
                }
            }


            var product = _productRepo.Where(a => a.Id == productId).FirstOrDefault();
            var subUnit = _blUnit.Where(a => a.Id == product.SubUnitId).Select(a => a.Name).FirstOrDefault();
            if (product.UnitRate == null)
            {
                product.UnitRate = 0;
            }
            var pro = new
            {
                inventoy = product.Inventory,
                unitrate = product.UnitRate,
                subUnit,
                marketercommision = product.MarketerCommission
            };

            var price = _blProductPriceList.Where(a => a.PriceTypeId == priceTypeId && a.ProductId == productId)
                .Select(a => a.Price).FirstOrDefault();


            return Json(new { discount, pro, gifPrice, price }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.DeleteFactor)]
        [FactorUpdateCheck]
        public JsonResult DeleteFactor(int id, short periodId)
        {
            var factor = _factorRepo.Find(id, periodId);

            if (factor.InvoiceId == null)
            {
                if (factor.FactorItems.Any())
                {
                    foreach (var item in factor.FactorItems)
                    {
                        _productRepo.UpdateProductPending(item.ProductId, item.MainUnitCount ?? 0, false);
                    }
                }
            }

            if (_sanadRepo.Delete(factor.SanadId ?? 0, periodId, out bool isUsed))
            {
                factor.IsDeleted = true;
                if (_factorRepo.Update(factor))
                {
                    return Json(new JsonData { Success = true });
                }
                else
                {
                    return Json(new JsonData { Success = false });
                }

            }
            else
            {
                return Json(new JsonData { Success = false, IsUsed = isUsed });
            }
        }

        [AjaxOnly]
        [HttpGet]
        [AccessControl(ActionsEnum.ShowFactors)]
        public ActionResult ShowFactorDetails(int id, short periodId)
        {
            var factor = _factorRepo.Find(id, periodId);
            var factorItems = _blFactorItem.Where(a => a.FactorId == factor.Id && a.PeriodId == periodId).ToList();
            //string wareHouse = _blWarehouse.Where(x => x.Id == factorItems.FirstOrDefault().ProductId).SingleOrDefault().Name;
            ShowFactorViewModel model = new ShowFactorViewModel()
            {
                Factor = factor,
                FactorItems = factorItems,
            };

            return PartialView(model);
        }

        [AjaxOnly]
        [HttpGet]
        [CoustomAuthrize]
        public ActionResult FactorDetailsWithoutPrices(int id, short periodId)
        {
            var factor = _factorRepo.Find(id, periodId);
            var factorItems = _blFactorItem.Where(a => a.FactorId == factor.Id && a.PeriodId == periodId).ToList();

            ShowFactorViewModel model = new ShowFactorViewModel()
            {
                Factor = factor,
                FactorItems = factorItems
            };

            return PartialView("_FactorDetailsWithoutPrices", model);
        }

        [AjaxOnly]
        [HttpGet]
        [AccessControl(ActionsEnum.AddFactorForOrder)]
        [FiscalCheck]
        public ActionResult FactorForOrder(int id)
        {
            List<FactorItem> factorItems = GetFactorItemsByOrder(id);
            Factor factor = GetFactorByOrder(id, factorItems);

            ShowFactorViewModel model = new ShowFactorViewModel
            {
                OrderId = id,
                Factor = factor,
                FactorItems = factorItems
            };

            return PartialView("_FactorForOrder", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddFactorForOrder)]
        [FiscalCheck]
        public ActionResult FactorForOrder(ShowFactorViewModel viewModel)
        {
            DateTime currentTime = DateTime.Now;

            Order order = _orderRepo.Find(viewModel.OrderId);

            if (!ModelState.IsValid)
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Warning, position: ToastPosition.TopCenter);
                return RedirectToAction("AllOrders", "Order");
            }

            List<FactorItem> factorItems = GetFactorItemsByOrder(viewModel.OrderId);
            Factor factor = GetFactorByOrder(viewModel.OrderId, factorItems);

            factor.Additions = viewModel.Factor.Additions;
            factor.Deductions = viewModel.Factor.Deductions;
            factor.DiscountOnFactor = viewModel.Factor.DiscountOnFactor;
            factor.Description = viewModel.Factor.Description;
            factor.FinalPrice = factor.FinalPrice - viewModel.Factor.DiscountOnFactor +
                                viewModel.Factor.Additions - viewModel.Factor.Deductions;
            short fiscalId = Fiscal.GetFiscalId();
            int addedFactorId = _factorRepo.GetLastIdentity(fiscalId) + 1;
            factor.Id = addedFactorId;
            factor.PeriodId = fiscalId;
            factor.CreatorUserId = UserIdentity.Getuserid();
            factor.CreatedDate = currentTime;
            factor.MarketerId = order.Customer.MarketerId;
            factor.IsBuy = false;
            factor.FactorTypeId = (byte)FactorType.Foroosh;

            int result = _factorRepo.Add(factor);
            if (result <= 0)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره فاکتور", type: ToastType.Error, position: ToastPosition.TopCenter);
                return RedirectToAction("AllOrders", "Order");
            }

            List<Product> productsToUpdate = new List<Product>();
            foreach (var factorItem in factorItems)
            {
                factorItem.PeriodId = fiscalId;
                factorItem.FactorId = result;

                var product = _productRepo.Find(factorItem.ProductId);
                product.PendingsCount += factorItem.MainUnitCount ?? 0;
                productsToUpdate.Add(product);
            }

            bool factorItemsAdded = _blFactorItem.AddRange(factorItems);
            if (!factorItemsAdded)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره آیتم های فاکتور", type: ToastType.Error, position: ToastPosition.TopCenter);
                return RedirectToAction("AllOrders", "Order");
            }

            _productRepo.UpdateRange(productsToUpdate);

            int addedSanadId = _sanadRepo.GetLastIdentity(fiscalId) + 1;
            string factorSanadDescription = $"فروش طی فاکتور شماره {addedFactorId}";

            #region Sanad Items

            List<SanadItem> sanadItemsList = new List<SanadItem>();

            #region HesabDaryaftani

            int hesabDaryaftaniTafsilyId = _tafsiliRepo.Where(a =>
                a.SarfaslId == (short)SarfaslEnums.HesabDaryaftani &&
                a.CustomerId == factor.CustomerId).Select(a => a.Id).FirstOrDefault();

            SanadItem hesabDaryaftaniSanadItem = new SanadItem
            {
                SandId = addedSanadId,
                PeriodId = fiscalId,
                IssueTrackingNum = null,
                ChequeId = null,
                Description = factorSanadDescription,
                SarfaslId = (short)SarfaslEnums.HesabDaryaftani,
                TafsiliId = hesabDaryaftaniTafsilyId,
                Bedeh = factor.FinalPrice,
                Bestan = 0,
                ConfirmDate = currentTime
            };
            sanadItemsList.Add(hesabDaryaftaniSanadItem);

            #endregion

            if (factor.MarketerId.HasValue)
            {
                #region Marketer

                int hesabPardakhtaniTafsilyId = _tafsiliRepo.Where(a =>
                    a.SarfaslId == (short)SarfaslEnums.HesabPardakhtani &&
                    a.PersonnelId == factor.MarketerId).Select(a => a.Id).FirstOrDefault();

                SanadItem hesabPardakhtaniSanadItem = new SanadItem()
                {
                    SandId = addedSanadId,
                    PeriodId = fiscalId,
                    IssueTrackingNum = null,
                    ChequeId = null,
                    Description = factorSanadDescription,
                    SarfaslId = (short)SarfaslEnums.HesabPardakhtani,
                    TafsiliId = hesabPardakhtaniTafsilyId,
                    Bedeh = 0,
                    Bestan = factor.MarketerTotalCommission,
                    ConfirmDate = currentTime
                };
                sanadItemsList.Add(hesabPardakhtaniSanadItem);

                #endregion

                #region BazaryabiVaPercent

                SanadItem bazaryabiVaPercentSanadItem = new SanadItem
                {
                    SandId = addedSanadId,
                    PeriodId = fiscalId,
                    IssueTrackingNum = null,
                    ChequeId = null,
                    Description = factorSanadDescription,
                    SarfaslId = (short)SarfaslEnums.HazinehBazaryabiVaPorsant,
                    TafsiliId = null,
                    Bedeh = factor.MarketerTotalCommission,
                    Bestan = 0,
                    ConfirmDate = currentTime
                };
                sanadItemsList.Add(bazaryabiVaPercentSanadItem);

                #endregion
            }

            #region Takhfifat

            SanadItem takhfifatSanadItem = new SanadItem()
            {
                SandId = addedSanadId,
                PeriodId = fiscalId,
                IssueTrackingNum = null,
                ChequeId = null,
                Description = factorSanadDescription,
                SarfaslId = (short)SarfaslEnums.HazinehTakhfifatNagdiForosh,
                TafsiliId = null,
                Bedeh = factor.TotalDiscount + factor.DiscountOnFactor,
                Bestan = 0,
                ConfirmDate = currentTime
            };
            sanadItemsList.Add(takhfifatSanadItem);

            #endregion

            #region MaliatBarArzeshAfzodeh

            SanadItem maliatSanadItem = new SanadItem()
            {
                SandId = addedSanadId,
                PeriodId = fiscalId,
                IssueTrackingNum = null,
                ChequeId = null,
                Description = factorSanadDescription,
                SarfaslId = (short)SarfaslEnums.MaliatBarArzeshAfzodehForosh,
                TafsiliId = null,
                Bedeh = 0,
                Bestan = factor.TotalTax,
                ConfirmDate = currentTime
            };
            sanadItemsList.Add(maliatSanadItem);

            #endregion

            #region Foroosh

            foreach (var item in factor.FactorItems)
            {

                var foroshTafsilyId = _tafsiliRepo.Where(a =>
                    a.SarfaslId == (short)SarfaslEnums.ForosheKala &&
                    a.ProductId == item.ProductId).Select(a => a.Id).FirstOrDefault();

                SanadItem foroshSanadItem = new SanadItem()
                {
                    SandId = addedSanadId,
                    PeriodId = fiscalId,
                    IssueTrackingNum = null,
                    ChequeId = null,
                    Description = factorSanadDescription,
                    SarfaslId = (short)SarfaslEnums.ForosheKala,
                    TafsiliId = foroshTafsilyId,
                    Bedeh = 0,
                    Bestan = item.TotalPrice,
                    ConfirmDate = currentTime
                };

                sanadItemsList.Add(foroshSanadItem);
            }

            #endregion

            var totalBedeh = sanadItemsList.Sum(i => i.Bedeh);
            var totalBestan = sanadItemsList.Sum(i => i.Bestan);

            if (totalBestan != totalBedeh)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره سند حسابداری: سند حسابداری تراز نیست!", type: ToastType.Error, position: ToastPosition.TopCenter);
                return RedirectToAction("AllOrders", "Order");
            }

            #endregion

            Sanad sanad = new Sanad
            {
                Id = addedSanadId,
                PeriodId = fiscalId,
                ConfirmDate = currentTime,
                CreatedDate = currentTime,
                CreatedByUserId = 1, // TODO
                Description = factorSanadDescription,
                IsAutomatic = true,
                IsConfirmed = true,
                TotalAmount = totalBedeh,
                SanadTypeId = (byte)SandType.FactorForosh
            };

            bool sanadIsAdded = _sanadRepo.Add(sanad);
            if (!sanadIsAdded)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره سند حسابداری", type: ToastType.Error, position: ToastPosition.TopCenter);
                return RedirectToAction("AllOrders", "Order");
            }

            bool sanadItemsAreAdded = _sanadItemRepo.AddRange(sanadItemsList);
            if (!sanadItemsAreAdded)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره آیتم های سند حسابداری", type: ToastType.Error, position: ToastPosition.TopCenter);
                return RedirectToAction("AllOrders", "Order");
            }

            var addedFactor = _factorRepo.Find(addedFactorId, fiscalId);
            addedFactor.SanadId = addedSanadId;
            addedFactor.PeriodId = fiscalId;
            bool factorSanadIsUpdated = _factorRepo.Update(addedFactor);
            if (!factorSanadIsUpdated)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره شماره سند حسابداری در فاکتور", type: ToastType.Error, position: ToastPosition.TopCenter);
                return RedirectToAction("AllOrders", "Order");
            }

            order.FactorId = addedFactorId;
            order.PeriodId = fiscalId;
            bool orderIsUpdated = _orderRepo.Update(order);
            if (!orderIsUpdated)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در ذخیره شماره فاکتور در سفارش خرید", type: ToastType.Error, position: ToastPosition.TopCenter);
                return RedirectToAction("AllOrders", "Order");
            }

            TempData["SaveMessage"] = Notification.Show($"فاکتور شماره {addedFactorId} با موفقیت ثبت شد.", type: ToastType.Success, position: ToastPosition.TopCenter);
            return RedirectToAction("AllOrders", "Order");
        }

        private Factor GetFactorByOrder(int orderId, List<FactorItem> factorItems)
        {
            var order = _orderRepo.Find(orderId);

            var factor = new Factor
            {
                CustomerId = order.CustomerId,
                CreatedDate = DateTime.Now,
                MarketerId = _customerRepo.Find(order.CustomerId).MarketerId,
                IsBuy = false,
                IsDeleted = false,
                IsPreFactor = false,
                Description = $"فاکتور فروش خودکار برای سفارش شماره {order.Id}",
                Customer = order.Customer,
                TotalPrice = factorItems.Sum(i => i.TotalPrice),
                TotalDiscount = factorItems.Sum(i => i.Discount),
                TotalTax = factorItems.Sum(i => i.Tax),
                MarketerTotalCommission = factorItems.Sum(i => i.MarketerCommission)
            };
            factor.FinalPrice = factor.TotalPrice - factor.TotalDiscount + factor.TotalTax - factor.DiscountOnFactor +
                                factor.Additions - factor.Deductions;

            return factor;
        }

        private List<FactorItem> GetFactorItemsByOrder(int orderId)
        {
            var orderItems = _orderItemRepo.Where(i => i.OrderId == orderId);

            var factorItems = new List<FactorItem>();
            foreach (var orderItem in orderItems)
            {
                var product = orderItem.Product;
                var factorItem = new FactorItem
                {
                    Product = product,
                    ProductId = orderItem.ProductId,
                    MainUnitCount = orderItem.MainUnitCount,
                    SubUnitCount = orderItem.SubUnitCount,
                    UnitsRate = orderItem.UnitRate,
                    UnitPrice = orderItem.UnitPrice,
                    Discount = orderItem.UnitDiscount,
                    MarketerCommission = product.MarketerCommission ?? 0 * (decimal)orderItem.MainUnitCount,
                    Description = orderItem.Description
                };

                factorItem.TotalPrice = (decimal)(factorItem.MainUnitCount ?? 0) * (factorItem.UnitPrice - factorItem.Discount);
                var activePeriod = _fiscalRepo.Where(i => !i.IsClosed).FirstOrDefault();
                byte vat = 0;
                if (activePeriod != null)
                {
                    vat = activePeriod.Vat;
                }

                factorItem.Tax = factorItem.TotalPrice * vat / 100;
                factorItem.FinalPrice = factorItem.TotalPrice + factorItem.Tax;

                factorItems.Add(factorItem);
            }

            return factorItems;
        }

        [HttpGet]
        [AccessControl(ActionsEnum.AddInvoice)]
        public ActionResult InvoiceModal(int id)
        {
            Factor factor = _factorRepo.Find(id, Fiscal.GetFiscalId());
            if (factor == null)
            {
                return HttpNotFound();
            }
            string residHavale = factor.IsBuy ? "رسید" : "حواله";

            var thisWareHouseId = factor.FactorItems.FirstOrDefault()?.Product.WarehouseId;

            InvoiceForFactorViewModel viewModel = new InvoiceForFactorViewModel();
            viewModel.FactorId = id;
            viewModel.factor = _factorRepo.Where(x => x.Id == id).ToList();
            //viewModel.Invoice = _factorRepo.Where(x => x.Id == id).ToList();

            if (thisWareHouseId != null)
            {
                viewModel.WarehouseId = thisWareHouseId.Value;
                viewModel.WarehouseName = _blWarehouse.Find(thisWareHouseId.Value).Name;

                bool isReceiveInvoice;
                if (factor.FactorTypeId == (byte)FactorType.Kharid ||
                    factor.FactorTypeId == (byte)FactorType.BargashAzForoosh)
                {
                    isReceiveInvoice = true;
                }
                else
                {
                    isReceiveInvoice = false;
                }

                viewModel.Invoice = new Invoice
                {
                    FactorNum = factor.Id.ToString(),
                    IsReceive = isReceiveInvoice,
                    ThisWareHouseId = thisWareHouseId.Value,
                    Description = $"{residHavale} خودکار برای فاکتور شماره {id}"
                };
            }

            viewModel.InvoiceItems = new List<InvoiceItem>();
            var groupedFactorItems = factor.FactorItems.GroupBy(i => i.ProductId).Select(n =>
                new FactorItem
                {
                    ProductId = n.FirstOrDefault()?.ProductId ?? 0,
                    Product = _productRepo.Find(n.FirstOrDefault()?.ProductId ?? 0),
                    MainUnitCount = n.Sum(i => i.MainUnitCount),
                    SubUnitCount = n.Sum(i => i.SubUnitCount),
                    UnitsRate = n.FirstOrDefault()?.UnitsRate
                }).ToList();
            foreach (var factorItem in groupedFactorItems)
            {
                viewModel.InvoiceItems.Add(new InvoiceItem
                {
                    Product = factorItem.Product,
                    ProductId = factorItem.ProductId,
                    MainUnitCount = factorItem.MainUnitCount ?? 0,
                    SubUnitCount = factorItem.SubUnitCount ?? 0,
                    UnitsRate = factorItem.UnitsRate
                });
            }

            return PartialView("_InvoiceForFactor", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessControl(ActionsEnum.AddInvoice)]
        public ActionResult InvoiceModal(int? id, bool isBuy)
        {
            TempData["SaveMessage"] = Notification.Show("خطا در ذخیره اطلاعات", type: ToastType.Error, position: ToastPosition.TopCenter);

            if (!id.HasValue)
            {
                return View("_Error404");
            }

            IQueryable<Factor> buyFactorsWithoutInvoice =
                _factorRepo.Where(i => !i.IsPreFactor && !i.IsDeleted && !i.InvoiceId.HasValue
                                       && (i.FactorTypeId == (byte)FactorType.Kharid || i.FactorTypeId == (byte)FactorType.BargashAzForoosh));

            IQueryable<Factor> sellFactorsWithoutInvoice =
                _factorRepo.Where(i => !i.IsPreFactor && !i.IsDeleted && !i.InvoiceId.HasValue &&
                                       i.IsConfirmed.HasValue && i.IsConfirmed.Value == true
                                       && (i.FactorTypeId == (byte)FactorType.Foroosh || i.FactorTypeId == (byte)FactorType.BargashAzKharid));

            if (!ModelState.IsValid)
            {
                TempData["SaveMessage"] = Notification.Show(ModelState.GetErorr(), type: ToastType.Error, position: ToastPosition.TopCenter);

                if (isBuy)
                {
                    return RedirectToAction("BuyFactorsWithoutInvoice", buyFactorsWithoutInvoice);
                }
                else
                {
                    return RedirectToAction("SellFactorsWithoutInvoice", sellFactorsWithoutInvoice);
                }
            }

            Factor factor = _factorRepo.Find(id.Value, Fiscal.GetFiscalId());
            if (factor == null)
            {
                if (isBuy)
                {
                    return RedirectToAction("BuyFactorsWithoutInvoice", buyFactorsWithoutInvoice);
                }
                else
                {
                    return RedirectToAction("SellFactorsWithoutInvoice", sellFactorsWithoutInvoice);
                }
            }

            string residHavale = factor.IsBuy ? "رسید" : "حواله";

            var thisWareHouseId = factor.FactorItems.FirstOrDefault()?.Product.WarehouseId;
            if (thisWareHouseId == null)
            {
                if (isBuy)
                {
                    return RedirectToAction("BuyFactorsWithoutInvoice", buyFactorsWithoutInvoice);
                }
                else
                {
                    return RedirectToAction("SellFactorsWithoutInvoice", sellFactorsWithoutInvoice);
                }
            }

            bool isReceiveInvoice;
            if (factor.FactorTypeId == (byte)FactorType.Kharid ||
                factor.FactorTypeId == (byte)FactorType.BargashAzForoosh)
            {
                isReceiveInvoice = true;
            }
            else
            {
                isReceiveInvoice = false;
            }

            Invoice invoice = new Invoice
            {
                IsReceive = isReceiveInvoice,
                ThisWareHouseId = thisWareHouseId.Value,
                FactorNum = id.ToString(),
                CreatedDateTime = DateTime.Now,
                CreatorUserId = UserIdentity.Getuserid(),
                IsCompleted = true,
                Description = $"{residHavale} خودکار برای فاکتور شماره {id}"
            };

            int addedInvoiceId = _blInvoice.Add(invoice);

            List<InvoiceItem> invoiceItems = new List<InvoiceItem>();
            var groupedFactorItems = factor.FactorItems.GroupBy(i => i.ProductId).Select(n =>
                new FactorItem
                {
                    ProductId = n.FirstOrDefault()?.ProductId ?? 0,
                    MainUnitCount = n.Sum(i => i.MainUnitCount),
                    SubUnitCount = n.Sum(i => i.SubUnitCount),
                    UnitsRate = n.FirstOrDefault()?.UnitsRate
                }).ToList();

            foreach (var factorItem in groupedFactorItems)
            {
                invoiceItems.Add(new InvoiceItem
                {
                    InvoiceId = addedInvoiceId,
                    ProductId = factorItem.ProductId,
                    MainUnitCount = factorItem.MainUnitCount ?? 0,
                    SubUnitCount = factorItem.SubUnitCount ?? 0,
                    UnitsRate = factorItem.UnitsRate
                });
            }

            bool invoiceItemsAreAdded = _blInvoiceItem.AddRange(invoiceItems);
            if (!invoiceItemsAreAdded)
            {
                Invoice incompleteInvoice = _blInvoice.Find(addedInvoiceId);
                incompleteInvoice.IsCompleted = false;
                _blInvoice.Update(incompleteInvoice);

                TempData["SaveMessage"] = Notification.Show($"خطا در صدور خودکار {residHavale}", type: ToastType.Error, position: ToastPosition.TopCenter);
                if (isBuy)
                {
                    return RedirectToAction("BuyFactorsWithoutInvoice", buyFactorsWithoutInvoice);
                }
                else
                {
                    return RedirectToAction("SellFactorsWithoutInvoice", sellFactorsWithoutInvoice);
                }
            }

            List<Product> productsToUpdate = new List<Product>();
            foreach (var invoiceItem in invoiceItems)
            {
                var product = _productRepo.Find(invoiceItem.ProductId);
                if (factor.FactorTypeId == (byte)FactorType.Foroosh)
                {
                    product.PendingsCount -= invoiceItem.MainUnitCount;
                }
                var factorItem = _blFactorItem.Where(x => x.FactorId == id && x.ProductId == invoiceItem.ProductId).FirstOrDefault();
                int unitId = (int)factorItem.UnitId;
                bool isMain = _blUnit.Where(x => x.Id == unitId).FirstOrDefault().IsMain;
                if (!isMain)
                {
                    if (invoice.IsReceive)
                    {
                        double inventoruChange = (double)factorItem.UnitsRate * (double)factorItem.MainUnitCount;
                        product.Inventory += inventoruChange;
                    }
                    else
                    {
                        double inventoruChange = (double)factorItem.UnitsRate * (double)factorItem.MainUnitCount;
                        product.Inventory -= inventoruChange;
                    }
                }
                else if (isMain)
                {
                    if (invoice.IsReceive)
                    {
                        product.Inventory += (int)invoiceItem.MainUnitCount;
                    }
                    else
                    {
                        product.Inventory -= (int)invoiceItem.MainUnitCount;
                    }
                }
                //if (invoice.IsReceive)
                //{
                //    product.Inventory += invoiceItem.MainUnitCount;
                //}
                //else
                //{
                //    product.Inventory -= invoiceItem.MainUnitCount;
                //}

                productsToUpdate.Add(product);
            }

            bool productsAreUpdated = _productRepo.UpdateRange(productsToUpdate);

            if (!productsAreUpdated)
            {
                TempData["SaveMessage"] = Notification.Show("خطا در به روزرسانی اطلاعات کالاها", type: ToastType.Error, position: ToastPosition.TopCenter);
                if (isBuy)
                {
                    return RedirectToAction("BuyFactorsWithoutInvoice", buyFactorsWithoutInvoice);
                }
                else
                {
                    return RedirectToAction("SellFactorsWithoutInvoice", sellFactorsWithoutInvoice);
                }
            }

            factor.InvoiceId = addedInvoiceId;
            _factorRepo.Update(factor);

            TempData["SaveMessage"] = Notification.Show($"{residHavale} جدید با شناسه {addedInvoiceId} با موفقیت صادر شد.", type: ToastType.Success, position: ToastPosition.TopCenter);


            if (isBuy)
            {
                return RedirectToAction("BuyFactorsWithoutInvoice", buyFactorsWithoutInvoice);
            }
            else
            {
                return RedirectToAction("SellFactorsWithoutInvoice", sellFactorsWithoutInvoice);
            }
        }

        [HttpGet]
        [AjaxOnly]
        [CoustomAuthrize]
        public JsonResult GetProductDetailsForBuyFactor(short productId)
        {
            var product = _productRepo.Where(a => a.Id == productId).FirstOrDefault();

            var subUnit = _blUnit.Where(a => a.Id == product.SubUnitId).Select(a => a.Name).FirstOrDefault();
            if (product?.UnitRate == null)
            {
                product.UnitRate = 0;
            }
            var pro = new
            {
                inventoy = product.Inventory,
                unitrate = product.UnitRate,
                subUnit,
            };


            return Json(new { pro }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxOnly]
        [CoustomAuthrize]
        public JsonResult GetMarketerName(int customerId)
        {
            var customer = _customerRepo.Find(customerId);

            object result;
            if (customer?.MarketerId != null)
            {
                result = new { id = customer.MarketerId, name = customer.Personnel.FullName };
            }
            else
            {
                result = new { name = "-" };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxOnly]
        [CoustomAuthrize]
        public JsonResult GetSellProductInfo(short productId, int customerId, short? priceTypeId = null)
        {
            var product = _productRepo.Find(productId);
            decimal? unitPrice = null;
            if (priceTypeId.HasValue)
            {
                unitPrice = _blProductPriceList.Where(
                    i => i.PriceTypeId == priceTypeId && i.ProductId == productId)?.FirstOrDefault()?.Price;
            }

            ProductInfoAjax productInfo = new ProductInfoAjax();

            if (product == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            if (product.Unit != null)
            {
                productInfo.MainUnitId = product.Unit.Id;
                productInfo.MainUnitName = product.Unit.Name;
            }

            if (product.Unit1 != null)
            {
                productInfo.SubUnitId = product.Unit1.Id;
                productInfo.SubUnitName = product.Unit1.Name;
            }

            if (product.UnitRate.HasValue)
            {
                productInfo.UnitRate = product.UnitRate.Value;
            }

            productInfo.UnitPrice = (long)(unitPrice ?? 0);
            productInfo.Commission = (long)(product.MarketerCommission ?? 0);

            var pendingCount = _productRepo.GetLivePendingCount(productId, true);
            productInfo.AvailableCount = product.Inventory - pendingCount;

            productInfo.Discount = GetDiscount(productId, customerId);

            return Json(productInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxOnly]
        [CoustomAuthrize]
        public JsonResult GetBuyProductInfo(short productId, short? priceTypeId = null)
        {
            var product = _productRepo.Find(productId);
            decimal? unitPrice = null;
            if (priceTypeId.HasValue)
            {
                unitPrice = _blProductPriceList.Where(
                    i => i.PriceTypeId == priceTypeId && i.ProductId == productId)?.FirstOrDefault()?.Price;
            }

            ProductInfoAjax productInfo = new ProductInfoAjax();

            if (product == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            if (product.Unit != null)
            {
                productInfo.MainUnitId = product.Unit.Id;
                productInfo.MainUnitName = product.Unit.Name;
            }

            if (product.Unit1 != null)
            {
                productInfo.SubUnitId = product.Unit1.Id;
                productInfo.SubUnitName = product.Unit1.Name;
            }

            if (product.UnitRate.HasValue)
            {
                productInfo.UnitRate = product.UnitRate.Value;
            }

            productInfo.UnitPrice = (long)(unitPrice ?? 0);

            return Json(productInfo, JsonRequestBehavior.AllowGet);
        }

        private DiscountAjax GetDiscount(short productId, int customerId)
        {
            try
            {
                var offerItem = _offerItemRepo.Where(i =>
                    i.ProductId == productId &&
                    i.Offer.IsActive &&
                    i.Offer.StartDate <= DateTime.Now &&
                    i.Offer.ExpDate >= DateTime.Now).FirstOrDefault();

                if (offerItem == null)
                {
                    return null;
                }

                var customerCategoryId = _customerRepo.Find(customerId)?.CustomerCategoryId;
                if (!string.IsNullOrWhiteSpace(offerItem.Offer.ForCustomerCategories))
                {
                    var offerForCustomerCats = offerItem.Offer.ForCustomerCategories.Split(',');
                    if (offerForCustomerCats.Any())
                    {
                        if (!customerCategoryId.HasValue || !offerForCustomerCats.Contains(customerCategoryId.Value.ToString()))
                        {
                            return null;
                        }
                    }
                }


                DiscountAjax discount = new DiscountAjax
                {
                    OfferId = offerItem.OfferId,
                    OfferTypeId = offerItem.OfferTypeId,
                    OfferName = offerItem.Offer.Name
                };
                switch (offerItem.OfferTypeId)
                {
                    case 1: // ریالی
                        discount.DiscountAmount = (long)(offerItem.DiscountAmount ?? 0);
                        break;

                    case 2: // درصدی
                        discount.DiscountPercent = offerItem.DiscountPercent ?? 0;
                        break;

                    case 3: // کالایی
                        if (offerItem.Product != null)
                        {
                            discount.GiftProductId = offerItem.GiftProductId ?? 0;
                            discount.GiftProductName = offerItem.Product.Name;
                            discount.MinProductCount = offerItem.MinProductCount;
                            discount.GiftProductCount = offerItem.GiftProductCount;
                            discount.GiftMainUnitId = offerItem.Product.Unit.Id;
                            discount.GiftMainUnitName = offerItem.Product.Unit.Name;

                            var pendingCount = _productRepo.GetLivePendingCount(productId, true);
                            discount.GiftAvailableCount = offerItem.Product.Inventory - pendingCount;
                        }
                        break;

                    default:
                        return null;
                }

                return discount;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}