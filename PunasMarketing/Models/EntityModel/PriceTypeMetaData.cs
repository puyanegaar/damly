using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class PriceTypeMetaData
    {
        [Display(Name = "نوع قیمت")]
        public short Id { get; set; }

        [Display(Name = "نوع قیمت")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(100, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }
    }

}

namespace PunasMarketing.Models.DomainModel
{

    [MetadataType(typeof(PriceTypeMetaData))]
    public partial class PriceType
    {

    }
}