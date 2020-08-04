using PunasMarketing.Models.EntityModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Models.EntityModel
{
    public class InvoiceItemMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "شماره حواله / رسید")]
        public int InvoiceId { get; set; }

        [Display(Name = "محصول")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public short ProductId { get; set; }

        [Display(Name = "تعداد واحد اصلی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public double MainUnitCount { get; set; }

        [Display(Name = "تعداد واحد فرعی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public double SubUnitCount { get; set; }

        [Display(Name = "نسبت واحد اصلی به فرعی")]
        public double? UnitsRate { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(InvoiceItemMetaData))]
    public partial class InvoiceItem
    {

    }
}