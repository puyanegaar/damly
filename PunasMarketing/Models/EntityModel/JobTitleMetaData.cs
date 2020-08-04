using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    internal class JobTitleMetaData
    {
        [Display(Name = "شناسه")]
        public int Id { get; set; }

        [Display(Name = "عنوان شغلی")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکتر نمی تواند بیش از {1} باشد")]
        public string Name { get; set; }

        [Display(Name = "بخش مربوطه")]
        [Required(ErrorMessage = "{0} را انتخاب نمایید")]
        public int SectionId { get; set; }
    }

  }

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(JobTitleMetaData))]
    public partial class JobTitle
    {
    }
}