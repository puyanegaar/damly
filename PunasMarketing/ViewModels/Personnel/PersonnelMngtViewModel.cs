using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.ViewModels.Personnel
{
    public class PersonnelMngtViewModel
    {
        public PunasMarketing.Models.DomainModel.Personnel personnel { get; set; }
        public Models.DomainModel.User User { get; set; }
        public IEnumerable<PunasMarketing.Models.DomainModel.Action> Actions { get; set; }
        public IEnumerable<PunasMarketing.Models.DomainModel.Section> sections { get; set; }
        public IEnumerable<AcademicDegreeMetaData> AcademicDegrees { get; set; }
        public List<PersonnelReport_Result> FinancialReport { get; set; }
        public List<Province> Provinces { get; set; }
        public List<City> Cities { get; set; }
        public List<MarketerRegion> MarketerRegions { get; set; }

        [Display(Name = "مناطق شهری")]
        public List<Models.DomainModel.Region> Regions { get; set; }

        public short[] ActionsId { get; set; }
        public short SectionId { get; set; }

        [Display(Name = "استان")]
        public short ProvinceId { get; set; }

        [Display(Name = "شهر")]
        public short CityId { get; set; }
    }
}