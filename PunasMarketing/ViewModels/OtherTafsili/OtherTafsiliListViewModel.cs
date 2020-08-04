using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.OtherTafsili
{
    public class OtherTafsiliListViewModel
    {
        public IEnumerable<Models.DomainModel.OtherTafsili> OtherTafsilis { get; set; }
        public IEnumerable<Models.DomainModel.Sarfasl> MoeenSarfasls { get; set; }
    }
}