using PunasMarketing.Models.EntityModel;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Models.EntityModel
{
    public class SarfaslMetaData
    {
        [Display(Name = "شناسه سند")]
        public short Id { get; set; }

        [Display(Name = "نام حساب")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        [StringLength(200, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }

        [Display(Name = "کد حساب")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد کنید")]
        public byte Code { get; set; }

        [Display(Name = "کد کامل حساب (کدینگ)")]
        public string Coding { get; set; }

        public bool IsConstant { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(SarfaslMetaData))]
    public partial class Sarfasl
    {
        public string CodingAndName => $"{Coding} - {Name}";
        public string StrCode { get; set; }

    }
}