using PunasMarketing.Models.EntityModel;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Models.EntityModel
{
    internal class ProvinceMetaData
    {
        [Display(Name = "شناسه")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "استان مورد نظر را انتخاب کنید")]
        public int Id { get; set; }

        [Display(Name = "استان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "استان مورد نظر را انتخاب کنید")]
        public string Name { get; set; }
    }


}

namespace PunasMarketing.Models.DomainModel
{

    [MetadataType(typeof(ProvinceMetaData))]
    public partial class Province
    {

    }
}