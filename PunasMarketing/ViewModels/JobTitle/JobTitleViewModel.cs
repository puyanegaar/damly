using System.Collections.Generic;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.ViewModels.JobTitle
{
    public class CreateJobTitleViewModel
    {
        
        public IEnumerable<Section> Sections { get; set; }
        public Models.DomainModel.JobTitle JobTitleModel { get; set; }
    }
}