using PunasMarketing.Models.DomainModel;
using System.Collections.Generic;

namespace PunasMarketing.ViewModels.Region
{
    public class RegionViewModel
    {
        public IEnumerable<City> Cities { get; set; }
        public IEnumerable<Province> Provinces { get; set; }
        public Models.DomainModel.Region RegionModel { get; set; }
    }
}