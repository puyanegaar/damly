using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class SectionMetaData
    {
        [Display(Name = "شناسه سند")]
        public int Id { get; set; }

        [Display(Name = "نام بخش")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }
    }
}



namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(SectionMetaData))]
    public partial class Section
    {

    }
}