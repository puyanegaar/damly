using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Sarfasl
{
    public class SarfaslListViewModel
    {
        public List<Models.DomainModel.Sarfasl> Sarfasls { get; set; }
        public List<SarfaslHierarchy> SarfaslHierarchies { get; set; }
    }
}