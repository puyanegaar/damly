using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Sarfasl
{
    public class SarfaslHierarchy
    {
        public int SarfaslId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Coding { get; set; }
        public bool IsConstant { get; set; }

        public List<SarfaslHierarchy> Children { get; set; }
    }
}