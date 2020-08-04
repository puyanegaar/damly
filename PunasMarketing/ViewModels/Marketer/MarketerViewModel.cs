using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.ViewModels.Marketer
{
    public class MarketerViewModel
    {
        public Models.DomainModel.Personnel Marketer { get; set; }
        public IEnumerable<Models.DomainModel.City> Cities { get; set; }
        public IEnumerable<Models.DomainModel.Province> Provinces { get; set; }

        public IEnumerable<Models.EntityModel.AcademicDegreeMetaData> Academics { get; set; }

        [Display(Name = "تاریخ تولد")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string StrBirthDate { get; set; }

        [Display(Name = "تاریخ استخدام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} را وارد نمایید")]
        public string StrHireDate { get; set; }

        [Display(Name = "تاریخ قطع همکاری")]
        public string StrFireDate { get; set; }
    }
}