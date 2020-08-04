using PunasMarketing.Models.PrintModel;
using PunasMarketing.Models.Repositories;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Web.Mvc;
using PunasMarketing.Models.Enums;
using System;
using System.Linq;

namespace PunasMarketing.Controllers
{
    public enum PrintType
    {
        [Description("Factors")]
        Factors,
        [Description("Invoices")]
        Invoices,
        [Description("Warehouses")]
        Warehouses,
        [Description("CustmorFinancial")]
        CustmorFinancial
    }

    public class PrintController : Controller
    {
        private readonly FactorRepository _factorRepo;
        private readonly FactorItemRepository _factorItemRepo;
        private readonly InvoiceRepository _invoiceRepo;
        private readonly InvoiceItemRepository _invoiceItemRepo;
        private readonly WarehouseRepository _warehouseRepository;
        private readonly CustomerRepository _customerRepo;

        public PrintController(
            FactorRepository factorRepo,
            FactorItemRepository factorItemRepo,
            InvoiceRepository invoiceRepo,
            InvoiceItemRepository invoiceItemRepo,
            WarehouseRepository warehouseRepository,
            CustomerRepository customerRepo,
            ProductRepository productRepository)
        {
            _customerRepo = customerRepo;
            _factorRepo = factorRepo;
            _factorItemRepo = factorItemRepo;
            _invoiceRepo = invoiceRepo;
            _invoiceItemRepo = invoiceItemRepo;
            _warehouseRepository = warehouseRepository;
        }

        [HttpGet]
        public ActionResult Factor(int id, short fiscalId)
        {
            var dbFactor = _factorRepo.Find(id, fiscalId);
            var dbFactorItems = _factorItemRepo.Where(i => i.FactorId == id && i.PeriodId == fiscalId);

            Factor printFactor = new Factor();
            if (dbFactor != null)
            {
                string name = string.Empty;
                string mobile = string.Empty;
                if (dbFactor.FactorTypeId == (int)FactorType.Kharid || dbFactor.FactorTypeId == (int)FactorType.BargashAzKharid)
                {
                    name = dbFactor.Supplier?.Name;
                    mobile = dbFactor.Supplier?.Mobile;
                }
                else if (dbFactor.FactorTypeId == (int)FactorType.Foroosh || dbFactor.FactorTypeId == (int)FactorType.BargashAzForoosh)
                {
                    name = $"{dbFactor.Customer?.BusinessName}-{dbFactor.Customer?.OwnerName}";
                    mobile = dbFactor.Customer?.Mobile1;
                    if (string.IsNullOrWhiteSpace(mobile))
                    {
                        mobile = dbFactor.Customer?.Mobile2;
                    }
                }

                printFactor = new Factor
                {
                    Id = dbFactor.Id,
                    CustomerName = name,
                    CustomerMobile = mobile,
                    FactorType = ((FactorType)dbFactor.FactorTypeId).GetDescription(),
                    TotalPrice = dbFactor.TotalPrice.ToPrice(),
                    DiscountOnFactor = dbFactor.DiscountOnFactor.ToPrice(),
                    TotalDiscount = dbFactor.TotalDiscount.ToPrice(),
                    Additions = dbFactor.Additions.ToPrice(),
                    Deductions = dbFactor.Deductions.ToPrice(),
                    TotalTax = dbFactor.TotalTax.ToPrice(),
                    FinalPrice = dbFactor.FinalPrice.ToPrice(),
                    Date = dbFactor.CreatedDate.ToPersianDateTime().ToStringDate(),
                    Description = dbFactor.Description
                };
            }

            List<FactorItem> printFactorItems = new List<FactorItem>();
            if (dbFactorItems != null)
            {
                int row = 0;
                foreach (var factorItem in dbFactorItems)
                {
                    FactorItem printFactorItem = new FactorItem
                    {
                        Row = ++row,
                        ProductName = factorItem.Product.Name,
                        Count = factorItem.MainUnitCount ?? 0,
                        UnitName = factorItem.Product.Unit.Name,
                        UnitPrice = factorItem.UnitPrice.ToPrice(),
                        Discount = factorItem.Discount.ToPrice(),
                        Tax = factorItem.Tax.ToPrice(),
                        TotalPrice = factorItem.TotalPrice.ToPrice(),
                        FinalPrice = factorItem.FinalPrice.ToPrice(),
                        Description = factorItem.Description
                    };

                    printFactorItems.Add(printFactorItem);
                }
            }

            return PrintByType(PrintType.Factors,
                "Factor", printFactor, "FactorItem", printFactorItems);
        }

        [HttpGet]
        public ActionResult Invoice(int id)
        {
            var invoice = _invoiceRepo.Find(id);
            var invoiceItems = _invoiceItemRepo.Where(i => i.InvoiceId == id);

            var printInvoice = new Invoice();
            if (invoice != null)
            {
                var invoiceType = "";

                if (invoice.OtherWareHouseId.HasValue)
                {
                    invoiceType = "انبار به انبار";
                }
                else if (invoice.PersonnelId.HasValue)
                {
                    invoiceType = invoice.IsReceive ? "تولید به انبار" : "انبار به تولید";
                }
                else if (!string.IsNullOrWhiteSpace(invoice.FactorNum))
                {
                    invoiceType = invoice.IsReceive ? "خارج به انبار" : "انبار به خارج";
                }

                printInvoice = new Invoice
                {
                    Id = invoice.Id,
                    ResidOrHavaleh = invoice.IsReceive ? "رسید" : "حواله",
                    InvocieType = invoiceType,
                    ThisWarehouseName = invoice.Warehouse1.Name,
                    CreatorUserFullName = invoice.User.Personnel.FullName,
                    Date = invoice.CreatedDateTime.ToPersianDateTime().ToStringDate(),
                    Description = invoice.Description
                };
            }


            List<InvoiceItem> printInvoiceItems = new List<InvoiceItem>();
            int row = 0;
            foreach (var invoiceItem in invoiceItems)
            {
                InvoiceItem printFactorItem = new InvoiceItem
                {
                    Row = ++row,
                    ProductName = invoiceItem.Product.Name,
                    Count = invoiceItem.MainUnitCount,
                    UnitName = invoiceItem.Product.Unit.Name
                };

                printInvoiceItems.Add(printFactorItem);
            }

            return PrintByType(PrintType.Invoices, "Invoice", printInvoice, "InvoiceItem", printInvoiceItems);
        }

