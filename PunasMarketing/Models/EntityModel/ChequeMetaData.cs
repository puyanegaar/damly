using System;
using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel.Cheque;

namespace PunasMarketing.Models.EntityModel.Cheque
{
    public class ChequeMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "شماره چک")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(20, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string ChequeNum { get; set; }


        [Display(Name = "تاریخ سررسید")]
        public DateTime DueDate { get; set; }

       
        public decimal Amount { get; set; }

        [Display(Name = "بانک صادرکننده چک")]
        public short? BankId { get; set; }

      

        [Display(Name = "حساب چک")]// اگر چک خودمان بود
        //[Required(ErrorMessage = "{0} را وارد نمایید")]
        public short? BankAccountId { get; set; }
      

        [Display(Name = "نام صاحب چک")] // در وجه چه کسی است
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string OwnerName { get; set; }

        [Display(Name = "وضعیت چک")]
        public byte StatusId { get; set; }


        [Display(Name = "توضیحات")]
        [StringLength(100, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Description { get; set; }

        public string BranchName { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(ChequeMetaData))]
    public partial class Cheque
    {
        [Display(Name = "نام طرف حساب")]
        public string CounterPartyName { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusName { get; set; }

        [Display(Name = "مبلغ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string Price { get; set; }

        [Display(Name = "به نام")]
        public string ChequeAccountNumber { get; set; }

        [Display(Name = "شماره فیش دریافت/پرداخت")]
        public int ReceiptId { get; set; }

        public int ChequeSanadId {get; set;}

        public int ChequeSanadSecondId { get; set; }

        [Display(Name = "تاریخ سررسید")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string DudateText { get; set; }
    }
}