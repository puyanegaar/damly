using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.ViewModels.Warehouse
{
    public class WarehouseViewModel
    {
        public Models.DomainModel.Warehouse Warehouse { get; set; }
        public IEnumerable<Models.DomainModel.Personnel> Personnels { get; set; }
        public IEnumerable<Models.DomainModel.JobTitle> JobTitles { get; set; }

        [Display(Name = "عنوان شغلی")]
        [Required(ErrorMessage = "{0} را انتخاب نمایید")]
        public int? JobTitleId { get; set; }
    }
}