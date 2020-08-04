using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class TafsiliMetaData
    {
        //[Display(Name = "شناسه سند")]
        //public int Id { get; set; }

        //[Display(Name = "کد پرسنل")]
        //public int? PersonnelId { get; set; }

        //[Display(Name = "کد مشتری")]
        //public int? CustomerId { get; set; }

        //[Display(Name = "کد تأمین کننده")]
        //public short? SupplierId { get; set; }

        //[Display(Name = "کد کالا")]
        //public short? ProductId { get; set; }

        //[Display(Name = "کد حساب بانکی")]
        //public short? BankAccId { get; set; }

        //[Display(Name = "کد صندوق")]
        //public short? CashDeskId { get; set; }

        //[Display(Name = "کد حساب تفصیلی")]
        //public short? OtherTafsiliId { get; set; }

        //public short? SarfaslId { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(TafsiliMetaData))]
    public partial class Tafsili
    {
        public int TafisiliCode
        {
            get
            {
                int s = (CustomerId ?? 0) + (PersonnelId ?? 0) + (SupplierId ?? 0) + (ProductId ?? 0) + (CashDeskId ?? 0) + (BankAccId ?? 0) + (OtherTafsiliId ?? 0);
                return s;
            }
        }
    }
}