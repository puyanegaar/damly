using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.ViewModels.Personnel
{
    public class PersonnelViewModel
    {
        public Models.DomainModel.Personnel personnel { get; set; }
        public IEnumerable<Models.DomainModel.City> Cities { get; set; }
        public IEnumerable<Models.DomainModel.Province> Provinces { get; set; }

        public IEnumerable<Models.EntityModel.AcademicDegreeMetaData> Academics { get; set; }
        public IEnumerable<Models.DomainModel.Section> Sections { get; set; }
        public IEnumerable<Models.DomainModel.JobTitle> JobTitles { get; set; }

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