using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class ActionMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "بخش")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را انتخاب کنید")]
        public int SectionId { get; set; }

        [Display(Name = "نام عملیات")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(ActionMetaData))]
    public partial class Action
    {

    }
}