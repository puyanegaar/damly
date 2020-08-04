using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class WarehouseMetaData
    {
        [Display(Name = "شناسه سند")]
        public short Id { get; set; }

        [Display(Name = "نام انبار")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }

        [Display(Name = "آیدی انباردار")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
        public int KeeperId { get; set; }

        [Display(Name = "شماره تلفن")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(15, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Tell { get; set; }

        [Display(Name = "نشانی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(100, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Address { get; set; }

        [Display(Name = "توضیحات")]
        [StringLength(150, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(WarehouseMetaData))]
    public partial class Warehouse
    {

    }
}