using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class CashDeskMetaData
    {
        [Display(Name = "شناسه")]
        public short Id { get; set; }

        [Display(Name = "نام صندوق")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }
    }
}


namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(CashDeskMetaData))]
    public partial class CashDesk
    {

    }
}