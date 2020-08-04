using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.OtherTafsili
{
    public class AddUpdateOtherTafsiliViewModel
    {
        public Models.DomainModel.OtherTafsili OtherTafsili { get; set; }
        public IEnumerable<Models.DomainModel.Sarfasl> MoeenSarfasls { get; set; }
    }
}