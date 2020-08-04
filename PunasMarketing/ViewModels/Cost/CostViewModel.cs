using System.Collections.Generic;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.Cost
{
    public class CreateCostViewModel
    {
        public IEnumerable<CostCategory> CostCategories { get; set; }
        public Models.DomainModel.Cost CostModel { get; set; }
    }
}