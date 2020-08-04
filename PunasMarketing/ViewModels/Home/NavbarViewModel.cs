using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.Home
{
    public class NavbarViewModel
    {
        public IEnumerable<PunasMarketing.Models.DomainModel.FiscalPeriod> Fiscal { get; set; }

        public short CurrentFiscalId { get; set; }
    }
}