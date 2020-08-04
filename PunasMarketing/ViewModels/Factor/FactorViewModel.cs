using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.LocalModel;

namespace PunasMarketing.ViewModels.Factor
{
    public class FactorViewModel
    {
        public IEnumerable<Models.DomainModel.Supplier> Suppliers { get; set; }
        public IEnumerable<SelectListItem> SelectCustomers { get; set; }
        public IEnumerable<SelectListItem> SelectSuppliers { get; set; }
        public IEnumerable<Models.DomainModel.Product> Products { get; set; }
        public FactorItem FactorItem { get; set; }
        public Models.DomainModel.Factor Factor { get; set; }
        public IEnumerable<Unit> Units { get; set; }
        public IEnumerable<SelectListItem> PriceTypes { get; set; }
        public PriceType PriceType { get; set; }
        public byte TaxPercent { get; set; }
        public bool AssignCommission { get; set; }
        public bool AssignOffer { get; set; }

        [Display(Name = "تاریخ فاکتور")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string StrCreatedDate { get; set; }
    }

    public class AddFactorViewModel
    {
        public Models.DomainModel.Factor Factor { get; set; }
        public List<AddFactorItemAjax> FactorItems { get; set; }
        public bool AssignCommission { get; set; }
        public bool AssignOffer { get; set; }
        public string StrCreateDate { get; set; }
    }

    public class ShowFactorViewModel
    {
        public int OrderId { get; set; }
        public Models.DomainModel.Factor Factor { get; set; }
        public List<FactorItem> FactorItems { get; set; }
    }

    public class BackFactorViewModel
    {
        public Models.DomainModel.Factor Factor { get; set; }
        public List<BackFactorItems> FactorItems { get; set; }
        public string StrCreateDate { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        public byte TaxPercent { get; set; }
    }


    public class AddFactorItemAjax
    {
        [Display(Name = "کالا")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public short ProductId { get; set; }

        [Display(Name = "تعداد واحد اصلی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public double Count { get; set; }

        [Display(Name = "قیمت واحد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "کمیسیون بازاریاب")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal Commission { get; set; }

        [Display(Name = "واحد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public int UnitId { get; set; }
        [Display(Name = "قیمت کل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "تخفیف واحد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal Discount { get; set; }

        [Display(Name = "مالیات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal Tax { get; set; }

        [Display(Name = "قیمت نهایی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public decimal Finalprice { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }
    }

    public class ProductInfoAjax
    {
        public short MainUnitId { get; set; }
        public short SubUnitId { get; set; }
        public int UnitRate { get; set; }
        public string MainUnitName { get; set; }
        public string SubUnitName { get; set; }
        public long UnitPrice { get; set; }
        public long Commission { get; set; }
        public double AvailableCount { get; set; }
        public DiscountAjax Discount { get; set; }
    }

    public class DiscountAjax
    {
        public int OfferId { get; set; }
        public string OfferName { get; set; }
        public byte OfferTypeId { get; set; }
        public long? DiscountAmount { get; set; }
        public byte? DiscountPercent { get; set; }

        #region Gift

        public short? GiftProductId { get; set; }
        public string GiftProductName { get; set; }
        public double? MinProductCount { get; set; }
        public double? GiftProductCount { get; set; }
        public short? GiftMainUnitId { get; set; }
        public string GiftMainUnitName { get; set; }
        public double? GiftAvailableCount { get; set; }

        #endregion

    }

    public class buyFactorViewModel
    {
        public int Id { get; set; }
        public short PeriodId { get; set; }
        public Nullable<int> SanadId { get; set; }
        public Nullable<short> SanadPeriodId { get; set; }
        public Nullable<int> ReturnForFactorId { get; set; }
        public Nullable<short> ReturnedPeriodId { get; set; }
        public string PaperFactorCode { get; set; }
        public Nullable<short> SupplierId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountOnFactor { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal Additions { get; set; }
        public decimal Deductions { get; set; }
        public decimal TotalTax { get; set; }
        public decimal FinalPrice { get; set; }
        public int CreatorUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> MarketerId { get; set; }
        public decimal MarketerTotalCommission { get; set; }
        public Nullable<int> InvoiceId { get; set; }
        public bool IsBuy { get; set; }
        public byte FactorTypeId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPreFactor { get; set; }
        public Nullable<bool> IsConfirmed { get; set; }
        public string Description { get; set; }

        public virtual PunasMarketing.Models.DomainModel.Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactorItem> FactorItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PunasMarketing.Models.DomainModel.Factor> Factors1 { get; set; }
        public virtual PunasMarketing.Models.DomainModel.Factor Factor1 { get; set; }
        public virtual FiscalPeriod FiscalPeriod { get; set; }
        public virtual PunasMarketing.Models.DomainModel.Invoice Invoice { get; set; }
        public virtual PunasMarketing.Models.DomainModel.Personnel Personnel { get; set; }
        public virtual PunasMarketing.Models.DomainModel.Sanad Sanad { get; set; }
        public virtual PunasMarketing.Models.DomainModel.Supplier Supplier { get; set; }
        public virtual PunasMarketing.Models.DomainModel.User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PunasMarketing.Models.DomainModel.Order> Orders { get; set; }
        public string lsWareHouse { get; set; }
    }
}