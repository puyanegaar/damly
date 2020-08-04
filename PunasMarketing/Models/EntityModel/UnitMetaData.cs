using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    internal class UnitMetaData
    {
        [Display(Name = "شناسه سند")]
        public short Id { get; set; }

        [Display(Name = "نام واحد اندازه گیری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }

        [Display(Name = "واحد اصلی")]
        public bool IsMain { get; set; }
    }

}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(UnitMetaData))]
    public partial class Unit
    {

    }
}