        [HttpGet]
        public ActionResult Warehouse(int id)
        {
            var warehouse = _warehouseRepository.Find(id);

            var printWarehouse = new Warehouse();
            if (printWarehouse != null)
            {
                printWarehouse = new Warehouse
                {
                    Id = warehouse.Id,
                    Name = warehouse.Name,
                    Responsible = warehouse.User.Personnel.FullName,
                    Date = DateTime.Now.ToPersianDateTime().ToStringDate()
                };
            }

            List<WarehouseItem> printWarehouseItems = new List<WarehouseItem>();
            foreach (var product in warehouse.Products)
            {
                WarehouseItem warehouseItem = new WarehouseItem
                {
                    Name = product.Name,
                    UnitName = product.Unit.Name,
                    Code = product.ProductCode,
                    Stock = product.Inventory.ToString().ToPrice()
                };

                printWarehouseItems.Add(warehouseItem);
            }

            return PrintByType(PrintType.Warehouses, "WareHouse", printWarehouse, "Items", printWarehouseItems);
        }

        [HttpGet]
        public ActionResult CustmorFinancial(int id)
        {
            //var warehouse = _warehouseRepository.Find(id);
            var select = _customerRepo.Find(id);
            List<Models.DomainModel.CustomerlReport_Result> FinancialReport = new List<Models.DomainModel.CustomerlReport_Result>();
            FinancialReport = _customerRepo.GetFinancialReport(Fiscal.GetFiscalId(), id);

            decimal totalBedeh = FinancialReport.Sum(i => i.Bedeh);
            decimal totalBestan = FinancialReport.Sum(i => i.Bestan);
            decimal mandeh = totalBestan - totalBedeh;
            string mandehDescription = string.Empty;

            if (mandeh > 0)
            {
                mandehDescription = "بستانکار";
            }
            else if (mandeh < 0)
            {
                mandehDescription = "بدهکار";
            }
            else
            {
                mandehDescription = "-";
            }
            Customer printCustomer = new Customer
            {
                Id=select.Id,
                Date = DateTime.Now.ToPersianDateTime().ToStringDate(),
                mandehDes = mandehDescription,
                mandePrice = mandeh.ToPrice(),
                Mobile = select.Mobile1 + " " + select.Mobile2,
                Name = select.OwnerName +" "+ select.BusinessName,
                totalBedeh = totalBedeh.ToPrice(),
                totalBestan = totalBestan.ToPrice()

            };
            List<CustomerItems> FinancialItems = new List<CustomerItems>();
            foreach (var item in FinancialReport)
            {
                CustomerItems Items = new CustomerItems
                {
                    Coding = item.Coding,
                    Name = item.Name + "/" + item.TafsiliName,
                    Description = item.Description,
                    Bedeh = item.Bedeh.ToPrice(),
                    Bestan = item.Bestan.ToPrice(),
                };

                FinancialItems.Add(Items);
            }

            return PrintByType(PrintType.CustmorFinancial, "printCustomer", printCustomer, "Items", FinancialItems);
        }

        private ActionResult PrintByType(PrintType printType, string headerName, object header, string itemsName, object items)
        {
            try
            {
                string directoryPhysicalPath = System.Web.Hosting.HostingEnvironment.MapPath(
                    $"~/Content/Reports/{printType.GetDescription()}/");
                if (string.IsNullOrWhiteSpace(directoryPhysicalPath))
                {
                    return View("../Account/PrintError");
                }
                Directory.CreateDirectory(directoryPhysicalPath);

                string dateWithDash = (((dynamic)header).Date as string)?.Replace('/', '-');
                string pdfFileName = $"{printType.GetDescription()}-{dateWithDash ?? ""}-{((dynamic)header).Id}";

                string stimulFilePath = $"{directoryPhysicalPath}{printType.GetDescription()}.mrt";
                string pdfFilePath = $"{directoryPhysicalPath}{pdfFileName}.pdf";

                SavePdf(stimulFilePath, pdfFilePath, headerName, header, itemsName, items);

                #region Show Pdf file

                var fileStream = new FileStream(pdfFilePath, FileMode.Open, FileAccess.Read);
                //var fsResult = new FileStreamResult(fileStream, "application/pdf");
                //return fsResult;

                return File(fileStream, "application/pdf");

                #endregion
            }
            catch (Exception e)
            {
                return View("../Account/PrintError");
            }
        }

        private void SavePdf(string stimulFilePath, string pdfFilePath,
            string headerName, object header, string itemsName, object items)
        {
            StiReport report = new StiReport();

            report.Load(stimulFilePath);
            report.RegData(headerName, header);
            report.RegData(itemsName, items);

            StiOptions.Export.Pdf.ReduceFontFileSize = false;
            report.Render(true);
            StiPdfExportService pdfExport = new StiPdfExportService();
            pdfExport.ExportPdf(report, pdfFilePath);
        }
    }
}
