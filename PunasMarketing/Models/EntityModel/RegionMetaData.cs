using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class RegionMetaData
    {
        [Display(Name = "شناسه")]
        public short Id { get; set; }

        [Display(Name = "نام منطقه شهری")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }

        [Display(Name = "نام شهر")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public short CityId { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(RegionMetaData))]
    public partial class Region
    {
        [Display(Name = "نام استان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public short StateId { get; set; }
    }
}