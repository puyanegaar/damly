using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Personnel
{
    public class PersonnelListViewModel
    {
        public IEnumerable<Models.DomainModel.Personnel> Personnels { get; set; }
        public IEnumerable<Models.DomainModel.JobTitle> JobTitles { get; set; }

    }
}